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

namespace FlatSharp.TypeModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A type model. Declares both properties of a type and how to serialize/parse that type.
    /// </summary>
    public interface ITypeModel
    {
        /// <summary>
        /// The type of the item in the CLR.
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        VTableEntry[] VTableLayout { get; }

        /// <summary>
        /// Indicates if this item is fixed size or not.
        /// </summary>
        bool IsFixedSize { get; }

        /// <summary>
        /// Indicates if this type model can be part of a struct.
        /// </summary>
        bool IsValidStructMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a table.
        /// </summary>
        bool IsValidTableMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a vector.
        /// </summary>
        bool IsValidVectorMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a union.
        /// </summary>
        bool IsValidUnionMember { get; }

        /// <summary>
        /// Indicates if this type model can be a sorted vector key.
        /// </summary>
        bool IsValidSortedVectorKey { get; }

        /// <summary>
        /// Gets the maximum inline size of this item when padded for alignment, when stored in a table or vector.
        /// </summary>
        int MaxInlineSize { get; }

        /// <summary>
        /// Validates a default value.
        /// </summary>
        bool ValidateDefaultValue(object defaultValue);

        /// <summary>
        /// Implements a serialize method for the the type of this type model.
        /// </summary>
        CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context);

        /// <summary>
        /// Implements a parse method for the type of this type model. The method must return a value of type <see cref="ITypeModel.ClrType"/>.
        /// </summary>
        CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context);

        /// <summary>
        /// Implements a get max size method for the type of this type model. The method must return an int32 value representing
        /// the maximum possible serialized size of the given item, even with alignment errors.
        /// </summary>
        CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context);

        /// <summary>
        /// Returns an expression that asserts the given item is non-null. 
        /// </summary>
        /// <param name="itemVariableName">The variable name of the item.</param>
        /// <returns>An expression that tests for null (ie, "item == null").</returns>
        string GetThrowIfNullInvocation(string itemVariableName);

        /// <summary>
        /// Returns a boolean expression testing if the given item is null or not.
        /// </summary>
        /// <param name="itemVariableName">The variable name of the item.</param>
        string GetNonNullConditionExpression(string itemVariableName);

        /// <summary>
        /// Travses the object graph to identify types needed to build a serializer.
        /// </summary>
        void TraverseObjectGraph(HashSet<Type> seenTypes);

        /// <summary>
        /// Formats the given default value into a C# literal. Not all implementations support this.
        /// </summary>
        string FormatDefaultValueAsLiteral(object defaultValue);

        /// <summary>
        /// Formats the given string as a literal of this type. Not all implementations support this.
        /// </summary>
        string FormatStringAsLiteral(string value);

        /// <summary>
        /// Initializes and validates the type model.
        /// </summary>
        void Initialize();
    }

    /// <summary>
    /// Some type models can specify that they supply a span comparer.
    /// </summary>
    public interface ISpanComparerProvider
    {
        /// <summary>
        /// The type of the span comaprer used for this type model.
        /// </summary>
        Type SpanComparerType { get; }
    }

    public class VTableEntry
    {
        public VTableEntry(int size, int alignment)
        {
            this.InlineSize = size;
            this.Alignment = alignment;
        }

        public int InlineSize { get; }

        public int Alignment { get; }
    }
}
