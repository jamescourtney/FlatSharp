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
    /// Describes the schema of a FlatBuffer table. Tables, analgous to classes, provide for mutable schema definitions over time
    /// by declaring a vatble mapping of field indexes to offsets.
    /// </summary>
    public class TableTypeModel : RuntimeTypeModel
    {
        /// <summary>
        /// Maps vtable index -> type model.
        /// </summary>
        private readonly Dictionary<int, TableMemberModel> memberTypes = new Dictionary<int, TableMemberModel>();

        /// <summary>
        /// Contains the vtable indices that have already been occupied.
        /// </summary>
        private readonly HashSet<int> occupiedVtableSlots = new HashSet<int>();

        internal TableTypeModel(Type clrType, ITypeModelProvider typeModelProvider) : base(clrType, typeModelProvider)
        {
        }

        /// <summary>
        /// Tables are always addressed by reference, so the alignment is uoffset_t.
        /// </summary>
        public override int Alignment => sizeof(uint);

        /// <summary>
        /// The inline size. Tables are always "reference" types, so this is a fixed uoffset_t.
        /// </summary>
        public override int InlineSize => sizeof(uint);

        /// <summary>
        /// Tables can have vectors and other arbitrary data.
        /// </summary>
        public override bool IsFixedSize => false;

        /// <summary>
        /// Tables can't be part of structs.
        /// </summary>
        public override bool IsValidStructMember => false;

        /// <summary>
        /// Tables can be part of tables.
        /// </summary>
        public override bool IsValidTableMember => true;

        /// <summary>
        /// Tables can be part of unions.
        /// </summary>
        public override bool IsValidUnionMember => true;

        /// <summary>
        /// Tables can be part of vectors.
        /// </summary>
        public override bool IsValidVectorMember => true;

        /// <summary>
        /// Tables can't be keys of sorted vectors.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        /// <summary>
        /// Gets the maximum used index in this vtable.
        /// </summary>
        public int MaxIndex => this.occupiedVtableSlots.Max();

        /// <summary>
        /// Maps the table index to the details about that member.
        /// </summary>
        public IReadOnlyDictionary<int, TableMemberModel> IndexToMemberMap => this.memberTypes;

        /// <summary>
        /// The default .ctor used for subclassing.
        /// </summary>
        public ConstructorInfo DefaultConstructor { get; private set; }

        /// <summary>
        /// The property type used as a key.
        /// </summary>
        public PropertyInfo KeyProperty { get; private set; }
        
        /// <summary>
        /// Gets the maximum size of a table assuming all members are populated include the vtable offset. 
        /// Does not consider alignment of the table, but does consider worst-case alignment of the members.
        /// </summary>
        internal int NonPaddedMaxTableInlineSize
        {
            // add sizeof(int) for soffset_t to vtable.
            get => this.IndexToMemberMap.Values.Sum(x => x.ItemTypeModel.MaxInlineSize) + sizeof(int);
        }

        public override void Initialize()
        {
            var tableAttribute = this.ClrType.GetCustomAttribute<FlatBufferTableAttribute>();
            if (tableAttribute == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create table type model from type {this.ClrType.Name} because it does not have a [FlatBufferTable] attribute.");
            }

            EnsureClassCanBeInheritedByOutsideAssembly(this.ClrType, out var ctor);
            this.DefaultConstructor = ctor;

            var properties = this.ClrType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(x => new
                {
                    Property = x,
                    Attribute = x.GetCustomAttribute<FlatBufferItemAttribute>(),
                })
                .Where(x => x.Attribute != null)
                .Where(x => !x.Attribute.Deprecated)
                .Select(x => new
                {
                    x.Property,
                    x.Attribute,
                    ItemTypeModel = this.typeModelProvider.CreateTypeModel(x.Property.PropertyType),
                })
                .ToList();

            ushort maxIndex = 0;

            if (!properties.Any())
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create table type model from type {this.ClrType.Name} because it does not have any non-static [FlatBufferItem] properties.");
            }

            foreach (var property in properties)
            {
                bool hasDefaultValue = false;

                if (property.Attribute.Key)
                {
                    if (this.KeyProperty != null)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} has more than one [FlatBufferItemAttribute] with Key set to true.");
                    }
                    
                    if (!property.ItemTypeModel.IsValidSortedVectorKey)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} declares a key property on a type that that does not support being a key in a sorted vector.");
                    }

                    this.KeyProperty = property.Property;
                }

                ValidateSortedVector(property.Attribute, property.ItemTypeModel, property.Property);

                object defaultValue = null;
                if (!object.ReferenceEquals(property.Attribute.DefaultValue, null))
                {
                    hasDefaultValue = true;
                    defaultValue = property.Attribute.DefaultValue;
                }
                
                ushort index = property.Attribute.Index;
                maxIndex = Math.Max(index, maxIndex);

                TableMemberModel model = new TableMemberModel(
                    property.ItemTypeModel,
                    property.Property,
                    index,
                    hasDefaultValue,
                    defaultValue,
                    property.Attribute.SortedVector,
                    property.Attribute.Key);

                for (int i = 0; i < model.VTableSlotCount; ++i)
                {
                    if (!this.occupiedVtableSlots.Add(index + i))
                    {
                        throw new InvalidFlatBufferDefinitionException($"FlatBuffer Table {this.ClrType.Name} already defines a property with index {index}. This may happen when unions are declared as these are double-wide members.");
                    }
                }

                this.memberTypes[index] = model;
            }
        }

        private static void ValidateSortedVector(FlatBufferItemAttribute itemAttribute, ITypeModel itemTypeModel, PropertyInfo propertyInfo)
        {
            if (itemAttribute.SortedVector)
            {
                // This must be a vector, and the inner model must have an item with the 'key' property.
                if (itemTypeModel is BaseVectorTypeModel vectorTypeModel)
                {
                    var vectorMemberModel = vectorTypeModel.ItemTypeModel;
                    if (vectorMemberModel is TableTypeModel tableModel)
                    {
                        if (tableModel.KeyProperty == null)
                        {
                            throw new InvalidFlatBufferDefinitionException($"Property '{propertyInfo.Name}' declares a sorted vector, but the member is a table that does not have a key.");
                        }
                    }
                    else
                    {
                        throw new InvalidFlatBufferDefinitionException($"Property '{propertyInfo.Name}' declares a sorted vector, but the member is not a table.");
                    }
                }
                else
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{propertyInfo.Name}' declares the sortedVector option, but the property is not a vector type.");
                }
            }
        }

        internal static void EnsureClassCanBeInheritedByOutsideAssembly(Type type, out ConstructorInfo defaultConstructor)
        {
            if (!type.IsClass)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is not a class.");
            }

            if (type.IsSealed)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is sealed.");
            }

            if (type.IsAbstract)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is abstract.");
            }

            if (type.BaseType != typeof(object))
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} its base class is not System.Object.");
            }

            if (!type.IsPublic && !type.IsNestedPublic)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't create type model from type {type.Name} because it is not public.");
            }

            defaultConstructor =
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => c.IsPublic || c.IsFamily || c.IsFamilyOrAssembly)
                .Where(c => c.GetParameters().Length == 0)
                .SingleOrDefault();

            if (defaultConstructor == null)
            {
                throw new InvalidFlatBufferDefinitionException($"Can't find a public/protected default default constructor for {type.Name}");
            }
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            var type = this.ClrType;
            int maxIndex = this.MaxIndex;
            int maxInlineSize = this.NonPaddedMaxTableInlineSize;

            // Start by asking for the worst-case number of bytes from the serializationcontext.
            string methodStart =
