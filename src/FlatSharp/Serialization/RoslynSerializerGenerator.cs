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
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.Formatting;

    /// <summary>
    /// Generates a collection of methods to help serialize the given root type.
    /// Does recursive traversal of the object graph and builds a set of methods to assist with populating vtables and writing values.
    /// 
    /// Eventually, everything must reduce to a built in type of string / scalar, which this will then call out to.
    /// </summary>
    internal class RoslynSerializerGenerator
    {
        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);

        private readonly Dictionary<Type, string> maxSizeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> writeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> readMethods = new Dictionary<Type, string>();

        private readonly FlatBufferSerializerOptions options;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public RoslynSerializerGenerator(FlatBufferSerializerOptions options)
        {
            this.options = options;
        }

        public ISerializer<TRoot> Compile<TRoot>() where TRoot : class
        {
            this.DefineMethods(RuntimeTypeModel.CreateFrom(typeof(TRoot)));
            this.ImplementInterfaceMethod(typeof(TRoot));
            this.ImplementMethods();

            var runtime = typeof(System.Runtime.CompilerServices.MethodImplAttribute).Assembly;
            var sysRuntime = typeof(Span<byte>).Assembly;

            string assemblyName = Path.GetRandomFileName();
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Span<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IList<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(SerializationContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(TRoot).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ValueType).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                MetadataReference.CreateFromFile(typeof(System.IO.InvalidDataException).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            };

            string template =
$@"
            namespace Generated
            {{
                using System;
                using System.Collections.Generic;
                using System.Runtime.CompilerServices;
                using FlatSharp;
                
                public sealed class Serializer : {nameof(IGeneratedSerializer<byte>)}<{GetCompilableTypeName(typeof(TRoot))}>
                {{
                    {string.Join("\r\n", this.methodDeclarations.Select(x => x.ToFullString()))}
                }}
            }}
";

            var node = CSharpSyntaxTree.ParseText(template, ParseOptions);

            string formattedText;
            using (var workspace = new AdhocWorkspace())
            {
                var formattedNode = Formatter.Format(node.GetRoot(), workspace);
                formattedText = formattedNode.ToString();
            }

            StrongNameProvider snProvider = new DesktopStrongNameProvider();

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithModuleName("FlatSharpDynamicAssembly")
                .WithOverflowChecks(true)
                .WithAllowUnsafe(false)
                .WithOptimizationLevel(OptimizationLevel.Release);

            CSharpCompilation compilation = CSharpCompilation.Create(
                "FlatSharpDynamicAssembly",
                new[] { node },
                references,
                options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    throw new InvalidOperationException("Unable to compile. This represents a bug in FlatSharp type model validation. Errors = " + string.Join("\r\n", failures));
                }

                ms.Position = 0;
                byte[] assemblyData = ms.ToArray();

                Assembly assembly = Assembly.Load(assemblyData);
                object item = Activator.CreateInstance(assembly.GetTypes()[0]);
                var serializer = (IGeneratedSerializer<TRoot>)item;

                return new GeneratedSerializerWrapper<TRoot>(
                    serializer,
                    assembly,
                    formattedText,
                    assemblyData);
            }
        }

        /// <summary>
        /// Recursively crawls through the object graph and looks for methods to define.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dictionary"></param>
        private void DefineMethods(RuntimeTypeModel model)
        {
            if (model.SchemaType == FlatBufferSchemaType.Scalar || model.SchemaType == FlatBufferSchemaType.String)
            {
                // Built in; we can call out to those when we need to.
                return;
            }

            if (this.writeMethods.ContainsKey(model.ClrType))
            {
                // Already done.
                return;
            }

            string nameBase = Guid.NewGuid().ToString("n");
            if (model is VectorTypeModel vectorModel2)
            {
                if (vectorModel2.IsList)
                {
                    nameBase = "ListVector_" + Guid.NewGuid().ToString("n");
                }
                else if (vectorModel2.IsArray)
                {
                    nameBase = "ArrayVector_" + Guid.NewGuid().ToString("n");
                }
                else
                {
                    Debug.Assert(vectorModel2.IsMemoryVector);
                    nameBase = "MemoryVector_" + Guid.NewGuid().ToString("n");
                }
            }

            this.writeMethods[model.ClrType] = $"WriteInlineValueOf_{nameBase}";
            this.maxSizeMethods[model.ClrType] = $"GetMaxSizeOf_{nameBase}";

            // Unions are given a read method here, but it is never invoked. Similiarly, it is not
            // defined.
            this.readMethods[model.ClrType] = $"Read_{nameBase}";

            if (model is TableTypeModel tableModel)
            {
                foreach (var member in tableModel.IndexToMemberMap.Values)
                {
                    this.DefineMethods(member.ItemTypeModel);
                }
            }
            else if (model is StructTypeModel structModel)
            {
                foreach (var member in structModel.Members)
                {
                    this.DefineMethods(member.ItemTypeModel);
                }
            }
            else if (model is VectorTypeModel vectorModel)
            {
                this.DefineMethods(vectorModel.ItemTypeModel);
            }
            else if (model is UnionTypeModel unionModel)
            {
                foreach (var member in unionModel.UnionElementTypeModel)
                {
                    this.DefineMethods(member);
                }
            }
        }

        private void ImplementInterfaceMethod(Type rootType)
        {
            {
                string methodText =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void Write(SpanWriter writer, Span<byte> target, {GetCompilableTypeName(rootType)} root, int offset, SerializationContext context)
                {{
                    {this.writeMethods[rootType]}(writer, target, root, offset, context);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }

            {
                string methodText =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public int GetMaxSize({GetCompilableTypeName(rootType)} root)
                {{
                    return {this.maxSizeMethods[rootType]}(root);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }

            {
                string methodText =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public {GetCompilableTypeName(rootType)} Parse(InputBuffer buffer, int offset)
                {{
                    return {this.readMethods[rootType]}(buffer, offset);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }
        }

        private void ImplementMethods()
        {
            foreach (var pair in this.writeMethods)
            {
                Type type = pair.Key;
                this.ImplementInlineWriteMethod(type);
            }

            foreach (var pair in this.maxSizeMethods)
            {
                Type type = pair.Key;
                this.ImplementGetMaxSizeMethod(type);
            }

            foreach (var pair in this.readMethods)
            {
                Type type = pair.Key;
                this.ImplementReadMethod(type);
            }
        }

        private void ImplementGetMaxSizeMethod(Type type)
        {
            // Parameters: T item
            // Return: max total size for item.
            var typeModel = RuntimeTypeModel.CreateFrom(type);

            if (typeModel.SchemaType == FlatBufferSchemaType.Struct)
            {
                return;
            }

            string methodName = this.maxSizeMethods[type];

            string header =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int {methodName}({GetCompilableTypeName(type)} item)
            {{
";
            List<string> body = new List<string>();

            if (typeModel is TableTypeModel tableModel)
            {
                // Tables are slightly trickier. We need to figure out:
                // 1) Maximum size of vtable
                // 2) Maximum size of table.
                // 3) Maximum size of any subtables / vectors / strings.

                // Max size of vtable = field count * 2 + 4;
                int vtableSize = (tableModel.IndexToMemberMap.Keys.Max() + 1) * 2;
                vtableSize += (2 * sizeof(ushort)) + SerializationHelpers.GetMaxPadding(sizeof(ushort));

                body.Add("int runningSum = 0;");

                int constantInlineSize = 0;

                // Now we compute the inline size of each element.
                foreach (var pair in tableModel.IndexToMemberMap)
                {
                    var itemModel = pair.Value.ItemTypeModel;
                    constantInlineSize += itemModel.InlineSize + SerializationHelpers.GetMaxPadding(itemModel.Alignment);

                    string subMethod = null;
                    if (itemModel.SchemaType == FlatBufferSchemaType.String)
                    {
                        subMethod = FullMethodName(ReflectedMethods.SerializationHelpers_GetMaxSizeOfStringMethod);
                    }
                    else if (itemModel.SchemaType == FlatBufferSchemaType.Table || itemModel.SchemaType == FlatBufferSchemaType.Vector || itemModel.SchemaType == FlatBufferSchemaType.Union)
                    {
                        subMethod = this.maxSizeMethods[itemModel.ClrType];
                    }

                    // if we're dealing with an item that isn't stored completely inline (string/vector/subtable/union), then we need to branch.
                    if (subMethod != null)
                    {
                        var variableName = $"indexValue_{pair.Key}";
                        body.Add($"var {variableName} = item.{pair.Value.PropertyInfo.Name};");

                        bool isMemoryVector = itemModel is VectorTypeModel vectorModel && vectorModel.IsMemoryVector;
                        if (!isMemoryVector)
                        {
                            body.Add($"if ({variableName} != null)");
                        }

                        body.Add("{");
                        body.Add($"runningSum += {subMethod}({variableName});");
                        body.Add("}");
                    }
                }

                body.Add("return runningSum + " + (constantInlineSize + vtableSize) + ";");
            }
            else if (typeModel is VectorTypeModel vectorModel)
            {
                var itemTypeModel = vectorModel.ItemTypeModel;

                string lengthPropertyName = "Count";
                if (vectorModel.IsMemoryVector || vectorModel.IsArray)
                {
                    lengthPropertyName = "Length";
                }

                // For fixed-size items, we can just multiply count * length to get our answer.
                bool isFixedSizeItem = itemTypeModel.SchemaType == FlatBufferSchemaType.Scalar ||
                                       itemTypeModel.SchemaType == FlatBufferSchemaType.Struct;

                if (isFixedSizeItem)
                {
                    // we know they have fixed size now, so the answer is pretty easy:
                    // 4 + maxpadding(4) + maxpadding(itemSize) + count * length
                    int constantSize = sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint)) + SerializationHelpers.GetMaxPadding(vectorModel.ItemTypeModel.InlineSize);
                    body.Add($"return {constantSize} + ({itemTypeModel.InlineSize} * item.{lengthPropertyName});");
                }
                else
                {
                    Debug.Assert(vectorModel.IsList || vectorModel.IsArray);

                    string itemMaxSize;
                    if (itemTypeModel.ClrType == typeof(string))
                    {
                        itemMaxSize = FullMethodName(ReflectedMethods.SerializationHelpers_GetMaxSizeOfStringMethod);
                    }
                    else
                    {
                        itemMaxSize = this.maxSizeMethods[itemTypeModel.ClrType];
                    }

                    body.Add($"int count = item.{lengthPropertyName};");

                    // uoffset and padding.
                    body.Add($"int maxSize = {sizeof(uint) + SerializationHelpers.GetMaxPadding(sizeof(uint))};");

                    body.Add("for (int i = 0; i < count; ++i) {");
                    body.Add("    var current = item[i];");
                    body.Add($"   {GetNonNullCheckInvocation(itemTypeModel, "current")};");
                    body.Add($"   maxSize += {itemMaxSize}(current);");
                    body.Add("}");
                    body.Add("return maxSize;");
                }
            }
            else if (typeModel is UnionTypeModel unionTypeModel)
            {
                body.Add("switch (item?.Discriminator) {");

                for (int i = 0; i < unionTypeModel.UnionElementTypeModel.Length; ++i)
                {
                    var subModel = unionTypeModel.UnionElementTypeModel[i];
                    body.Add($"case {i + 1}:");

                    // Unions never store data inline. So, we *always* have a uoffset_t
                    // and a byte for the discriminator.
                    body.Add("return ");
                    body.Add($" sizeof(byte) + sizeof(uint) + {SerializationHelpers.GetMaxPadding(sizeof(uint))}");

                    if (subModel.SchemaType == FlatBufferSchemaType.Struct)
                    {
                        body.Add($"+ {subModel.InlineSize} + {SerializationHelpers.GetMaxPadding(subModel.Alignment)};");
                    }
                    else if (subModel.SchemaType == FlatBufferSchemaType.Vector || subModel.SchemaType == FlatBufferSchemaType.Table)
                    {
                        string subMethod = this.maxSizeMethods[subModel.ClrType];
                        body.Add($"+ {subMethod}(item.Item{i + 1});");
                    }
                    else if (subModel.SchemaType == FlatBufferSchemaType.String)
                    {
                        string subMethod = FullMethodName(ReflectedMethods.SerializationHelpers_GetMaxSizeOfStringMethod);
                        body.Add($"+ {subMethod}(item.Item{i + 1});");
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }

                body.Add("default: return 0;");
                body.Add("}");
            }

            string text = $"{header} {string.Join("\r\n", body)} }}";
            var node = CSharpSyntaxTree.ParseText(text, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementInlineWriteMethod(Type type)
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
                else
                {
                    // Array implements IList.
                    this.ImplementListVectorInlineWriteMethod(vectorModel);
                }
            }
            else if (typeModel is UnionTypeModel unionModel)
            {
                this.ImplementUnionInlineWriteMethod(unionModel);
            }
        }

        private void ImplementTableInlineWriteMethod(TableTypeModel tableModel)
        {
            // parameters: (T value, T defaultValue_notUsed, ref int sizeNeeded)
            var type = tableModel.ClrType;
            var methodName = this.writeMethods[tableModel.ClrType];
            int maxIndex = tableModel.MaxIndex;

            int maxInlineSize = sizeof(int);
            foreach (var item in tableModel.IndexToMemberMap.Values)
            {
                maxInlineSize += item.ItemTypeModel.InlineSize + SerializationHelpers.GetMaxPadding(item.ItemTypeModel.Alignment);
            }

            string header =
$@"
            private static void {methodName} (SpanWriter writer, Span<byte> span, {GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
                int tableStart = context.{nameof(SerializationContext.AllocateSpace)}({maxInlineSize}, sizeof(int));
                writer.{nameof(SpanWriter.WriteUOffset)}(span, originalOffset, tableStart, context);
                int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

                var vtable = context.{nameof(SerializationContext.VTableBuilder)};
                vtable.StartObject({maxIndex});

";

            List<string> body = new List<string>();

            // load the properties of the object into locals.
            foreach (var kvp in tableModel.IndexToMemberMap)
            {
                var index = kvp.Key;
                var memberModel = kvp.Value;
                var itemTypeModel = memberModel.ItemTypeModel;

                if (itemTypeModel is UnionTypeModel unionModel)
                {
                    body.Add($"var index{index}DiscriminatorOffset = 0;");
                    body.Add($"var index{index}Offset = 0;");
                    body.Add($"var index{index}Union = item.{memberModel.PropertyInfo.Name};");
                }
                else
                {
                    body.Add($"var index{index}Value = item.{memberModel.PropertyInfo.Name};");
                    body.Add($"int index{index}Offset = 0;");
                }

                string condition = $"if (index{index}Value != {GetDefaultValueToken(memberModel)})";
                if (itemTypeModel is VectorTypeModel vector && vector.IsMemoryVector)
                {
                    // Memory is a struct and can't be null, and 0-length vectors are valid.
                    // Therefore, we just need to omit the conditional check entirely.
                    condition = string.Empty;
                }

                if (itemTypeModel.SchemaType != FlatBufferSchemaType.Union)
                {
                    body.Add(
$@"
                    {{
                        {condition}
                        {{
                            currentOffset += {FullMethodName(ReflectedMethods.SerializationHelpers_GetAlignmentErrorMethod)}(currentOffset, {memberModel.ItemTypeModel.Alignment});
                            index{index}Offset = currentOffset;
                            vtable.SetOffset({index}, currentOffset - tableStart);
                            currentOffset += {memberModel.ItemTypeModel.InlineSize};
                        }}
                    }}
");
                }
                else
                {
                    body.Add(
$@"
                        if ((index{index}Union?.Discriminator ?? (byte)0) != 0)
                        {{
                            index{index}DiscriminatorOffset = currentOffset;
                            vtable.SetOffset({index}, currentOffset - tableStart);
                            currentOffset++;

                            currentOffset += {FullMethodName(ReflectedMethods.SerializationHelpers_GetAlignmentErrorMethod)}(currentOffset, sizeof(uint));
                            index{index}Offset = currentOffset;
                            vtable.SetOffset({index + 1}, currentOffset - tableStart);
                            currentOffset += sizeof(uint);
                        }}");
                }
            }

            body.Add("int tableLength = currentOffset - tableStart;");
            body.Add($"context.Offset -= {maxInlineSize} - tableLength;");
            body.Add($"int vtablePosition = vtable.EndObject(span, writer, tableLength);");
            body.Add("writer.WriteInt(span, tableStart - vtablePosition, tableStart, context);");

            foreach (var kvp in tableModel.IndexToMemberMap)
            {
                var index = kvp.Key;
                string valueVariableName = $"index{index}Value";
                string offsetVariableName = $"index{index}Offset";
                var typeModel = kvp.Value.ItemTypeModel;

                body.Add($"if ({offsetVariableName} != 0) {{");

                if (typeModel.SchemaType != FlatBufferSchemaType.Union)
                {
                    body.Add(this.GetSerializeInvocation(kvp.Value.ItemTypeModel.ClrType, valueVariableName, offsetVariableName));
                }
                else
                {
                    var unionModel = (UnionTypeModel)typeModel;

                    // Union formatter.
                    body.Add(this.GetSerializeInvocation(typeof(byte), $"index{index}Union.Discriminator", $"index{index}DiscriminatorOffset"));

                    body.Add($"switch (index{index}Union.Discriminator) {{");
                    for (int i = 0; i < unionModel.UnionElementTypeModel.Length; ++i)
                    {
                        var unionElementModel = unionModel.UnionElementTypeModel[i];

                        body.Add($"case {i + 1}:");

                        if (unionElementModel.SchemaType == FlatBufferSchemaType.Struct)
                        {
                            // must allocate space, since structs want to write inline.
                            body.Add($"var writeOffset = context.AllocateSpace({unionElementModel.InlineSize}, {unionElementModel.Alignment});");
                            body.Add($"writer.WriteUOffset(span, index{index}Offset, writeOffset, context);");
                            body.Add($"index{index}Offset = writeOffset;");
                        }

                        body.Add(this.GetSerializeInvocation(unionModel.UnionElementTypeModel[i].ClrType, $"index{index}Union.Item{i + 1}", $"index{index}Offset"));
                        body.Add("break;");
                    }
                    body.Add($"}}");
                }

                body.Add($"}}");
            }

            string text = $"{header} {string.Join("\r\n", body)} }}";
            var node = CSharpSyntaxTree.ParseText(text, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementStructInlineWriteMethod(StructTypeModel structModel)
        {
            var type = structModel.ClrType;
            var methodName = this.writeMethods[structModel.ClrType];

            string header =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void {methodName} (SpanWriter writer, Span<byte> span, {GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
";
            List<string> body = new List<string>();
            for (int i = 0; i < structModel.Members.Count; ++i)
            {
                var memberInfo = structModel.Members[i];
                string invocation = this.GetSerializeInvocation(memberInfo.ItemTypeModel.ClrType, $"item.{memberInfo.PropertyInfo.Name}", $"{memberInfo.Offset} + originalOffset");
                body.Add(invocation);
            }

            string text = $"{header} {string.Join("\r\n", body)} }}";
            var node = CSharpSyntaxTree.ParseText(text, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementListVectorInlineWriteMethod(VectorTypeModel vectorModel)
        {
            var type = vectorModel.ClrType;
            var methodName = this.writeMethods[vectorModel.ClrType];
            var itemTypeModel = vectorModel.ItemTypeModel;
            string propertyName = vectorModel.IsList ? "Count" : "Length";

            string method =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void {methodName} (SpanWriter writer, Span<byte> span, {GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
                int count = item.{propertyName};
                int vectorOffset = context.AllocateVector({itemTypeModel.Alignment}, count, {itemTypeModel.InlineSize});
                writer.WriteUOffset(span, originalOffset, vectorOffset, context);
                writer.WriteInt(span, count, vectorOffset, context);
                vectorOffset += sizeof(int);
                for (int i = 0; i < count; ++i)
                {{
                      var current = item[i];
                      {GetNonNullCheckInvocation(itemTypeModel, "current")};
                      {this.GetSerializeInvocation(itemTypeModel.ClrType, "current", "vectorOffset")}
                      vectorOffset += {itemTypeModel.InlineSize};
                }}
            }}
";

            var node = CSharpSyntaxTree.ParseText(method, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementMemoryVectorInlineWriteMethod(VectorTypeModel vectorModel)
        {
            var type = vectorModel.ClrType;
            var methodName = this.writeMethods[vectorModel.ClrType];

            var itemTypeModel = vectorModel.ItemTypeModel;

            string writerMethodName = $"{nameof(SpanWriter.WriteReadOnlyMemoryBlock)}<{itemTypeModel.ClrType}>";
            if (itemTypeModel.ClrType == typeof(byte))
            {
                writerMethodName = nameof(SpanWriter.WriteReadOnlyByteMemoryBlock);
            }

            string method =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void {methodName} (SpanWriter writer, Span<byte> span, {GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
                writer.{writerMethodName}(span, item, originalOffset, {itemTypeModel.Alignment}, {itemTypeModel.InlineSize}, context);
            }}
";

            var node = CSharpSyntaxTree.ParseText(method, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementUnionInlineWriteMethod(UnionTypeModel typeModel)
        {
            Type type = typeModel.ClrType;
            var methodName = this.writeMethods[type];

            string method =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void {methodName} (SpanWriter writer, Span<byte> span, {GetCompilableTypeName(type)} item, int originalOffset, SerializationContext context)
            {{
                // TODO!
            }}
";

            var node = CSharpSyntaxTree.ParseText(method, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementReadMethod(Type type)
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
            string readMethodName = this.readMethods[typeModel.ClrType];

            // Static factory method.
            {
                string body =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static {GetCompilableTypeName(typeModel.ClrType)} {readMethodName} (InputBuffer memory, int offset)
                {{
                    // Follow the UOffset_T pointer.
                    return new {className}(memory, offset + memory.ReadUOffset(offset));
                }}
";

                var node = CSharpSyntaxTree.ParseText(body, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }

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
                    string compilableTypeName = GetCompilableTypeName(propertyType);

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
                    {GetAccessModifier(propertyInfo.GetGetMethod())} override {compilableTypeName} {propertyInfo.Name}
                    {{
                        {getter}
                        {setter}
                    }}
";

                    propertyOverrides.Add(@override);
                }

                string classDefinition =
$@"
                private sealed class {className} : {GetCompilableTypeName(typeModel.ClrType)}
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
            string defaultValue = GetDefaultValueToken(memberModel);

            return
$@"
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get 
                    {{
                        if (!this.{hasValueFieldName})
                        {{
                            var buffer = this.buffer;
                            int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, {index});
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
            string defaultValue = GetDefaultValueToken(memberModel);

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
                    structOffsetAdjustment = "offsetLocation += buffer.ReadUOffset(offsetLocation);";
                }

                string @case =
$@"
                    case {unionIndex}:
                        {structOffsetAdjustment}
                        this.{valueFieldName} = new {GetCompilableTypeName(unionModel.ClrType)}({this.GetReadInvocation(unionMember.ClrType, "buffer", "offsetLocation")});
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
                        int discriminatorLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, {index});
                        int offsetLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, {index + 1});
                            
                        if (discriminatorLocation == 0) {{
                            this.{valueFieldName} = {defaultValue};
                        }}
                        else {{
                            byte discriminator = buffer.ReadByte(discriminatorLocation);
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
            string readMethodName = this.readMethods[typeModel.ClrType];

            // Static factory method.
            {
                string body =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static {GetCompilableTypeName(typeModel.ClrType)} {readMethodName} (InputBuffer memory, int offset)
                {{
                    return new {className}(memory, offset);
                }}
";

                var node = CSharpSyntaxTree.ParseText(body, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }

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
                    string compilableTypeName = GetCompilableTypeName(propertyType);

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
                    {GetAccessModifier(propertyInfo.GetGetMethod())} override {compilableTypeName} {propertyInfo.Name}
                    {{
                        {getter}
                        {setter}
                    }}
";

                    propertyOverrides.Add(@override);
                }

                string classDefinition =
$@"
                private sealed class {className} : {GetCompilableTypeName(typeModel.ClrType)}
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
            string readMethodName = this.readMethods[typeModel.ClrType];
            string invocation = $"{nameof(InputBuffer.ReadMemoryBlock)}<{GetCompilableTypeName(typeModel.ItemTypeModel.ClrType)}>";
            if (typeModel.ItemTypeModel.ClrType == typeof(byte))
            {
                invocation = nameof(InputBuffer.ReadByteMemoryBlock);
            }

            string body =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static {GetCompilableTypeName(typeModel.ClrType)} {readMethodName} (InputBuffer memory, int offset)
            {{
                return memory.{invocation}(offset, {typeModel.ItemTypeModel.InlineSize});
            }}
";

            var node = CSharpSyntaxTree.ParseText(body, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementListVectorReadMethod(VectorTypeModel typeModel)
        {
            string readMethodName = this.readMethods[typeModel.ClrType];

            string kindToCreate = nameof(FlatBufferVector<byte>);
            if (this.options.CacheListVectorData)
            {
                kindToCreate = nameof(FlatBufferCacheVector<byte>);
            }

            string body =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static {GetCompilableTypeName(typeModel.ClrType)} {readMethodName} (InputBuffer memory, int offset)
            {{
                return {this.CreateFlatBufferVector(typeModel, kindToCreate)};
            }}
";

            var node = CSharpSyntaxTree.ParseText(body, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private void ImplementArrayVectorReadMethod(VectorTypeModel typeModel)
        {
            string readMethodName = this.readMethods[typeModel.ClrType];
            var itemTypeModel = typeModel.ItemTypeModel;

            string statement;
            if (itemTypeModel is ScalarTypeModel scalarModel && (scalarModel.InlineSize == 1 || BitConverter.IsLittleEndian))
            {
                statement = $"memory.{nameof(InputBuffer.ReadMemoryBlock)}<{GetCompilableTypeName(itemTypeModel.ClrType)}>(offset, {itemTypeModel.InlineSize}).ToArray()";
            }
            else
            {
                statement = $"{this.CreateFlatBufferVector(typeModel, nameof(FlatBufferVector<byte>))}.ToArray()";
            }
            string body =
$@"
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static {GetCompilableTypeName(typeModel.ClrType)} {readMethodName} (InputBuffer memory, int offset)
            {{
                return {statement};
            }}
";

            var node = CSharpSyntaxTree.ParseText(body, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
        }

        private string CreateFlatBufferVector(VectorTypeModel typeModel, string vectorTypeName)
        {
            return $@"new {vectorTypeName}<{GetCompilableTypeName(typeModel.ItemTypeModel.ClrType)}>(
                    memory, 
                    offset + memory.ReadUOffset(offset), 
                    {typeModel.ItemTypeModel.InlineSize}, 
                    (b, o) => {this.GetReadInvocation(typeModel.ItemTypeModel.ClrType, "b", "o")})";
        }

        private static string FullMethodName(MethodInfo info)
        {
            return $"{info.DeclaringType.FullName}.{info.Name}";
        }

        private static string GetCompilableTypeName(Type t)
        {
            string name;
            if (t.IsGenericType)
            {
                List<string> parameters = new List<string>();
                foreach (var generic in t.GetGenericArguments())
                {
                    parameters.Add(GetCompilableTypeName(generic));
                }

                name = $"{t.FullName.Split('`')[0]}<{string.Join(", ", parameters)}>";
            }
            else
            {
                name = t.FullName;
            }

            name = name.Replace('+', '.');
            return name;
        }

        private string GetSerializeInvocation(Type type, string value, string offset)
        {
            if (ReflectedMethods.ILWriters.TryGetValue(type, out var memberWriteMethod))
            {
                return $"writer.{memberWriteMethod.Name}(span, {value}, {offset}, context);";
            }
            else
            {
                return $"{this.writeMethods[type]}(writer, span, {value}, {offset}, context);";
            }
        }

        private static string GetNonNullCheckInvocation(RuntimeTypeModel typeModel, string variableName)
        {
            if (typeModel.SchemaType == FlatBufferSchemaType.Scalar)
            {
                return string.Empty;
            }

            return $"{FullMethodName(ReflectedMethods.SerializationHelpers_EnsureNonNull(typeModel.ClrType))}({variableName})";
        }

        private string GetReadInvocation(Type type, string buffer, string offset)
        {
            if (ReflectedMethods.InputBufferReaders.TryGetValue(type, out var readMethod))
            {
                return $"{buffer}.{readMethod.Name}({offset})";
            }
            else
            {
                return $"{this.readMethods[type]}({buffer}, {offset})";
            }
        }

        private static string GetDefaultValueToken(TableMemberModel memberModel)
        {
            var itemTypeModel = memberModel.ItemTypeModel;

            string defaultValue = $"default({GetCompilableTypeName(memberModel.ItemTypeModel.ClrType)})";
            if (memberModel.HasDefaultValue)
            {
                string literalSpecifier = string.Empty;

                if (itemTypeModel.ClrType == typeof(float))
                {
                    literalSpecifier = "f";
                }
                else if (itemTypeModel.ClrType == typeof(uint))
                {
                    literalSpecifier = "U";
                }
                else if (itemTypeModel.ClrType == typeof(double))
                {
                    literalSpecifier = "d";
                }
                else if (itemTypeModel.ClrType == typeof(long))
                {
                    literalSpecifier = "L";
                }
                else if (itemTypeModel.ClrType == typeof(ulong))
                {
                    literalSpecifier = "UL";
                }

                defaultValue = $"{memberModel.DefaultValue}{literalSpecifier}";
            }

            return defaultValue;
        }

        private static string GetAccessModifier(MethodBase method)
        {
            if (method.IsPublic)
            {
                return "public";
            }

            if (method.IsFamilyOrAssembly)
            {
                return "protected internal";
            }

            if (method.IsFamily)
            {
                return "protected";
            }

            if (method.IsPrivate)
            {
                return "private";
            }

            if (method.IsAssembly)
            {
                return "internal";
            }

            throw new InvalidOperationException("Unexpected method visibility: " + method.Name);
        }
    }
}
