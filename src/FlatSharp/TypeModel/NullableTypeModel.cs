/*
 * Copyright 2020 James Courtney
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

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a FlatSharp type model for nullable value types.
/// </summary>
public class NullableTypeModel : RuntimeTypeModel
{
    private Type underlyingType;
    private ITypeModel underlyingTypeModel;

    internal NullableTypeModel(TypeModelContainer container, Type type) : base(type, container)
    {
        this.underlyingType = null!;
        this.underlyingTypeModel = null!;
    }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public override FlatBufferSchemaType SchemaType => this.underlyingTypeModel.SchemaType;

    /// <summary>
    /// Layout when in a vtable.
    /// </summary>
    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => this.underlyingTypeModel.PhysicalLayout;

    /// <summary>
    /// The nullable is invariant if it's underlying model is.
    /// </summary>
    public override bool IsParsingInvariant => this.underlyingTypeModel.IsParsingInvariant;

    /// <summary>
    /// Scalars are fixed size.
    /// </summary>
    public override bool IsFixedSize => this.underlyingTypeModel.IsFixedSize;

    /// <summary>
    /// Nullables can be part of Tables.
    /// </summary>
    public override bool IsValidTableMember => this.underlyingTypeModel.IsValidTableMember;

    /// <summary>
    /// Defer to underlying type for serializing.
    /// </summary>
    public override bool SerializesInline
        => this.underlyingTypeModel.SerializesInline;

    /// <summary>
    /// We need context only if our underlying type does.
    /// </summary>
    public override bool SerializeMethodRequiresContext
        => this.underlyingTypeModel.SerializeMethodRequiresContext;

    /// <summary>
    /// Defer to underlying type model about whether we need this.
    /// </summary>
    public override TableFieldContextRequirements TableFieldContextRequirements 
        => this.underlyingTypeModel.TableFieldContextRequirements;

    public override bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel) 
        => this.underlyingTypeModel.TryGetUnderlyingVectorType(out typeModel);

    public override IEnumerable<ITypeModel> Children => new[] { this.underlyingTypeModel };

    /// <summary>
    /// Validates a default value.
    /// </summary>
    public override bool ValidateDefaultValue(object defaultValue) => false;

    public override void Initialize()
    {
        base.Initialize();

        Type? under = Nullable.GetUnderlyingType(this.ClrType);
        FlatSharpInternal.Assert(under is not null, "Nullable type model created for a type that is not Nullable<T>.");

        this.underlyingType = under;
        this.underlyingTypeModel = this.typeModelContainer.CreateTypeModel(this.underlyingType);
    }

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        var ctx = context with { ValueVariableName = $"{context.ValueVariableName}.Value" };

        string body = $@"
            if ({context.ValueVariableName}.HasValue)
            {{
                return {ctx.GetMaxSizeInvocation(this.underlyingType)};
            }}

            return 0;
        ";

        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true
        };
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        string innerParse = context.GetParseInvocation(this.underlyingType);
        string body = $"return {innerParse};";

        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        // NULL FORGIVENESS
        string body = (context with { ValueVariableName = $"{context.ValueVariableName}!.Value" }).GetSerializeInvocation(this.underlyingType);

        return new CodeGeneratedMethod($"{body};")
        {
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        string body = $@"
            if ({context.ItemVariableName}.HasValue)
            {{
                return {context.MethodNameMap[this.underlyingType]}({context.ItemVariableName}.Value);
            }}

            return null;
        ";

        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.ClrType.GetGlobalCompilableTypeName();
    }
}
