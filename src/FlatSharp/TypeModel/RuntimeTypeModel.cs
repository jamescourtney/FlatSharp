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
using System.Linq;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a type model for an object that can be parsed and serialized to/from a FlatBuffer. While generally most useful to the 
/// FlatSharp codegen, this can be used to introspect on how FlatSharp interprets your schema.
/// </summary>
public abstract class RuntimeTypeModel : ITypeModel
{
    protected readonly TypeModelContainer typeModelContainer;

    internal RuntimeTypeModel(Type clrType, TypeModelContainer typeModelContainer)
    {
        this.ClrType = clrType;
        this.typeModelContainer = typeModelContainer;
    }

    /// <summary>
    /// Initializes this runtime type model instance.
    /// </summary>
    public virtual void Initialize() 
    {
    }

    public virtual void Validate()
    {
    }

    /// <summary>
    /// The type of the item in the CLR.
    /// </summary>
    public Type ClrType { get; }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public abstract FlatBufferSchemaType SchemaType { get; }

    /// <summary>
    /// Gets the layout of this type model's vtable.
    /// </summary>
    public abstract ImmutableArray<PhysicalLayoutElement> PhysicalLayout { get; }

    /// <summary>
    /// Indicates if this item is fixed size or not.
    /// </summary>
    public abstract bool IsFixedSize { get; }

    /// <summary>
    /// Indicates if this type model can be part of a struct.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public virtual bool IsValidStructMember => false;

    /// <summary>
    /// Indicates if this type model can be part of a table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public virtual bool IsValidTableMember => false;

    /// <summary>
    /// Indicates if this type model can be part of a vector.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public virtual bool IsValidVectorMember => false;

    /// <summary>
    /// Indicates if this type model can be part of a union.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public virtual bool IsValidUnionMember => false;

    /// <summary>
    /// Indicates if this type model can be a sorted vector key.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public virtual bool IsValidSortedVectorKey => false;

    /// <summary>
    /// When true, indicates that this type model's serialize method writes inline, rather than by offset.
    /// </summary>
    public abstract bool SerializesInline { get; }

    /// <summary>
    /// Serialization method requires a <see cref="SerializationContext"/> argument.
    /// </summary>
    public virtual bool SerializeMethodRequiresContext => true;

    /// <summary>
    /// All methods require a <see cref="TableFieldContext"/> argument.
    /// </summary>
    public virtual TableFieldContextRequirements TableFieldContextRequirements => TableFieldContextRequirements.None;

    /// <summary>
    /// Gets the maximum inline size of this item when padded for alignment, when stored in a table or vector.
    /// </summary>
    public virtual int MaxInlineSize => this.PhysicalLayout.Sum(x => x.InlineSize + SerializationHelpers.GetMaxPadding(x.Alignment));

    /// <summary>
    /// Most things don't have an explicit constructor.
    /// </summary>
    public virtual ConstructorInfo? PreferredSubclassConstructor => null;

    /// <summary>
    /// Gets the children of this type model.
    /// </summary>
    public abstract IEnumerable<ITypeModel> Children { get; }

    public virtual bool IsParsingInvariant => false;

    /// <summary>
    /// Validates a default value.
    /// </summary>
    public virtual bool ValidateDefaultValue(object defaultValue)
    {
        return false;
    }

    /// <summary>
    /// Gets or creates a runtime type model from the given type. This is only used in test cases any more.
    /// </summary>
    internal static ITypeModel CreateFrom(Type type)
    {
        return TypeModelContainer.CreateDefault().CreateTypeModel(type);
    }

    public abstract CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context);

    public abstract CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context);

    public abstract CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context);

    public abstract CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context);

    public virtual string? CreateExtraClasses() => null;

    public virtual string FormatDefaultValueAsLiteral(object? defaultValue) => this.GetTypeDefaultExpression();

    [ExcludeFromCodeCoverage]
    public virtual bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel)
    {
        typeModel = null;
        return false;
    }

    [ExcludeFromCodeCoverage]
    public virtual bool TryGetSpanComparerType([NotNullWhen(true)] out Type? comparerType)
    {
        comparerType = null;
        return false;
    }

    [ExcludeFromCodeCoverage]
    public virtual bool TryGetTableKeyMember([NotNullWhen(true)] out TableMemberModel? tableMember)
    {
        tableMember = null;
        return false;
    }

    [ExcludeFromCodeCoverage]
    public virtual bool TryGetFileIdentifier([NotNullWhen(true)] out string? fileIdentifier)
    {
        fileIdentifier = null;
        return false;
    }

    public virtual void AdjustTableMember(TableMemberModel source)
    {
    }

    public virtual List<(ITypeModel, TableFieldContext)> GetFieldContexts()
    {
        return new();
    }

    public IEnumerable<Type> GetReferencedTypes()
    {
        return new[] { this.ClrType };
    }

    public abstract string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName);
}
