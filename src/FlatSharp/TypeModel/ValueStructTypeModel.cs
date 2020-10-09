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
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the type schema for a Flatbuffer struct. Structs, like C structs, are statically ordered sets of data
    /// whose schema may not be changed.
    /// </summary>
    public class ValueStructTypeModel : RuntimeTypeModel
    {
        private readonly List<(int offset, FieldInfo field, ITypeModel model)> members = new List<(int offset, FieldInfo field, ITypeModel model)>();

        private int inlineSize;
        private int maxAlignment = 1;

        internal ValueStructTypeModel(Type clrType, TypeModelContainer container) : base(clrType, container)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Struct;

        /// <summary>
        /// Layout of the vtable.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout =>
            new PhysicalLayoutElement[] { new PhysicalLayoutElement(this.inlineSize, this.maxAlignment) }.ToImmutableArray();

        /// <summary>
        /// Structs are composed of scalars.
        /// </summary>
        public override bool IsFixedSize => true;

        /// <summary>
        /// Structs can be part of structs.
        /// </summary>
        public override bool IsValidStructMember => true;

        /// <summary>
        /// Structs can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Structs can be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => true;

        /// <summary>
        /// Structs can be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => true;

        /// <summary>
        /// Structs can't be keys of sorted vectors.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Structs are written inline.
        /// </summary>
        public override bool SerializesInline => true;

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            return new CodeGeneratedMethod
            {
                MethodBody = $"return {this.MaxInlineSize};",
            };
        }

        public override string GetIsEqualToDefaultValueExpression(string variableName, string defaultValue)
        {
            // Value structs are never null, but can be considered default when all members are default.
            List<string> components = new List<string>();
            foreach (var member in this.members)
            {
                string innerComparison = member.model.GetIsEqualToDefaultValueExpression(
                    $"{variableName}.{member.field.Name}", 
                    $"default({CSharpHelpers.GetCompilableTypeName(member.field.FieldType)})");

                components.Add($"({innerComparison})");
            }

            return string.Join(" && ", components);
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            var propertyStatements = new List<string>();
            for (int i = 0; i < this.members.Count; ++i)
            {
                var member = this.members[i];
                propertyStatements.Add($@"
                    item.{member.field.Name} = {context.MethodNameMap[member.field.FieldType]}<{context.InputBufferTypeName}>(
                        {context.InputBufferVariableName}, 
                        {context.OffsetVariableName} + {member.offset});");
            }

            // For little endian architectures, we can do the equivalent of a reinterpret_cast operation.
            string body = $@"
            if (BitConverter.IsLittleEndian)
            {{
                var mem = {context.InputBufferVariableName}.{nameof(IInputBuffer.GetReadOnlyByteMemory)}({context.OffsetVariableName}, {this.inlineSize});
                return {typeof(MemoryMarshal).FullName}.{nameof(MemoryMarshal.Cast)}<byte, {CSharpHelpers.GetCompilableTypeName(this.ClrType)}>(mem.Span)[0];
            }}
            else
            {{
                var item = default({CSharpHelpers.GetCompilableTypeName(this.ClrType)});
                {string.Join("\r\n", propertyStatements)}
                return item;
            }}
";

            return new CodeGeneratedMethod { MethodBody = body };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var propertyStatements = new List<string>();
            for (int i = 0; i < this.members.Count; ++i)
            {
                var member = this.members[i];
                propertyStatements.Add($@"
                    {context.MethodNameMap[member.field.FieldType]}(
                        {context.SpanWriterVariableName}, 
                        {context.SpanVariableName},
                        {context.ValueVariableName}.{member.field.Name},
                        {context.OffsetVariableName} + {member.offset},
                        {context.SerializationContextVariableName});");
            }

            // For little endian architectures, we can do the equivalent of a reinterpret_cast operation.
            string body = $@"
            if (BitConverter.IsLittleEndian)
            {{
                var tempSpan = {typeof(MemoryMarshal).FullName}.{nameof(MemoryMarshal.Cast)}<byte, {CSharpHelpers.GetCompilableTypeName(this.ClrType)}>({context.SpanVariableName}.Slice({context.OffsetVariableName}, {this.inlineSize}));
                tempSpan[0] = {context.ValueVariableName};
            }}
            else
            {{
                {string.Join("\r\n", propertyStatements)}
            }}
";

            return new CodeGeneratedMethod { MethodBody = body };
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            return string.Empty;
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            return string.Empty;
        }



        public override void Initialize()
        {
            if (!this.ClrType.Attributes.HasFlag(TypeAttributes.ExplicitLayout) ||
                this.ClrType.StructLayoutAttribute?.Value != LayoutKind.Explicit)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create struct type model from type {this.ClrType.Name} because it does not have a [StructLayout(LayoutKind.Explicit)] attribute.");
            }

            var properties = this.ClrType
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => new
                {
                    Field = x,
                    OffsetAttribute = x.GetCustomAttribute<FieldOffsetAttribute>(),
                })
                .Where(x => x.OffsetAttribute != null);

            this.inlineSize = 0;

            foreach (var item in properties)
            {
                var offsetAttribute = item.OffsetAttribute;
                var field = item.Field;

                ITypeModel propertyModel = this.typeModelContainer.CreateTypeModel(field.FieldType);

                if (!propertyModel.IsValidStructMember || propertyModel.PhysicalLayout.Length > 1)
                {
                    throw new InvalidFlatBufferDefinitionException($"Struct property {field.Name} with type {field.FieldType.Name} cannot be part of a flatbuffer struct.");
                }

                if (this.ClrType.IsValueType && !propertyModel.ClrType.IsValueType)
                {
                    throw new InvalidFlatBufferDefinitionException($"Struct {this.ClrType.Name} property {field.Name} must be a value type if the struct is a value type.");
                }

                int propertySize = propertyModel.PhysicalLayout[0].InlineSize;
                int propertyAlignment = propertyModel.PhysicalLayout[0].Alignment;
                this.maxAlignment = Math.Max(propertyAlignment, this.maxAlignment);

                // Pad for alignment.
                this.inlineSize += SerializationHelpers.GetAlignmentError(this.inlineSize, propertyAlignment);

                this.members.Add((this.inlineSize, field, propertyModel));
                if (offsetAttribute?.Value != this.inlineSize)
                {
                    throw new InvalidFlatBufferDefinitionException($"Struct '{this.ClrType.Name}' property '{field.Name}' defines invalid [FieldOffset attribute. Expected [FieldOffset({this.inlineSize})].");
                }

                this.inlineSize += propertyModel.PhysicalLayout[0].InlineSize;
            }

            if (!this.ClrType.IsPublic && !this.ClrType.IsNestedPublic)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {this.ClrType.Name} because it is not public.");
            }
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            foreach (var member in this.members)
            {
                if (seenTypes.Add(member.field.FieldType))
                {
                    member.model.TraverseObjectGraph(seenTypes);
                }
            }
        }
    }
}
