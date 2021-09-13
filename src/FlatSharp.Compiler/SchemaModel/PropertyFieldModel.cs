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
    using System.Text;

    public record PropertyFieldModel
    {
        public PropertyFieldModel(
            BaseReferenceTypeSchemaModel parent,
            Field field,
            int index,
            FlatBufferSchemaElementType elementType,
            string? customGetter,
            IFlatSharpAttributes attributes)
        {
            this.Field = field;
            this.ElementType = elementType;
            this.Attributes = attributes;
            this.Parent = parent;
            this.CustomGetter = customGetter;
            this.FieldName = field.Name;
            this.Index = index;

            new FlatSharpAttributeValidator(elementType, $"{this.Parent.FullName}.{this.FieldName}")
            {
                NonVirtualValidator = _ => AttributeValidationResult.Valid,
                VectorTypeValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
                SortedVectorValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
                SharedStringValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
                SetterKindValidator = _ => AttributeValidationResult.Valid,
                ForceWriteValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
                WriteThroughValidator = _ => AttributeValidationResult.Valid,
            }.Validate(this.Attributes);

            FlatSharpInternal.Assert(this.Field.Type.BaseType.IsKnown(), "Base type was not known");
            FlatSharpInternal.Assert(
                this.Field.Type.ElementType == BaseType.None ||
                this.Field.Type.ElementType.IsKnown(), "Element type was not known");
        }

        public bool ProtectedGetter { get; init; }

        public BaseReferenceTypeSchemaModel Parent { get; init; }

        public Field Field { get; init; }

        public FlatBufferSchemaElementType ElementType { get; init; }

        public string? CustomGetter { get; init; }

        public IFlatSharpAttributes Attributes { get; init; }

        public bool DefaultOptional { get; init; }

        public string FieldName { get; init; }

        public int Index { get; init; }

        public bool HasDefaultValue => this.Field.DefaultDouble != 0 || this.Field.DefaultInteger != 0;

        public static bool TryCreate(BaseReferenceTypeSchemaModel parent, Field field, int previousIndex, [NotNullWhen(true)] out PropertyFieldModel? model)
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

            index = Math.Max(index, previousIndex + 1);

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

            string access = "public";
            if (this.ProtectedGetter)
            {
                access = "protected";
            }

            writer.AppendLine($"{access} {@virtual}{typeName} {this.FieldName} {{ get; {setter} }}");
        }

        public string GetDefaultValue()
        {
            if (!this.HasDefaultValue)
            {
                return "default!";
            }

            string typeName = this.Field.Type.ResolveTypeOrElementTypeName(this.Parent.Schema, this.Attributes);

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
                if (this.Field.Type.BaseType == BaseType.Bool)
                {
                    return "true";
                }

                return $"({typeName}){defaultInt}";
            }
        }

        private string GetAttribute()
        {
            string customAccessor = string.Empty;
            StringBuilder attribute = new($"[{nameof(FlatBufferItemAttribute)}({this.Index}");

            if (this.Field.Key)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.Key)} = true");
            }

            if (this.Attributes.SortedVector == true)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.SortedVector)} = true");
            }

            if (this.Field.Deprecated)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.Deprecated)} = true");
            }

            if (this.HasDefaultValue)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.DefaultValue)} = {this.GetDefaultValue()}");
            }

            if (this.Field.Required)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.Required)} = true");
            }

            if (!string.IsNullOrEmpty(this.CustomGetter))
            {
                customAccessor = $"[{nameof(FlatBufferMetadataAttribute)}({nameof(FlatBufferMetadataKind)}.{nameof(FlatBufferMetadataKind.Accessor)}, \"{this.CustomGetter}\")]";
            }

            bool emitForcedWrite = this.Attributes.ForceWrite == true
                                || (this.Attributes.ForceWrite == null && this.Parent.Attributes.ForceWrite == true && this.Field.Type.BaseType.IsScalar());

            if (emitForcedWrite)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.ForceWrite)} = true");
            }

            if (this.Attributes.WriteThrough ?? this.Parent.Attributes.WriteThrough == true)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.WriteThrough)} = true");
            }

            if (this.Attributes.SharedString == true)
            {
                attribute.Append($", {nameof(FlatBufferItemAttribute.SharedString)} = true");
            }

            attribute.Append(")]");
            attribute.Append(customAccessor);

            return attribute.ToString();
        }

        public string GetTypeName()
        {
            string typeName = this.Field.Type.ResolveTypeOrElementTypeName(this.Parent.Schema, this.Attributes);

            if (this.Field.Type.BaseType == BaseType.Vector)
            {
                this.TryGetElementKeyType(out string? keyType);
                typeName = this.GetVectorTypeName(typeName, keyType);
            }

            if (this.Parent.OptionalFieldsSupported && this.Field.Optional)
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
                    keyType = item.Value.Type.ResolveTypeOrElementTypeName(this.Parent.Schema, new FlatSharpAttributes(item.Value.Attributes));
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
                VectorType.IIndexedVector => $"IIndexedVector<{keyType ?? "string"}, {innerType}>",
                _ => throw new FlatSharpInternalException("Unknown vector kind: " + this.Attributes.VectorKind),
            };
        }

        private AttributeValidationResult ValidWhenParentIs<T>()
        {
            if (this.Parent is T)
            {
                return AttributeValidationResult.Valid;
            }

            return AttributeValidationResult.NeverValid;
        }
    }
}
