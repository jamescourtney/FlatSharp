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

namespace FlatSharp.Compiler.SchemaModel
{
    using System;

    using FlatSharp;
    using FlatSharp.Compiler.Schema;
    using FlatSharp.Attributes;
    using System.Diagnostics.CodeAnalysis;

    public record PropertyFieldModel : IFlatSharpAttributeSupportTester
    {
        public PropertyFieldModel(
            BaseReferenceTypeSchemaModel parent,
            Field field,
            int index,
            FlatBufferSchemaElementType elementType,
            string? customGetter,
            FlatSharpAttributes attributes)
        {
            this.Field = field;
            this.ElementType = elementType;
            this.Attributes = attributes;
            this.Parent = parent;
            this.CustomGetter = customGetter;
            this.FieldName = field.Name;
            this.Index = index;
        }

        public BaseReferenceTypeSchemaModel Parent { get; init; }

        public Field Field { get; init; }

        public FlatBufferSchemaElementType ElementType { get; init; }

        public string? CustomGetter { get; init; }

        public FlatSharpAttributes Attributes { get; init; }

        public bool DefaultOptional { get; init; }

        public string FieldName { get; init; }

        public int Index { get; init; }

        public bool HasDefaultValue => this.Field.DefaultDouble != 0 || this.Field.DefaultInteger != 0;

        public static bool TryCreate(BaseReferenceTypeSchemaModel parent, Field field, [NotNullWhen(true)] out PropertyFieldModel? model)
        {
            model = null;
            if (parent.ElementType != FlatBufferSchemaElementType.Table && parent.ElementType != FlatBufferSchemaElementType.Struct)
            {
                return false;
            }

            if (field.Type.BaseType == BaseType.Array)
            {
                // handled by StructVectorSchemaModel.
                return false;
            }

            if (field.Type.BaseType == BaseType.UType || field.Type.ElementType == BaseType.UType)
            {
                return false;
            }

            int index = field.Id;
            if (field.Type.ElementType == BaseType.Union || field.Type.BaseType == BaseType.Union)
            {
                // Unions and Vectors of unions come as two entries. The first is a "UType" that we skip. The second
                // is the "union" that we keep. However, we need to adjust the index down by one to account for this.
                index--;
            }

            var attributes = new FlatSharpAttributes(field.Attributes);

            var opts = parent.ElementType == FlatBufferSchemaElementType.Table
                                           ? (FlatBufferSchemaElementType.TableField)
                                           : (FlatBufferSchemaElementType.StructField);

            model = new PropertyFieldModel(parent, field, index, opts, string.Empty, attributes);
            return true;
        }

        public void WriteCode(CodeWriter writer)
        {
            writer.AppendLine(this.GetAttribute());

            string setter = this.Attributes.SetterKind switch
            {
                SetterKind.PublicInit => "init;",
                SetterKind.Protected => "protected set;",
                SetterKind.ProtectedInternal => "protected internal set;",
                SetterKind.ProtectedInit => "protected init;",
                SetterKind.ProtectedInternalInit => "protected internal init;",
                SetterKind.Private => "private set;",
                SetterKind.None => string.Empty,
                SetterKind.Public or _ => "set;",
            };

            string @virtual = (this.Attributes.NonVirtual ?? this.Parent.Attributes.NonVirtual) switch
            {
                false or null => "virtual ",
                true => string.Empty,
            };

            string typeName = this.GetTypeName();
            writer.AppendLine($"public {@virtual}{typeName} {this.FieldName} {{ get; {setter} }}");
        }

        public string GetDefaultValue()
        {
            if (!this.HasDefaultValue)
            {
                return "default!";
            }

            string typeName = this.GetSimpleTypeName();

            if (this.Field.DefaultDouble != 0)
            {
                return $"({typeName}){this.Field.DefaultDouble:G17}d";
            }
            else
            {
                FlatSharpInternal.Assert(this.Field.DefaultInteger != 0, "Expected default integer");

                string defaultInt = this.Field.DefaultInteger.ToString();
                if (this.Field.Type.BaseType == BaseType.ULong)
                {
                    defaultInt = $"{(ulong)this.Field.DefaultInteger}ul";
                }

                return $"({typeName}){defaultInt}";
            }
        }

        private string GetAttribute()
        {
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string defaultValue = string.Empty;
            string isDeprecated = string.Empty;
            string forceWrite = string.Empty;
            string customAccessor = string.Empty;
            string writeThrough = string.Empty;
            string required = string.Empty;

            if (this.Field.Key)
            {
                isKey = $", {nameof(FlatBufferItemAttribute.Key)} = true";
            }

            if (this.Attributes.SortedVector == true)
            {
                sortedVector = $", {nameof(FlatBufferItemAttribute.SortedVector)} = true";
            }

            if (this.Field.Deprecated)
            {
                isDeprecated = $", {nameof(FlatBufferItemAttribute.Deprecated)} = true";
            }

            if (this.HasDefaultValue)
            {
                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = {this.GetDefaultValue()}";
            }

            if (this.Field.Required)
            {
                required = $", {nameof(FlatBufferItemAttribute.Required)} = true";
            }

            if (this.Attributes.ForceWrite ?? this.Parent.Attributes.ForceWrite == true)
            {
                if (this.Field.Type.BaseType.IsScalar())
                {
                    forceWrite = $", {nameof(FlatBufferItemAttribute.ForceWrite)} = true";
                }
            }

            if (this.Attributes.WriteThrough ?? this.Parent.Attributes.WriteThrough == true)
            {
                writeThrough = $", {nameof(FlatBufferItemAttribute.WriteThrough)} = true";
            }

            return $"[{nameof(FlatBufferItemAttribute)}({this.Index}{defaultValue}{isDeprecated}{sortedVector}{isKey}{forceWrite}{writeThrough}{required})]{customAccessor}";
        }

