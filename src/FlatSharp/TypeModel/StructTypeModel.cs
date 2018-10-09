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
    using System.Linq;
    using System.Reflection;
    using FlatSharp.Attributes;

    /// <summary>
    /// Defines the type schema for a Flatbuffer struct. Structs, like C structs, are statically ordered sets of data
    /// whose schema may not be changed.
    /// </summary>
    public class StructTypeModel : RuntimeTypeModel
    {
        private readonly List<StructMemberModel> memberTypes = new List<StructMemberModel>();
        private int inlineSize;
        private int maxAlignment = 1;

        internal StructTypeModel(Type clrType) : base(clrType)
        {
        }

        /// <summary>
        /// Gets the schema type of this element.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Struct;

        /// <summary>
        /// Gets the alignment of this element.
        /// </summary>
        public override int Alignment => this.maxAlignment;

        /// <summary>
        /// Gets the size of this element, with padding taken into account.
        /// </summary>
        public override int InlineSize => this.inlineSize;

        /// <summary>
        /// Gets the members of this struct.
        /// </summary>
        public IReadOnlyList<StructMemberModel> Members => this.memberTypes;

        /// <summary>
        /// Gets the default constructor for this type.
        /// </summary>
        internal ConstructorInfo DefaultConstructor { get; private set; }

        protected override void Initialize()
        {
            var structAttribute = this.ClrType.GetCustomAttribute<FlatBufferStructAttribute>();
            if (structAttribute == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create struct type model from type {this.ClrType.Name} because it does not have a [FlatBufferStruct] attribute.");
            }

            TableTypeModel.EnsureClassCanBeInheritedByOutsideAssembly(this.ClrType, out var ctor);
            this.DefaultConstructor = ctor;

            var properties = this.ClrType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(x => new
                {
                    Property = x,
                    Attribute = x.GetCustomAttribute<FlatBufferItemAttribute>()
                })
                .Where(x => x.Attribute != null)
                .OrderBy(x => x.Attribute.Index);

            ushort expectedIndex = 0;
            this.inlineSize = 0;

            foreach (var item in properties)
            {
                var propertyAttribute = item.Attribute;
                var property = item.Property;

                if (propertyAttribute.Deprecated)
                {
                    throw new InvalidFlatBufferDefinitionException($"FlatBuffer struct {this.ClrType.Name} may not have deprecated properties");
                }

                ushort index = propertyAttribute.Index;
                if (index != expectedIndex)
                {
                    throw new InvalidFlatBufferDefinitionException($"FlatBuffer struct {this.ClrType.Name} does not declare an item with index {expectedIndex}. Structs must have sequenential indexes starting at 0.");
                }

                expectedIndex++;
                RuntimeTypeModel propertyModel = RuntimeTypeModel.CreateFrom(property.PropertyType);
                if (propertyModel.SchemaType != FlatBufferSchemaType.Scalar && propertyModel.SchemaType != FlatBufferSchemaType.Struct)
                {
                    throw new InvalidFlatBufferDefinitionException($"FlatBuffer struct {this.ClrType.Name} may only contain scalars and other structs. Property = {property.Name}");
                }

                int propertySize = propertyModel.InlineSize;
                int propertyAlignment = propertyModel.Alignment;
                this.maxAlignment = Math.Max(propertyAlignment, this.maxAlignment);

                // Pad for alignment.
                this.inlineSize += SerializationHelpers.GetAlignmentError(this.inlineSize, propertyAlignment);

                StructMemberModel model = new StructMemberModel(
                    propertyModel,
                    property,
                    index,
                    this.inlineSize);

                this.memberTypes.Add(model);
                this.inlineSize += propertyModel.InlineSize;
            }
        }
    }
}
