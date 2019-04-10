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

        private readonly SerializerCodeGenerator serializerCodeGenerator;
        private readonly ParserCodeGenerator parserCodeGenerator;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public RoslynSerializerGenerator(FlatBufferSerializerOptions options)
        {
            this.serializerCodeGenerator = new SerializerCodeGenerator(options, this.writeMethods);
            this.parserCodeGenerator = new ParserCodeGenerator(options, this.readMethods);
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
            this.serializerCodeGenerator.ImplementMethods();
            this.methodDeclarations.AddRange(this.serializerCodeGenerator.MethodDeclarations);

            this.parserCodeGenerator.ImplementMethods();
            this.methodDeclarations.AddRange(this.parserCodeGenerator.MethodDeclarations);

            foreach (var pair in this.maxSizeMethods)
            {
                Type type = pair.Key;
                this.ImplementGetMaxSizeMethod(type);
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

        private static string GetNonNullCheckInvocation(RuntimeTypeModel typeModel, string variableName)
        {
            if (typeModel.SchemaType == FlatBufferSchemaType.Scalar)
            {
                return string.Empty;
            }

            return $"{FullMethodName(ReflectedMethods.SerializationHelpers_EnsureNonNull(typeModel.ClrType))}({variableName})";
        }
    }
}
