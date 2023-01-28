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
    public static (string classDef, string className) CreateProgressiveVector(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.Progressive, "Expecting progressive");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Progressive);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.MethodNameResolver, context.Options.DeserializationOption, context.InputBufferTypeName);

        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";

        string classDef =
$$""""
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IFlatBufferDeserializedVector
        , IPoolableObject
        , IVisitable{{(itemTypeModel.ClrType.IsValueType ? "Value" : "Reference")}}Vector<{{baseTypeName}}>
        {{IfNot(itemTypeModel.ClrType.IsValueType, $", IIndexedVectorSource<{baseTypeName}>")}}
        where TInputBuffer : IInputBuffer
    {
        private const uint ChunkSize = 32;

        private int {{context.OffsetVariableName}};
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
            item.items = System.Buffers.ArrayPool<{{derivedTypeName}}{{nullableReference}}[]?>.Shared.Rent((int)((item.count / ChunkSize) + 1));
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

            {{(
                // For value types -- we can't rely on null to tell
                // us if the value is allocated or not, so just greedily
                // allocate the whole chunk. Chunks are relatively
                // small, so the overhead here is not enormous, and there
                // is no extra allocation since this is a value type.
                itemTypeModel.ClrType.IsValueType
                ? $$"""
                    int count = this.count;
                    int rowStartIndex = (int)(ChunkSize * rowIndex);

                    for (int i = 0; i < ChunkSize; ++i)
                    {
                        int targetIndex = rowStartIndex + i;
                        if (targetIndex >= count)
                        {
                            break;
                        }
                        row[i] = this.UnsafeParseItem(targetIndex);
                    }
                    """
                : string.Empty
            )}}

            return row;
        }

        private {{derivedTypeName}} ProgressiveGet(int index) => InlineProgressiveGet(index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} InlineProgressiveGet(int index)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);

            uint uindex = (uint)index;
            GetAddress(uindex, out uint rowIndex, out uint colIndex);

            var items = this.items;
            var row = this.GetOrCreateRow(items, rowIndex);
            var item = row[colIndex];

            {{(
                // Initialize the reference type if null.
                itemTypeModel.ClrType.IsValueType
              ? string.Empty
              : $$"""
                    if (item is null)
                    {
                        item = this.UnsafeParseItem(index);
                        row[colIndex] = item;
                    }
                  """
            )}}

            return item!;
        }

        private void ProgressiveSet(int index, {{baseTypeName}} value) => this.InlineProgressiveSet(index, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InlineProgressiveSet(int index, {{baseTypeName}} value)
        {
            {{(
                itemTypeModel.ClrType.IsValueType
              // value types can just be assigned directly.
              ? $$"""
                    {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);
                    uint uindex = (uint)index;
                    GetAddress(uindex, out uint rowIndex, out uint colIndex);
                    var items = this.items;
                    var row = this.GetOrCreateRow(items, rowIndex);
                    row[colIndex] = value;
                    this.WriteThrough(index, value);
                  """
              // Reference type write through happens *within* the object, not the vector.
              : $$"""
                    {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
                  """
            )}}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} UnsafeParseItem(int index)
        {
            int {{context.OffsetVariableName}} = this.offset + ({{inlineSize}} * index);
            return {{context.GetParseInvocation(itemTypeModel.ClrType)}};
        }

        {{CreateWriteThroughMethod(itemTypeModel, inlineSize, context, isEverWriteThrough)}}
        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}
        {{CreateImmutableVectorMethods(itemTypeModel)}}
        {{CreateIFlatBufferDeserializedVectorMethods(inlineSize, context.InputBufferVariableName, context.OffsetVariableName, "ProgressiveGet")}}
        {{CreateVisitorMethods(itemTypeModel, className, baseTypeName, derivedTypeName, "InlineProgressiveGet", "InlineProgressiveSet")}}
    }
"""";

        return (classDef, className);
    }
}
