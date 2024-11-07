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
    /// <summary>
    /// Creates a Lazy vector for non-unions.
    /// </summary>
    public static (string classDef, string className) CreateLazyUnionVector(
        ITypeModel itemTypeModel,
        ParserCodeGenContext context)
    {
        FlatSharpInternal.Assert(context.Options.DeserializationOption == FlatBufferDeserializationOption.Lazy, "Expecting lazy");

        string className = CreateVectorClassName(itemTypeModel, FlatBufferDeserializationOption.Lazy);
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string derivedTypeName = itemTypeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);

        string classDef =
$$$""""
    [System.Diagnostics.DebuggerDisplay("Lazy [ {{{itemTypeModel.ClrType.Name}}} ], Count = {Count}")]
    internal sealed class {{{className}}}<TInputBuffer>
        : object
        , IList<{{{baseTypeName}}}>
        , IReadOnlyList<{{{baseTypeName}}}>
        where TInputBuffer : IInputBuffer
    {
        private readonly int discriminatorVectorOffset;
        private readonly int offsetVectorOffset;
        private readonly int count;
        private readonly {{{context.InputBufferTypeName}}} {{{context.InputBufferVariableName}}};
        private readonly TableFieldContext {{{context.TableFieldContextVariableName}}};
        private readonly short {{{context.RemainingDepthVariableName}}};
        
        public {{{className}}}(
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
                {{{typeof(FSThrow).GGCTN()}}}.{{{nameof(FSThrow.InvalidData_UnionVectorMismatchedLength)}}}();
            }

            this.count = (int)offsetCount;
            this.discriminatorVectorOffset = dvo + sizeof(int);
            this.offsetVectorOffset = ovo + sizeof(int);
            this.{{{context.InputBufferVariableName}}} = memory;
            this.{{{context.RemainingDepthVariableName}}} = remainingDepth;
            this.{{{context.TableFieldContextVariableName}}} = fieldContext;
        }

        public {{{baseTypeName}}} this[int index]
        {
            get => this.SafeParseItem(index);
            set
            {
                {{{nameof(VectorUtilities)}}}.{{{nameof(VectorUtilities.CheckIndex)}}}(index, this.count);
                this.WriteThrough(index, value);
            }
        }

        public int Count => this.count;
    
        public FlatBufferDeserializationOption DeserializationOption => {{{nameof(FlatBufferDeserializationOption)}}}.{{{context.Options.DeserializationOption}}};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{{derivedTypeName}}} SafeParseItem(int index)
        {
            {{{nameof(VectorUtilities)}}}.{{{nameof(VectorUtilities.CheckIndex)}}}(index, this.count);
            return this.UnsafeParseItem(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private {{{derivedTypeName}}} UnsafeParseItem(int index)
        {
            var {{{context.OffsetVariableName}}} = (this.discriminatorVectorOffset + index, this.offsetVectorOffset + ({{{GetEfficientMultiply(sizeof(uint), "index")}}}));
            return {{{context.GetParseInvocation(itemTypeModel.ClrType)}}};
        }

        private void WriteThrough(int index, {{{baseTypeName}}} value)
        {
            {{{typeof(FSThrow).GGCTN()}}}.{{{nameof(FSThrow.NotMutable_DeserializedVector)}}}();
        }

        {{{CreateCommonReadOnlyVectorMethods(itemTypeModel, derivedTypeName)}}}
        {{{CreateImmutableVectorMethods(itemTypeModel)}}}
    }
"""";

        return (classDef, className);
    }
}
