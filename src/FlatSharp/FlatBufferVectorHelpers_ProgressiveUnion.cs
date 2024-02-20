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

using System.IO;

namespace FlatSharp.TypeModel;

internal static partial class FlatBufferVectorHelpers
{
    public static (string classDef, string className) CreateProgressiveUnionVector(
        ITypeModel itemTypeModel,
        ParserCodeGenContext context)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.Progressive, "Expecting progressive");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Progressive);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);

        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";
        int chunkSize = itemTypeModel.ClrType.IsValueType ? 8 : 32;

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("Progressive [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IPoolableObject
        where TInputBuffer : IInputBuffer
    {
        private const uint ChunkSize = {{chunkSize}};

        private int discriminatorVectorOffset;
        private int offsetVectorOffset;
        private int count;
        private {{context.InputBufferTypeName}} {{context.InputBufferVariableName}};
        private TableFieldContext {{context.TableFieldContextVariableName}};
        private short {{context.RemainingDepthVariableName}};
        private {{derivedTypeName}}{{nullableReference}}[]?[] items;
        private int inUse = 1;
        
#pragma warning disable CS8618
        private {{className}}() { }
#pragma warning restore CS8618

        public static {{className}}<TInputBuffer> GetOrCreate(
            TInputBuffer memory,
            ref (int offset0, int offset1) offsets,
            short remainingDepth,
            TableFieldContext fieldContext)
        {
            if (!ObjectPool.TryGet<{{className}}<TInputBuffer>>(out var item))
            {
                item = new {{className}}<TInputBuffer>();
            }

            int dvo = offsets.offset0;
            int ovo = offsets.offset1;

            uint discriminatorCount = memory.ReadUInt(dvo);
            uint offsetCount = memory.ReadUInt(ovo);

            if (discriminatorCount != offsetCount)
            {
                {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.InvalidData_UnionVectorMismatchedLength)}}();
            }

            item.count = (int)offsetCount;
            item.discriminatorVectorOffset = dvo + sizeof(int);
            item.offsetVectorOffset = ovo + sizeof(int);
            
            item.{{context.InputBufferVariableName}} = memory;
            item.{{context.TableFieldContextVariableName}} = fieldContext;
            item.{{context.RemainingDepthVariableName}} = remainingDepth;

            {{StrykerSuppressor.SuppressNextLine()}}
            int progressiveMinLength = (int)(item.count / ChunkSize) + 1;
            item.items = System.Buffers.ArrayPool<{{derivedTypeName}}{{nullableReference}}[]?>.Shared.Rent(progressiveMinLength);
            item.inUse = 1;

            return item;
        }

        public {{baseTypeName}} this[int index]
        {
            get => this.ProgressiveGet(index);
            set => this.ProgressiveSet(index, value);
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
                    this.discriminatorVectorOffset = -1;
                    this.offsetVectorOffset = -1;

                    this.{{context.InputBufferVariableName}} = default({{context.InputBufferTypeName}})!;
                    this.{{context.TableFieldContextVariableName}} = null!;
                    this.{{context.RemainingDepthVariableName}} = -1;

                    var items = this.items;
                    this.items = null!;

                    if (items is null)
                    {
                        return;
                    }

                    for (int i = 0; i < items.Length; ++i)
                    {
                        var block = items[i];

                        if (block is null)
                        {
                            continue;
                        }

                        {{(
                            // return poolable reference types at this point.
                            !itemTypeModel.ClrType.IsValueType && typeof(IPoolableObject).IsAssignableFrom(itemTypeModel.ClrType)
                            ? $$"""
                                for (int j = 0; j < block.Length; ++j)
                                {
                                    block[j]?.ReturnToPool(true);
                                }
                                """
                            : string.Empty
                        )}}

                        System.Buffers.ArrayPool<{{derivedTypeName}}{{nullableReference}}>.Shared.Return(block, true);
                        items[i] = null;
                    }

                    System.Buffers.ArrayPool<{{derivedTypeName}}{{nullableReference}}[]?>.Shared.Return(items);
                    ObjectPool.Return(this);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetAddress(uint index, out uint rowIndex, out uint colIndex)
        {
            rowIndex = index / ChunkSize;
            colIndex = index % ChunkSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}}{{nullableReference}}[] GetOrCreateRow({{derivedTypeName}}{{nullableReference}}[]?[] items, uint rowIndex)
        {
            return items[rowIndex] ?? this.CreateRow(items, rowIndex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private {{derivedTypeName}}{{nullableReference}}[] CreateRow({{derivedTypeName}}{{nullableReference}}[]?[] items, uint rowIndex)
        {
            var row = System.Buffers.ArrayPool<{{derivedTypeName}}{{nullableReference}}>.Shared.Rent((int)ChunkSize);
            items[rowIndex] = row;

             {{ // For value types -- we can't rely on null to tell
                // us if the value is allocated or not, so just greedily
                // allocate the whole chunk. Chunks are relatively
                // small, so the overhead here is not enormous, and there
                // is no extra allocation since this is a value type.
                // Unchecked is considered safe here since we have already
                // validated indexes.
                If(itemTypeModel.ClrType.IsValueType,
                  $$"""
                    unchecked
                    {
                        int absoluteStartIndex = (int)({{GetEfficientMultiply(chunkSize, "rowIndex")}});
                        int copyCount = {{chunkSize}};
                        int remainingItems = this.count - absoluteStartIndex;

                        {{StrykerSuppressor.SuppressNextLine("equality")}}
                        if (remainingItems < {{chunkSize}})
                        {
                            copyCount = remainingItems;
                        }

                        for (int i = 0; i < copyCount; ++i)
                        {
                            row[i] = this.UnsafeParseItem(i + absoluteStartIndex);
                        }
                    }
                    """
            )}}

            return row;
        }

        private {{derivedTypeName}} ProgressiveGet(int index)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);

            uint uindex = (uint)index;
            GetAddress(uindex, out uint rowIndex, out uint colIndex);

            var items = this.items;
            var row = this.GetOrCreateRow(items, rowIndex);
            var item = row[colIndex];

            {{  // Initialize the reference type if null.
                IfNot(itemTypeModel.ClrType.IsValueType,
                $$"""
                    if (item is null)
                    {
                        item = this.UnsafeParseItem(index);
                        row[colIndex] = item;
                    }
                  """
            )}}

            return item!;
        }

        private void ProgressiveSet(int index, {{baseTypeName}} value)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);
            {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.NotMutable_DeserializedVector)}}();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} UnsafeParseItem(int index)
        {
            var {{context.OffsetVariableName}} = (this.discriminatorVectorOffset + index, this.offsetVectorOffset + ({{GetEfficientMultiply(sizeof(uint), "index")}}));
            return {{context.GetParseInvocation(itemTypeModel.ClrType)}};
        }

        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}
        {{CreateImmutableVectorMethods(itemTypeModel)}}
    }
"""";

        return (classDef, className);
    }
}
