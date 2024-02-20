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

namespace FlatSharp.TypeModel;

internal static partial class FlatBufferVectorHelpers
{
    /// <summary>
    /// Creates a Lazy vector for non-unions.
    /// </summary>
    public static (string classDef, string className) CreateLazyVector(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.Lazy, "Expecting lazy");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Lazy);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);
        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("Lazy [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IFlatBufferDeserializedVector
        , IPoolableObject
        where TInputBuffer : IInputBuffer
    {
        private int {{context.OffsetVariableName}};
        private int count;
        private {{context.InputBufferTypeName}} {{context.InputBufferVariableName}};
        private TableFieldContext {{context.TableFieldContextVariableName}};
        private short {{context.RemainingDepthVariableName}};

        private int inUse = 1;
        
#pragma warning disable CS8618
        private {{className}}() { }
#pragma warning restore CS8618

        public static {{className}}<TInputBuffer> GetOrCreate(
            TInputBuffer memory,
            int offset,
            short remainingDepth,
            TableFieldContext fieldContext)
        {
            if (!ObjectPool.TryGet<{{className}}<TInputBuffer>>(out var item))
            {
                item = new {{className}}<TInputBuffer>();
            }

            item.count = (int)memory.ReadUInt(offset);
            item.offset = offset + sizeof(uint);
            item.{{context.InputBufferVariableName}} = memory;
            item.{{context.TableFieldContextVariableName}} = fieldContext;
            item.{{context.RemainingDepthVariableName}} = remainingDepth;

            item.inUse = 1;

            return item;
        }

        public {{baseTypeName}} this[int index]
        {
            get => this.SafeParseItem(index);
            set
            {
                {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);
                this.UnsafeWriteThrough(index, value);
            }
        }

        public int Count => this.count;

        public FlatBufferDeserializationOption DeserializationOption => {{nameof(FlatBufferDeserializationOption)}}.{{context.Options.DeserializationOption}};

        {{StrykerSuppressor.ExcludeFromCodeCoverage()}}
        public void ReturnToPool(bool force = false)
        {
            if (this.DeserializationOption.ShouldReturnToPool(force))
            {
                if (System.Threading.Interlocked.Exchange(ref inUse, 0) == 1)
                {
                    this.count = -1;
                    this.offset = -1;

                    this.{{context.InputBufferVariableName}} = default({{context.InputBufferTypeName}})!;
                    this.{{context.TableFieldContextVariableName}} = null!;
                    this.{{context.RemainingDepthVariableName}} = -1;

                    ObjectPool.Return(this);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} SafeParseItem(int index)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);
            return this.UnsafeParseItem(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} UnsafeParseItem(int index)
        {
            int {{context.OffsetVariableName}} = this.offset + ({{GetEfficientMultiply(inlineSize, "index")}});
            return {{context.GetParseInvocation(itemTypeModel.ClrType)}};
        }

        {{CreateWriteThroughMethod(itemTypeModel, inlineSize, context, isEverWriteThrough)}}
        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}
        {{CreateImmutableVectorMethods(itemTypeModel)}}
        {{CreateIFlatBufferDeserializedVectorMethods(inlineSize, context.InputBufferVariableName, context.OffsetVariableName, "SafeParseItem")}}
    }
"""";

        return (classDef, className);
    }
}
