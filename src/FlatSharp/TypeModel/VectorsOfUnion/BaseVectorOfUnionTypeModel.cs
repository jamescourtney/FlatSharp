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

using System.Collections.Immutable;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a vector of union type model.
/// </summary>
public abstract class BaseVectorOfUnionTypeModel : RuntimeTypeModel
{
    internal BaseVectorOfUnionTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
    {
        this.ItemTypeModel = null!;
    }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Vector;

    /// <summary>
    /// Layout of the vtable.
    /// </summary>
    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout =>
        new PhysicalLayoutElement[]
        {
            new PhysicalLayoutElement(sizeof(uint), sizeof(uint)),  // discriminator vector
            new PhysicalLayoutElement(sizeof(uint), sizeof(uint)),  // offset vector
        }.ToImmutableArray();

    /// <summary>
    /// Vectors are arbitrary in length.
    /// </summary>
    public override bool IsFixedSize => false;

    /// <summary>
    /// Vectors can be part of tables.
    /// </summary>
    public override bool IsValidTableMember => true;

    /// <summary>
    /// Gets the type model for this vector's elements.
    /// </summary>
    public ITypeModel ItemTypeModel { get; private set; }

    /// <summary>
    /// The name of the length property of this vector type.
    /// </summary>
    public abstract string LengthPropertyName { get; }

    /// <summary>
    /// Vectors are by-reference.
    /// </summary>
    public override bool SerializesInline => false;

    /// <summary>
    /// Defer to the union type under us as to whether context is needed.
    /// </summary>
    public override TableFieldContextRequirements TableFieldContextRequirements => this.ItemTypeModel.TableFieldContextRequirements | TableFieldContextRequirements.Parse;

    public override IEnumerable<ITypeModel> Children => new[] { this.ItemTypeModel };

    protected virtual string Indexer(string index) => $"[{index}]";

    public override bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel)
    {
        typeModel = this.ItemTypeModel;
        return true;
    }

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        // 2 vectors.
        int baseSize = 2 * (sizeof(int) + SerializationHelpers.GetMaxPadding(sizeof(int)));

        var itemContext = context with
        {
            ValueVariableName = "current",
        };

        string body =
        $@"
            int count = {context.ValueVariableName}.{this.LengthPropertyName};
            int length = {baseSize} + (count * (sizeof(byte) + sizeof(int)));

            for (int i = 0; i < count; ++i)
            {{
                var {itemContext.ValueVariableName} = {context.ValueVariableName}{this.Indexer("i")};
                length += {itemContext.GetMaxSizeInvocation(this.ItemTypeModel.ClrType)};
            }}

            return length;";

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        var type = this.ClrType;
        var itemTypeModel = this.ItemTypeModel;

        var innerContext = context with
        {
            ValueVariableName = "current",
            OffsetVariableName = "tuple"
        };

        string body = $@"
            int count = {context.ValueVariableName}.{this.LengthPropertyName};
            long discriminatorVectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}(sizeof(byte), count, sizeof(byte));
            {context.TargetVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.OffsetVariableName}.offset0, discriminatorVectorOffset);
            {context.TargetVariableName}.WriteInt32(discriminatorVectorOffset, count);
            discriminatorVectorOffset += sizeof(int);

            long offsetVectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}(sizeof(int), count, sizeof(int));
            {context.TargetVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.OffsetVariableName}.offset1, offsetVectorOffset);
            {context.TargetVariableName}.WriteInt32(offsetVectorOffset, count);
            offsetVectorOffset += sizeof(int);

            for (int i = 0; i < count; ++i)
            {{
                var current = {context.ValueVariableName}{this.Indexer("i")};

                var tuple = (discriminatorVectorOffset, offsetVectorOffset);
                {innerContext.GetSerializeInvocation(itemTypeModel.ClrType)};

                discriminatorVectorOffset++;
                offsetVectorOffset += sizeof(int);
            }}";

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        string parameters = parameters = $"{context.ItemVariableName}, {context.MethodNameMap[this.ItemTypeModel.ClrType]}";

        string body = $"return {nameof(VectorCloneHelpers)}.{nameof(VectorCloneHelpers.CloneVectorOfUnion)}<{this.ItemTypeModel.GetCompilableTypeName()}>({parameters});";
        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public sealed override void Initialize()
    {
        base.Initialize();

        this.ItemTypeModel = this.typeModelContainer.CreateTypeModel(this.OnInitialize());

        FlatSharpInternal.Assert(this.ItemTypeModel.SchemaType == FlatBufferSchemaType.Union, "Union vectors can't contain non-union elements.");
    }

    /// <summary>
    /// Returns the type of union.
    /// </summary>
    public abstract Type OnInitialize();
}
