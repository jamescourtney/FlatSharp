﻿/*
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
    public static (string classDef, string className) CreateGreedyUnionVector(
        ITypeModel itemTypeModel,
        ParserCodeGenContext context)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.Greedy, "Expecting greedy");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Greedy);

        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);

        string classDef =
$$""""
    [System.Diagnostics.DebuggerDisplay("Greedy [ {{itemTypeModel.ClrType.Name}} ], Count = {Count}")]
    internal sealed class {{className}}<TInputBuffer>
        : object
        , IList<{{baseTypeName}}>
        , IReadOnlyList<{{baseTypeName}}>
        where TInputBuffer : IInputBuffer
    {
        private readonly List<{{derivedTypeName}}> list;

        public {{className}}(
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
            
            var list = new List<{{derivedTypeName}}>(offsetCount);
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
            get => this.GetItem(index);
            set => this.SetItem(index, value);
        }

        public int Count => this.list.Count;

        public FlatBufferDeserializationOption DeserializationOption => {{nameof(FlatBufferDeserializationOption)}}.{{context.Options.DeserializationOption}};

        private {{derivedTypeName}} GetItem(int index) => this.list[index];
        private void SetItem(int index, {{baseTypeName}} value) => {{typeof(FSThrow).GGCTN()}}.{{nameof(FSThrow.NotMutable_DeserializedVector)}}();

        {{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}
        {{CreateImmutableVectorMethods(itemTypeModel)}}
    }
"""";

        return (classDef, className);
    }
}
