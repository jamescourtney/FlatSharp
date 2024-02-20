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
using System.Linq;
using System.Threading;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines a vector type model.
/// </summary>
public class UnionTypeModel : RuntimeTypeModel
{
    private ITypeModel[] memberTypeModels;

    internal UnionTypeModel(Type unionType, TypeModelContainer provider) : base(unionType, provider)
    {
        this.memberTypeModels = null!;
    }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Union;

    /// <summary>
    /// Unions are "double-wide" vtable items.
    /// </summary>
    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout =>
        new[]
        {
            new PhysicalLayoutElement(sizeof(byte), sizeof(byte)),
            new PhysicalLayoutElement(sizeof(uint), sizeof(uint))
        }.ToImmutableArray();

    /// <summary>
    /// Unions can be invariant on the parse path, depending on their children.
    /// </summary>
    public override bool IsParsingInvariant => this.Children.All(x => x.IsParsingInvariant);

    /// <summary>
    /// Unions are not fixed because they contain tables.
    /// </summary>
    public override bool IsFixedSize => false;

    /// <summary>
    /// Unions can be part of tables.
    /// </summary>
    public override bool IsValidTableMember => true;

    /// <summary>
    /// Unions are pointers.
    /// </summary>
    public override bool SerializesInline => false;

    /// <summary>
    /// Gets the type model for this union's members. Index 0 corresponds to discriminator 1.
    /// </summary>
    public ITypeModel[] UnionElementTypeModel => this.memberTypeModels;

    /// <summary>
    /// We need it to pass through.
    /// </summary>
    public override TableFieldContextRequirements TableFieldContextRequirements =>
        this.memberTypeModels.Select(x => x.TableFieldContextRequirements).Aggregate(TableFieldContextRequirements.None, (a, b) => a | b);

    /// <summary>
    /// Unions have an implicit dependency on <see cref="byte"/> for the discriminator.
    /// </summary>
    public override IEnumerable<ITypeModel> Children => this.memberTypeModels.Concat(new[] { this.typeModelContainer.CreateTypeModel(typeof(byte)) });

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        List<string> switchCases = new List<string>();
        for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
        {
            var unionMember = this.UnionElementTypeModel[i];
            int unionIndex = i + 1;

            var itemContext = context with
            {
                ValueVariableName = $"{context.ValueVariableName}.Item{unionIndex}",
            };

            string @case =
$@"
                case {unionIndex}:
                    return {sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint))} + {itemContext.GetMaxSizeInvocation(unionMember.ClrType)};";

            switchCases.Add(@case);
        }
        string discriminatorPropertyName = nameof(FlatBufferUnion<int, int>.Discriminator);

        string body =
$@"
            byte discriminator = {context.ValueVariableName}.{discriminatorPropertyName};
            switch (discriminator)
            {{
                {string.Join("\r\n", switchCases)}
                default:
                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.GGCTN()}>(discriminator);
                    return 0;
            }}
