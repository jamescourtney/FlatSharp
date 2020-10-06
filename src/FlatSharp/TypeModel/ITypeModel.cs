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
    using System.Collections.Immutable;

    /// <summary>
    /// A type model. Declares both properties of a type and how to serialize/parse that type.
    /// </summary>
    public interface ITypeModel
    {
        /// <summary>
        /// Gets the schema element type that this type model represents. Note that this is not a 1:1 relationship with the type of class. There can
        /// be multiple implementations of ITypeModel that satisfy a particular schema type.
        /// </summary>
        FlatBufferSchemaType SchemaType { get; }

        /// <summary>
        /// The type of the item in the CLR.
        /// </summary>
        Type ClrType { get; }

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        ImmutableArray<PhysicalLayoutElement> PhysicalLayout { get; }

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
        /// When true, indicates that this type model must always be written to the buffer. This suppresses null checks and default value checks.
        /// </summary>
        bool MustAlwaysSerialize { get; }

        /// <summary>
        /// When true, indicates that this type model serializes directly at the offset provided (ie, it does not write a uoffset).
        /// </summary>
        bool SerializesInline { get; }

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
        /// Attempts to format the given default value into a C# literal. Not all implementations support this.
        /// </summary>
        bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal);

        /// <summary>
        /// Attempts to format the given string as a literal of this type. Not all implementations support this.
        /// </summary>
        bool TryFormatStringAsLiteral(string value, out string literal);

        /// <summary>
        /// For vectors, retrieves the inner type model. Other types return false.
        /// </summary>
        bool TryGetUnderlyingVectorType(out ITypeModel typeModel);

        /// <summary>
        /// Attempts to get the type implementing <see cref="ISpanComparer"/> for the type model.
        /// </summary>
        bool TryGetSpanComparerType(out Type comparerType);

        /// <summary>
        /// Attempts to get the key of the table. Will return false when there is no key or the type model does not represent a table.
        /// </summary>
        bool TryGetTableKeyMember(out TableMemberModel tableMember);

        /// <summary>
        /// Invoked when this type model is being serialized as part of a table. Provides an opportunity to override or supplement
        /// the declared configuration in the table. This method will not be invoked
        /// when the type is serialized in other contexts, such as vectors and structs.
        /// </summary>
        TableMemberModel AdjustTableMember(TableMemberModel source);

        /// <summary>
        /// Initializes and validates the type model.
        /// </summary>
        void Initialize();
    }
}