        public string GetTypeName()
        {
            string typeName = this.GetSimpleTypeName();

            if (this.Parent.OptionalFieldsSupported && this.Field.Optional)
            {
                typeName += "?";
            }

            return typeName;
        }

        private string GetSimpleTypeName()
        {
            FlatBufferType type = this.Field.Type;

            bool isVector = type.BaseType == BaseType.Vector;
            BaseType baseType = type.BaseType;

            if (isVector)
            {
                baseType = type.ElementType;
            }

            string typeName;
            if (type.Index == -1)
            {
                if (baseType == BaseType.String && this.Attributes.SharedString == true)
                {
                    typeName = "SharedString";
                }
                else
                {
                    // Default value. This means that this is a simple built-in type.
                    FlatSharpInternal.Assert(baseType.TryGetBuiltInTypeName(out string? temp), "Failed to get type name");
                    typeName = temp;
                }
            }
            else if (baseType == BaseType.Obj)
            {
                // table or struct.
                typeName = this.Parent.Schema.Objects[type.Index].Name;
            }
            else
            {
                // enum (or union).
                typeName = this.Parent.Schema.Enums[type.Index].Name;
            }

            if (isVector)
            {
                this.TryGetElementKeyType(out string? keyType);
                typeName = this.GetVectorTypeName(typeName, keyType);
            }

            return typeName;
        }

        private bool TryGetElementKeyType(out string? keyType)
        {
            keyType = null;

            if (this.Field.Type.BaseType != BaseType.Vector ||
                this.Field.Type.ElementType != BaseType.Obj ||
                this.Field.Type.Index == -1)
            {
                return false;
            }

            var table = this.Parent.Schema.Objects[this.Field.Type.Index];
            foreach (var item in table.Fields)
            {
                if (item.Value.Key)
                {
                    FlatSharpInternal.Assert(item.Value.Type.BaseType.TryGetBuiltInTypeName(out keyType), "Couldn't format key type");
                    return true;
                }
            }

            return false;
        }

        private string GetVectorTypeName(string innerType, string? keyType)
        {
            return this.Attributes.VectorKind switch
            {
                VectorType.IList or null => $"IList<{innerType}>",
                VectorType.IReadOnlyList => $"IReadOnlyList<{innerType}>",
                VectorType.Array => $"{innerType}[]",
                VectorType.Memory => $"Memory<{innerType}>",
                VectorType.ReadOnlyMemory => $"ReadOnlyMemory<{innerType}>",
                VectorType.IIndexedVector => $"IIndexedVector<{keyType}, {innerType}>",
                _ => throw new FlatSharpInternalException("Unknown vector kind: " + this.Attributes.VectorKind),
            };
        }

        FlatBufferSchemaElementType IFlatSharpAttributeSupportTester.ElementType => this.ElementType;

        string IFlatSharpAttributeSupportTester.FullName => $"{this.Parent.FullName}.{this.FieldName}";

        bool IFlatSharpAttributeSupportTester.SupportsNonVirtual(bool nonVirtualValue) => true;

        bool IFlatSharpAttributeSupportTester.SupportsVectorType(VectorType vectorType) => true;

        bool IFlatSharpAttributeSupportTester.SupportsDeserializationOption(FlatBufferDeserializationOption option) => false;

        bool IFlatSharpAttributeSupportTester.SupportsSortedVector(bool sortedVectorOption) => true;

        bool IFlatSharpAttributeSupportTester.SupportsSharedString(bool sharedStringOption) => true;

        bool IFlatSharpAttributeSupportTester.SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => false;

        bool IFlatSharpAttributeSupportTester.SupportsSetterKind(SetterKind setterKind) => true;

        bool IFlatSharpAttributeSupportTester.SupportsForceWrite(bool forceWriteOption) => true;

        bool IFlatSharpAttributeSupportTester.SupportsUnsafeStructVector(bool unsafeStructVector) => false;

        bool IFlatSharpAttributeSupportTester.SupportsMemoryMarshal(MemoryMarshalBehavior option) => false;

        bool IFlatSharpAttributeSupportTester.SupportsWriteThrough(bool writeThroughOption) => true;

        bool IFlatSharpAttributeSupportTester.SupportsRpcInterface(bool rpcInterface) => true;
    }
}