$@"
                int tableStart = {context.SerializationContextVariableName}.{nameof(SerializationContext.AllocateSpace)}({maxInlineSize}, sizeof(int));
                {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, tableStart, {context.SerializationContextVariableName});
                int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

                var vtable = {context.SerializationContextVariableName}.{nameof(SerializationContext.VTableBuilder)};
                vtable.StartObject({maxIndex});
";

            List<string> body = new List<string>();
            List<string> writers = new List<string>();

            // load the properties of the object into locals.
            foreach (var kvp in this.IndexToMemberMap)
            {
                string prepare, write;
                if (kvp.Value.ItemTypeModel is UnionTypeModel)
                {
                    (prepare, write) = GetUnionSerializeBlocks(kvp.Key, kvp.Value, context);
                }
                else
                {
                    (prepare, write) = GetStandardSerializeBlocks(kvp.Key, kvp.Value, context);
                }

                body.Add(prepare);
                writers.Add(write);
            }

            // We probably over-allocated. Figure out by how much and back up the cursor.
            // Then we can write the vtable.
            body.Add("int tableLength = currentOffset - tableStart;");
            body.Add($"{context.SerializationContextVariableName}.{nameof(SerializationContext.Offset)} -= {maxInlineSize} - tableLength;");
            body.Add($"int vtablePosition = vtable.{nameof(VTableBuilder.EndObject)}(span, {context.SpanWriterVariableName}, tableLength);");
            body.Add($"{context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}(span, tableStart - vtablePosition, tableStart, context);");

            body.AddRange(writers);

            // These methods are often enormous, and inlining can have a detrimental effect on perf.
            return new CodeGeneratedMethod
            {
                MethodBody = $"{methodStart} \r\n {string.Join("\r\n", body)}",
                IsMethodInline = false,
            };
        }

        private (string prepareBlock, string serializeBlock) GetStandardSerializeBlocks(
            int index, 
            TableMemberModel memberModel,
            SerializationCodeGenContext context)
        {
            string valueVariableName = $"index{index}Value";
            string offsetVariableName = $"index{index}Offset";

            string condition = $"if ({valueVariableName} != {memberModel.DefaultValueToken})";
            if ((memberModel.ItemTypeModel is MemoryVectorTypeModel) || memberModel.IsKey)
            {
                // 1) Memory is a struct and can't be null, and 0-length vectors are valid.
                //    Therefore, we just need to omit the conditional check entirely.

                // 2) For sorted vector keys, we must include the value since some other 
                //    libraries cannot do binary search with omitted keys.
                condition = string.Empty;
            }

            string keyCheckMethodCall = string.Empty;
            if (memberModel.IsKey)
            {
                keyCheckMethodCall = $"{nameof(SortedVectorHelpers)}.{nameof(SortedVectorHelpers.EnsureKeyNonNull)}({valueVariableName});";
            }

            string prepareBlock =
$@"
                    var {valueVariableName} = {context.ValueVariableName}.{memberModel.PropertyInfo.Name};
                    int {offsetVariableName} = 0;
                    {keyCheckMethodCall}
                    {condition} 
                    {{
                            currentOffset += {CSharpHelpers.GetFullMethodName(ReflectedMethods.SerializationHelpers_GetAlignmentErrorMethod)}(currentOffset, {memberModel.ItemTypeModel.Alignment});
                            {offsetVariableName} = currentOffset;
                            vtable.{nameof(VTableBuilder.SetOffset)}({index}, currentOffset - tableStart);
                            currentOffset += {memberModel.ItemTypeModel.InlineSize};
                    }}";

            string sortInvocation = string.Empty;
            if (memberModel.IsSortedVector)
            {
                BaseVectorTypeModel vectorModel = (BaseVectorTypeModel)memberModel.ItemTypeModel;
                TableTypeModel tableModel = (TableTypeModel)vectorModel.ItemTypeModel;
                TableMemberModel keyMember = tableModel.IndexToMemberMap.Single(x => x.Value.PropertyInfo == tableModel.KeyProperty).Value;
                ITypeModel keyTypeModel = keyMember.ItemTypeModel;
                ISpanComparerProvider spanComparerProvider = keyTypeModel as ISpanComparerProvider;

                if (spanComparerProvider == null)
                {
                    throw new InvalidFlatBufferDefinitionException("Key type models must implement the 'ISpanComparerProvider' interface.");
                }

                string inlineSize = keyTypeModel is ScalarTypeModel ? keyTypeModel.InlineSize.ToString() : "null";

                string defaultValue = keyMember.DefaultValueToken;

                sortInvocation = $"{nameof(SortedVectorHelpers)}.{nameof(SortedVectorHelpers.SortVector)}(" +
                                    $"span, {offsetVariableName}, {keyMember.Index}, {inlineSize}, new {CSharpHelpers.GetCompilableTypeName(spanComparerProvider.SpanComparerType)}({defaultValue}))";

                sortInvocation = $"{context.SerializationContextVariableName}.{nameof(SerializationContext.AddPostSerializeAction)}((span, ctx) => {sortInvocation});";
            }

            string serializeBlock =
$@"
                    if ({offsetVariableName} != 0)
                    {{
                        {context.With(valueVariableName: valueVariableName, offsetVariableName: offsetVariableName)
                                .GetSerializeInvocation(memberModel.ItemTypeModel.ClrType)};
                        {sortInvocation}
                    }}";

            return (prepareBlock, serializeBlock);
        }

        private static (string prepareBlock, string serializeBlock) GetUnionSerializeBlocks(
            int index, 
            TableMemberModel memberModel, 
            SerializationCodeGenContext context)
        {
            UnionTypeModel unionModel = (UnionTypeModel)memberModel.ItemTypeModel;

            string valueVariableName = $"index{index}Value";
            string discriminatorOffsetVariableName = $"index{index}DiscriminatorOffset";
            string valueOffsetVariableName = $"index{index}ValueOffset";
            string discriminatorValueVariableName = $"index{index}Discriminator";

            string prepareBlock =
$@"
                    var {valueVariableName} = {context.ValueVariableName}.{memberModel.PropertyInfo.Name};
                    int {discriminatorOffsetVariableName} = 0;
                    int {valueOffsetVariableName} = 0;
                    byte {discriminatorValueVariableName} = 0;

                    if (!object.ReferenceEquals({valueVariableName}, null) && {valueVariableName}.{nameof(FlatBufferUnion<int>.Discriminator)} != 0)
                    {{
                            {discriminatorValueVariableName} = {valueVariableName}.{nameof(FlatBufferUnion<int>.Discriminator)};
                            {discriminatorOffsetVariableName} = currentOffset;
                            vtable.{nameof(VTableBuilder.SetOffset)}({index}, currentOffset - tableStart);
                            currentOffset++;

                            currentOffset += {CSharpHelpers.GetFullMethodName(ReflectedMethods.SerializationHelpers_GetAlignmentErrorMethod)}(currentOffset, sizeof(uint));
                            {valueOffsetVariableName} = currentOffset;
                            vtable.{nameof(VTableBuilder.SetOffset)}({index + 1}, currentOffset - tableStart);
                            currentOffset += sizeof(uint);
                    }}";

            List<string> switchCases = new List<string>();
            for (int i = 0; i < unionModel.UnionElementTypeModel.Length; ++i)
            {
                var elementModel = unionModel.UnionElementTypeModel[i];
                var unionIndex = i + 1;

                string structAdjustment = string.Empty;
                if (elementModel is StructTypeModel)
                {
                    // Structs are generally written in-line, with the exception of unions.
                    // So, we need to do the normal allocate space dance here, since we're writing
                    // a pointer to a struct.
                    structAdjustment =
$@"
                        var writeOffset = context.{nameof(SerializationContext.AllocateSpace)}({elementModel.InlineSize}, {elementModel.Alignment});
                        {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteUOffset)}(span, {valueOffsetVariableName}, writeOffset, context);
                        {valueOffsetVariableName} = writeOffset;";
                }

                string @case =
$@"
                    case {unionIndex}:
                    {{
                        {structAdjustment}
                        {context.MethodNameMap[elementModel.ClrType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, {valueVariableName}.Item{unionIndex}, {valueOffsetVariableName}, {context.SerializationContextVariableName});
                    }}
                        break;";

                switchCases.Add(@case);
            }

            string serializeBlock =
$@"
                    if ({discriminatorOffsetVariableName} != 0)
                    {{
                        {context.SpanWriterVariableName}.{nameof(SpanWriter.WriteByte)}({context.SpanVariableName}, {discriminatorValueVariableName}, {discriminatorOffsetVariableName}, {context.SerializationContextVariableName});
                        switch ({discriminatorValueVariableName})
                        {{
                            {string.Join("\r\n", switchCases)}
                            default: throw new InvalidOperationException(""Unexpected"");
                        }}
                    }}";

            return (prepareBlock, serializeBlock);
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            // We have to implement two items: The table class and the overall "read" method.
            // Let's start with the read method.
            string className = "tableReader_" + Guid.NewGuid().ToString("n");

            // Build up a list of property overrides.
            var propertyOverrides = new List<GeneratedProperty>();
            foreach (var item in this.IndexToMemberMap)
            {
                int index = item.Key;
                var value = item.Value;

                GeneratedProperty propertyStuff;
                if (value.ItemTypeModel is UnionTypeModel)
                {
                    propertyStuff = CreateUnionTableGetter(value, index, context);
                }
                else
                {
                    propertyStuff = CreateStandardTableProperty(value, index, context);
                }

                propertyOverrides.Add(propertyStuff);
            }

            string classDefinition = CSharpHelpers.CreateDeserializeClass(
                className,
                this.ClrType,
                propertyOverrides,
                context.Options);

            return new CodeGeneratedMethod
            {
                ClassDefinition = classDefinition,
                MethodBody = $"return new {className}({context.InputBufferVariableName}, {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBuffer.ReadUOffset)}({context.OffsetVariableName}));"
            };
        }

        /// <summary>
        /// Generates a standard getter for a normal vtable entry.
        /// </summary>
        private static GeneratedProperty CreateStandardTableProperty(
            TableMemberModel memberModel, 
            int index, 
            ParserCodeGenContext context)
        {
            Type propertyType = memberModel.ItemTypeModel.ClrType;
            GeneratedProperty property = new GeneratedProperty(context.Options, index, memberModel.PropertyInfo);

            property.ReadValueMethodDefinition =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    private static {CSharpHelpers.GetCompilableTypeName(propertyType)} {property.ReadValueMethodName}({nameof(InputBuffer)} buffer, int offset)
                    {{
                        int absoluteLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(offset, {index});
                        if (absoluteLocation == 0) {{
                            return {memberModel.DefaultValueToken};
                        }}
                        else {{
                            return {context.MethodNameMap[propertyType]}(buffer, absoluteLocation);
                        }}
                    }}
";

            return property;
        }

        /// <summary>
        /// Generates a special property getter for union types. This stems from
        /// the fact that unions occupy two spots in the table's vtable to deserialize one
        /// logical field. This means that the logic to read them must also be special.
        /// </summary>
        private static GeneratedProperty CreateUnionTableGetter(
            TableMemberModel memberModel, 
            int index, 
            ParserCodeGenContext context)
        {
            Type propertyType = memberModel.ItemTypeModel.ClrType;
            UnionTypeModel unionModel = (UnionTypeModel)memberModel.ItemTypeModel;

            GeneratedProperty generatedProperty = new GeneratedProperty(context.Options, index, memberModel.PropertyInfo);

            // Start by generating switch cases. The codegen'ed union types have
            // well-defined constructors for each constituent type, so this .ctor
            // will always be available.
            List<string> switchCases = new List<string>();
            for (int i = 0; i < unionModel.UnionElementTypeModel.Length; ++i)
            {
                var unionMember = unionModel.UnionElementTypeModel[i];
                int unionIndex = i + 1;

                string structOffsetAdjustment = string.Empty;
                if (unionMember is StructTypeModel)
                {
                    structOffsetAdjustment = $"offsetLocation += buffer.{nameof(InputBuffer.ReadUOffset)}(offsetLocation);";
                }

                string @case =
$@"
                    case {unionIndex}:
                        {structOffsetAdjustment}
                        return new {CSharpHelpers.GetCompilableTypeName(unionModel.ClrType)}({context.MethodNameMap[unionMember.ClrType]}(buffer, offsetLocation));
";
                switchCases.Add(@case);
            }


            generatedProperty.ReadValueMethodDefinition =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    private static {CSharpHelpers.GetCompilableTypeName(propertyType)} {generatedProperty.ReadValueMethodName}({nameof(InputBuffer)} buffer, int offset)
                    {{
                        int discriminatorLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(offset, {index});
                        int offsetLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(offset, {index + 1});
                            
                        if (discriminatorLocation == 0) {{
                            return {memberModel.DefaultValueToken};
                        }}
                        else {{
                            byte discriminator = buffer.{nameof(InputBuffer.ReadByte)}(discriminatorLocation);
                            if (discriminator == 0 && offsetLocation != 0)
                                throw new System.IO.InvalidDataException(""FlatBuffer union had discriminator set but no offset."");
                            switch (discriminator)
                            {{
                                {string.Join("\r\n", switchCases)}
                                default:
                                    return {memberModel.DefaultValueToken};
                            }}
                        }}
                    }}
";
            return generatedProperty;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            int vtableEntryCount = this.MaxIndex + 1;

            // vtable length + table length + 2 * entryCount + padding to 2-byte alignment.
            int maxVtableSize = sizeof(ushort) * (2 + vtableEntryCount) + SerializationHelpers.GetMaxPadding(sizeof(ushort));
            int maxTableSize = this.NonPaddedMaxTableInlineSize + SerializationHelpers.GetMaxPadding(this.Alignment);

            List<string> statements = new List<string>();
            foreach (var kvp in this.IndexToMemberMap)
            {
                int index = kvp.Key;
                var member = kvp.Value;
                var itemModel = member.ItemTypeModel;

                if (itemModel.IsFixedSize)
                {
                    // This should already be accounted for in table.NonPaddedMax size above.
                    continue;
                }

                string variableName = $"index{index}Value";
                statements.Add($"var {variableName} = {context.ValueVariableName}.{member.PropertyInfo.Name};");

                string statement =
$@" 
                    if ({itemModel.GetNonNullConditionExpression(variableName)})
                    {{
                        runningSum += {context.MethodNameMap[itemModel.ClrType]}({variableName});
                    }}";

                statements.Add(statement);
            }

            string body =
$@"
            int runningSum = {maxTableSize} + {maxVtableSize};
            {string.Join("\r\n", statements)};
            return runningSum;
";
            return new CodeGeneratedMethod { MethodBody = body };
        }

        public override string GetNonNullConditionExpression(string itemVariableName)
        {
            return $"{itemVariableName} != null";
        }

        public override string GetThrowIfNullInvocation(string itemVariableName)
        {
            return $"{nameof(SerializationHelpers)}.{nameof(SerializationHelpers.EnsureNonNull)}({itemVariableName})";
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            seenTypes.Add(this.ClrType);
            foreach (var member in this.memberTypes.Values)
            {
                if (seenTypes.Add(member.ItemTypeModel.ClrType))
                {
                    member.ItemTypeModel.TraverseObjectGraph(seenTypes);
                }
            }
        }
    }
}
