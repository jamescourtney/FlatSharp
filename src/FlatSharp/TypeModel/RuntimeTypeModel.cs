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
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines a type model for an object that can be parsed and serialized to/from a FlatBuffer. While generally most useful to the 
    /// FlatSharp codegen, this can be used to introspect on how FlatSharp interprets your schema.
    /// </summary>
    public abstract class RuntimeTypeModel : ITypeModel
    {
        protected readonly ITypeModelProvider typeModelProvider;

        internal RuntimeTypeModel(Type clrType, ITypeModelProvider typeModelProvider)
        {
            this.ClrType = clrType;
            this.typeModelProvider = typeModelProvider;
        }

        /// <summary>
        /// Initializes this runtime type model instance.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// The type of the item in the CLR.
        /// </summary>
        public Type ClrType { get; }

        /// <summary>
        /// The alignment of this runtime type.
        /// </summary>
        public abstract int Alignment { get; }

        /// <summary>
        /// The inline size of this runtime type. For references, this will be the size of a reference.
        /// For scalars and structs, this will be the total size of the object.
        /// </summary>
        public abstract int InlineSize { get; }

        /// <summary>
        /// Indicates if this item is fixed size or not.
        /// </summary>
        public abstract bool IsFixedSize { get; }

        /// <summary>
        /// Indicates if this type model can be part of a struct.
        /// </summary>
        public abstract bool IsValidStructMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a table.
        /// </summary>
        public abstract bool IsValidTableMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a vector.
        /// </summary>
        public abstract bool IsValidVectorMember { get; }

        /// <summary>
        /// Indicates if this type model can be part of a union.
        /// </summary>
        public abstract bool IsValidUnionMember { get; }

        /// <summary>
        /// Indicates if this type model can be a sorted vector key.
        /// </summary>
        public abstract bool IsValidSortedVectorKey { get; }

        /// <summary>
        /// Gets the maximum inline size of this item when padded for alignment, when stored in a table or vector.
        /// </summary>
        public virtual int MaxInlineSize
        {
            get => this.InlineSize + SerializationHelpers.GetMaxPadding(this.Alignment);
        }

        /// <summary>
        /// Indicates if this type is built into FlatSharp and no parser needs to be generated.
        /// </summary>
        public virtual bool IsBuiltInType
        {
            get => false;
        }

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public virtual bool ValidateDefaultValue(object defaultValue)
        {
            return false;
        }

        /// <summary>
        /// Gets or creates a runtime type model from the given type.
        /// </summary>
        internal static ITypeModel CreateFrom(Type type)
        {
            return new InternalTypeModelProvider().CreateTypeModel(type);
        }

        public abstract CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context);

        public abstract CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context);

        public abstract CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context);

        public abstract string GetThrowIfNullInvocation(string itemVariableName);

        public virtual string GetNonNullConditionExpression(string itemVariableName)
        {
            return $"!object.ReferenceEquals({itemVariableName}, null)";
        }

        public abstract void TraverseObjectGraph(HashSet<Type> seenTypes);

        public virtual string FormatDefaultValueAsLiteral(object defaultValue) => throw new InvalidOperationException();

        public virtual string FormatStringAsLiteral(string value) => throw new InvalidOperationException();
    }
}
