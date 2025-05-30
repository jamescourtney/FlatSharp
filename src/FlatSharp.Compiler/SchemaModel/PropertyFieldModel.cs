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

using FlatSharp.Compiler.Schema;
using FlatSharp.Attributes;
using System.Text;
using FlatSharp.CodeGen;
using System.Security.Cryptography.X509Certificates;

namespace FlatSharp.Compiler.SchemaModel;

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
        this.BackingFieldName = null;

        new FlatSharpAttributeValidator(elementType, $"{this.Parent.FullName}.{this.FieldName}")
        {
            VectorTypeValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
            SortedVectorValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
            SharedStringValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
            SetterKindValidator = kind =>
            {
                switch (this.Attributes.SetterKind)
                {
#if !NET5_0_OR_GREATER
                    case SetterKind.PublicInit:
                    case SetterKind.ProtectedInit:
                    case SetterKind.ProtectedInternalInit:
                        return AttributeValidationResult.NeedsAtLeastDotNet5;
#endif
                    default:
                        return AttributeValidationResult.Valid;
                }
            },
            ForceWriteValidator = _ => this.ValidWhenParentIs<TableSchemaModel>(),
            WriteThroughValidator = _ => AttributeValidationResult.Valid,
            PartialPropertyValidator = _ => AttributeValidationResult.Valid,
        }.Validate(this.Attributes);

        if (this.IsPartial)
        {
            this.BackingFieldName = $"__flatsharp_backing_field_{this.FieldName}";
        }

        FlatSharpInternal.Assert(this.Field.Type.BaseType.IsKnown(), "Base type was not known");
        FlatSharpInternal.Assert(
            this.Field.Type.ElementType == BaseType.None ||
            this.Field.Type.ElementType.IsKnown(), "Element type was not known");
    }

    private string? BackingFieldName { get; }

    public bool ProtectedGetter { get; init; }

    public BaseReferenceTypeSchemaModel Parent { get; init; }

    public Field Field { get; init; }

    public FlatBufferSchemaElementType ElementType { get; init; }

    public string? CustomGetter { get; init; }

    public IFlatSharpAttributes Attributes { get; init; }

    public string FieldName { get; init; }

    public int Index { get; init; }

    public bool IsPartial => this.Attributes.PartialProperty ?? this.Parent.Attributes.PartialProperty ?? false;

    public bool HasDefaultValue => this.Field.DefaultDouble != 0 || this.Field.DefaultInteger != 0;

    public static bool TryCreate(BaseReferenceTypeSchemaModel parent, Field field, int previousIndex, [NotNullWhen(true)] out PropertyFieldModel? model)
    {
        model = null;
        if (parent.ElementType != FlatBufferSchemaElementType.Table && parent.ElementType != FlatBufferSchemaElementType.ReferenceStruct)
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
                                       ? FlatBufferSchemaElementType.TableField
                                       : FlatBufferSchemaElementType.StructField;

        model = new PropertyFieldModel(parent, field, index, opts, string.Empty, attributes);
        return true;
    }

    public void WriteCode(CompileContext context, CodeWriter writer)
    {
        writer.AppendSummaryComment(this.Field.Documentation);
        writer.AppendLine(this.GetAttribute());

        if (this.Field.Deprecated)
        {
            writer.AppendLine("[Obsolete]");
        }

        this.Attributes.EmitAsMetadata(writer);

        SetterKind setterKind = this.Attributes.SetterKind ?? SetterKind.Public;

        string setter = setterKind switch
        {
            SetterKind.PublicInit => "init",
            SetterKind.Protected => "protected set",
            SetterKind.ProtectedInternal => "protected internal set",
            SetterKind.ProtectedInit => "protected init",
            SetterKind.ProtectedInternalInit => "protected internal init",
            SetterKind.None => "private set",
            SetterKind.Public or _ => "set",
        };

        bool hasBackingField = !string.IsNullOrEmpty(this.BackingFieldName);

        if (!string.IsNullOrEmpty(setter))
        {
            if (hasBackingField)
            {
                setter = $"{setter} => this.{this.BackingFieldName} = value";
            }

            setter = setter + ";";
        }

        string typeName = this.GetTypeName();
        string getter = "get;";

        if (hasBackingField)
        {
            getter = $"get => this.{this.BackingFieldName};";
        }

        string access = "public";
        if (this.ProtectedGetter)
        {
            access = "protected";
        }

        string partial = string.Empty;
        if (context.CompilePass == CodeWritingPass.LastPass && this.IsPartial)
        {
            partial = "partial";
        }

        string property = $"{access} virtual {partial} {typeName} {this.FieldName} {{ {getter} {setter} }}";

        bool isPublicSetter = setterKind == SetterKind.Public || setterKind == SetterKind.PublicInit;
        if (this.Field.Required == true && isPublicSetter)
        {
            // Required keyword is tricky with visibility and setters.
            writer.BeginPreprocessorIf(CSharpHelpers.Net7PreprocessorVariable, $"required {property}")
                  .Else(property)
                  .Flush();
        }
        else
        {
            writer.AppendLine(property);
        }

        if (hasBackingField)
        {
            writer.AppendLine("[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]");
            writer.AppendLine($"private {typeName} {this.BackingFieldName};");
        }
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
            if (double.IsNaN(this.Field.DefaultDouble))
            {
                return $"{typeName}.NaN";
            }
            else if (double.IsPositiveInfinity(this.Field.DefaultDouble))
            {
                return $"{typeName}.PositiveInfinity";
            }
            else if (double.IsNegativeInfinity(this.Field.DefaultDouble))
            {
                return $"{typeName}.NegativeInfinity";
            }
            else
            {
                FlatSharpInternal.Assert(double.IsFinite(this.Field.DefaultDouble), "Expected finite default double");
                return $"({typeName}){this.Field.DefaultDouble:G17}d";
            }
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

            return $"({typeName})({defaultInt})";
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
            customAccessor = $"[{nameof(FlatBufferMetadataAttribute)}({nameof(FlatBufferMetadataKind)}.{nameof(FlatBufferMetadataKind.Accessor)}, \"\", \"{this.CustomGetter}\")]";
        }

        bool emitForcedWrite = this.Attributes.ForceWrite ?? this.Parent.Attributes.ForceWrite ?? false;
        emitForcedWrite &= this.Field.Type.BaseType.IsScalar();

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
            if (item.Key)
            {
                keyType = item.Type.ResolveTypeOrElementTypeName(this.Parent.Schema, new FlatSharpAttributes(item.Attributes));
                return true;
            }
        }

        return false;
    }

    private string GetVectorTypeName(string innerType, string? keyType)
    {
        return this.Attributes.VectorKind switch
        {
            VectorType.IList => $"IList<{innerType}>",
            VectorType.IReadOnlyList => $"IReadOnlyList<{innerType}>",
            VectorType.Memory => $"Memory<{innerType}>",
            VectorType.ReadOnlyMemory => $"ReadOnlyMemory<{innerType}>",
            VectorType.IIndexedVector => $"IIndexedVector<{keyType ?? "string"}, {innerType}>",
            VectorType.UnityNativeArray => $"Unity.Collections.NativeArray<{innerType}>",

            // Unqualified ubyte vectors default to Memory.
            null => this.Field.Type.ElementType == BaseType.UByte 
                  ? $"Memory<{innerType}>"
                  : $"IList<{innerType}>",

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
