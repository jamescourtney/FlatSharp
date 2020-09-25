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
        /// Layout when in a vtable.
        /// </summary>
        public override VTableEntry[] VTableLayout { get; } = new VTableEntry[] { new VTableEntry(sizeof(uint), sizeof(uint)) };

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

                for (int i = 0; i < model.ItemTypeModel.VTableLayout.Length; ++i)
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
                Span<int> offsets = stackalloc int[{maxIndex + 1}];
                int offsetTemp;
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

            string condition = $"if ({valueVariableName} != {memberModel.DefaultValueToken})";
            if (memberModel.ItemTypeModel is MemoryVectorTypeModel)
            {
                // 1) Memory is a struct and can't be null, and 0-length vectors are valid.
                //    Therefore, we just need to omit the conditional check entirely.
                condition = string.Empty;
            }

            List<string> prepareBlockComponents = new List<string>();
            for (int i = 0; i < memberModel.ItemTypeModel.VTableLayout.Length; ++i)
            {
                var layout = memberModel.ItemTypeModel.VTableLayout[i];

                prepareBlockComponents.Add($@"
                            currentOffset += {CSharpHelpers.GetFullMethodName(ReflectedMethods.SerializationHelpers_GetAlignmentErrorMethod)}(currentOffset, {layout.Alignment});
                            offsets[{index + i}] = currentOffset;
                            vtable.{nameof(VTableBuilder.SetOffset)}({index + i}, currentOffset - tableStart);
                            currentOffset += {layout.InlineSize};
                ");
            }

            string prepareBlock =
$@"
                    var {valueVariableName} = {context.ValueVariableName}.{memberModel.PropertyInfo.Name};
                    {condition} 
                    {{
                            {string.Join("\r\n", prepareBlockComponents)}
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

                if (keyTypeModel.VTableLayout.Length != 1)
                {
                    throw new InvalidFlatBufferDefinitionException($"'{keyTypeModel.ClrType.Name}' cannot be used as a sorted vector key (wrong vtable layout).");
                }

                string inlineSize = keyTypeModel is ScalarTypeModel ? keyTypeModel.VTableLayout[0].InlineSize.ToString() : "null";

                string defaultValue = keyMember.DefaultValueToken;

                sortInvocation = @$"
                    int index{index}Offset0 = offsets[{index}];
                    {context.SerializationContextVariableName}.{nameof(SerializationContext.AddPostSerializeAction)}(
                        (span, ctx) =>
                        {nameof(SortedVectorHelpers)}.{nameof(SortedVectorHelpers.SortVector)}(
                            span, 
                            index{index}Offset0, 
                            {keyMember.Index}, 
                            {inlineSize}, 
                            new {CSharpHelpers.GetCompilableTypeName(spanComparerProvider.SpanComparerType)}({defaultValue})));";
            }

            string adjustedOffsetVariableName = "offsetTemp";
            if (memberModel.ItemTypeModel.VTableLayout.Length != 1)
            {
                // simplify implementation for normal-width vtables.
                adjustedOffsetVariableName = $"offsets.Slice({index}, {memberModel.ItemTypeModel.VTableLayout.Length})";
            }

            string serializeBlock =
                $@"
                    offsetTemp = offsets[{index}];
                    if (offsetTemp != 0)
                    {{
                        {context.MethodNameMap[memberModel.ItemTypeModel.ClrType]}(
                            {context.SpanWriterVariableName},
                            {context.SpanVariableName},
                            {valueVariableName},
                            {adjustedOffsetVariableName},
                            {context.SerializationContextVariableName});
                        {sortInvocation}
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
                if (value.ItemTypeModel.VTableLayout.Length > 1)
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

        private static GeneratedProperty CreateWideTableProperty(
            TableMemberModel memberModel,
            int index,
            ParserCodeGenContext context)
        {
            Type propertyType = memberModel.ItemTypeModel.ClrType;
            GeneratedProperty property = new GeneratedProperty(context.Options, index, memberModel.PropertyInfo);

            List<string> locationGetters = new List<string>();
            for (int i = 1; i < memberModel.ItemTypeModel.VTableLayout.Length; ++i)
            {
                locationGetters.Add($"absoluteLocations[{i}] = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(offset, {index + i});");
            }

            property.ReadValueMethodDefinition =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    private static {CSharpHelpers.GetCompilableTypeName(propertyType)} {property.ReadValueMethodName}({nameof(InputBuffer)} buffer, int offset)
                    {{
                        int firstLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(offset, {index});
                        if (firstLocation == 0)
                        {{
                            return {memberModel.DefaultValueToken};
                        }}

                        Span<int> absoluteLocations = stackalloc int[{memberModel.ItemTypeModel.VTableLayout.Length}];
                        absoluteLocations[0] = firstLocation;

                        {string.Join("\r\n", locationGetters)}
                        return {context.MethodNameMap[propertyType]}(buffer, absoluteLocations);
                    }}
";

            return property;
        }

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            int vtableEntryCount = this.MaxIndex + 1;

            // vtable length + table length + 2 * entryCount + padding to 2-byte alignment.
            int maxVtableSize = sizeof(ushort) * (2 + vtableEntryCount) + SerializationHelpers.GetMaxPadding(sizeof(ushort));
            int maxTableSize = this.NonPaddedMaxTableInlineSize + SerializationHelpers.GetMaxPadding(this.VTableLayout.Single().Alignment);

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
