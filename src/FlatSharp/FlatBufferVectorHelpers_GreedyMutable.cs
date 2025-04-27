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

        string IfMutable(string str)
        {
            if (isEverWriteThrough)
            {
                return $$"""
                        if ({{context.TableFieldContextVariableName}}.{{nameof(TableFieldContext.WriteThrough)}})
                        {
                            {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.NotMutable_GreedyMutableWriteThrough)}}();
                        }

                        {{str}}
                        """;
            }

            return str;
        }
        
        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("GreedyMutable [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        where TInputBuffer : IInputBuffer
    {
        private readonly TableFieldContext {{context.TableFieldContextVariableName}};
        private readonly List<{{baseTypeName}}> list;

        public {{className}}(
            TInputBuffer {{context.InputBufferVariableName}},
            long {{context.OffsetVariableName}},
            short {{context.RemainingDepthVariableName}},
            TableFieldContext {{context.TableFieldContextVariableName}})
        {
            int count = (int){{context.InputBufferVariableName}}.ReadUInt({{context.OffsetVariableName}});
            {{context.OffsetVariableName}} += sizeof(int);
            
            this.{{context.TableFieldContextVariableName}} = {{context.TableFieldContextVariableName}};
            
            var list = new List<{{baseTypeName}}>();
            this.list = list;
            
            for (int i = 0; i < count; ++i)
            {
                var item = {{context.GetParseInvocation(itemTypeModel.ClrType)}};
                list.Add(item);
                {{context.OffsetVariableName}} += {{inlineSize}};
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
            {{IfMutable("this.list[index] = value;")}}
        }

        public void Add({{baseTypeName}} item)
        {
            {{IfMutable("this.list.Add(item);")}}
        }

        public void Clear()
        {
            {{IfMutable("this.list.Clear();")}}
        }

        public void Insert(int index, {{baseTypeName}} item)
        {
            {{IfMutable("this.list.Insert(index, item);")}}
        }

        public void RemoveAt(int index)
        {
            {{IfMutable("this.list.RemoveAt(index);")}}
        }

        public bool Remove({{baseTypeName}} item)
        {
            {{IfMutable("return this.list.Remove(item);")}}
        }

        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, baseTypeName)}}
    }
"""";

        return (classDef, className);
    }
}
