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
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace FlatSharp.TypeModel;

internal static partial class FlatBufferVectorHelpers
{
    private static string If(bool condition, string value)
    {
        if (condition)
        {
            return value;
        }

        return string.Empty;
    }

    private static string IfNot(bool condition, string value)
    {
        return If(!condition, value);
    }

    private static string CreateVectorClassName(ITypeModel itemModel, FlatBufferDeserializationOption option)
    {
        // Horribly inefficient
        using SHA256 sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(itemModel.GetCompilableTypeName()));
        Guid g = new Guid(hash.Take(16).ToArray());

        return $"GeneratedVector_{g:n}_{option}";
    }

    private static string GetNullableReferenceAnnotation(ITypeModel model)
    {
        return model.ClrType.IsValueType
             ? string.Empty
             : "?";
    }

    public static string CreateCommonReadOnlyVectorMethods(
        ITypeModel itemTypeModel,
        string derivedTypeName)
    {
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string nullableReference = GetNullableReferenceAnnotation(itemTypeModel);

        return $$"""
            public bool Contains({{baseTypeName}}{{nullableReference}} item)
                => {{typeof(VectorsCommon).GetGlobalCompilableTypeName()}}.Contains(this, item);
                 
            public int IndexOf({{baseTypeName}}{{nullableReference}} item)
                => {{typeof(VectorsCommon).GetGlobalCompilableTypeName()}}.IndexOf(this, item);
                 
            public void CopyTo({{baseTypeName}}[]? array, int arrayIndex) 
                => {{typeof(VectorsCommon).GetGlobalCompilableTypeName()}}.CopyTo(this, array, arrayIndex);
                 
            public IEnumerator<{{baseTypeName}}> GetEnumerator()
                => {{typeof(VectorsCommon).GetGlobalCompilableTypeName()}}.GetEnumerator(this);
                 
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
            """;
    }

    private static string GetEfficientMultiply(
        int inlineSize,
        string indexVariableName)
    {
        FlatSharpInternal.Assert(inlineSize != 0, "invalid inline size");

        bool isPowerOf2 = (inlineSize & (inlineSize - 1)) == 0;
        if (!isPowerOf2)
        {
            // Slow multiply.
            return $"{inlineSize} * {indexVariableName}";
        }

        int mask = inlineSize;
        int shift = 0;
        while (mask > 1)
        {
            mask >>= 1;
            shift++;
        }

        FlatSharpInternal.Assert((1 << shift) == inlineSize, $"expected to recompute inlinesize. Expected = {inlineSize}, Actual = {(1 << shift)}");
        return $"{indexVariableName} << {shift}";
    }

    private static string CreateIFlatBufferDeserializedVectorMethods(
        int inlineSize,
        string inputBufferVariableName,
        string offsetVariableName,
        string checkedParseItemMethodName)
    {
        return
          $$"""
            IInputBuffer IFlatBufferDeserializedVector.InputBuffer => this.{{inputBufferVariableName}};
            
            int IFlatBufferDeserializedVector.ItemSize => {{inlineSize}};

            int IFlatBufferDeserializedVector.OffsetBase => this.{{offsetVariableName}};
                
            object IFlatBufferDeserializedVector.ItemAt(int index) => this.{{checkedParseItemMethodName}}(index)!;
            
            int IFlatBufferDeserializedVector.OffsetOf(int index)
            {
                {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.Count);
                return this.{{offsetVariableName}} + ({{GetEfficientMultiply(inlineSize, "index")}});
            }
            """;
    }

    private static string CreateImmutableVectorMethods(
        ITypeModel itemTypeModel)
    {
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string nullableReference = GetNullableReferenceAnnotation(itemTypeModel);

        string notMutableThrow = $"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.NotMutable_DeserializedVector)}()";
        return
            $$"""
              public bool IsReadOnly => true;

              public void Add({{baseTypeName}} item) => {{notMutableThrow}};
              public void Clear() => {{notMutableThrow}};
              public void Insert(int index, {{baseTypeName}} item) => {{notMutableThrow}};
              public void RemoveAt(int index) => {{notMutableThrow}};
              public bool Remove({{baseTypeName}} item) => {{notMutableThrow}};
              """;
    }

    public static string CreateWriteThroughMethod(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        string writeThroughBody = $"{typeof(FSThrow).GGCTN()}.{nameof(FSThrow.NotMutable_DeserializedVector)}();";

        if (isEverWriteThrough)
        {
            var serializeContext = context.GetWriteThroughContext("data", "value", "0");

            writeThroughBody = @$"
                if (!{context.TableFieldContextVariableName}.WriteThrough)
                {{
                    {nameof(FSThrow)}.{nameof(FSThrow.NotMutable)}();
                }}

                int offset = this.offset + ({GetEfficientMultiply(inlineSize, "index")});
                Span<byte> {serializeContext.SpanVariableName} = {context.InputBufferVariableName}.GetSpan().Slice(offset, {inlineSize});

                {serializeContext.GetSerializeInvocation(itemTypeModel.ClrType)};
            ";
        }

        return $@"
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UnsafeWriteThrough(int index, {itemTypeModel.GetGlobalCompilableTypeName()} value) 
        {{ 
            {writeThroughBody} 
        }}";
    }
}
