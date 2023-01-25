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
        string uncheckedReadMethodName)
    {
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string nullableReference = GetNullableReferenceAnnotation(itemTypeModel);

        return $$"""
                public bool Contains({{baseTypeName}}{{nullableReference}} item) => this.IndexOf(item) >= 0;
                 
                public int IndexOf({{baseTypeName}}{{nullableReference}} item)
                {
                {{(
                    itemTypeModel.ClrType.IsValueType
                  ? string.Empty
                  : $$"""
                      // FlatBuffer vectors are not allowed to have null by definition.
                      if (item is null)
                      {
                        return -1;
                      }
                      """
                )}}
                 
                    int count = this.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        var parsed = this.{{uncheckedReadMethodName}}(i);
                        if (item.Equals(parsed))
                        {
                            return i;
                        }
                    }
                 
                    return -1;
                }
                 
                public void CopyTo({{baseTypeName}}[]? array, int arrayIndex)
                {
                    if (array is null)
                    {
                        throw new ArgumentNullException(nameof(array));
                    }
                 
                    var count = this.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        array[arrayIndex + i] = this.{{uncheckedReadMethodName}}(i);
                    }
                }
                 
                public IEnumerator<{{baseTypeName}}> GetEnumerator()
                {
                    int count = this.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        yield return this.{{uncheckedReadMethodName}}(i);
                    }
                }
                 
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
                """;
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
                
            object IFlatBufferDeserializedVector.ItemAt(int index) => this.{{checkedParseItemMethodName}}(index)!;
            
            int IFlatBufferDeserializedVector.OffsetOf(int index)
            {
                {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.CheckIndex)}}(index, this.Count);
                return this.{{offsetVariableName}} + ({{inlineSize}} * index);
            }
            """;
    }

    private static string CreateImmutableVectorMethods(
        ITypeModel itemTypeModel)
    {
        string baseTypeName = itemTypeModel.GetGlobalCompilableTypeName();
        string nullableReference = GetNullableReferenceAnnotation(itemTypeModel);

        return
            $$"""
              public bool IsReadOnly => true;

              public void Add({{baseTypeName}} item) => {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
              public void Clear() => {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
              public void Insert(int index, {{baseTypeName}} item) => {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
              public void RemoveAt(int index) => {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
              public bool Remove({{baseTypeName}} item)
              {
                  {{nameof(VectorUtilities)}}.{{nameof(VectorUtilities.ThrowInlineNotMutableException)}}();
                  return false;
              }
              """;
    }

    private static string CreateVisitorMethods(
        ITypeModel itemTypeModel,
        string className,
        string baseTypeName,
        string derivedTypeName,
        string indexedCheckedReadMethod,
        string indexCheckedWriteMethod)
    {
        return
            $$"""
              {{(
                  itemTypeModel.ClrType.IsValueType
                ? $$"""
                    public TReturn Accept<TVisitor, TReturn>(TVisitor visitor) where TVisitor : IValueVectorVisitor<{{baseTypeName}}, TReturn>
                        => visitor.Visit<SimpleVector>(new SimpleVector(this));
                    """
                : $$"""
                    public TReturn Accept<TVisitor, TReturn>(TVisitor visitor) where TVisitor : IReferenceVectorVisitor<{{baseTypeName}}, TReturn>
                        => visitor.Visit<{{derivedTypeName}}, SimpleVector>(new SimpleVector(this));
                    """
              )}}
              
              private struct SimpleVector : ISimpleVector<{{derivedTypeName}}>
              {
                  private readonly {{className}}<TInputBuffer> vector;
              
                  public SimpleVector({{className}}<TInputBuffer> vector)
                  {
                      this.vector = vector;
                      this.Count = vector.Count;
                  }
              
                  public int Count { get; }
              
                  public {{derivedTypeName}} this[int index] 
                  {
                      [MethodImpl(MethodImplOptions.AggressiveInlining)]
                      get => this.vector.{{indexedCheckedReadMethod}}(index);
              
                      [MethodImpl(MethodImplOptions.AggressiveInlining)]
                      set => this.vector.{{indexCheckedWriteMethod}}(index, value);
                  }
              }
              """;
    }

    public static string CreateWriteThroughMethod(
        ITypeModel itemTypeModel,
        int inlineSize,
        ParserCodeGenContext context,
        bool isEverWriteThrough)
    {
        string writeThroughBody = $"{nameof(VectorUtilities)}.{nameof(VectorUtilities.ThrowInlineNotMutableException)}();";
        if (isEverWriteThrough)
        {
            var serializeContext = context.GetWriteThroughContext("data", "value", "0");

            writeThroughBody = @$"
                {nameof(VectorUtilities)}.{nameof(VectorUtilities.CheckIndex)}(index, this.count);

                if (!{context.TableFieldContextVariableName}.WriteThrough)
                {{
                    {nameof(VectorUtilities)}.{nameof(VectorUtilities.ThrowNotMutableException)}();
                }}

                int offset = this.offset + ({inlineSize} * index);
                Span<byte> {serializeContext.SpanVariableName} = {context.InputBufferVariableName}.GetSpan().Slice(offset, {inlineSize});

                {serializeContext.GetSerializeInvocation(itemTypeModel.ClrType)};
            ";
        }

        return $@"
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteThrough(int index, {itemTypeModel.GetGlobalCompilableTypeName()} value) 
        {{ 
            {writeThroughBody} 
        }}";
    }
}
