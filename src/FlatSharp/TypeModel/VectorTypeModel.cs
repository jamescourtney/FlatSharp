/*
 * Copyright 2018 James Courtney
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
 
namespace FlatSharp.TypeModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines a vector type model.
    /// </summary>
    public class VectorTypeModel : RuntimeTypeModel, ITypeModel
    {
        private ITypeModel memberTypeModel;
        private bool isList;
        private bool isMemory;
        private bool isArray;
        private bool isReadOnly;

        internal VectorTypeModel(Type vectorType) : base(vectorType)
        {
        }

        /// <summary>
        /// Gets the schema type of this element.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Vector;

        /// <summary>
        /// Gets the required alignment of this element.
        /// </summary>
        public override int Alignment => sizeof(uint);

        /// <summary>
        /// Gets the inline size of this element.
        /// </summary>
        public override int InlineSize => sizeof(uint);

        /// <summary>
        /// Vectors are arbitrary in length.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Vectors can't be part of structs.
        /// </summary>
        public override bool IsValidStructMember => false;

        /// <summary>
        /// Vectors can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Vectors can't be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => false;

        /// <summary>
        /// Vectors can't be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => false;

        /// <summary>
        /// Vector's can't be keys of sorted vectors.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Gets the type model for this vector's elements.
        /// </summary>
        public ITypeModel ItemTypeModel => this.memberTypeModel;

        /// <summary>
        /// Indicates whether this vector's type is <see cref="System.Memory{T}"/> or <see cref="System.ReadOnlyMemory{T}"/>.
        /// </summary>
        public bool IsMemoryVector => this.isMemory;

        /// <summary>
        /// Indicates whether this vector's type is <see cref="IList{T}"/> or <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        public bool IsList => this.isList;

        /// <summary>
        /// Indicates whether this vector's type is an array. Note that arrays are deserialized greedily.
        /// </summary>
        public bool IsArray => this.isArray;

        /// <summary>
        /// Indicates if this vector's type is read-only.
        /// </summary>
        public bool IsReadOnly => this.isReadOnly;

        /// <summary>
        /// Gets the size of each member of this vector, with padding for alignment.
        /// </summary>
        public int PaddedMemberInlineSize
        {
            get
            {
                int itemInlineSize = this.ItemTypeModel.InlineSize;
                int itemAlignment = this.ItemTypeModel.Alignment;

                return itemInlineSize + SerializationHelpers.GetAlignmentError(itemInlineSize, itemAlignment); 
            }
        }

        internal string LengthPropertyName
        {
            get
            {
                if (this.IsArray)
                {
                    return nameof(Array.Length);
                }
                
                if (this.IsList)
                {
                    return nameof(IList<string>.Count);
                }

                if (this.IsMemoryVector)
                {
                    return nameof(Memory<byte>.Length);
                }

                throw new InvalidOperationException("Unexpected type of vector.");
            }
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            // count of items + padding(uoffset_t);
            int fixedSize = sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint));
            string lengthProperty = $"{context.ValueVariableName}.{this.LengthPropertyName}";

            string body;
            if (this.ItemTypeModel.IsFixedSize)
            {
                // Constant size items. We can reduce these reasonably well.
                body = $"return {fixedSize} + {SerializationHelpers.GetMaxPadding(this.ItemTypeModel.Alignment)} + ({this.PaddedMemberInlineSize} * {lengthProperty});";
            }
            else
            {
                var itemContext = context.With(valueVariableName: "itemTemp");

                body =
    $@"
                    int length = {lengthProperty};
                    int runningSum = {fixedSize} + {SerializationHelpers.GetMaxPadding(this.ItemTypeModel.Alignment)} + ({this.PaddedMemberInlineSize} * length);
                    for (int i = 0; i < length; ++i)
                    {{
                        var itemTemp = {context.ValueVariableName}[i];
                        {this.ItemTypeModel.GetThrowIfNullInvocation("itemTemp")};
                        runningSum += {itemContext.GetMaxSizeInvocation(this.ItemTypeModel.ClrType)};
                    }}
                    return runningSum;";
            }

            return new CodeGeneratedMethod
            {
                MethodBody = body,
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            var type = this.ClrType;
            var itemTypeModel = this.ItemTypeModel;

            // Params: Buffer, UOffset after following, Padded size of each member, delegate invocation for parsing individual items.
            string createFlatBufferVector = 
            $@"new {nameof(FlatBufferVector<int>)}<{CSharpHelpers.GetCompilableTypeName(itemTypeModel.ClrType)}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBuffer.ReadUOffset)}({context.OffsetVariableName}), 
                    {this.PaddedMemberInlineSize}, 
                    (b, o) => {context.MethodNameMap[itemTypeModel.ClrType]}(b, o))";

            string body;
            if (this.isMemory)
            {
                string method = nameof(InputBuffer.ReadByteMemoryBlock);
                if (this.ClrType == typeof(ReadOnlyMemory<byte>))
                {
                    method = nameof(InputBuffer.ReadByteReadOnlyMemoryBlock);
                }

                string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";

                // Memory is faster in situations where we can get away with it.
                if (context.Options.GreedyDeserialize)
                {
                    body = $"return {memoryVectorRead}.ToArray().AsMemory();";
                }
                else
                {
                    body = $"return {memoryVectorRead};";
                }
            }
            else if (this.isArray)
            {
                string method = nameof(InputBuffer.ReadByteMemoryBlock);
                if (this.ClrType == typeof(ReadOnlyMemory<byte>))
                {
                    method = nameof(InputBuffer.ReadByteReadOnlyMemoryBlock);
                }

                string memoryVectorRead = $"{context.InputBufferVariableName}.{method}({context.OffsetVariableName})";

                if (itemTypeModel.ClrType == typeof(byte))
                {
                    // can handle this as memory.
                    body = $"return {memoryVectorRead}.ToArray();";
                }
                else
                {
                    body = $"return ({createFlatBufferVector}).ToArray();";
                }
            }
            else
            {
                if (context.Options.PreallocateVectors)
                {
                    // We just call .ToList(). Noe that when full greedy mode is on, these items will be 
                    // greedily initialized as we traverse the list. Otherwise, they'll be allocated lazily.
                    body = $"({createFlatBufferVector}).{nameof(SerializationHelpers.FlatBufferVectorToList)}()";
                    
                    if (!context.Options.GenerateMutableObjects)
                    {
                        // Finally, if we're not in the business of making mutable objects, then convert the list to read only.
                        body += ".AsReadOnly()";
                    }

                    body = $"return {body};";
                }
                else
                {
                    body = $"return {createFlatBufferVector};";
                }
            }

            return new CodeGeneratedMethod { MethodBody = body };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var type = this.ClrType;
            var itemTypeModel = this.ItemTypeModel;

            string body;
            if (this.isMemory)
            {
                body = $"{context.SpanWriterVariableName}.{nameof(SpanWriter.WriteReadOnlyByteMemoryBlock)}({context.SpanVariableName}, {context.ValueVariableName}, {context.OffsetVariableName}, {itemTypeModel.Alignment}, {itemTypeModel.InlineSize}, {context.SerializationContextVariableName});";
            }
            else
            {
                string propertyName = this.LengthPropertyName;

                body = $@"
                int count = {context.ValueVariableName}.{propertyName};
                int vectorOffset = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateVector)}({itemTypeModel.Alignment}, count, {this.PaddedMemberInlineSize});
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, vectorOffset, {context.SerializationContextVariableName});
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, count, vectorOffset, {context.SerializationContextVariableName});
                vectorOffset += sizeof(int);
                for (int i = 0; i < count; ++i)
                {{
                      var current = {context.ValueVariableName}[i];
                      {itemTypeModel.GetThrowIfNullInvocation("current")};
                      {context.MethodNameMap[itemTypeModel.ClrType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, current, vectorOffset, {context.SerializationContextVariableName});
                      vectorOffset += {this.PaddedMemberInlineSize};
                }}";
            }

            return new CodeGeneratedMethod { MethodBody = body };
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            if (this.isMemory)
            {
                return "true";
            }
            else
            {
                return $"{itemVariableName} != null";
            }
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            if (this.isMemory)
            {
                return string.Empty;
            }
            else
            {
                return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
            }
        }

        protected override void Initialize()
        {
            bool isValidType = false;
            Type innerType = null;
            if (this.ClrType.IsGenericType)
            {
                var genericType = this.ClrType.GetGenericTypeDefinition();
                if (genericType == typeof(Memory<>) || genericType == typeof(ReadOnlyMemory<>))
                {
                    this.isMemory = true;
                    this.isReadOnly = genericType == typeof(ReadOnlyMemory<>);
                    isValidType = true;
                    innerType = this.ClrType.GetGenericArguments()[0];
                }
                else if (genericType == typeof(IList<>) || genericType == typeof(IReadOnlyList<>))
                {
                    this.isList = true;
                    this.isReadOnly = genericType == typeof(IReadOnlyList<>);
                    isValidType = true;
                    innerType = this.ClrType.GetGenericArguments()[0];
                }
            }
            else if (this.ClrType.IsArray)
            {
                isValidType = true;
                this.isReadOnly = false;
                this.isArray = true;
                innerType = this.ClrType.GetElementType();
            }

            if (!isValidType)
            {
                throw new InvalidFlatBufferDefinitionException($"Cannot build a vector from type: {this.ClrType}. Only List, ReadOnlyList, Memory, ReadOnlyMemory, and Arrays are supported.");
            }

            this.memberTypeModel = (ITypeModel)RuntimeTypeModel.CreateFrom(innerType);
            if (!this.memberTypeModel.IsValidVectorMember)
            {
                throw new InvalidFlatBufferDefinitionException($"Vectors may not contain {this.memberTypeModel.SchemaType}.");
            }

            if (this.isMemory)
            {
                if (this.memberTypeModel is ScalarTypeModel scalarModel && scalarModel.ClrType == typeof(byte))
                {
                    // allowed
                }
                else
                {
                    throw new InvalidFlatBufferDefinitionException("Vectors may only be Memory<T> or ReadOnlyMemory<T> when the type is an unsigned byte.");
                }
            }
        }
        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            if (seenTypes.Add(this.memberTypeModel.ClrType))
            {
                this.memberTypeModel.TraverseObjectGraph(seenTypes);
            }
        }
    }
}
