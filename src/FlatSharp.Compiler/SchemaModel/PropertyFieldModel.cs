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
            BaseSchemaModel parent,
            Field field,
            FlatBufferSchemaElementType elementType,
            string? customGetter,
            FlatSharpAttributes attributes,
            bool defaultOptional)
        {
            this.Field = field;
            this.ElementType = elementType;
            this.Attributes = attributes;
            this.Parent = parent;
            this.CustomGetter = customGetter;
            this.DefaultOptional = defaultOptional;
        }

        public BaseSchemaModel Parent { get; init; }

        public Field Field { get; init; }

        public FlatBufferSchemaElementType ElementType { get; init; }

        public string? CustomGetter { get; init; }

        public FlatSharpAttributes Attributes { get; init; }

        public bool DefaultOptional { get; init; }

        public static bool TryCreate(BaseSchemaModel parent, Field field, [NotNullWhen(true)] out PropertyFieldModel? model)
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

            var attributes = new FlatSharpAttributes(field.Attributes);

            var opts = parent.ElementType == FlatBufferSchemaElementType.Table
                                           ? (FlatBufferSchemaElementType.TableField, true)
                                           : (FlatBufferSchemaElementType.StructField, false);

            model = new PropertyFieldModel(parent, field, opts.Item1, string.Empty, attributes, opts.Item2);
            return true;
        }

        public void WriteCode(CodeWriter writer, int index)
        {
            writer.AppendLine(this.GetAttribute(index));

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
            writer.AppendLine($"public {@virtual}{typeName} {this.Field.Name} {{ get; {setter} }}");
        }

        private string GetAttribute(int index)
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

            if (this.Field.DefaultDouble != 0)
            {
                FlatSharpInternal.Assert(this.Field.Type.BaseType.TryGetBuiltInTypeName(out string? rawTypeName),  "Couldn't get type name");
                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = ({rawTypeName}){this.Field.DefaultDouble:G17}d";
            }
            else if (this.Field.DefaultInteger != 0)
            {
                FlatSharpInternal.Assert(this.Field.Type.BaseType.TryGetBuiltInTypeName(out string? rawTypeName), "Couldn't get type name");

                string suffix = "L";
                if (this.Field.Type.BaseType == BaseType.ULong)
                {
                    // special
                    suffix = "ul";
                }

                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = ({rawTypeName}){this.Field.DefaultInteger}{suffix}";
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

            return $"[{nameof(FlatBufferItemAttribute)}({index}{defaultValue}{isDeprecated}{sortedVector}{isKey}{forceWrite}{writeThrough}{required})]{customAccessor}";
        }

        private string GetTypeName()
        {
            FlatBufferType type = this.Field.Type;

            bool isVector = type.BaseType == BaseType.Vector;
            BaseType baseType = type.BaseType;

            if (isVector)
            {
                baseType = type.ElementType;
            }

            string typeName = string.Empty;
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

            bool optional = false;
            if (this.Field.Required)
            {
                optional = false;
            }
            else if (this.Field.Optional)
            {
                optional = true;
            }
            else if (this.Field.Type.BaseType.IsScalar())
            {
                optional = false;
            }

            if (isVector)
            {
                this.TryGetElementKeyType(out string? keyType);
                typeName = this.GetVectorTypeName(typeName, keyType);
            }

            if (optional)
            {
                typeName += "?";
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

        string IFlatSharpAttributeSupportTester.FullName => $"{this.Parent.FullName}.{this.Field.Name}";

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
