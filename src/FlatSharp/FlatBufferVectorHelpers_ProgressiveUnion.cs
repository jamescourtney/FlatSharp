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
        FlatSharpInternal.Assert(itemTypeModel.ClrType.IsValueType, "expecting value type union");
            
        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Progressive);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);
        int chunkSize = 32;

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("Progressive [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        where TInputBuffer : IInputBuffer
    {
        private const uint ChunkSize = {{chunkSize}};

        private readonly int discriminatorVectorOffset;
        private readonly int offsetVectorOffset;
        private readonly int count;
        private readonly {{context.InputBufferTypeName}} {{context.InputBufferVariableName}};
        private readonly TableFieldContext {{context.TableFieldContextVariableName}};
        private readonly short {{context.RemainingDepthVariableName}};
        private readonly {{derivedTypeName}}?[]?[] items;
        
        public {{className}}(
            TInputBuffer memory,
            ref (int offset0, int offset1) offsets,
            short remainingDepth,
            TableFieldContext fieldContext)
        {
            int dvo = offsets.offset0;
            int ovo = offsets.offset1;

            uint discriminatorCount = memory.ReadUInt(dvo);
            uint offsetCount = memory.ReadUInt(ovo);

            if (discriminatorCount != offsetCount)
            {
                {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.InvalidData_UnionVectorMismatchedLength)}}();
            }

            this.count = (int)offsetCount;
            this.discriminatorVectorOffset = dvo + sizeof(int);
            this.offsetVectorOffset = ovo + sizeof(int);
            
            this.{{context.InputBufferVariableName}} = memory;
            this.{{context.TableFieldContextVariableName}} = fieldContext;
            this.{{context.RemainingDepthVariableName}} = remainingDepth;

            {{StrykerSuppressor.SuppressNextLine()}}
            int progressiveMinLength = (int)(this.count / ChunkSize) + 1;
            this.items = new {{derivedTypeName}}?[]?[progressiveMinLength];
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
        private {{derivedTypeName}}?[] GetOrCreateRow({{derivedTypeName}}?[]?[] items, uint rowIndex)
        {
            return items[rowIndex] ?? this.CreateRow(items, rowIndex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private {{derivedTypeName}}?[] CreateRow({{derivedTypeName}}?[]?[] items, uint rowIndex)
        {
            var row = new {{derivedTypeName}}?[(int)ChunkSize];
            items[rowIndex] = row;
            return row;
        }

        private {{derivedTypeName}} ProgressiveGet(int index)
        {
            {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.count);

            uint uindex = (uint)index;
            GetAddress(uindex, out uint rowIndex, out uint colIndex);

            var items = this.items;
            var row = this.GetOrCreateRow(items, rowIndex);
            {{derivedTypeName}}? item = row[colIndex];
            
            if (item is null)
            {
                item = this.UnsafeParseItem(index);
                row[colIndex] = item;
            }

            return item.Value;
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
