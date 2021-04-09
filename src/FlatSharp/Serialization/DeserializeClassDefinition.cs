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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.TypeModel;

    internal class DeserializeClassDefinition
    {
        private const string InputBufferVariableName = "__buffer";
        private const string OffsetVariableName = "__offset";
        private const string VTableLocationVariableName = "__vtableOffset";
        private const string VTableMaxIndexVariableName = "__vtableMaxIndex";

        private readonly ITypeModel typeModel;
        private readonly FlatBufferSerializerOptions options;

        private readonly List<string> fieldDefinitions = new();
        private readonly List<string> propertyOverrides = new();
        private readonly List<string> ctorStatements = new();
        private readonly List<string> readMethods = new();

        private readonly string vtableOffsetAccessor;
        private readonly string vtableMaxIndexAccessor;

        private readonly MethodInfo? onDeserializeMethod;

        public DeserializeClassDefinition(
            string className,
            MethodInfo? onDeserializeMethod,
            ITypeModel typeModel,
            FlatBufferSerializerOptions options)
        {
            this.ClassName = className;
            this.typeModel = typeModel;
            this.options = options;
            this.onDeserializeMethod = onDeserializeMethod;

            if (!this.options.GreedyDeserialize)
            {
                // maintain reference to buffer.
                this.fieldDefinitions.Add($"private readonly TInputBuffer {InputBufferVariableName};");
                this.fieldDefinitions.Add($"private readonly int {OffsetVariableName};");

                this.ctorStatements.Add($"this.{InputBufferVariableName} = buffer;");
                this.ctorStatements.Add($"this.{OffsetVariableName} = offset;");
            }

            if (this.typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                this.vtableMaxIndexAccessor = "__vtableMaxIndex";
                this.vtableOffsetAccessor = "__vtableLocation";

                this.ctorStatements.Add(
                    $"buffer.{nameof(InputBufferExtensions.InitializeVTable)}(offset, out var {this.vtableOffsetAccessor}, out var {this.vtableMaxIndexAccessor});");

                if (!options.GreedyDeserialize)
                {
                    this.fieldDefinitions.Add($"private readonly int {VTableLocationVariableName};");
                    this.fieldDefinitions.Add($"private readonly int {VTableMaxIndexVariableName};");

                    this.ctorStatements.Add($"this.{VTableLocationVariableName} = {this.vtableOffsetAccessor};");
                    this.ctorStatements.Add($"this.{VTableMaxIndexVariableName} = {this.vtableMaxIndexAccessor};");

                    this.vtableMaxIndexAccessor = $"this.{VTableMaxIndexVariableName}";
                    this.vtableOffsetAccessor = $"this.{VTableLocationVariableName}";
                }
            }
            else
            {
                this.vtableMaxIndexAccessor = $"default";
                this.vtableOffsetAccessor = $"default";
            }
        }

        public bool HasEmbeddedBufferReference => !this.options.GreedyDeserialize;

        public bool HasEmbeddedVTableInfo => !this.options.GreedyDeserialize && this.typeModel.SchemaType == FlatBufferSchemaType.Table;

        public string ClassName { get; }

        public void AddProperty(ItemMemberModel itemModel, string readValueMethodName)
        {
            this.AddFieldDefinitions(itemModel);
            this.AddPropertyDefinitions(itemModel);
            this.AddCtorStatements(itemModel);
            this.AddReadMethod(itemModel, readValueMethodName);
        }

        private void AddFieldDefinitions(ItemMemberModel itemModel)
        {
            if (this.options.Lazy || !itemModel.IsVirtual)
            {
                return;
            }

            string typeName = itemModel.ItemTypeModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
            this.fieldDefinitions.Add($"private {typeName} {GetFieldName(itemModel)};");

            if (!this.options.GreedyDeserialize)
            {
                this.fieldDefinitions.Add($"private bool {GetHasValueFieldName(itemModel)};");
            }
        }

        private void AddReadMethod(ItemMemberModel itemModel, string readValueMethodName)
        {
            string inlining = "System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining";
            string attribute = $"[{typeof(System.Runtime.CompilerServices.MethodImplAttribute).FullName}({inlining})]";

            string body = itemModel.CreateReadItemBody(
                readValueMethodName,
                "buffer",
                "offset",
                "vtableOffset",
                "maxVtableIndex");

            string typeName = itemModel.ItemTypeModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
            this.readMethods.Add(
                $@"
                {attribute}
                private static {typeName} {GetReadIndexMethodName(itemModel)}(
                    TInputBuffer buffer, 
                    int offset, 
                    int vtableOffset, 
                    int maxVtableIndex)
                {{
                    {body};
                }}");
        }

        private void AddPropertyDefinitions(ItemMemberModel itemModel)
        {
            if (!itemModel.IsVirtual)
            {
                return;
            }

            string getterBody;
            string setter = string.Empty;
            var accessModifiers = CSharpHelpers.GetPropertyAccessModifiers(itemModel.PropertyInfo);

            {
                string readUnderlyingInvocation = $"{GetReadIndexMethodName(itemModel)}(this.{InputBufferVariableName}, this.{OffsetVariableName}, {this.vtableOffsetAccessor}, {this.vtableMaxIndexAccessor})";
                if (this.options.GreedyDeserialize)
                {
                    getterBody = $"return this.{GetFieldName(itemModel)};";
                }
                else if (this.options.Lazy)
                {
                    getterBody = $"return {readUnderlyingInvocation};";
                }
                else
                {
                    getterBody = $@"
                            if (!this.{GetHasValueFieldName(itemModel)})
                            {{
                                this.{GetFieldName(itemModel)} = {readUnderlyingInvocation};
                                this.{GetHasValueFieldName(itemModel)} = true;
                            }}
                            return this.{GetFieldName(itemModel)};
                        ";
                }
            }

            if (itemModel.PropertyInfo.SetMethod is not null)
            {
                string verb = "set";
                string setterBody;

                MethodInfo? methodInfo = itemModel.PropertyInfo.SetMethod;
                if (methodInfo is not null)
                {
                    // see if set is init-only.
                    bool isInitOnly = methodInfo.ReturnParameter.GetRequiredCustomModifiers().Any(
                        x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
                    
                    if (isInitOnly)
                    {
                        verb = "init";
                    }
                }

                if (!this.options.GenerateMutableObjects)
                {
                    setterBody = $"throw new NotMutableException();";
                }
                else if (this.options.GreedyDeserialize)
                {
                    setterBody = $"this.{GetFieldName(itemModel)} = value;";
                }
                else
                {
                    setterBody = $"this.{GetFieldName(itemModel)} = value; this.{GetHasValueFieldName(itemModel)} = true;";
                }

                setter = $"{accessModifiers.setModifier.ToCSharpString()} {verb} {{ {setterBody} }}";
            }

            string typeName = itemModel.ItemTypeModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
            this.propertyOverrides.Add($@"
                {accessModifiers.propertyModifier.ToCSharpString()} override {typeName} {itemModel.PropertyInfo.Name}
                {{ 
                    {accessModifiers.getModifer.ToCSharpString()} get
                    {{
                        {getterBody}
                    }}
                    {setter}
                }}");
        }

        private void AddCtorStatements(ItemMemberModel itemModel)
        {
            if (!this.options.GreedyDeserialize && itemModel.IsVirtual)
            {
                return;
            }

            string assignment = $"base.{itemModel.PropertyInfo.Name}";
            if (itemModel.IsVirtual)
            {
                assignment = $"this.{GetFieldName(itemModel)}";
            }

            this.ctorStatements.Add($"{assignment} = {GetReadIndexMethodName(itemModel)}(buffer, offset, {this.vtableOffsetAccessor}, {this.vtableMaxIndexAccessor});");
        }

        public override string ToString()
        {
            ConstructorInfo? ctor = this.typeModel.PreferredSubclassConstructor;
            if (ctor is null)
            {
                throw new InvalidFlatBufferDefinitionException($"Unable to find a usable subclass constructor for '{this.typeModel.GetCompilableTypeName()}'.");
            }

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

            return 
            $@"
                private sealed class {this.ClassName}<TInputBuffer> 
                    : {typeModel.GetCompilableTypeName()} 
                    , {nameof(IFlatBufferDeserializedObject)}
                    where TInputBuffer : IInputBuffer
                {{
                    private static readonly {nameof(FlatBufferDeserializationContext)} __CtorContext 
                        = new {nameof(FlatBufferDeserializationContext)}({nameof(FlatBufferDeserializationOption)}.{options.DeserializationOption});

                    {string.Join("\r\n", this.fieldDefinitions)}

                    public {this.ClassName}(TInputBuffer buffer, int offset) : base({baseParams})
                    {{
                        {string.Join("\r\n", this.ctorStatements)}
                        {onDeserializedStatement}
                    }}

                    Type {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.TableOrStructType)} => typeof({typeModel.GetCompilableTypeName()});
                    {nameof(FlatBufferDeserializationContext)} {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.DeserializationContext)} => __CtorContext;
                    {nameof(IInputBuffer)}? {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.InputBuffer)} => {this.GetBufferReference()};

                    {string.Join("\r\n", this.propertyOverrides)}

                    {string.Join("\r\n", this.readMethods)}
                }}
";
        }

        private static string GetFieldName(ItemMemberModel itemModel) => $"__index{itemModel.Index}Value";

        private static string GetHasValueFieldName(ItemMemberModel itemModel) => $"__hasIndex{itemModel.Index}Value";

        private static string GetReadIndexMethodName(ItemMemberModel itemModel) => $"ReadIndex{itemModel.Index}Value";

        private string GetBufferReference()
        {
            if (this.HasEmbeddedBufferReference)
            {
                return $"this.{InputBufferVariableName}";
            }

            return "null";
        }
    }
}
