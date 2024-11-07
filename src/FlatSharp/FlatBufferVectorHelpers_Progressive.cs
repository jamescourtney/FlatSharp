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
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);

        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";

        int chunkSize = 32;

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("Progressive [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IFlatBufferDeserializedVector
        where TInputBuffer : IInputBuffer
    {
        private const uint ChunkSize = {{chunkSize}};

        private readonly int {{context.OffsetVariableName}};
        private readonly int count;
        private readonly {{context.InputBufferTypeName}} {{context.InputBufferVariableName}};
        private readonly TableFieldContext {{context.TableFieldContextVariableName}};
        private readonly short {{context.RemainingDepthVariableName}};
        private readonly {{derivedTypeName}}{{nullableReference}}[]?[] items;
        
        public {{className}}(
            TInputBuffer memory,
            int offset,
            short remainingDepth,
            TableFieldContext fieldContext)
        {
            int count = (int)memory.ReadUInt(offset);
            this.count = count;
            this.offset = offset + sizeof(uint);
            this.{{context.InputBufferVariableName}} = memory;
            this.{{context.TableFieldContextVariableName}} = fieldContext;
            this.{{context.RemainingDepthVariableName}} = remainingDepth;

            {{StrykerSuppressor.SuppressNextLine()}}
            int progressiveMinLength = (int)(count / ChunkSize) + 1;
            this.items = new {{derivedTypeName}}{{nullableReference}}[]?[progressiveMinLength];
        }

        public {{baseTypeName}} this[int index]
        {
            get => this.ProgressiveGet(index);
            set => this.ProgressiveSet(index, value);
        }

        public int Count => this.count;
    
        public FlatBufferDeserializationOption DeserializationOption => {{nameof(FlatBufferDeserializationOption)}}.{{context.Options.DeserializationOption}};

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
            var row = new {{derivedTypeName}}{{nullableReference}}[(int)ChunkSize];
            items[rowIndex] = row;

            {{
                // For value types -- we can't rely on null to tell
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

                        int offset = this.offset + ({{GetEfficientMultiply(inlineSize, "absoluteStartIndex")}});
                        for (int i = 0; i < copyCount; ++i)
                        {
                            row[i] = this.UnsafeParseFromOffset(offset);
                            offset += {{inlineSize}};
                        }
                    }
                    """
            )}}

            return row;
        }

        private {{derivedTypeName}} ProgressiveGet(int index)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);

            uint uindex = unchecked((uint)index);
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

        private void ProgressiveSet(int index, {{baseTypeName}} value) => this.InlineProgressiveSet(index, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InlineProgressiveSet(int index, {{baseTypeName}} value)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);
            {{If(itemTypeModel.ClrType.IsValueType,
                 // value types can just be assigned directly.
                 // Note: vector write through is not allowed for reference types, 
                 // so the below will simply throw.
                 $$"""
                    uint uindex = (uint)index;
                    GetAddress(uindex, out uint rowIndex, out uint colIndex);
                    var row = this.GetOrCreateRow(this.items, rowIndex);
                    row[colIndex] = value;
                  """
            )}}
            this.UnsafeWriteThrough(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} UnsafeParseItem(int index)
        {
            int offset = this.offset + ({{GetEfficientMultiply(inlineSize, "index")}});
            return UnsafeParseFromOffset(offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{derivedTypeName}} UnsafeParseFromOffset(int {{context.OffsetVariableName}})
        {
            return {{context.GetParseInvocation(itemTypeModel.ClrType)}};
        }

        {{CreateWriteThroughMethod(itemTypeModel, inlineSize, context, isEverWriteThrough)}}
        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}
        {{CreateImmutableVectorMethods(itemTypeModel)}}
        {{CreateIFlatBufferDeserializedVectorMethods(inlineSize, context.InputBufferVariableName, context.OffsetVariableName, "ProgressiveGet")}}
    }
"""";

        return (classDef, className);
    }
}
