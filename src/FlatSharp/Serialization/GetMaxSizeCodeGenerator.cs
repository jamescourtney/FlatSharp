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
    using System.Diagnostics;
    using System.Linq;
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Generates a collection of methods to help compute the maximum serialized size for a given type.
    /// </summary>
    internal class GetMaxSizeCodeGenerator
    {
        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);

        private readonly FlatBufferSerializerOptions options;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public GetMaxSizeCodeGenerator(FlatBufferSerializerOptions options, IReadOnlyDictionary<Type, string> methodNames)
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
                this.ImplementTableGetMaxSizeMethod(tableModel);
            }
            else if (typeModel is StructTypeModel structModel)
            {
                this.ImplementStructGetMaxSizeMethod(structModel);
            }
            else if (typeModel is VectorTypeModel vectorModel)
            {
                this.ImplementVectorGetMaxSizeMethod(vectorModel);
            }
            else if (typeModel is UnionTypeModel unionModel)
            {
                this.ImplementUnionGetMaxSizeMethod(unionModel);
            }
            else if (typeModel is EnumTypeModel enumModel)
            {
                this.ImplementEnumGetMaxSizeMethod(enumModel);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void ImplementTableGetMaxSizeMethod(TableTypeModel tableModel)
        {
            int vtableEntryCount = tableModel.MaxIndex + 1;

            // vtable length + table length + 2 * entryCount + padding to 2-byte alignment.
            int maxVtableSize = sizeof(ushort) * (2 + vtableEntryCount) + SerializationHelpers.GetMaxPadding(sizeof(ushort));
            int maxTableSize = tableModel.NonPaddedMaxTableInlineSize + SerializationHelpers.GetMaxPadding(tableModel.Alignment);

            List<string> statements = new List<string>();
            foreach (var kvp in tableModel.IndexToMemberMap)
            {
                int index = kvp.Key;
                var member = kvp.Value;

                if (member.ItemTypeModel.IsFixedSize)
                {
                    // This should already be accounted for in table.NonPaddedMax size above.
                    continue;
                }

                string variableName = $"index{index}Value";
                statements.Add($"var {variableName} = item.{member.PropertyInfo.Name};");

                string statement = $"runningSum += {this.InvokeGetMaxSizeMethod(member.ItemTypeModel, variableName)};";
                string condition = $"if (!object.ReferenceEquals({variableName}, null))";
                if (member.ItemTypeModel is VectorTypeModel vectorModel && vectorModel.IsMemoryVector)
                {
                    condition = string.Empty;
                }

                statement =
$@" 
                    {condition}
                    {{
                        {statement}
                    }}";

                statements.Add(statement);
            }

            string body =
$@"
            int runningSum = {maxTableSize} + {maxVtableSize};
            {string.Join("\r\n", statements)};
            return runningSum;
";

            this.GenerateGetMaxSizeMethod(tableModel.ClrType, body);
        }

        private void ImplementStructGetMaxSizeMethod(StructTypeModel structModel)
        {
            string body = $"return {structModel.MaxInlineSize};";
            this.GenerateGetMaxSizeMethod(structModel.ClrType, body);
        }

        private void ImplementEnumGetMaxSizeMethod(EnumTypeModel enumModel)
        {
            string body = $"return {enumModel.MaxInlineSize};";
            this.GenerateGetMaxSizeMethod(enumModel.ClrType, body);
        }

        /// <summary>
        /// Gets the max size of the vector itself, not the uoffset_t as part of the containing table.
        /// </summary>
        private void ImplementVectorGetMaxSizeMethod(VectorTypeModel vectorModel)
        {
            var itemModel = vectorModel.ItemTypeModel;
            string body;

            // count of items + padding(uoffset_t);
            int fixedSize = sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint));
            string lengthProperty = $"item.{vectorModel.LengthPropertyName}";

            // Constant size items. We can reduce these reasonably well.
            if (itemModel.IsFixedSize)
            {
                body = $"return {fixedSize} + {SerializationHelpers.GetMaxPadding(itemModel.Alignment)} + ({vectorModel.PaddedMemberInlineSize} * {lengthProperty});";
            }
            else if (itemModel.SchemaType == FlatBufferSchemaType.Table || itemModel.SchemaType == FlatBufferSchemaType.String)
            {
                Debug.Assert(itemModel.Alignment == sizeof(uint));
                Debug.Assert(itemModel.InlineSize == sizeof(uint));

                body =
$@"
                    int length = {lengthProperty};
                    int runningSum = {fixedSize} + {SerializationHelpers.GetMaxPadding(itemModel.Alignment)} + ({vectorModel.PaddedMemberInlineSize} * length);
                    for (int i = 0; i < length; ++i)
                    {{
                        var itemTemp = item[i];
                        {CSharpHelpers.GetNonNullCheckInvocation(itemModel, "itemTemp")};
                        runningSum += {this.InvokeGetMaxSizeMethod(itemModel, "itemTemp")};
                    }}
                    return runningSum;";
            }
            else
            {
                throw new NotImplementedException("Vector.GetMaxSize is not implemented for schema type of " + itemModel.SchemaType);
            }

            this.GenerateGetMaxSizeMethod(vectorModel.ClrType, body);
        }

        private void ImplementUnionGetMaxSizeMethod(UnionTypeModel unionModel)
        {
            List<string> switchCases = new List<string>();
            for (int i = 0; i < unionModel.UnionElementTypeModel.Length; ++i)
            {
                var unionMember = unionModel.UnionElementTypeModel[i];
                int unionIndex = i + 1;
                string @case = 
$@"
                    case {unionIndex}:
                        return {this.InvokeGetMaxSizeMethod(unionMember, $"item.Item{unionIndex}")};";

                switchCases.Add(@case);
            }
            string discriminatorPropertyName = nameof(FlatBufferUnion<int, int>.Discriminator);

            string body =
$@"
            switch (item.{discriminatorPropertyName})
            {{
                {string.Join("\r\n", switchCases)}
                default:
                    throw new System.InvalidOperationException(""Exception determining type of union. Discriminator = "" + item.{discriminatorPropertyName});
            }}
";
            this.GenerateGetMaxSizeMethod(unionModel.ClrType, body);
        }

        private string InvokeGetMaxSizeMethod(RuntimeTypeModel model, string parameter)
        {
            if (model.SchemaType == FlatBufferSchemaType.String)
            {
                return $"{CSharpHelpers.GetFullMethodName(ReflectedMethods.SerializationHelpers_GetMaxSizeOfStringMethod)}({parameter})";
            }
            else
            {
                return $"{this.MethodNames[model.ClrType]}({parameter})";
            }
        }

        /// <summary>
        /// Gets a method to serialize the given type with the given body.
        /// </summary>
        private void GenerateGetMaxSizeMethod(Type type, string body, bool inline = true)
        {
            string inlineDeclaration = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";
            if (!inline)
            {
                inlineDeclaration = string.Empty;
            }

            string declaration = 
$@"
            {inlineDeclaration}
            private static int {this.MethodNames[type]}({CSharpHelpers.GetCompilableTypeName(type)} item)
            {{
                {body}
            }}";

            var node = CSharpSyntaxTree.ParseText(declaration, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }
    }
}
