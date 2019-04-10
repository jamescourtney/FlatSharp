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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Generates a collection of methods to help serialize the given root type.
    /// </summary>
    internal class ParserCodeGenerator
    {
        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);

        private readonly FlatBufferSerializerOptions options;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public ParserCodeGenerator(FlatBufferSerializerOptions options, IReadOnlyDictionary<Type, string> methodNames)
        {
            this.options = options;
            this.MethodNames = methodNames;
        }

        public IReadOnlyDictionary<Type, string> MethodNames { get; }

        public IEnumerable<SyntaxNode> MethodDeclarations => this.methodDeclarations;

        public void ImplementMethods()
        {
            foreach (var pair in this.MethodNames)
            {
                Type type = pair.Key;
                this.ImplementMethod(type);
            }
        }

        private void ImplementMethod(Type type)
        {
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            if (typeModel is TableTypeModel tableModel)
            {
                this.ImplementTableReadMethod(tableModel);
            }
            else if (typeModel is StructTypeModel structModel)
            {
                this.ImplementStructReadMethod(structModel);
            }
            else if (typeModel is VectorTypeModel vectorModel)
            {
                if (vectorModel.IsMemoryVector)
                {
                    this.ImplementMemoryVectorReadMethod(vectorModel);
                }
                else if (vectorModel.IsArray)
                {
                    this.ImplementArrayVectorReadMethod(vectorModel);
                }
                else
                {
                    this.ImplementListVectorReadMethod(vectorModel);
                }
            }
            else if (typeModel is UnionTypeModel unionModel)
            {
                // Explicitly left empty.
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void ImplementTableReadMethod(TableTypeModel typeModel)
        {
            // We have to implement two items: The table class and the overall "read" method.
            // Let's start with the read method.
            string className = "tableReader_" + Guid.NewGuid().ToString("n");

            // Static factory method.
            this.GenerateMethodDefinition(typeModel.ClrType, $"return new {className}(memory, offset + memory.{nameof(InputBuffer.ReadUOffset)}(offset));");

            // Implement the class
            {
                // Build up a list of property overrides.
                List<string> propertyOverrides = new List<string>();
                List<string> fieldDefinitions = new List<string>();
                foreach (var item in typeModel.IndexToMemberMap)
                {
                    int index = item.Key;
                    var value = item.Value;
                    PropertyInfo propertyInfo = value.PropertyInfo;
                    Type propertyType = propertyInfo.PropertyType;
                    string compilableTypeName = CSharpHelpers.GetCompilableTypeName(propertyType);

                    string hasValueFieldName = $"hasIndex{index}";
                    string valueFieldName = $"index{index}Value";

                    fieldDefinitions.Add($"private bool {hasValueFieldName};");
                    fieldDefinitions.Add($"private {compilableTypeName} {valueFieldName};");

                    string getter;
                    if (value.ItemTypeModel is UnionTypeModel)
                    {
                        getter = this.CreateUnionTableGetter(value, index, valueFieldName, hasValueFieldName);
                    }
                    else
                    {
                        getter = this.CreateStandardTableGetter(value, index, valueFieldName, hasValueFieldName);
                    }

                    string setter = string.Empty;
                    if (!value.IsReadOnly)
                    {
                        setter = "set { throw new NotMutableException(); }";
                    }

                    string @override =
$@"
                    {CSharpHelpers.GetAccessModifier(propertyInfo.GetGetMethod())} override {compilableTypeName} {propertyInfo.Name}
                    {{
                        {getter}
                        {setter}
                    }}
";

                    propertyOverrides.Add(@override);
                }

                string classDefinition =
$@"
                private sealed class {className} : {CSharpHelpers.GetCompilableTypeName(typeModel.ClrType)}
                {{
                    private readonly InputBuffer buffer;
                    private readonly int offset;

                    {string.Join("\r\n", fieldDefinitions)}
        
                    public {className}(InputBuffer buffer, int offset)
                    {{
                        this.buffer = buffer;
                        this.offset = offset;
                    }}

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
                var node = CSharpSyntaxTree.ParseText(classDefinition, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }
        }

        /// <summary>
        /// Generates a standard getter for a normal vtable entry.
        /// </summary>
        private string CreateStandardTableGetter(TableMemberModel memberModel, int index, string valueFieldName, string hasValueFieldName)
        {
            Type propertyType = memberModel.ItemTypeModel.ClrType;
            string defaultValue = CSharpHelpers.GetDefaultValueToken(memberModel);

            return
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get 
                    {{
                        if (!this.{hasValueFieldName})
                        {{
                            var buffer = this.buffer;
                            int absoluteLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(this.offset, {index});
                            if (absoluteLocation == 0) {{
                                this.{valueFieldName} = {defaultValue};
                            }}
                            else {{
                                this.{valueFieldName} = {this.GetReadInvocation(propertyType, "buffer", "absoluteLocation")};
                            }}
                            this.{hasValueFieldName} = true;
                        }}

                        return this.{valueFieldName};
                    }}
";
        }

        /// <summary>
        /// Generates a special property getter for union types. This stems from
        /// the fact that unions occupy two spots in the table's vtable to deserialize one
        /// logical field. This means that the logic to read them must also be special.
        /// </summary>
        private string CreateUnionTableGetter(TableMemberModel memberModel, int index, string valueFieldName, string hasValueFieldName)
        {
            Type propertyType = memberModel.ItemTypeModel.ClrType;
            string defaultValue = CSharpHelpers.GetDefaultValueToken(memberModel);

            UnionTypeModel unionModel = (UnionTypeModel)memberModel.ItemTypeModel;

            // Start by generating switch cases. The codegen'ed union types have
            // well-defined constructors for each constituent type, so this .ctor
            // will always be available.
            List<string> switchCases = new List<string>();
            for (int i = 0; i < unionModel.UnionElementTypeModel.Length; ++i)
            {
                var unionMember = unionModel.UnionElementTypeModel[i];
                int unionIndex = i + 1;
                string structOffsetAdjustment = string.Empty;
                if (unionMember.SchemaType == FlatBufferSchemaType.Struct)
                {
                    structOffsetAdjustment = $"offsetLocation += buffer.{nameof(InputBuffer.ReadUOffset)}(offsetLocation);";
                }

                string @case =
$@"
                    case {unionIndex}:
                        {structOffsetAdjustment}
                        this.{valueFieldName} = new {CSharpHelpers.GetCompilableTypeName(unionModel.ClrType)}({this.GetReadInvocation(unionMember.ClrType, "buffer", "offsetLocation")});
                        break;
";

                switchCases.Add(@case);
            }

            return
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get 
                {{
                    if (!this.{hasValueFieldName})
                    {{
                        var buffer = this.buffer;
                        int discriminatorLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(this.offset, {index});
                        int offsetLocation = buffer.{nameof(InputBuffer.GetAbsoluteTableFieldLocation)}(this.offset, {index + 1});
                            
                        if (discriminatorLocation == 0) {{
                            this.{valueFieldName} = {defaultValue};
                        }}
                        else {{
                            byte discriminator = buffer.{nameof(InputBuffer.ReadByte)}(discriminatorLocation);
                            if (discriminator == 0 && offsetLocation != 0)
                                throw new System.IO.InvalidDataException(""FlatBuffer union had discriminator set but no offset."");
                            switch (discriminator)
                            {{
                                {string.Join("\r\n", switchCases)}
                                default:
                                    this.{valueFieldName} = {defaultValue};
                                    break;
                            }}
                        }}
                        this.{hasValueFieldName} = true;
                    }}
                    return this.{valueFieldName};
                }}
";
        }
        
        private void ImplementStructReadMethod(StructTypeModel typeModel)
        {
            // We have to implement two items: The table class and the overall "read" method.
            // Let's start with the read method.
            string className = "structReader_" + Guid.NewGuid().ToString("n");

            // Static factory method.
            this.GenerateMethodDefinition(typeModel.ClrType, $"return new {className}(memory, offset);");

            // Implement the class
            {
                // Build up a list of property overrides.
                List<string> propertyOverrides = new List<string>();
                List<string> fieldDefinitions = new List<string>();
                for (int index = 0; index < typeModel.Members.Count; ++index)
                {
                    var value = typeModel.Members[index];
                    PropertyInfo propertyInfo = value.PropertyInfo;
                    Type propertyType = propertyInfo.PropertyType;
                    string compilableTypeName = CSharpHelpers.GetCompilableTypeName(propertyType);

                    string hasValueFieldName = $"hasIndex{index}";
                    string valueFieldName = $"index{index}Value";

                    fieldDefinitions.Add($"private bool {hasValueFieldName};");
                    fieldDefinitions.Add($"private {compilableTypeName} {valueFieldName};");

                    string getter =
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get 
                    {{
                        if (!this.{hasValueFieldName})
                        {{
                            this.{valueFieldName} = {this.GetReadInvocation(propertyType, "this.buffer", $"this.offset + {value.Offset}")};
                            this.{hasValueFieldName} = true;
                        }}

                        return this.{valueFieldName};
                    }}
";

                    string setter = string.Empty;
                    if (!value.IsReadOnly)
                    {
                        setter = "set { throw new NotMutableException(); }";
                    }

                    string @override =
$@"
                    {CSharpHelpers.GetAccessModifier(propertyInfo.GetGetMethod())} override {compilableTypeName} {propertyInfo.Name}
                    {{
                        {getter}
                        {setter}
                    }}
";

                    propertyOverrides.Add(@override);
                }

                string classDefinition =
$@"
                private sealed class {className} : {CSharpHelpers.GetCompilableTypeName(typeModel.ClrType)}
                {{
                    private readonly InputBuffer buffer;
                    private readonly int offset;

                    {string.Join("\r\n", fieldDefinitions)}
        
                    public {className}(InputBuffer buffer, int offset)
                    {{
                        this.buffer = buffer;
                        this.offset = offset;
                    }}

                    {string.Join("\r\n", propertyOverrides)}
                }}
";
                var node = CSharpSyntaxTree.ParseText(classDefinition, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }
        }

        private void ImplementMemoryVectorReadMethod(VectorTypeModel typeModel)
        {
            string readMethodName = this.MethodNames[typeModel.ClrType];
            string invocation = $"{nameof(InputBuffer.ReadMemoryBlock)}<{CSharpHelpers.GetCompilableTypeName(typeModel.ItemTypeModel.ClrType)}>";
            if (typeModel.ItemTypeModel.ClrType == typeof(byte))
            {
                invocation = nameof(InputBuffer.ReadByteMemoryBlock);
            }

            this.GenerateMethodDefinition(typeModel.ClrType, $"return memory.{invocation}(offset, {typeModel.ItemTypeModel.InlineSize});");
        }

        private void ImplementListVectorReadMethod(VectorTypeModel typeModel)
        {
            string kindToCreate = nameof(FlatBufferVector<byte>);
            if (this.options.CacheListVectorData)
            {
                kindToCreate = nameof(FlatBufferCacheVector<byte>);
            }

            this.GenerateMethodDefinition(typeModel.ClrType, $"return {this.CreateFlatBufferVector(typeModel, kindToCreate)};");
        }

        private void ImplementArrayVectorReadMethod(VectorTypeModel typeModel)
        {
            var itemTypeModel = typeModel.ItemTypeModel;

            string statement;
            if (itemTypeModel is ScalarTypeModel scalarModel && (scalarModel.InlineSize == 1 || BitConverter.IsLittleEndian))
            {
                statement = $"memory.{nameof(InputBuffer.ReadMemoryBlock)}<{CSharpHelpers.GetCompilableTypeName(itemTypeModel.ClrType)}>(offset, {itemTypeModel.InlineSize}).ToArray()";
            }
            else
            {
                statement = $"{this.CreateFlatBufferVector(typeModel, nameof(FlatBufferVector<byte>))}.ToArray()";
            }

            this.GenerateMethodDefinition(typeModel.ClrType, $"return {statement};");
        }

        private string CreateFlatBufferVector(VectorTypeModel typeModel, string vectorTypeName)
        {
            return $@"new {vectorTypeName}<{CSharpHelpers.GetCompilableTypeName(typeModel.ItemTypeModel.ClrType)}>(
                    memory, 
                    offset + memory.{nameof(InputBuffer.ReadUOffset)}(offset), 
                    {typeModel.ItemTypeModel.InlineSize}, 
                    (b, o) => {this.GetReadInvocation(typeModel.ItemTypeModel.ClrType, "b", "o")})";
        }

        private string GetReadInvocation(Type type, string buffer, string offset)
        {
            if (ReflectedMethods.InputBufferReaders.TryGetValue(type, out var readMethod))
            {
                return $"{buffer}.{readMethod.Name}({offset})";
            }
            else
            {
                return $"{this.MethodNames[type]}({buffer}, {offset})";
            }
        }

        private void GenerateMethodDefinition(Type type, string body)
        {
            string methodDef = 
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static {CSharpHelpers.GetCompilableTypeName(type)} {this.MethodNames[type]} (InputBuffer memory, int offset)
            {{
                {body}
            }}
";

            var node = CSharpSyntaxTree.ParseText(methodDef, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }
    }
}
