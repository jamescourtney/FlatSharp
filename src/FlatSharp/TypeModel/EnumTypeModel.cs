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

using System.Collections.Immutable;
using FlatSharp.Attributes;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines an Enum FlatSharp type model, which derives from the scalar type model.
/// </summary>
public class EnumTypeModel : RuntimeTypeModel
{
    private ITypeModel underlyingTypeModel;

    internal EnumTypeModel(Type type, TypeModelContainer typeModelContainer) : base(type, typeModelContainer)
    {
        this.underlyingTypeModel = null!;
    }

    public override void Initialize()
    {
        base.Initialize();

        Type enumType = this.ClrType;
        Type underlyingType = Enum.GetUnderlyingType(enumType);

        this.underlyingTypeModel = this.typeModelContainer.CreateTypeModel(underlyingType);

        var attribute = enumType.GetCustomAttribute<FlatBufferEnumAttribute>();
        FlatSharpInternal.Assert(
            attribute is not null,
            $"Enum '{CSharpHelpers.GetCompilableTypeName(enumType)}' is not tagged with a [FlatBufferEnum] attribute.");

        FlatSharpInternal.Assert(
            attribute.DeclaredUnderlyingType == Enum.GetUnderlyingType(enumType),
            $"Enum '{CSharpHelpers.GetCompilableTypeName(enumType)}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{CSharpHelpers.GetCompilableTypeName(Enum.GetUnderlyingType(enumType))}'");
    }

    public override bool IsParsingInvariant => true;

    public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Scalar;

    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingTypeModel.PhysicalLayout;

    public override bool IsFixedSize => this.underlyingTypeModel.IsFixedSize;

    public override bool IsValidStructMember => true;

    public override bool IsValidTableMember => true;

    public override bool IsValidVectorMember => true;

    public override bool SerializesInline => true;

    public override bool SerializeMethodRequiresContext => this.underlyingTypeModel.SerializeMethodRequiresContext;

    public override IEnumerable<ITypeModel> Children => new[] { this.underlyingTypeModel };

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        Type underlyingType = this.underlyingTypeModel.ClrType;
        string underlyingTypeName = CSharpHelpers.GetGlobalCompilableTypeName(underlyingType);

        var innerContext = context with
        {
            ValueVariableName = $"({underlyingTypeName}){context.ValueVariableName}"
        };

        return new CodeGeneratedMethod($"return {innerContext.GetMaxSizeInvocation(underlyingType)};")
        {
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        Type underlyingType = this.underlyingTypeModel.ClrType;
        string body = $@"
            {CSharpHelpers.GetAssertSizeOfStatement(this, $"sizeof({this.underlyingTypeModel.ClrType.GetGlobalCompilableTypeName()})")}
            return ({this.GetCompilableTypeName()}){context.GetParseInvocation(underlyingType)};";

        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        Type underlyingType = this.underlyingTypeModel.ClrType;
        string underlyingTypeName = CSharpHelpers.GetGlobalCompilableTypeName(underlyingType);

        var innerContext = context with
        {
            ValueVariableName = $"({underlyingTypeName}){context.ValueVariableName}"
        };

        string body = $@"
            {CSharpHelpers.GetAssertSizeOfStatement(this, $"sizeof({this.underlyingTypeModel.ClrType.GetGlobalCompilableTypeName()})")}
            {innerContext.GetSerializeInvocation(underlyingType)};
        ";

        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {context.ItemVariableName};")
        {
            IsMethodInline = true,
        };
    }

    public override string FormatDefaultValueAsLiteral(object? defaultValue)
    {
        if (defaultValue is not null)
        {
            object underlyingValue = Convert.ChangeType(defaultValue, this.underlyingTypeModel.ClrType);
            return $"({this.GetCompilableTypeName()}){this.underlyingTypeModel.FormatDefaultValueAsLiteral(underlyingValue)}";
        }

        return base.FormatDefaultValueAsLiteral(defaultValue);
    }

    /// <summary>
    /// Validates a default value.
    /// </summary>
    public override bool ValidateDefaultValue(object defaultValue)
    {
        return defaultValue.GetType() == this.ClrType;
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
