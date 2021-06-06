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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FlatSharp.TypeModel;

    internal class DeserializeClassDefinition
    {
        protected const string InputBufferVariableName = "__buffer";
        protected const string OffsetVariableName = "__offset";
        protected const string VTableLocationVariableName = "__vtableOffset";
        protected const string VTableMaxIndexVariableName = "__vtableMaxIndex";

        protected readonly ITypeModel typeModel;
        protected readonly FlatBufferSerializerOptions options;

        protected readonly List<string> propertyOverrides = new();
        protected readonly List<string> initializeStatements = new();
        protected readonly List<string> readMethods = new();
        protected readonly List<string> recycleMethodInvocations = new();

        // Maps field name -> field initializer.
        protected readonly Dictionary<string, string> instanceFieldDefinitions = new();
        protected readonly Dictionary<string, string> staticFieldDefinitions = new();

        protected readonly string vtableOffsetAccessor;
        protected readonly string vtableMaxIndexAccessor;

        protected readonly MethodInfo? onDeserializeMethod;

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
                this.instanceFieldDefinitions[InputBufferVariableName] = $"private TInputBuffer {InputBufferVariableName};";
                this.instanceFieldDefinitions[OffsetVariableName] = $"private int {OffsetVariableName};";

                this.initializeStatements.Add($"this.{InputBufferVariableName} = buffer;");
                this.initializeStatements.Add($"this.{OffsetVariableName} = offset;");
            }

            if (this.typeModel.SchemaType == FlatBufferSchemaType.Table)
            {
                this.vtableMaxIndexAccessor = "__vtableMaxIndex";
                this.vtableOffsetAccessor = "__vtableLocation";

                this.initializeStatements.Add(
                    $"buffer.{nameof(InputBufferExtensions.InitializeVTable)}(offset, out var {this.vtableOffsetAccessor}, out var {this.vtableMaxIndexAccessor});");

                if (!options.GreedyDeserialize)
                {
                    this.instanceFieldDefinitions[VTableLocationVariableName] = $"private int {VTableLocationVariableName};";
                    this.instanceFieldDefinitions[VTableMaxIndexVariableName] = $"private int {VTableMaxIndexVariableName};";

                    this.initializeStatements.Add($"this.{VTableLocationVariableName} = {this.vtableOffsetAccessor};");
                    this.initializeStatements.Add($"this.{VTableMaxIndexVariableName} = {this.vtableMaxIndexAccessor};");

                    this.vtableMaxIndexAccessor = $"this.{VTableMaxIndexVariableName}";
                    this.vtableOffsetAccessor = $"this.{VTableLocationVariableName}";
                }
            }
            else
            {
                this.vtableMaxIndexAccessor = "default";
                this.vtableOffsetAccessor = "default";
            }
        }

        public static DeserializeClassDefinition Create(
            string className,
            MethodInfo? onDeserializeMethod,
            ITypeModel typeModel,
            FlatBufferSerializerOptions options,
            int poolSize)
        {
            if (poolSize != 0)
            {
                return new PoolingDeserializeClassDefinition(className, onDeserializeMethod, typeModel, options, poolSize);
            }
            else
            {
                return new DeserializeClassDefinition(className, onDeserializeMethod, typeModel, options);
            }
        }

        public bool HasEmbeddedBufferReference => !this.options.GreedyDeserialize;

        public string ClassName { get; }

        public void AddProperty(ItemMemberModel itemModel, string readValueMethodName, string writeValueMethodName, string? recycleMethodName)
        {
            this.AddFieldDefinitions(itemModel);
            this.AddPropertyDefinitions(itemModel, writeValueMethodName);
            this.AddCtorStatements(itemModel);
            this.AddReadMethod(itemModel, readValueMethodName);

            if (!this.options.Lazy)
            {
                if (!string.IsNullOrEmpty(recycleMethodName) && itemModel.ItemTypeModel.HasRecyclableDescendant())
                {
                    this.recycleMethodInvocations.Add($"{recycleMethodName}(this.{GetFieldName(itemModel)});");
                }
            }

            if (itemModel.IsWriteThrough)
            {
                if (this.options.DeserializationOption != FlatBufferDeserializationOption.VectorCacheMutable)
                {
                    throw new InvalidFlatBufferDefinitionException(
                        $"Property '{itemModel.PropertyInfo.Name}' of {this.typeModel.SchemaType} '{this.typeModel.GetCompilableTypeName()}' specifies the WriteThrough option. However, WriteThrough is only supported when using deserialization option 'VectorCacheMutable'.");
                }

                this.AddWriteThroughMethod(itemModel, writeValueMethodName);
            }
        }

        protected virtual void AddFieldDefinitions(ItemMemberModel itemModel)
        {
            if (this.options.Lazy || !itemModel.IsVirtual)
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

        private void AddReadMethod(ItemMemberModel itemModel, string readValueMethodName)
        {
            string body = itemModel.CreateReadItemBody(
                readValueMethodName,
                "buffer",
                "offset",
                "vtableOffset",
                "maxVtableIndex");

            string typeName = itemModel.GetNullableAnnotationTypeName(this.typeModel.SchemaType);
            this.readMethods.Add(
                $@"
                {GetAggressiveInliningAttribute()}
                private static {typeName} {GetReadIndexMethodName(itemModel)}(
                    TInputBuffer buffer, 
                    int offset, 
                    int vtableOffset, 
                    int maxVtableIndex)
                {{
                    {body};
                }}");
        }

        private void AddWriteThroughMethod(ItemMemberModel itemModel, string writeValueMethodName)
        {
            this.readMethods.Add(
                $@"
                    {GetAggressiveInliningAttribute()}
                    private static void {GetWriteMethodName(itemModel)}(
                        TInputBuffer buffer,
                        int offset,
                        {itemModel.ItemTypeModel.GetCompilableTypeName()} value)
                    {{
                        {itemModel.CreateWriteThroughBody(writeValueMethodName, "buffer", "offset", "value")}
                    }}");
        }

        private void AddPropertyDefinitions(ItemMemberModel itemModel, string writeValueMethodName)
        {
            if (!itemModel.IsVirtual)
            {
                return;
            }

            string setter = string.Empty;
            var accessModifiers = CSharpHelpers.GetPropertyAccessModifiers(itemModel.PropertyInfo);

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
                this.initializeStatements.Add($"{assignment} = {GetReadIndexMethodName(itemModel)}(buffer, offset, {this.vtableOffsetAccessor}, {this.vtableMaxIndexAccessor});");
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
                    , {typeof(IFlatBufferDeserializedObject).GetCompilableTypeName()}
                    where TInputBuffer : IInputBuffer
                {{
                    private static readonly {typeof(FlatBufferDeserializationContext).GetCompilableTypeName()} __CtorContext 
                        = new {typeof(FlatBufferDeserializationContext).GetCompilableTypeName()}({typeof(FlatBufferDeserializationOption).GetCompilableTypeName()}.{options.DeserializationOption});

                    {string.Join("\r\n", this.staticFieldDefinitions.Values)}

                    {string.Join("\r\n", this.instanceFieldDefinitions.Values)}

                    public static {this.ClassName}<TInputBuffer> GetOrCreate(TInputBuffer buffer, int offset)
                    {{
                        {this.GetGetOrCreateMethodBody()}
                    }}

                    {this.GetCtorMethodDefinition(onDeserializedStatement, baseParams)}

                    {typeof(Type).GetCompilableTypeName()} {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.TableOrStructType)} => typeof({typeModel.GetCompilableTypeName()});
                    {typeof(FlatBufferDeserializationContext).GetCompilableTypeName()} {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.DeserializationContext)} => __CtorContext;
                    {typeof(IInputBuffer).GetCompilableTypeName()}? {nameof(IFlatBufferDeserializedObject)}.{nameof(IFlatBufferDeserializedObject.InputBuffer)} => {this.GetBufferReference()};

                    void {nameof(IRecyclable)}.{nameof(IRecyclable.DangerousRecycle)}()
                    {{
                        {string.Join("\r\n", this.recycleMethodInvocations)}
                        {this.GetDangerousReleaseMethodBody()}
                    }}

                    {string.Join("\r\n", this.propertyOverrides)}
                    {string.Join("\r\n", this.readMethods)}
                }}
";
        }

        protected virtual string GetSetterBody(ItemMemberModel itemModel)
        {
            string setterBody;

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
                setterBody = $"this.{GetFieldName(itemModel)} = value; this.{GetHasValueFieldName(itemModel)} |= {GetHasValueFieldMask(itemModel)};";
                if (itemModel.IsWriteThrough)
                {
                    setterBody += $"{GetWriteMethodName(itemModel)}({this.GetBufferReference()}, {OffsetVariableName}, value);";
                }
            }

            return setterBody;
        }

        protected virtual string GetGetterBody(ItemMemberModel itemModel)
        {
            string readUnderlyingInvocation = $"{GetReadIndexMethodName(itemModel)}(this.{InputBufferVariableName}, this.{OffsetVariableName}, {this.vtableOffsetAccessor}, {this.vtableMaxIndexAccessor})";
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

        protected virtual string GetDangerousReleaseMethodBody() => string.Empty;

        protected static string GetFieldName(ItemMemberModel itemModel) => $"__index{itemModel.Index}Value";

        protected static string GetHasValueFieldName(int index) => $"__mask{index}";

        protected static string GetHasValueFieldName(ItemMemberModel itemModel) => GetHasValueFieldName(itemModel.Index / 8);

        protected static string GetHasValueFieldMask(ItemMemberModel itemModel) => $"(byte){1 << (itemModel.Index % 8)}";

        protected static string GetWriteMethodName(ItemMemberModel itemModel) => $"WriteIndex{itemModel.Index}Value";

        protected static string GetReadIndexMethodName(ItemMemberModel itemModel) => $"ReadIndex{itemModel.Index}Value";

        private static string GetAggressiveInliningAttribute()
        {
            string inlining = "System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining";
            string attribute = $"[{typeof(System.Runtime.CompilerServices.MethodImplAttribute).FullName}({inlining})]";
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
}
