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
        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("GreedyMutable [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IPoolableObject
        where TInputBuffer : IInputBuffer
    {
        private readonly List<{{baseTypeName}}> list;
        private int inUse = 1;

        private {{className}}(int count)
        {
            this.list = new List<{{baseTypeName}}>(count);
        }

        public static {{className}}<TInputBuffer> GetOrCreate(
            TInputBuffer {{context.InputBufferVariableName}},
            ref (int offset0, int offset1) offsets,
            short {{context.RemainingDepthVariableName}},
            TableFieldContext {{context.TableFieldContextVariableName}})
        {
            int dvo = offsets.offset0;
            int ovo = offsets.offset1;

            int discriminatorCount = (int){{context.InputBufferVariableName}}.ReadUInt(dvo);
            int offsetCount = (int){{context.InputBufferVariableName}}.ReadUInt(ovo);

            if (discriminatorCount != offsetCount)
            {
                {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.InvalidData_UnionVectorMismatchedLength)}}();
            }

            dvo += sizeof(uint);
            ovo += sizeof(uint);

            if (ObjectPool.TryGet(out {{className}}<TInputBuffer>? list))
            {{StrykerSuppressor.SuppressNextLine("block")}}
            {
    #if NET6_0_OR_GREATER
                {{StrykerSuppressor.SuppressNextLine("statement")}}
                list.list.EnsureCapacity(discriminatorCount);
    #endif
            }
            else
            {
                list = new {{className}}<TInputBuffer>(discriminatorCount);
            }

            var innerList = list.list;
            for (int i = 0; i < discriminatorCount; ++i)
            {
                var {{context.OffsetVariableName}} = (dvo, ovo);
                var item = {{context.GetParseInvocation(itemTypeModel.ClrType)}};

                innerList.Add(item);

                dvo += sizeof(byte);
                ovo += sizeof(uint);
            }

            list.inUse = 1;

            return list;
        }

        {{StrykerSuppressor.ExcludeFromCodeCoverage()}}
        public void ReturnToPool(bool force)
        {
            if (force)
            {
                if (System.Threading.Interlocked.Exchange(ref this.inUse, 0) != 0)
                {
                    {{(
                        !itemTypeModel.ClrType.IsValueType && typeof(IPoolableObject).IsAssignableFrom(itemTypeModel.ClrType)
                      ? $$"""
                            foreach (var item in this.list)
                            {
                              item.ReturnToPool(true);
                            }
                          """
                      : string.Empty
                    )}}

                    this.list.Clear();
                    ObjectPool.Return(this);
                }
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
