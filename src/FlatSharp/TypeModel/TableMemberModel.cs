/*
 * Copyright 2018 James Courtney
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

using FlatSharp.Attributes;

namespace FlatSharp.TypeModel;

/// <summary>
/// Describes a member of a FlatBuffer table.
/// </summary>
public class TableMemberModel : ItemMemberModel
{
    private bool hasWriteThroughAttribute;

    public TableMemberModel(
        ITypeModel propertyModel,
        PropertyInfo propertyInfo,
        FlatBufferItemAttribute attribute) : base(propertyModel, propertyInfo, attribute)
    {
        this.DefaultValue = attribute.DefaultValue;
        this.IsSortedVector = attribute.SortedVector;
        this.IsKey = attribute.Key;
        this.IsDeprecated = attribute.Deprecated;
        this.ForceWrite = attribute.ForceWrite;
        this.IsSharedString = attribute.SharedString;
        this.hasWriteThroughAttribute = this.IsWriteThrough;
        
        if (this.IsWriteThrough && this.ItemTypeModel.SchemaType == FlatBufferSchemaType.Vector)
        {
            // Reset writethrough to false. The attribute indicates that the members of the vector
            // are write-through-able, but the actual vector is not.
            this.IsWriteThrough = false;
        }
    }

    internal override void Validate()
    {
        base.Validate();

        if (!this.ItemTypeModel.IsValidTableMember)
        {
            throw new InvalidFlatBufferDefinitionException($"Table property '{this.FriendlyName}' cannot be part of a flatbuffer table.");
        }

        if (this.DefaultValue is not null && !this.ItemTypeModel.ValidateDefaultValue(this.DefaultValue))
        {
            throw new InvalidFlatBufferDefinitionException($"Table property '{this.FriendlyName}' declared default value of type {this.ItemTypeModel.ClrType.Name}, but the value was of type {this.DefaultValue.GetType().GetCompilableTypeName()}. Please ensure that the property is allowed to have a default value and that the types match.");
        }

        static bool IsValueStruct(ITypeModel model) => model.SchemaType == FlatBufferSchemaType.Struct && model.ClrType.IsValueType;

        if (this.hasWriteThroughAttribute)
        {
            if (IsValueStruct(this.ItemTypeModel))
            {
                FlatSharpInternal.Assert(
                    this.IsRequired,
                    $"Table property '{this.FriendlyName}' declared the WriteThrough attribute, but the field is not marked as required. WriteThrough fields must also be required.");

                FlatSharpInternal.Assert(
                    !this.ItemTypeModel.SerializeMethodRequiresContext,
                    "write through struct expected serialization context");
            }
            else if (this.ItemTypeModel.SchemaType == FlatBufferSchemaType.Vector)
            {
                FlatSharpInternal.Assert(this.ItemTypeModel.TryGetUnderlyingVectorType(out ITypeModel? underlyingModel), "failed to get underlying vector type");
                if (!IsValueStruct(underlyingModel))
                {
                    throw new InvalidFlatBufferDefinitionException($"Table property '{this.FriendlyName}' declared the WriteThrough on a vector. Vector WriteThrough is only valid for value type structs.");
                }

                FlatSharpInternal.Assert(
                    !underlyingModel.SerializeMethodRequiresContext,
                    "write through struct vector member expects serialization context");
            }
            else
            {
                throw new InvalidFlatBufferDefinitionException($"Table property '{this.FriendlyName}' declared the WriteThrough attribute. WriteThrough on tables is only supported for value type structs.");
            }
        }

        if (this.IsRequired)
        {
            FlatSharpInternal.Assert(
                this.ItemTypeModel.SchemaType != FlatBufferSchemaType.Scalar,
                $"Table property '{this.FriendlyName}' declared the Required attribute. Required is only valid on non-scalar table fields.");

            FlatSharpInternal.Assert(
                this.DefaultValue is null,
                $"Table property '{this.FriendlyName}' declared the Required attribute and also declared a Default Value. These two items are incompatible.");
        }

        if (this.IsSharedString)
        {
            if (this.ItemTypeModel.SchemaType == FlatBufferSchemaType.String)
            {
                // regular ol' string
            }
            else if (this.ItemTypeModel.TryGetUnderlyingVectorType(out ITypeModel? memberModel) && memberModel.SchemaType == FlatBufferSchemaType.String)
            {
                // vector of string.
            }
            else
            {
                throw new InvalidFlatBufferDefinitionException($"Table property '{this.FriendlyName}' declared the SharedString attribute. This is only supported on strings and vectors of strings.");
            }
        }
    }

    /// <summary>
    /// The default value of the table member.
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// Indicates if the member vector should be sorted before serializing.
    /// </summary>
    public bool IsSortedVector { get; set; }

    /// <summary>
    /// Indicates that this property is the key for the table.
    /// </summary>
    public bool IsKey { get; set; }

    /// <summary>
    /// Indicates that this property is deprecated and serializers need not be generated for it.
    /// </summary>
    public bool IsDeprecated { get; set; }

    /// <summary>
    /// Indicates that this field should always be written to a table, even
    /// if the default value matches.
    /// </summary>
    public bool ForceWrite { get; set; }

    /// <summary>
    /// Indicates if strings within this member should be shared.
    /// </summary>
    public bool IsSharedString { get; set; }

    /// <summary>
    /// Returns a C# literal that is equal to the default value.
    /// </summary>
    public string DefaultValueLiteral => this.ItemTypeModel.FormatDefaultValueAsLiteral(this.DefaultValue);

    public override string CreateReadItemBody(
        ParserCodeGenContext context,
        string vtableVariableName)
    {
        if (this.ItemTypeModel.PhysicalLayout.Length == 1)
        {
            return this.CreateSingleWidthReadItemBody(context, vtableVariableName);
        }
        else
        {
            return this.CreateWideReadItemBody(context, vtableVariableName);
        }
    }

    public override string CreateWriteThroughBody(
        SerializationCodeGenContext context,
        string vtableVariableName)
    {
        FlatSharpInternal.Assert(this.ItemTypeModel.PhysicalLayout.Length == 1, "Writethrough not expected for wide vtable items");
        var adjustedContext = context with
        {
            OffsetVariableName = "absoluteLocation",
        };

        return $@"
            {GetFindFieldLocationBlock(adjustedContext.OffsetVariableName, context.OffsetVariableName, vtableVariableName)}
            {adjustedContext.GetSerializeInvocation(this.PropertyInfo.PropertyType)};";
    }

    private string CreateSingleWidthReadItemBody(
        ParserCodeGenContext context,
        string vtableVariableName)
    {
        var adjustedContext = context with
        {
            OffsetVariableName = "absoluteLocation",
        };

        return $@"
            {GetFindFieldLocationBlock(adjustedContext.OffsetVariableName, context.OffsetVariableName, vtableVariableName)}
            return {adjustedContext.GetParseInvocation(this.PropertyInfo.PropertyType)};";
    }

    private string CreateWideReadItemBody(
        ParserCodeGenContext context,
        string vtableVariableName)
    {
        int items = this.ItemTypeModel.PhysicalLayout.Length;

        List<string> relativeOffsets = new();
        List<string> absoluteLocations = new();

        for (int i = 0; i < items; ++i)
        {
            int idx = this.Index + i;

            relativeOffsets.Add($@"
                int relativeOffset{i} = {vtableVariableName}.OffsetOf<{context.InputBufferTypeName}>({context.InputBufferVariableName}, {idx});
                bool isZero{i} = relativeOffset{i} == 0;
                allZero &= isZero{i};

                {StrykerSuppressor.SuppressNextLine("assignment")}
                anyZero |= isZero{i};
            ");

            absoluteLocations.Add($"relativeOffset{i} + {context.OffsetVariableName}");
        }

        var adjustedContext = context with
        {
            OffsetVariableName = "absoluteLocations",
            IsOffsetByRef = true,
        };

        return $@"
            bool allZero = true;
            bool anyZero = false;

            {string.Join("\r\n", relativeOffsets)}

            if (allZero)
            {{
                {GetNotPresentStatement()}
            }}

            if (anyZero)
            {{
                {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidData_UnionOnlyPartiallyPresent)}(""{this.FriendlyName}"");
            }}

            var absoluteLocations = ({string.Join(", ", absoluteLocations)});
            return {adjustedContext.GetParseInvocation(this.PropertyInfo.PropertyType)};";
    }

    private string GetNotPresentStatement()
    {
        if (this.IsRequired)
        {
            return $"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidData_RequiredPropertyNotSet)}(\"{this.FriendlyName}\");";
        }
        else
        {
            return $"return {this.DefaultValueLiteral};";
        }
    }

    private string GetFindFieldLocationBlock(
        string locationVariableName,
        string offsetVariableName,
        string vtableVariableName)
    {
        return $@"
            int {locationVariableName};
            {{
                int relativeOffset = {vtableVariableName}.OffsetOf(buffer, {this.Index});
                if (relativeOffset == 0)
                {{
                    {this.GetNotPresentStatement()}
                }}

                {locationVariableName} = {offsetVariableName} + relativeOffset;
            }}
        ";
    }
}
