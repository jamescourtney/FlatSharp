/*
 * Copyright 2021 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using FlatSharp.TypeModel;

namespace FlatSharp.CodeGen;

internal class DeserializeClassDefinition
{
    protected const string InputBufferVariableName = "__buffer";
    protected const string OffsetVariableName = "__offset";
    protected const string VTableVariableName = "__vtable";

    protected readonly ITypeModel typeModel;
    protected readonly FlatBufferSerializerOptions options;

    protected readonly List<string> propertyOverrides = new();
    protected readonly List<string> initializeStatements = new();
    protected readonly List<string> readMethods = new();

    // Maps field name -> field initializer.
    protected readonly Dictionary<string, string> instanceFieldDefinitions = new();
    protected readonly Dictionary<string, string> staticFieldDefinitions = new();
    protected readonly MethodInfo? onDeserializeMethod;
    protected readonly string vtableTypeName;
    protected readonly string vtableAccessor;

    private DeserializeClassDefinition(
        string className,
        MethodInfo? onDeserializeMethod,
        ITypeModel typeModel,
        int maxVtableIndex,
        FlatBufferSerializerOptions options)
    {
        this.ClassName = className;
        this.typeModel = typeModel;
        this.options = options;
        this.vtableTypeName = GetVTableTypeName(maxVtableIndex);
        this.onDeserializeMethod = onDeserializeMethod;

        if (!this.options.GreedyDeserialize)
        {
            // maintain reference to buffer.
            this.instanceFieldDefinitions[InputBufferVariableName] = $"private TInputBuffer {InputBufferVariableName};";
            this.instanceFieldDefinitions[OffsetVariableName] = $"private int {OffsetVariableName};";
            this.initializeStatements.Add($"this.{InputBufferVariableName} = buffer;");
            this.initializeStatements.Add($"this.{OffsetVariableName} = offset;");
        }

        this.vtableAccessor = "default";
        if (this.typeModel.SchemaType == FlatBufferSchemaType.Table)
        {
            if (this.options.GreedyDeserialize)
            {
                // Greedy tables decode a vtable in the constructor but don't stor eit.
                this.initializeStatements.Add($"{this.vtableTypeName}.Create<TInputBuffer>(buffer, offset, out var vtable);");
                this.vtableAccessor = "vtable";
            }
            else
            {
                // non-greedy tables also carry a vtable.
                this.vtableAccessor = $"this.{VTableVariableName}";
                this.initializeStatements.Add($"{this.vtableTypeName}.Create<TInputBuffer>(buffer, offset, out {this.vtableAccessor});");
                this.instanceFieldDefinitions[VTableVariableName] = $"private {this.vtableTypeName} {VTableVariableName};";
            }
        }
    }

    public static DeserializeClassDefinition Create(
        string className,
        MethodInfo? onDeserializeMethod,
        ITypeModel typeModel,
        int maxVTableIndex,
        FlatBufferSerializerOptions options)
    {
        return new DeserializeClassDefinition(className, onDeserializeMethod, typeModel, maxVTableIndex, options);
    }

    public bool HasEmbeddedBufferReference => !this.options.GreedyDeserialize;

    public string ClassName { get; }

    public void AddProperty(
        ItemMemberModel itemModel,
        ParserCodeGenContext context)
    {
        this.AddFieldDefinitions(itemModel);
        this.AddPropertyDefinitions(itemModel, context.SerializeMethodNameMap[itemModel.ItemTypeModel.ClrType]);
        this.AddCtorStatements(itemModel);
        this.AddReadMethod(itemModel, context);

        if (itemModel.IsWriteThrough)
        {
            if (!this.options.SupportsWriteThrough)
            {
                throw new InvalidFlatBufferDefinitionException(
                    $"Property '{itemModel.PropertyInfo.Name}' of {this.typeModel.SchemaType} '{this.typeModel.GetCompilableTypeName()}' specifies the WriteThrough option. However, WriteThrough is only supported when using deserialization option 'Progressive' or 'Lazy'.");
            }

            this.AddWriteThroughMethod(itemModel, context);
        }
    }

    protected virtual void AddFieldDefinitions(ItemMemberModel itemModel)
    {
        if (!itemModel.IsVirtual || this.options.Lazy)
        {
            return;
        }

        if (!this.options.GreedyDeserialize)
        {
            this.instanceFieldDefinitions[GetHasValueFieldName(itemModel)] = $"private byte {GetHasValueFieldName(itemModel)};";
        }

        string typeName = itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
        this.instanceFieldDefinitions[GetFieldName(itemModel)] = $"private {typeName} {GetFieldName(itemModel)};";
    }

    private void AddReadMethod(
        ItemMemberModel itemModel,
        ParserCodeGenContext ctx)
    {
        ctx = ctx with
        {
            InputBufferTypeName = "TInputBuffer",
            OffsetVariableName = "offset",
            InputBufferVariableName = "buffer",
        };

        string body = itemModel.CreateReadItemBody(
            ctx,
            "vtable");

        string typeName = itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
        this.readMethods.Add($@"
            {GetAggressiveInliningAttribute()}
            private static {typeName} {GetReadIndexMethodName(itemModel)}(
                TInputBuffer buffer, 
                int offset, 
                {this.vtableTypeName} vtable)
            {{
                {body}
            }}");
    }

    private void AddWriteThroughMethod(ItemMemberModel itemModel, ParserCodeGenContext parserContext)
    {
        var context = parserContext.GetWriteThroughContext(
            $"buffer.{nameof(InputBufferExtensions.AsSpan)}()",
            "value",
            "offset");

        this.readMethods.Add($@"
            {GetAggressiveInliningAttribute()}
            private static void {GetWriteMethodName(itemModel)}(
                TInputBuffer buffer,
                int offset,
                {itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType)} value,
                {this.vtableTypeName} vtable)
            {{
                {itemModel.CreateWriteThroughBody(context, "vtable")}
            }}");
    }

    private void AddPropertyDefinitions(ItemMemberModel itemModel, string writeValueMethodName)
    {
        if (!itemModel.IsVirtual)
        {
            return;
        }

        string setter = string.Empty;
        var accessModifiers = CSharpHelpers.GetPropertyAccessModifiers(itemModel.PropertyInfo, this.options.ConvertProtectedInternalToProtected);

        if (itemModel.PropertyInfo.SetMethod is not null)
        {
            string setterBody = this.GetSetterBody(itemModel);

            string verb = "set";
            if (itemModel.SetterKind == ItemMemberModel.SetMethodKind.Init)
            {
                verb = "init";
            }

            setter = $@"
                {accessModifiers.setModifier.ToCSharpString()} {verb} 
                {{ 
                    {setterBody} 
                }}";
        }

        string typeName = itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
        this.propertyOverrides.Add($@"
            {accessModifiers.propertyModifier.ToCSharpString()} override {typeName} {itemModel.PropertyInfo.Name}
            {{ 
                {accessModifiers.getModifer.ToCSharpString()} get
                {{
                    {this.GetGetterBody(itemModel)}
                }}
                {setter}
            }}");
    }

    private void AddCtorStatements(ItemMemberModel itemModel)
    {
        var classification = itemModel.ItemTypeModel.ClassifyContextually(this.typeModel.SchemaType);

        string assignment = $"base.{itemModel.PropertyInfo.Name}";
        if (itemModel.IsVirtual)
        {
            assignment = $"this.{GetFieldName(itemModel)}";
        }

        if (this.options.GreedyDeserialize || !itemModel.IsVirtual)
        {
            this.initializeStatements.Add($"{assignment} = {GetReadIndexMethodName(itemModel)}(buffer, offset, {this.vtableAccessor});");
        }
        else if (!this.options.Lazy)
        {
            if (classification.IsRequiredReference() || (classification.IsOptionalReference() && itemModel.IsRequired))
            {
                this.initializeStatements.Add($"{assignment} = null!;");
            }
        }
    }

    public override string ToString()
    {
        ConstructorInfo? ctor = this.typeModel.PreferredSubclassConstructor;
        FlatSharpInternal.Assert(ctor is not null, $"Unable to find a usable subclass constructor for '{this.typeModel.GetCompilableTypeName()}'.");

        string onDeserializedStatement = string.Empty;
        if (this.onDeserializeMethod is not null)
        {
            onDeserializedStatement = $"base.{this.onDeserializeMethod.Name}(__CtorContext);";
        }

        string baseParams = string.Empty;
        if (ctor.GetParameters().Length != 0)
        {
            baseParams = "__CtorContext";
        }

        string interfaceGlobalName = typeof(IFlatBufferDeserializedObject).GetGlobalCompilableTypeName();

        return
        $@"
            private sealed class {this.ClassName}<TInputBuffer> 
                : {typeModel.GetGlobalCompilableTypeName()}
                , {interfaceGlobalName}
                where TInputBuffer : IInputBuffer
            {{
                private static readonly {typeof(FlatBufferDeserializationContext).GetGlobalCompilableTypeName()} __CtorContext 
                    = new {typeof(FlatBufferDeserializationContext).GetGlobalCompilableTypeName()}({typeof(FlatBufferDeserializationOption).GetGlobalCompilableTypeName()}.{options.DeserializationOption});

                {string.Join("\r\n", this.staticFieldDefinitions.Values)}

                {string.Join("\r\n", this.instanceFieldDefinitions.Values)}

                public static {this.ClassName}<TInputBuffer> GetOrCreate(TInputBuffer buffer, int offset)
                {{
                    {this.GetGetOrCreateMethodBody()}
                }}

                {this.GetCtorMethodDefinition(onDeserializedStatement, baseParams)}

                {typeof(Type).GetGlobalCompilableTypeName()} {interfaceGlobalName}.{nameof(IFlatBufferDeserializedObject.TableOrStructType)} => typeof({typeModel.GetCompilableTypeName()});
                {typeof(FlatBufferDeserializationContext).GetGlobalCompilableTypeName()} {interfaceGlobalName}.{nameof(IFlatBufferDeserializedObject.DeserializationContext)} => __CtorContext;
                {typeof(IInputBuffer).GetGlobalCompilableTypeName()}? {interfaceGlobalName}.{nameof(IFlatBufferDeserializedObject.InputBuffer)} => {this.GetBufferReference()};
                bool {interfaceGlobalName}.{nameof(IFlatBufferDeserializedObject.CanSerializeWithMemoryCopy)} => {this.options.CanSerializeWithMemoryCopy.ToString().ToLowerInvariant()};

                {string.Join("\r\n", this.propertyOverrides)}
                {string.Join("\r\n", this.readMethods)}
            }}
        ";
    }

    protected virtual string GetSetterBody(ItemMemberModel itemModel)
    {
        List<string> setterLines = new List<string>();

        bool writeThrough = this.options.SupportsWriteThrough && itemModel.IsWriteThrough;

        if (this.options.GenerateMutableObjects || writeThrough)
        {
            if (!options.Lazy)
            {
                // If we aren't lazy, we need a backing field.
                setterLines.Add($"this.{GetFieldName(itemModel)} = value;");

                if (!options.GreedyDeserialize)
                {
                    // If we aren't lazy and aren't greedy, we also need an IsPresent mask.
                    setterLines.Add($"this.{GetHasValueFieldName(itemModel)} |= {GetHasValueFieldMask(itemModel)};");
                }
            }

            // Finally, if we are writethrough enabled, we need to do that too.
            if (writeThrough)
            {
                setterLines.Add($"{GetWriteMethodName(itemModel)}({this.GetBufferReference()}, {OffsetVariableName}, value, {this.vtableAccessor});");
            }
        }
        else
        {
            setterLines.Add($"throw new NotMutableException();");
        }

        return string.Join("\r\n", setterLines);
    }

    protected virtual string GetGetterBody(ItemMemberModel itemModel)
    {
        string readUnderlyingInvocation = $"{GetReadIndexMethodName(itemModel)}(this.{InputBufferVariableName}, this.{OffsetVariableName}, {this.vtableAccessor})";
        if (this.options.GreedyDeserialize)
        {
            return $"return this.{GetFieldName(itemModel)};";
        }
        else if (this.options.Lazy)
        {
            return $"return {readUnderlyingInvocation};";
        }
        else
        {
            return $@"
                if ((this.{GetHasValueFieldName(itemModel)} & {GetHasValueFieldMask(itemModel)}) == 0)
                {{
                    this.{GetFieldName(itemModel)} = {readUnderlyingInvocation};
                    this.{GetHasValueFieldName(itemModel)} |= {GetHasValueFieldMask(itemModel)};
                }}
                return this.{GetFieldName(itemModel)};
            ";
        }
    }

    protected virtual string GetGetOrCreateMethodBody()
    {
        return $@"
            var item = new {this.ClassName}<TInputBuffer>(buffer, offset);
            return item;
        ";
    }

    protected virtual string GetCtorMethodDefinition(string onDeserializedStatement, string baseCtorParams)
    {
        return $@"
            private {this.ClassName}(TInputBuffer buffer, int offset) : base({baseCtorParams}) 
            {{ 
                {string.Join("\r\n", this.initializeStatements)}
                {onDeserializedStatement}
            }}
        ";
    }

    protected static string GetFieldName(ItemMemberModel itemModel) => $"__index{itemModel.Index}Value";

    protected static string GetHasValueFieldName(int index) => $"__mask{index}";

    protected static string GetHasValueFieldName(ItemMemberModel itemModel) => GetHasValueFieldName(itemModel.Index / 8);

    protected static string GetHasValueFieldMask(ItemMemberModel itemModel) => $"(byte){1 << (itemModel.Index % 8)}";

    protected static string GetWriteMethodName(ItemMemberModel itemModel) => $"WriteIndex{itemModel.Index}Value";

    protected static string GetReadIndexMethodName(ItemMemberModel itemModel) => $"ReadIndex{itemModel.Index}Value";

    protected static string GetVTableTypeName(int maxVtableIndex)
    {
        if (maxVtableIndex >= 8)
        {
            return nameof(VTableGeneric);
        }
        else if (maxVtableIndex >= 4)
        {
            return nameof(VTable8);
        }
        else
        {
            FlatSharpInternal.Assert(maxVtableIndex < 4, "expected small vtable");
            return nameof(VTable4);
        }
    }

    private static string GetAggressiveInliningAttribute()
    {
        string inlining = "System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining";
        string attribute = $"[{typeof(MethodImplAttribute).GetCompilableTypeName()}({inlining})]";
        return attribute;
    }

    protected string GetBufferReference()
    {
        if (this.HasEmbeddedBufferReference)
        {
            return $"this.{InputBufferVariableName}";
        }

        return "null";
    }
}
