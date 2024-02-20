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
using System.Linq;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a vector type model.
/// </summary>
public abstract class BaseVectorTypeModel : RuntimeTypeModel
{
    // count of items + padding(uoffset_t);
    protected static readonly int VectorMinSize = sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint));

    internal BaseVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
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
        new PhysicalLayoutElement[] { new PhysicalLayoutElement(sizeof(uint), sizeof(uint)) }.ToImmutableArray();

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
    /// Vectors don't intrinsically care about this, but the elements may.
    /// </summary>
    public override TableFieldContextRequirements TableFieldContextRequirements =>
        this.ItemTypeModel.TableFieldContextRequirements | TableFieldContextRequirements.Parse;

    public override IEnumerable<ITypeModel> Children => new[] { this.ItemTypeModel };

    /// <summary>
    /// Gets the size of each member of this vector, with padding for alignment.
    /// </summary>
    public int PaddedMemberInlineSize
    {
        get
        {
            int itemInlineSize = this.ItemTypeModel.PhysicalLayout[0].InlineSize;
            int itemAlignment = this.ItemTypeModel.PhysicalLayout[0].Alignment;

            return itemInlineSize + SerializationHelpers.GetAlignmentError(itemInlineSize, itemAlignment);
        }
    }

    public override bool TryGetUnderlyingVectorType([NotNullWhen(true)] out ITypeModel? typeModel)
    {
        typeModel = this.ItemTypeModel;
        return true;
    }

    public sealed override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        string lengthProperty = $"{context.ValueVariableName}.{this.LengthPropertyName}";

        string body;
        if (this.ItemTypeModel.IsFixedSize)
        {
            // Constant size items. We can reduce these reasonably well.
            body = $"return {VectorMinSize + SerializationHelpers.GetMaxPadding(this.ItemTypeModel.PhysicalLayout[0].Alignment)} + ({this.PaddedMemberInlineSize} * {lengthProperty});";
        }
        else
        {
            var itemContext = context with
            {
                ValueVariableName = "current",
            };

            string loopBody = $@"
                {this.GetThrowIfNullStatement("current")}
                runningSum += {itemContext.GetMaxSizeInvocation(this.ItemTypeModel.ClrType)};";

            body = $@"
                int count = {lengthProperty};
                int runningSum = {this.MaxInlineSize + VectorMinSize};
                {this.CreateLoop(context.Options, context.ValueVariableName, "count", "current", loopBody)}

                return runningSum;";
        }

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        var type = this.ClrType;
        var itemTypeModel = this.ItemTypeModel;

        var innerLoopContext = context with
        {
            ValueVariableName = "current",
            OffsetVariableName = "vectorOffset"
        };

        string loopBody = $@"
            {this.GetThrowIfNullStatement("current")}
            {innerLoopContext.GetSerializeInvocation(itemTypeModel.ClrType)};
            vectorOffset += {this.PaddedMemberInlineSize};";

        string body = $@"
            int count = {context.ValueVariableName}.{this.LengthPropertyName};
            int vectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}({itemTypeModel.PhysicalLayout[0].Alignment}, count, {this.PaddedMemberInlineSize});
            {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, vectorOffset);
            {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, vectorOffset);
            vectorOffset += sizeof(int);

            {this.CreateLoop(context.Options, context.ValueVariableName, "count", "current", loopBody)}";

        return new CodeGeneratedMethod(body);
    }

    /// <summary>
    /// Creates a loop that executes the given body.
    /// </summary>
    protected abstract string CreateLoop(
        FlatBufferSerializerOptions options,
        string vectorVariableName,
        string numberofItemsVariableName,
        string expectedVariableName,
        string body);

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        string parameters;
        if (this.ItemTypeModel.ClrType.IsValueType)
        {
            parameters = context.ItemVariableName;
        }
        else
        {
            parameters = $"{context.ItemVariableName}, {context.MethodNameMap[this.ItemTypeModel.ClrType]}";
        }

        string body = $"return {nameof(VectorCloneHelpers)}.{nameof(VectorCloneHelpers.Clone)}<{this.ItemTypeModel.GetCompilableTypeName()}>({parameters});";
        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public sealed override void Initialize()
    {
        base.Initialize();
        this.ItemTypeModel = this.typeModelContainer.CreateTypeModel(this.OnInitialize());
    }

    public override void Validate()
    {
        FlatSharpInternal.Assert(
            this.ItemTypeModel.IsValidVectorMember,
            $"Type '{this.ItemTypeModel.GetCompilableTypeName()}' is not a valid vector member.");

        FlatSharpInternal.Assert(
            this.ItemTypeModel.PhysicalLayout.Length == 1,
            $"Vectors may only store vtable layouts with one item. Consider a custom vector type model for other vector kinds.");

        base.Validate();
    }

    /// <summary>
    /// Returns the underlying type of this vector.
    /// </summary>
    public abstract Type OnInitialize();

    protected string GetThrowIfNullStatement(string variableName)
    {
        if (this.ItemTypeModel.IsNonNullableClrValueType())
        {
            // can't be null.
            return string.Empty;
        }

        return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({variableName});";
    }

    /// <summary>
    /// Validates that write through is valid.
    /// </summary>
    /// <returns>True if the given model is ever used in a write through context.</returns>
    internal static bool ValidateWriteThrough(
        bool writeThroughSupported,
        ITypeModel model,
        TypeModelContainer container,
        IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> contexts)
    {
        // Helper to validate writethrough for a single model.
        static bool ValidateModel(
            ITypeModel model,
            IReadOnlyDictionary<ITypeModel, HashSet<TableFieldContext>> contexts,
            bool writeThroughSupported)
        {
            if (contexts.TryGetValue(model, out var fieldsForModel))
            {
                var firstWriteThrough = fieldsForModel.Where(x => x.WriteThrough).FirstOrDefault();
                if (firstWriteThrough is not null && !writeThroughSupported)
                {
                    throw new InvalidFlatBufferDefinitionException($"Field '{firstWriteThrough.FullName}' declares the WriteThrough option. WriteThrough is only supported for IList vectors.");
                }

                return firstWriteThrough is not null;
            }

            return false;
        }

        if (ValidateModel(model, contexts, writeThroughSupported))
        {
            return true;
        }

        if (model.ClrType.IsValueType)
        {
            // Some vectors are structs: Memory<T> and UnityNativeArray<T>. We have
            // to reverse those back to a type here and then see if *those* are writethrough.
            Type nullable = typeof(Nullable<>).MakeGenericType(model.ClrType);
            if (container.TryGetTypeModel(nullable, out var nullableModel))
            {
                return ValidateModel(nullableModel, contexts, writeThroughSupported);
            }
        }

        return false;
    }
}
