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
    using System.Linq;

    /// <summary>
    /// Defines a type model for an object that can be parsed and serialized to/from a FlatBuffer. While generally most useful to the 
    /// FlatSharp codegen, this can be used to introspect on how FlatSharp interprets your schema.
    /// </summary>
    public abstract class RuntimeTypeModel : ITypeModel
    {
        protected readonly TypeModelContainer typeModelContainer;

        internal RuntimeTypeModel(Type clrType, TypeModelContainer typeModelContainer)
        {
            this.ClrType = clrType;
            this.typeModelContainer = typeModelContainer;
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
        /// Gets the schema type.
        /// </summary>
        public abstract FlatBufferSchemaType SchemaType { get; }

        /// <summary>
        /// Gets the layout of this type model's vtable.
        /// </summary>
        public abstract ImmutableArray<PhysicalLayoutElement> PhysicalLayout { get; }

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
        /// When true, indicates that this type model's serialize method writes inline, rather than by offset.
        /// </summary>
        public abstract bool SerializesInline { get; }

        /// <summary>
        /// Gets the maximum inline size of this item when padded for alignment, when stored in a table or vector.
        /// </summary>
        public virtual int MaxInlineSize => this.PhysicalLayout.Sum(x => x.InlineSize + SerializationHelpers.GetMaxPadding(x.Alignment));

        /// <summary>
        /// In general, we don't set this to true.
        /// </summary>
        public virtual bool MustAlwaysSerialize => false;

        /// <summary>
        /// Validates a default value.
        /// </summary>
        public virtual bool ValidateDefaultValue(object defaultValue)
        {
            return false;
        }

        /// <summary>
        /// Gets or creates a runtime type model from the given type. This is only used in test cases any more.
        /// </summary>
        internal static ITypeModel CreateFrom(Type type)
        {
            return TypeModelContainer.CreateDefault().CreateTypeModel(type);
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

        public virtual bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
        {
            literal = null;
            return false;
        }

        public virtual bool TryFormatStringAsLiteral(string value, out string literal)
        {
            literal = null;
            return false;
        }

        public virtual bool TryGetUnderlyingVectorType(out ITypeModel typeModel)
        {
            typeModel = null;
            return false;
        }

        public virtual bool TryGetSpanComparerType(out Type comparerType)
        {
            comparerType = null;
            return false;
        }

        public virtual bool TryGetTableKeyMember(out TableMemberModel tableMember)
        {
            tableMember = null;
            return false;
        }

        public virtual TableMemberModel AdjustTableMember(TableMemberModel source)
        {
            return source;
        }
    }
}