";
        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        List<string> switchCases = new List<string>();

        (string? extraClass, string createNew) = GetUnionHelperClass(context);

        for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
        {
            var unionMember = this.UnionElementTypeModel[i];
            int unionIndex = i + 1;

            string inlineAdjustment = string.Empty;
            if (unionMember.SerializesInline)
            {
                inlineAdjustment = $"offsetLocation += buffer.{nameof(InputBufferExtensions.ReadUOffset)}(offsetLocation);";
            }

            var itemContext = context with
            {
                OffsetVariableName = "offsetLocation",
                IsOffsetByRef = false,
            };

            string @case =
$@"
                case {unionIndex}:
                    {inlineAdjustment}
                    return {createNew}({itemContext.GetParseInvocation(unionMember.ClrType)});
";
            switchCases.Add(@case);
        }

        string body = $@"
            byte discriminator = {context.InputBufferVariableName}.{nameof(IInputBuffer.ReadByte)}({context.OffsetVariableName}.offset0);
            int offsetLocation = {context.OffsetVariableName}.offset1;

            switch (discriminator)
            {{
                {string.Join("\r\n", switchCases)}
                default:
                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.GGCTN()}>(discriminator);
                    return default({this.GGCTN()});
            }}
        ";

        return new CodeGeneratedMethod(body) { ClassDefinition = extraClass };
    }

    private (string? classDef, string createNewUnion) GetUnionHelperClass(ParserCodeGenContext context)
    {
        if (this.ClrType.IsValueType || !typeof(IPoolableObject).IsAssignableFrom(this.ClrType))
        {
            // Nothing special for value-type or non-poolable unions.
            return (null, $"new {this.GetGlobalCompilableTypeName()}");
        }

        string className = "unionReader_" + Guid.NewGuid().ToString("n");

        List<string> getOrCreates = new();
        List<string> returnToPoolCases = new();

        for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
        {
            int unionIndex = i + 1; // unions start at 1.
            string itemType = this.UnionElementTypeModel[i].GetGlobalCompilableTypeName();

            getOrCreates.Add($@"
                public static {className} GetOrCreate({itemType} value)
                {{
                    if (!{typeof(ObjectPool).GetGlobalCompilableTypeName()}.{nameof(ObjectPool.TryGet)}<{className}>(out var union))
                    {{
                        union = new {className}();
                    }}

                    union.discriminator = {unionIndex};
                    union.Item{unionIndex} = value;
                    union.isAlive = 1;

                    return union;
                }}
            ");

            string recursiveReturn = string.Empty;
            if (typeof(IPoolableObject).IsAssignableFrom(this.UnionElementTypeModel[i].ClrType))
            {
                recursiveReturn = $"this.Item{unionIndex}?.ReturnToPool(true);";
            }

            returnToPoolCases.Add($@"
                    case {unionIndex}:
                    {{
                        {recursiveReturn}
                        this.Item{unionIndex} = default({itemType})!;
                    }}
                    break;
                ");
        }

        string returnCondition = string.Empty;
        if (!context.Options.Lazy)
        {
            returnCondition = "if (unsafeForce)";
        }

        // Reference type unions are much more special!
        string extraClass = $@"
            private sealed class {className} : {this.ClrType.GetGlobalCompilableTypeName()}
            {{
                private int isAlive;

                {string.Join("\r\n", getOrCreates)}

                public override void ReturnToPool(bool unsafeForce = false)
                {{
                    {returnCondition}
                    {{
                        int alive = {typeof(Interlocked).GetGlobalCompilableTypeName()}.Exchange(ref this.isAlive, 0);
                        if (alive > 0)
                        {{
                            switch (base.discriminator)
                            {{
                                {string.Join("\r\n", returnToPoolCases)}
                            }}
                            
                            base.discriminator = -1;
                            {typeof(ObjectPool).GetGlobalCompilableTypeName()}.{nameof(ObjectPool.Return)}(this);
                        }}
                    }}
                }}
            }}
        ";

        return (extraClass, $"{className}.GetOrCreate");
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        List<string> switchCases = new List<string>();
        for (int i = 0; i < this.UnionElementTypeModel.Length; ++i)
        {
            var elementModel = this.UnionElementTypeModel[i];
            var unionIndex = i + 1;

            string inlineAdjustment;

            if (elementModel.SerializesInline)
            {
                // Structs are generally written in-line, with the exception of unions.
                // So, we need to do the normal allocate space dance here, since we're writing
                // a pointer to a struct.
                inlineAdjustment =$@"
                    var writeOffset = context.{nameof(SerializationContext.AllocateSpace)}({elementModel.PhysicalLayout.Single().InlineSize}, {elementModel.PhysicalLayout.Single().Alignment});
                    {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}(span, {context.OffsetVariableName}.offset1, writeOffset);";
            }
            else
            {
                inlineAdjustment = $"var writeOffset = {context.OffsetVariableName}.offset1;";
            }

            var caseContext = context with
            {
                ValueVariableName = $"{context.ValueVariableName}.Item{unionIndex}",
                OffsetVariableName = "writeOffset",
                IsOffsetByRef = false,
            };

            string @case =
$@"
                case {unionIndex}:
                {{
                    {inlineAdjustment}
                    {caseContext.GetSerializeInvocation(elementModel.ClrType)};
                }}
                break;";

            switchCases.Add(@case);
        }

        string serializeBlock = $@"
            byte discriminatorValue = {context.ValueVariableName}.Discriminator;
            {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteByte)}(
                {context.SpanVariableName}, 
                discriminatorValue, 
                {context.OffsetVariableName}.offset0);

            switch (discriminatorValue)
            {{
                {string.Join("\r\n", switchCases)}
                default: 
                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.GGCTN()}>(discriminatorValue);
                    break;
            }}";

        return new CodeGeneratedMethod(serializeBlock);
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        List<string> switchCases = new List<string>();

        for (int i = 0; i < this.memberTypeModels.Length; ++i)
        {
            int discriminator = i + 1;
            string cloneMethod = context.MethodNameMap[this.memberTypeModels[i].ClrType];
            switchCases.Add($@"
                case {discriminator}:
                    return new {this.GetGlobalCompilableTypeName()}({cloneMethod}({context.ItemVariableName}.Item{discriminator}));
                ");
        }

        string body = $@"
            byte discriminator = {context.ItemVariableName}.{nameof(IFlatBufferUnion.Discriminator)};
            switch (discriminator)
            {{
                {string.Join("\r\n", switchCases)}
                default: 
                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_InvalidUnionDiscriminator)}<{this.GGCTN()}>(discriminator);
                    return default({this.GGCTN()});
            }}
            ";

        return new CodeGeneratedMethod(body);
    }

    public override void Initialize()
    {
        // Look for the actual FlatBufferUnion.
        Type unionType = this.ClrType.GetInterfaces()
            .Single(x => x != typeof(IFlatBufferUnion) && typeof(IFlatBufferUnion).IsAssignableFrom(x));

        this.memberTypeModels = unionType.GetGenericArguments().Select(this.typeModelContainer.CreateTypeModel).ToArray();
    }

    public override void Validate()
    {
        base.Validate();
        HashSet<Type> uniqueTypes = new HashSet<Type>();

        foreach (var item in this.memberTypeModels)
        {
            FlatSharpInternal.Assert(
                item.IsValidUnionMember,
                $"Unions may not store '{item.GetCompilableTypeName()}'.");

            FlatSharpInternal.Assert(
                uniqueTypes.Add(item.ClrType),
                $"Unions must consist of unique types. The type '{item.GetCompilableTypeName()}' was repeated.");
        }
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        // improve?
        return this.ClrType.GetGlobalCompilableTypeName();
    }
}
