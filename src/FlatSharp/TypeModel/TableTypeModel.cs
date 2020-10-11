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
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.Attributes;

    /// <summary>
    /// Describes the schema of a FlatBuffer table. Tables, analgous to classes, provide for mutable schema definitions over time
    /// by declaring a vatble mapping of field indexes to offsets.
    /// </summary>
    public class TableTypeModel : RuntimeTypeModel
    {
        private readonly string ParseClassName = "tableReader_" + Guid.NewGuid().ToString("n");

        /// <summary>
        /// Maps vtable index -> type model.
        /// </summary>
        private readonly Dictionary<int, TableMemberModel> memberTypes = new Dictionary<int, TableMemberModel>();

        /// <summary>
        /// Contains the vtable indices that have already been occupied.
        /// </summary>
        private readonly HashSet<int> occupiedVtableSlots = new HashSet<int>();

        internal TableTypeModel(Type clrType, TypeModelContainer typeModelProvider) : base(clrType, typeModelProvider)
        {
        }

        /// <summary>
        /// Gets the schema type.
        /// </summary>
        public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Table;

        /// <summary>
        /// Layout when in a vtable.
        /// </summary>
        public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout => new PhysicalLayoutElement[] { new PhysicalLayoutElement(sizeof(uint), sizeof(uint)) }.ToImmutableArray();

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
        /// Tables are written by reference.
        /// </summary>
        public override bool SerializesInline => false;

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
        public TableMemberModel KeyMember { get; private set; }
        
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
                    ItemTypeModel = this.typeModelContainer.CreateTypeModel(x.Property.PropertyType),
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

                model = property.ItemTypeModel.AdjustTableMember(model);

                if (property.Attribute.Key)
                {
                    if (this.KeyMember != null)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} has more than one [FlatBufferItemAttribute] with Key set to true.");
                    }
                    
                    if (!property.ItemTypeModel.IsValidSortedVectorKey)
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} declares a key property on a type that that does not support being a key in a sorted vector.");
                    }

                    if (!property.ItemTypeModel.TryGetSpanComparerType(out _))
                    {
                        throw new InvalidFlatBufferDefinitionException($"Table {this.ClrType.Name} declares a key property on a type whose type model does not supply a ISpanComparer type.");
                    }

                    this.KeyMember = model;
                }

                ValidateSortedVector(model);

                for (int i = 0; i < model.ItemTypeModel.PhysicalLayout.Length; ++i)
                {
                    if (!this.occupiedVtableSlots.Add(index + i))
                    {
                        throw new InvalidFlatBufferDefinitionException($"FlatBuffer Table {this.ClrType.Name} already defines a property with index {index}. This may happen when unions are declared as these are double-wide members.");
                    }
                }

                this.memberTypes[index] = model;
            }
        }

        private static void ValidateSortedVector(TableMemberModel model)
        {
            if (model.IsSortedVector)
            {
                if (model.ItemTypeModel.SchemaType != FlatBufferSchemaType.Vector)
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares the sortedVector option, but the underlying type was not a vector.");
                }

                if (!model.ItemTypeModel.TryGetUnderlyingVectorType(out ITypeModel memberTypeModel))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares the sortedVector option, but the underlying type model did not report the underlying vector type.");
                }

                if (memberTypeModel.SchemaType != FlatBufferSchemaType.Table)
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares a sorted vector, but the member is not a table. Type = {model.ItemTypeModel?.ClrType.FullName}.");
                }

                if (!memberTypeModel.TryGetTableKeyMember(out TableMemberModel member))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares a sorted vector, but the member does not have a key defined. Type = {model.ItemTypeModel?.ClrType.FullName}.");
                }

                if (!member.ItemTypeModel.TryGetSpanComparerType(out _))
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares a sorted vector, but the key does not have an implementation of ISpanComparer. Keys must be non-nullable scalars or strings. KeyType = {member.ItemTypeModel.ClrType.FullName}");
                }

                if (member.ItemTypeModel.PhysicalLayout.Length != 1)
                {
                    throw new InvalidFlatBufferDefinitionException($"Property '{model.PropertyInfo.Name}' declares a sorted vector, but the sort key's vtable is not compatible with sorting. KeyType = {member.ItemTypeModel.ClrType.FullName}");
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
                {context.SpanWriterVariableName}.{nameof(SpanWriterExtensions.WriteUOffset)}({context.SpanVariableName}, {context.OffsetVariableName}, tableStart, {context.SerializationContextVariableName});
                int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

                Span<byte> vtable = stackalloc byte[{4 + 2 * (maxIndex + 1)}];
                int maxVtableIndex = -1;
                vtable.Clear(); // reset to 0. Random memory from the stack isn't trustworthy.
";

            List<string> body = new List<string>();
            List<string> writers = new List<string>();

            // load the properties of the object into locals.
            foreach (var kvp in this.IndexToMemberMap)
            {
                var (prepare, write) = GetStandardSerializeBlocks(kvp.Key, kvp.Value, context);
                body.Add(prepare);
                writers.Add(write);
            }

            // We probably over-allocated. Figure out by how much and back up the cursor.
            // Then we can write the vtable.
            body.Add("int tableLength = currentOffset - tableStart;");
            body.Add($"{context.SerializationContextVariableName}.{nameof(SerializationContext.Offset)} -= {maxInlineSize} - tableLength;");

            // write vtable header
            body.Add($"int vtableLength = 6 + (2 * maxVtableIndex);");
            body.Add($"{context.SpanWriterVariableName}.{nameof(ISpanWriter.WriteUShort)}(vtable, (ushort)vtableLength, 0, {context.SerializationContextVariableName});");
            body.Add($"{context.SpanWriterVariableName}.{nameof(ISpanWriter.WriteUShort)}(vtable, (ushort)tableLength, sizeof(ushort), {context.SerializationContextVariableName});");

            // Finish vtable.
            body.Add($"int vtablePosition = {context.SerializationContextVariableName}.{nameof(SerializationContext.FinishVTable)}({context.SpanVariableName}, vtable.Slice(0, vtableLength));");
            body.Add($"{context.SpanWriterVariableName}.{nameof(SpanWriter.WriteInt)}({context.SpanVariableName}, tableStart - vtablePosition, tableStart, {context.SerializationContextVariableName});");

            body.AddRange(writers);

            // These methods are often enormous, and inlining can have a detrimental effect on perf.
            return new CodeGeneratedMethod
            {
                MethodBody = $"{methodStart} \r\n {string.Join("\r\n", body)}",
            };
        }

        private (string prepareBlock, string serializeBlock) GetStandardSerializeBlocks(
            int index, 
            TableMemberModel memberModel,
            SerializationCodeGenContext context)
        {
            string OffsetVariableName(int i) => $"index{index + i}Offset";

            string valueVariableName = $"index{index}Value";

            // Set up a condition for serializing, unless the type model tells us not to.
            string condition = $"if ({valueVariableName} != {memberModel.DefaultValueToken})";
            if (memberModel.ItemTypeModel.MustAlwaysSerialize)
            {
                condition = string.Empty;
            }

            List<string> prepareBlockComponents = new List<string>();
            int vtableEntries = memberModel.ItemTypeModel.PhysicalLayout.Length;
            for (int i = 0; i < vtableEntries; ++i)
            {
                var layout = memberModel.ItemTypeModel.PhysicalLayout[i];

                prepareBlockComponents.Add($@"
                            currentOffset += {nameof(SerializationHelpers)}.{nameof(SerializationHelpers.GetAlignmentError)}(currentOffset, {layout.Alignment});
                            {OffsetVariableName(i)} = currentOffset;
                            {context.SpanWriterVariableName}.{nameof(ISpanWriter.WriteUShort)}(vtable, (ushort)(currentOffset - tableStart), {4 + 2 * (index + i)}, {context.SerializationContextVariableName});
                            maxVtableIndex = {index + i};
                            currentOffset += {layout.InlineSize};
                ");
            }

            string prepareBlock =
$@"
                    var {valueVariableName} = {context.ValueVariableName}.{memberModel.PropertyInfo.Name};
                    {string.Join("\r\n", Enumerable.Range(0, vtableEntries).Select(x => $"var {OffsetVariableName(x)} = 0;"))}
                    {condition} 
                    {{
                            {string.Join("\r\n", prepareBlockComponents)}
                    }}";

            string sortInvocation = string.Empty;
            if (memberModel.IsSortedVector)
            {
                if (!memberModel.ItemTypeModel.TryGetUnderlyingVectorType(out ITypeModel tableModel) ||
                    !tableModel.TryGetTableKeyMember(out TableMemberModel keyMember) ||
                    !keyMember.ItemTypeModel.TryGetSpanComparerType(out Type spanComparerType) ||
                    keyMember.ItemTypeModel.PhysicalLayout.Length != 1)
                {
                    string vtm = memberModel.ItemTypeModel.ClrType.FullName;
                    string ttm = tableModel?.GetType().FullName;
                    string ttn = tableModel?.ClrType.FullName;

                    throw new InvalidOperationException($"Internal error: Validation failed when writing sorted vector. VTM={vtm}, TTM={ttm}, TTN={ttn}");
                }

                string inlineSize = keyMember.ItemTypeModel.IsFixedSize ? keyMember.ItemTypeModel.PhysicalLayout[0].InlineSize.ToString() : "null";
                string defaultValue = keyMember.DefaultValueToken;

                sortInvocation = @$"
                    {context.SerializationContextVariableName}.{nameof(SerializationContext.AddPostSerializeAction)}(
                        (tempSpan, ctx) =>
                        {nameof(SortedVectorHelpers)}.{nameof(SortedVectorHelpers.SortVector)}(
                            tempSpan, 
                            {OffsetVariableName(0)}, 
                            {keyMember.Index}, 
                            {inlineSize}, 
                            new {CSharpHelpers.GetCompilableTypeName(spanComparerType)}({defaultValue})));";
            }

            string serializeBlock;
            if (vtableEntries == 1)
            {
                serializeBlock =
                    $@"
                    if ({OffsetVariableName(0)} != 0)
                    {{
                        {context.MethodNameMap[memberModel.ItemTypeModel.ClrType]}(
                            {context.SpanWriterVariableName},
                            {context.SpanVariableName},
                            {valueVariableName},
                            {OffsetVariableName(0)},
                            {context.SerializationContextVariableName});
                        {sortInvocation}
                    }}";
            }
            else
            {
                serializeBlock =
                    $@"
                    if ({OffsetVariableName(0)} != 0)
                    {{
                        var offsetTuple = ({string.Join(", ", Enumerable.Range(0, vtableEntries).Select(x => OffsetVariableName(x)))});
                        {context.MethodNameMap[memberModel.ItemTypeModel.ClrType]}(
                            {context.SpanWriterVariableName},
                            {context.SpanVariableName},
                            {valueVariableName},
                            ref offsetTuple,
                            {context.SerializationContextVariableName});
                        {sortInvocation}
                    }}";
            }

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
                if (value.ItemTypeModel.PhysicalLayout.Length > 1)
                {
                    propertyStuff = CreateWideTableProperty(value, index, context);
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
                MethodBody = $"return new {className}<{context.InputBufferTypeName}>({context.InputBufferVariableName}, {context.OffsetVariableName} + {context.InputBufferVariableName}.{nameof(InputBufferExtensions.ReadUOffset)}({context.OffsetVariableName}));"
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
            GeneratedProperty property = new GeneratedProperty(context.Options, index, memberModel);

            // These are always inline as they are only invoked from one place.
            property.ReadValueMethodDefinition =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    private static {CSharpHelpers.GetCompilableTypeName(propertyType)} {property.ReadValueMethodName}({context.InputBufferTypeName} buffer, int offset)
                    {{
                        int absoluteLocation = buffer.{nameof(InputBufferExtensions.GetAbsoluteTableFieldLocation)}(offset, {index});
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

        private static GeneratedProperty CreateWideTableProperty(
            TableMemberModel memberModel,
            int index,
            ParserCodeGenContext context)
        {
            const string FirstLocationVariableName = "firstLocation";

            Type propertyType = memberModel.ItemTypeModel.ClrType;
            GeneratedProperty property = new GeneratedProperty(context.Options, index, memberModel);

            List<string> locationGetters = new List<string> { FirstLocationVariableName };
            for (int i = 1; i < memberModel.ItemTypeModel.PhysicalLayout.Length; ++i)
            {
                locationGetters.Add($"buffer.{nameof(InputBufferExtensions.GetAbsoluteTableFieldLocation)}(offset, {index + i})");
            }

            // These are always inline as they are only invoked from one place.
            property.ReadValueMethodDefinition =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    private static {CSharpHelpers.GetCompilableTypeName(propertyType)} {property.ReadValueMethodName}({context.InputBufferTypeName} buffer, int offset)
                    {{
                        int {FirstLocationVariableName} = buffer.{nameof(InputBufferExtensions.GetAbsoluteTableFieldLocation)}(offset, {index});
                        if ({FirstLocationVariableName} == 0)
                        {{
                            return {memberModel.DefaultValueToken};
                        }}

                        var absoluteLocations = ({string.Join(", ", locationGetters)});
                        return {context.MethodNameMap[propertyType]}(buffer, ref absoluteLocations);
                    }}
";

            return property;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            int vtableEntryCount = this.MaxIndex + 1;

            // vtable length + table length + 2 * entryCount + padding to 2-byte alignment.
            int maxVtableSize = sizeof(ushort) * (2 + vtableEntryCount) + SerializationHelpers.GetMaxPadding(sizeof(ushort));
            int maxTableSize = this.NonPaddedMaxTableInlineSize + SerializationHelpers.GetMaxPadding(this.PhysicalLayout.Single().Alignment);

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

        public override bool TryGetTableKeyMember(out TableMemberModel tableMember)
        {
            tableMember = this.KeyMember;
            return tableMember != null;
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
