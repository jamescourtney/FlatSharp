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
    public static (string classDef, string className) CreateGreedyMutableUnionVector(
        ITypeModel itemTypeModel,
        ParserCodeGenContext context)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.GreedyMutable, "Expecting greedymutable");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.GreedyMutable);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        
        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("GreedyMutable [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        where TInputBuffer : IInputBuffer
    {
        private readonly List<{{baseTypeName}}> list;

        public {{className}}(
            TInputBuffer {{context.InputBufferVariableName}},
            ref (long offset0, long offset1) offsets,
            short {{context.RemainingDepthVariableName}},
            TableFieldContext {{context.TableFieldContextVariableName}})
        {
            long dvo = offsets.offset0;
            long ovo = offsets.offset1;

            int discriminatorCount = (int){{context.InputBufferVariableName}}.ReadUInt(dvo);
            int offsetCount = (int){{context.InputBufferVariableName}}.ReadUInt(ovo);

            if (discriminatorCount != offsetCount)
            {
                {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.InvalidData_UnionVectorMismatchedLength)}}();
            }

            dvo += sizeof(uint);
            ovo += sizeof(uint);
            
            var list = new List<{{baseTypeName}}>(offsetCount);
            this.list = list;
            
            for (int i = 0; i < discriminatorCount; ++i)
            {
                var {{context.OffsetVariableName}} = (dvo, ovo);
                var item = {{context.GetParseInvocation(itemTypeModel.ClrType)}};

                list.Add(item);

                dvo += sizeof(byte);
                ovo += sizeof(uint);
            }
        }

        public {{baseTypeName}} this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.GetItem(index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this.SetItem(index, value);
        }

        public int Count => this.list.Count;

        public bool IsReadOnly => false;

        public FlatBufferDeserializationOption DeserializationOption => {{nameof(FlatBufferDeserializationOption)}}.{{context.Options.DeserializationOption}};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{baseTypeName}} GetItem(int index) => this.list[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetItem(int index, {{baseTypeName}} value)
        {
            this.list[index] = value;
        }

        public void Add({{baseTypeName}} item)
        {
            this.list.Add(item);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public void Insert(int index, {{baseTypeName}} item)
        {
            this.list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public bool Remove({{baseTypeName}} item)
        {
            return this.list.Remove(item);
        }

        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, baseTypeName)}}
    }
"""";

        return (classDef, className);
    }
}
