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
    public static (string classDef, string className) CreateGreedyMutableVector(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.GreedyMutable, "Expecting greedymutable");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.GreedyMutable);

        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string nullableReference = itemTypeModel.ClrType.IsValueType ? string.Empty : "?";

        string classDef =
$$""""
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        , IPoolableObject
        , IVisitable{{(itemTypeModel.ClrType.IsValueType ? "Value" : "Reference")}}Vector<{{baseTypeName}}>
        {{IfNot(itemTypeModel.ClrType.IsValueType, $", IIndexedVectorSource<{baseTypeName}>")}}
        where TInputBuffer : IInputBuffer
    {
        private TableFieldContext {{context.TableFieldContextVariableName}};
        private readonly List<{{baseTypeName}}> list;
        private int inUse = 1;

#pragma warning disable CS8618
        private {{className}}(int count)
        {
            this.list = new List<{{baseTypeName}}>(count);
        }
#pragma warning restore CS8618

        public static {{className}}<TInputBuffer> GetOrCreate(
            TInputBuffer {{context.InputBufferVariableName}},
            int {{context.OffsetVariableName}},
            short {{context.RemainingDepthVariableName}},
            TableFieldContext {{context.TableFieldContextVariableName}})
        {
            int count = (int){{context.InputBufferVariableName}}.ReadUInt({{context.OffsetVariableName}});
            {{context.OffsetVariableName}} += sizeof(int);

            if (ObjectPool.TryGet(out {{className}}<TInputBuffer>? list))
            {
    #if NET6_0_OR_GREATER
                list.list.EnsureCapacity(count);
    #endif
            }
            else
            {
                list = new {{className}}<TInputBuffer>(count);
            }

            var innerList = list.list;
            for (int i = 0; i < count; ++i)
            {
                var item = {{context.GetParseInvocation(itemTypeModel.ClrType)}};
                innerList.Add(item);
                {{context.OffsetVariableName}} += {{inlineSize}};
            }

            list.{{context.TableFieldContextVariableName}} = {{context.TableFieldContextVariableName}};
            list.inUse = 1;

            return list;
        }

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
        private void ValidateMutation()
        {
            {{(
                // only need to check if this field is ever write through.
                isEverWriteThrough
              ? $$"""
                    if ({{context.TableFieldContextVariableName}}.{{nameof(TableFieldContext.WriteThrough)}})
                    {
                        {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowNotMutableException)}}();
                    }
                  """
              : string.Empty
            )}}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{baseTypeName}} GetItem(int index) => this.list[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetItem(int index, {{baseTypeName}} value)
        {
            this.ValidateMutation();
            this.list[index] = value;
        }

        public void Add({{baseTypeName}} item)
        {
            this.ValidateMutation();
            this.list.Add(item);
        }

        public void Clear()
        {
            this.ValidateMutation();
            this.list.Clear();
        }

        public void Insert(int index, {{baseTypeName}} item)
        {
            this.ValidateMutation();
            this.list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.ValidateMutation();
            this.list.RemoveAt(index);
        }

        public bool Remove({{baseTypeName}} item)
        {
            this.ValidateMutation();
            return this.list.Remove(item);
        }

        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, "GetItem")}}
        {{CreateVisitorMethods(itemTypeModel, className, baseTypeName, baseTypeName, "GetItem", "SetItem")}}
    }
"""";

        return (classDef, className);
    }
}
