﻿/*
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
using System.Linq;

namespace FlatSharp.CodeGen;

internal class DeserializeClassDefinition
{
    protected const string InputBufferVariableName = "__buffer";
    protected const string OffsetVariableName = "__offset";
    protected const string VTableVariableName = "__vtable";
    protected const string RemainingDepthVariableName = "__remainingDepth";
    protected const string CtorContextVariableName = "__CtorContext";

    protected readonly ITypeModel typeModel;
    protected readonly FlatBufferSerializerOptions options;

    protected readonly List<string> propertyOverrides = new();
    protected readonly List<string> initializeStatements = new();
    protected readonly List<string> readMethods = new();
    protected readonly List<ItemMemberModel> itemModels = new();

    // Maps field name -> field initializer.
    protected readonly Dictionary<string, string> instanceFieldDefinitions = new();
    protected readonly Dictionary<string, string> staticFieldDefinitions = new();
    protected readonly MethodInfo? onDeserializeMethod;
    protected readonly string vtableTypeName;
    protected readonly string vtableAccessor;
    protected readonly string remainingDepthAccessor;

    public DeserializeClassDefinition(
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
        this.vtableAccessor = "default";

        if (this.options.GreedyDeserialize)
        {
            this.remainingDepthAccessor = "remainingDepth";

            if (this.typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                // Greedy tables decode a vtable in the constructor but don't store it.
                this.initializeStatements.Add($"{this.vtableTypeName}.Create<TInputBuffer>(buffer, offset, out var vtable);");
                this.vtableAccessor = "vtable";
            }
        }
        else
        {
            this.remainingDepthAccessor = $"this.{RemainingDepthVariableName}";

            // maintain reference to buffer.
            this.instanceFieldDefinitions[InputBufferVariableName] = $"private TInputBuffer {InputBufferVariableName};";
            this.instanceFieldDefinitions[OffsetVariableName] = $"private int {OffsetVariableName};";
            this.instanceFieldDefinitions[RemainingDepthVariableName] = $"private short {RemainingDepthVariableName};";

            this.initializeStatements.Add($"this.{InputBufferVariableName} = buffer;");
            this.initializeStatements.Add($"this.{OffsetVariableName} = offset;");
            this.initializeStatements.Add($"{this.remainingDepthAccessor} = remainingDepth;");

            if (this.typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                // Non-greedy tables store the vtable.
                this.vtableAccessor = $"this.{VTableVariableName}";
                this.initializeStatements.Add($"{this.vtableTypeName}.Create<TInputBuffer>(buffer, offset, out {this.vtableAccessor});");
                this.instanceFieldDefinitions[VTableVariableName] = $"private {this.vtableTypeName} {VTableVariableName};";
            }
        }
    }

    public bool HasEmbeddedBufferReference => !this.options.GreedyDeserialize;

    public string ClassName { get; }

    public void AddProperty(
        ItemMemberModel itemModel,
        ParserCodeGenContext context)
    {
        this.itemModels.Add(itemModel);

        this.AddFieldDefinitions(itemModel);
        this.AddPropertyDefinitions(itemModel);
        this.AddCtorStatements(itemModel);
        this.AddReadMethod(itemModel, context);

        if (itemModel.IsWriteThrough)
        {
            if (this.options.SupportsWriteThrough)
            {
                this.AddWriteThroughMethod(itemModel, context);
            }
        }
    }

    protected virtual void AddFieldDefinitions(ItemMemberModel itemModel)
    {
        if (this.options.Lazy)
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
            RemainingDepthVariableName = "remainingDepth",
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
                {this.vtableTypeName} vtable,
                short remainingDepth)
            {{
                {body}
            }}");
    }

    private void AddWriteThroughMethod(ItemMemberModel itemModel, ParserCodeGenContext parserContext)
    {
        var context = parserContext.GetWriteThroughContext(
            $"buffer.{nameof(IInputBuffer.GetSpan)}()",
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

    private void AddPropertyDefinitions(ItemMemberModel itemModel)
    {
        string setter = string.Empty;
        var accessModifiers = CSharpHelpers.GetPropertyAccessModifiers(itemModel.PropertyInfo);

        if (itemModel.PropertyInfo.SetMethod is not null && !itemModel.PropertyInfo.SetMethod.IsPrivate)
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

        string required = string.Empty;

        // Net 6.0 doesn't define this, so leave open the opportunity for users to add their own polyfills.
        if (itemModel.PropertyInfo.GetCustomAttributes().Any(x => x.GetType().FullName == "System.Runtime.CompilerServices.RequiredMemberAttribute"))
        {
            FlatSharpInternal.Assert(itemModel.IsRequired, "Expecting the item to be required");
            required = " required";
        }

        string typeName = itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
        this.propertyOverrides.Add($@"
#if {CSharpHelpers.Net7PreprocessorVariable}
            {accessModifiers.propertyModifier.ToCSharpString()}{required} override {typeName} {itemModel.PropertyInfo.Name}
#else
            {accessModifiers.propertyModifier.ToCSharpString()} override {typeName} {itemModel.PropertyInfo.Name}
#endif
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

        string assignment = $"this.{GetFieldName(itemModel)}";

        if (this.options.GreedyDeserialize)
        {
            this.initializeStatements.Add($"{assignment} = {GetReadIndexMethodName(itemModel)}(buffer, offset, {this.vtableAccessor}, {this.remainingDepthAccessor});");
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
            onDeserializedStatement = $"base.{this.onDeserializeMethod.Name}({CtorContextVariableName});";
        }

        string baseParams = string.Empty;
        if (ctor.GetParameters().Length != 0)
        {
            baseParams = CtorContextVariableName;
        }

        string interfaceGlobalName = typeof(IFlatBufferDeserializedObject).GetGlobalCompilableTypeName();

        return
        $@"
            [System.Diagnostics.DebuggerDisplay(""{this.options.DeserializationOption} {this.typeModel.ClrType.Name}"")]
            internal sealed class {this.ClassName}<TInputBuffer> 
                : {typeModel.GetGlobalCompilableTypeName()}
                , {interfaceGlobalName}
                where TInputBuffer : IInputBuffer
            {{
                private static readonly {typeof(FlatBufferDeserializationContext).GetGlobalCompilableTypeName()} {CtorContextVariableName} 
                    = new {typeof(FlatBufferDeserializationContext).GetGlobalCompilableTypeName()}({typeof(FlatBufferDeserializationOption).GetGlobalCompilableTypeName()}.{options.DeserializationOption});

                {string.Join("\r\n", this.staticFieldDefinitions.Values)}

                {string.Join("\r\n", this.instanceFieldDefinitions.Values)}

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

        if (this.options.SupportsWriteThrough && itemModel.IsWriteThrough)
        {
            if (!options.Lazy)
            {
                FlatSharpInternal.Assert(options.DeserializationOption == FlatBufferDeserializationOption.Progressive, "Expecting progressive");
                setterLines.Add($"this.{GetFieldName(itemModel)} = value;");
                setterLines.Add($"{typeof(SerializationHelpers).GetGlobalCompilableTypeName()}.{nameof(SerializationHelpers.CombineMask)}(ref this.{GetHasValueFieldName(itemModel)}, {GetHasValueFieldMask(itemModel)});");
            }

            setterLines.Add($"{GetWriteMethodName(itemModel)}({this.GetBufferReference()}, {OffsetVariableName}, value, {this.vtableAccessor});");
        }
        else if (this.options.GenerateMutableObjects)
        {
            // For greedy mutable objects, we emit a special writethrough variant of NotMutableException to clearly express
            // that writethrough-enable properties become read-only when used with GreedyMutable serializers.
            FlatSharpInternal.Assert(options.DeserializationOption == FlatBufferDeserializationOption.GreedyMutable, "Expecting greedy mutable");

            if (itemModel.IsWriteThrough)
            {
                setterLines.Add($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.NotMutable_GreedyMutableWriteThrough)}();");
            }
            else
            {
                setterLines.Add($"this.{GetFieldName(itemModel)} = value;");
            }
        }
        else
        {
            setterLines.Add($"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.NotMutable)}();");
        }

        return string.Join("\r\n", setterLines);
    }

    protected virtual string GetGetterBody(ItemMemberModel itemModel)
    {
        string readUnderlyingInvocation = $"{GetReadIndexMethodName(itemModel)}(this.{InputBufferVariableName}, this.{OffsetVariableName}, {this.vtableAccessor}, {this.remainingDepthAccessor})";
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
                    {typeof(SerializationHelpers).GetGlobalCompilableTypeName()}.{nameof(SerializationHelpers.CombineMask)}(ref this.{GetHasValueFieldName(itemModel)}, {GetHasValueFieldMask(itemModel)});
                }}
                return this.{GetFieldName(itemModel)};
            ";
        }
    }

    protected virtual string GetCtorMethodDefinition(string onDeserializedStatement, string baseCtorParams)
    {
        return $@"
#pragma warning disable CS8618
#if {CSharpHelpers.Net7PreprocessorVariable}
            [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
#endif
            [{typeof(MethodImplAttribute).GetGlobalCompilableTypeName()}({typeof(MethodImplOptions).GetGlobalCompilableTypeName()}.AggressiveInlining)]
            public {this.ClassName}(TInputBuffer buffer, int offset, short remainingDepth) 
                : base({baseCtorParams}) 
            {{
                {string.Join("\r\n", this.initializeStatements)}
                {onDeserializedStatement}
            }}
#pragma warning restore CS8618
        ";
    }

    protected static string GetFieldName(ItemMemberModel itemModel) => $"__index{itemModel.Index}Value";

    protected static string GetHasValueFieldName(ItemMemberModel itemModel) => $"__mask{itemModel.Index / 8}";

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
