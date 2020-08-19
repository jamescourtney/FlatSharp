﻿/*
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
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Generates a collection of methods to help serialize the given root type.
    /// </summary>
    internal class SerializerCodeGenerator
    {
        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);

        private readonly FlatBufferSerializerOptions options;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public SerializerCodeGenerator(FlatBufferSerializerOptions options, IReadOnlyDictionary<Type, string> methodNames)
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
                this.ImplementTableInlineWriteMethod(tableModel);
            }
            else if (typeModel is StructTypeModel structModel)
            {
                this.ImplementStructInlineWriteMethod(structModel);
            }
            else if (typeModel is VectorTypeModel vectorModel)
            {
                if (vectorModel.IsMemoryVector)
                {
                    this.ImplementMemoryVectorInlineWriteMethod(vectorModel);
                }
                else if (vectorModel.IsArray || vectorModel.IsList)
                {
                    // Array implements IList.
                    this.ImplementListVectorInlineWriteMethod(vectorModel);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (typeModel is EnumTypeModel enumModel)
            {
                this.ImplementEnumInlineWriteMethod(enumModel);
            }
            else if (typeModel is UnionTypeModel unionModel)
            {
                // Skip
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void ImplementTableInlineWriteMethod(TableTypeModel tableModel)
        {
            var type = tableModel.ClrType;
            int maxIndex = tableModel.MaxIndex;
            int maxInlineSize = tableModel.NonPaddedMaxTableInlineSize;

            // Start by asking for the worst-case number of bytes from the serializationcontext.
            string methodStart =
$@"
                int tableStart = context.{nameof(SerializationContext.AllocateSpace)}({maxInlineSize}, sizeof(int));
                writer.{nameof(SpanWriter.WriteUOffset)}(span, originalOffset, tableStart, context);
                int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

                var vtable = context.{nameof(SerializationContext.VTableBuilder)};
                vtable.StartObject({maxIndex});
";

            List<string> body = new List<string>();
            List<string> writers = new List<string>();

            // load the properties of the object into locals.
            foreach (var kvp in tableModel.IndexToMemberMap)
            {
                string prepare, write;
                if (kvp.Value.ItemTypeModel.SchemaType == FlatBufferSchemaType.Union)
                {
                    (prepare, write) = this.GetUnionSerializeBlocks(kvp.Key, kvp.Value);
                }
                else
                {
                    (prepare, write) = this.GetStandardSerializeBlocks(kvp.Key, kvp.Value);
                }

                body.Add(prepare);
                writers.Add(write);
            }

            // We probably over-allocated. Figure out by how much and back up the cursor.
            // Then we can write the vtable.
            body.Add("int tableLength = currentOffset - tableStart;");
            body.Add($"context.{nameof(SerializationContext.Offset)} -= {maxInlineSize} - tableLength;");
            body.Add($"int vtablePosition = vtable.{nameof(VTableBuilder.EndObject)}(span, writer, tableLength);");
            body.Add($"writer.{nameof(SpanWriter.WriteInt)}(span, tableStart - vtablePosition, tableStart, context);");

            body.AddRange(writers);

            // These methods are often enormous, and inlining can have a detrimental effect on perf.
            this.GenerateSerializeMethod(type, $"{methodStart} \r\n {string.Join("\r\n", body)}", inline: false);
        }

        private (string prepareBlock, string serializeBlock) GetUnionSerializeBlocks(int index, TableMemberModel memberModel)
        {
            UnionTypeModel unionModel = (UnionTypeModel)memberModel.ItemTypeModel;

            string valueVariableName = $"index{index}Value";
            string discriminatorOffsetVariableName = $"index{index}DiscriminatorOffset";
            string valueOffsetVariableName = $"index{index}ValueOffset";
            string discriminatorValueVariableName = $"index{index}Discriminator";

            string prepareBlock =
$@"
                    var {valueVariableName} = item.{memberModel.PropertyInfo.Name};
                    int {discriminatorOffsetVariableName} = 0;
                    int {valueOffsetVariableName} = 0;
                    byte {discriminatorValueVariableName} = 0;

                    if ({valueVariableName} != null && {valueVariableName}.Discriminator != 0)
                    {{
                            {discriminatorValueVariableName} = {valueVariableName}.Discriminator;
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
                if (elementModel.SchemaType == FlatBufferSchemaType.Struct)
                {
                    // Structs are generally written in-line, with the exception of unions.
                    // So, we need to do the normal allocate space dance here, since we're writing
                    // a pointer to a struct.
                    structAdjustment =
$@"
                        var writeOffset = context.{nameof(SerializationContext.AllocateSpace)}({elementModel.InlineSize}, {elementModel.Alignment});
                        writer.{nameof(SpanWriter.WriteUOffset)}(span, {valueOffsetVariableName}, writeOffset, context);
                        {valueOffsetVariableName} = writeOffset;";
                }

                string @case =
$@"
                    case {unionIndex}:
                    {{
                        {structAdjustment}
                        {this.GetSerializeInvocation(elementModel.ClrType, $"{valueVariableName}.Item{unionIndex}", valueOffsetVariableName)}
                    }}
                        break;";

                switchCases.Add(@case);
            }

            string serializeBlock =
$@"
                    if ({discriminatorOffsetVariableName} != 0)
                    {{
                        {this.GetSerializeInvocation(typeof(byte), discriminatorValueVariableName, discriminatorOffsetVariableName)}
                        switch ({discriminatorValueVariableName})
                        {{
                            {string.Join("\r\n", switchCases)}
                            default: throw new InvalidOperationException(""Unexpected"");
                        }}
                    }}";

            return (prepareBlock, serializeBlock);
        }

        private (string prepareBlock, string serializeBlock) GetStandardSerializeBlocks(int index, TableMemberModel memberModel)
        {
            string valueVariableName = $"index{index}Value";
            string offsetVariableName = $"index{index}Offset";

            string condition = $"if ({valueVariableName} != {CSharpHelpers.GetDefaultValueToken(memberModel)})";
            if ((memberModel.ItemTypeModel is VectorTypeModel vector && vector.IsMemoryVector) || memberModel.IsKey)
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
                    var {valueVariableName} = item.{memberModel.PropertyInfo.Name};
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
                VectorTypeModel vectorModel = (VectorTypeModel)memberModel.ItemTypeModel;
                TableTypeModel tableModel = (TableTypeModel)vectorModel.ItemTypeModel;
                TableMemberModel keyMember = tableModel.IndexToMemberMap.Single(x => x.Value.PropertyInfo == tableModel.KeyProperty).Value;

                var builtInType = BuiltInType.BuiltInTypes[keyMember.ItemTypeModel.ClrType];
                string inlineSize = builtInType.TypeModel.SchemaType == FlatBufferSchemaType.Scalar ? builtInType.TypeModel.InlineSize.ToString() : "null";

                sortInvocation = $"{nameof(SortedVectorHelpers)}.{nameof(SortedVectorHelpers.SortVector)}(" +
                                    $"span, {offsetVariableName}, {keyMember.Index}, {inlineSize}, {CSharpHelpers.GetCompilableTypeName(builtInType.SpanComparerType)}.Instance);";
            }

            string serializeBlock =
$@"
                    if ({offsetVariableName} != 0)
                    {{
                        {this.GetSerializeInvocation(memberModel.ItemTypeModel.ClrType, valueVariableName, offsetVariableName)}
                        {sortInvocation}
                    }}";

            return (prepareBlock, serializeBlock);
        }

        private void ImplementStructInlineWriteMethod(StructTypeModel structModel)
        {
            var type = structModel.ClrType;

            List<string> body = new List<string>();
            for (int i = 0; i < structModel.Members.Count; ++i)
            {
                var memberInfo = structModel.Members[i];

                string propertyAccessor = $"item.{memberInfo.PropertyInfo.Name}";
                if (memberInfo.ItemTypeModel.SchemaType == FlatBufferSchemaType.Struct)
                {
                    // Force structs to be non-null. FlatSharp doesn't declare structs as structs,
                    // so we need to be careful that structs-within-structs are not null.
                    propertyAccessor += $" ?? new {CSharpHelpers.GetCompilableTypeName(memberInfo.ItemTypeModel.ClrType)}()";
                }

                string invocation = this.GetSerializeInvocation(memberInfo.ItemTypeModel.ClrType, propertyAccessor, $"{memberInfo.Offset} + originalOffset");
                body.Add(invocation);
            }

            this.GenerateSerializeMethod(type, string.Join("\r\n", body));
        }

        private void ImplementListVectorInlineWriteMethod(VectorTypeModel vectorModel)
        {
            var type = vectorModel.ClrType;
            var itemTypeModel = vectorModel.ItemTypeModel;
            string propertyName = vectorModel.LengthPropertyName;

            string body = $@"
                int count = item.{propertyName};
                int vectorOffset = context.{nameof(SerializationContext.AllocateVector)}({itemTypeModel.Alignment}, count, {vectorModel.PaddedMemberInlineSize});
                writer.{nameof(SpanWriter.WriteUOffset)}(span, originalOffset, vectorOffset, context);
                writer.{nameof(SpanWriter.WriteInt)}(span, count, vectorOffset, context);
                vectorOffset += sizeof(int);
                for (int i = 0; i < count; ++i)
                {{
                      var current = item[i];
                      {CSharpHelpers.GetNonNullCheckInvocation(itemTypeModel, "current")};
                      {this.GetSerializeInvocation(itemTypeModel.ClrType, "current", "vectorOffset")}
                      vectorOffset += {vectorModel.PaddedMemberInlineSize};
                }}";

            this.GenerateSerializeMethod(type, body);
        }

        private void ImplementMemoryVectorInlineWriteMethod(VectorTypeModel vectorModel)
        {
            var type = vectorModel.ClrType;
            var itemTypeModel = vectorModel.ItemTypeModel;

            string writerMethodName = $"{nameof(SpanWriter.WriteReadOnlyMemoryBlock)}<{CSharpHelpers.GetCompilableTypeName(itemTypeModel.ClrType)}>";
            if (itemTypeModel.ClrType == typeof(byte))
            {
                // Optimization: when we're writing bytes we don't have to change types.
                writerMethodName = nameof(SpanWriter.WriteReadOnlyByteMemoryBlock);
            }

            string body = $"writer.{writerMethodName}(span, item, originalOffset, {itemTypeModel.Alignment}, {itemTypeModel.InlineSize}, context);";
            this.GenerateSerializeMethod(type, body);
        }

        private void ImplementEnumInlineWriteMethod(EnumTypeModel enumModel)
        {
            var type = enumModel.ClrType;
            var underlyingType = Enum.GetUnderlyingType(type);
            string body = this.GetSerializeInvocation(underlyingType, $"({CSharpHelpers.GetCompilableTypeName(underlyingType)})item", "originalOffset");
            this.GenerateSerializeMethod(type, body);
        }

        private string GetSerializeInvocation(
            Type type, 
            string value, 
            string offset, 
            string spanVariableName = "span", 
            string writerVariableName = "writer", 
            string contextVariableName = "context")
        {
            if (BuiltInType.BuiltInTypes.TryGetValue(type, out var builtInType))
            {
                return $"writer.{builtInType.SpanWriterWrite.Name}({spanVariableName}, {value}, {offset}, {contextVariableName});";
            }
            else
            {
                return $"{this.MethodNames[type]}({writerVariableName}, {spanVariableName}, {value}, {offset}, {contextVariableName});";
            }
        }

        /// <summary>
        /// Gets a method to serialize the given type with the given body.
        /// </summary>
        private void GenerateSerializeMethod(Type type, string body, bool inline = true)
        {
            string inlineDeclaration = inline ? "[MethodImpl(MethodImplOptions.AggressiveInlining)]" : string.Empty;
            string declaration = $@"
            {inlineDeclaration}
            private static void {this.MethodNames[type]} (SpanWriter writer, Span<byte> span, {CSharpHelpers.GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
                {body}
            }}";

            var node = CSharpSyntaxTree.ParseText(declaration, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }
    }
}
