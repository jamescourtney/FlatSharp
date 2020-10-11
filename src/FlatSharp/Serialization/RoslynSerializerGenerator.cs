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
    using System.Buffers;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        public const string GeneratedSerializerClassName = "GeneratedSerializer";

        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest);
        private static readonly Dictionary<string, (Assembly, byte[])> AssemblyNameReferenceMapping = new Dictionary<string, (Assembly, byte[])>();

        private readonly Dictionary<Type, string> maxSizeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> writeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> readMethods = new Dictionary<Type, string>();

        private readonly FlatBufferSerializerOptions options;
        private readonly TypeModelContainer typeModelContainer;

        private readonly List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        private static readonly List<MetadataReference> commonReferences;

        static RoslynSerializerGenerator()
        {
            commonReferences = new List<MetadataReference>();

            foreach (string name in new[] { "netstandard", "System.Collections", "System.Runtime" })
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(name);
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        // For .NET 47, NetStandard may not be present in the GAC. Try to expand to see if we can grab it locally.
                        assembly = Assembly.LoadFile(Path.Combine(typeof(RoslynSerializerGenerator).Assembly.Location, $"{name}.dll"));
                    }
                    catch (FileNotFoundException)
                    {
                        // Method of last resort: Load our embedded resource.
                        var embeddedResourceName = typeof(RoslynSerializerGenerator).Assembly.GetManifestResourceNames().Single(
                            x => x.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

                        using var resourceStream = typeof(RoslynSerializerGenerator).Assembly.GetManifestResourceStream(embeddedResourceName);
                        using var memoryStream = new MemoryStream();
                        resourceStream.CopyTo(memoryStream);
                        assembly = Assembly.Load(memoryStream.ToArray());
                    }
                }

                var reference = MetadataReference.CreateFromFile(assembly.Location);
                commonReferences.Add(reference);
            }
        }

        public RoslynSerializerGenerator(FlatBufferSerializerOptions options, TypeModelContainer typeModelContainer)
        {
            this.options = options;
            this.typeModelContainer = typeModelContainer;
        }

        public ISerializer<TRoot> Compile<TRoot>() where TRoot : class
        {
            string code = this.GenerateCSharp<TRoot>();

            string template =
$@"
            namespace Generated
            {{
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Runtime.CompilerServices;
                using FlatSharp;
                using FlatSharp.Attributes;

                {code}
            }}";

            (Assembly assembly, Func<string> formattedTextFactory, byte[] assemblyData) = CompileAssembly(template, this.options.EnableAppDomainInterceptOnAssemblyLoad, typeof(TRoot).Assembly);

            object item = Activator.CreateInstance(assembly.GetTypes()[0]);
            var serializer = (IGeneratedSerializer<TRoot>)item;

            return new GeneratedSerializerWrapper<TRoot>(
                serializer,
                assembly,
                formattedTextFactory,
                assemblyData);
        }

        internal string GenerateCSharp<TRoot>(string visibility = "public")
        {
            ITypeModel rootModel = this.typeModelContainer.CreateTypeModel(typeof(TRoot));
            if (rootModel.SchemaType != FlatBufferSchemaType.Table)
            {
                throw new InvalidFlatBufferDefinitionException($"Can only compile [FlatBufferTable] elements as root types. Type '{typeof(TRoot).Name}' is a '{rootModel.GetType().Name}'.");
            }

            this.DefineMethods(rootModel);
            this.ImplementInterfaceMethod(typeof(TRoot));
            this.ImplementMethods();

            string code = $@"
                [{nameof(FlatSharpGeneratedSerializerAttribute)}({nameof(FlatBufferDeserializationOption)}.{this.options.DeserializationOption})]
                {visibility} sealed class {GeneratedSerializerClassName} : {nameof(IGeneratedSerializer<byte>)}<{CSharpHelpers.GetCompilableTypeName(typeof(TRoot))}>
                {{
                    {string.Join("\r\n", this.methodDeclarations.Select(x => x.ToFullString()))}
                }}
";

            return code;
        }

        internal static string GetFormattedText(string cSharpCode)
        {
            var root = RoslynSerializerGenerator.ApplySyntaxTransformations(CSharpSyntaxTree.ParseText(cSharpCode, ParseOptions).GetRoot());
            var tree = SyntaxFactory.SyntaxTree(root, ParseOptions);
            return GetFormattedTextFactory(tree)();
        }

        // Getting pretty code can be slow, so return a lambda that loads it lazily.
        internal static Func<string> GetFormattedTextFactory(SyntaxTree syntaxTree)
        {
            return () =>
            {
                using (var workspace = new AdhocWorkspace())
                {
                    var formattedNode = Formatter.Format(syntaxTree.GetRoot(), workspace);
                    string formatted = formattedNode.ToFullString();
                    return formatted;
                }
            };
        }

        internal static (Assembly assembly, Func<string> formattedTextFactory, byte[] assemblyData) CompileAssembly(
            string sourceCode,
            bool enableAppDomainIntercept,
            params Assembly[] additionalReferences)
        {
            List<MetadataReference> references = new List<MetadataReference>(commonReferences);
            foreach (Assembly additionalReference in additionalReferences)
            {
                if (AssemblyNameReferenceMapping.TryGetValue(additionalReference.GetName().Name, out var compilationRef))
                {
                    references.Add(MetadataReference.CreateFromImage(compilationRef.Item2));
                }
                else
                {
                    references.Add(MetadataReference.CreateFromFile(additionalReference.Location));
                }
            }

            references.AddRange(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Span<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IList<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(SerializationContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<int>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.ArrayList).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ValueType).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IBufferWriter<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IGeneratedSerializer<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(InvalidDataException).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ReadOnlyDictionary<,>).Assembly.Location),
            });

            var rootNode = ApplySyntaxTransformations(CSharpSyntaxTree.ParseText(sourceCode, ParseOptions).GetRoot());
            SyntaxTree tree = SyntaxFactory.SyntaxTree(rootNode);
            Func<string> formattedTextFactory = GetFormattedTextFactory(tree);

#if DEBUG
            string actualCSharp = tree.ToString();
            var debugCSharp = formattedTextFactory();
#endif

            string name = $"FlatSharpDynamicAssembly_{Guid.NewGuid():n}";
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithModuleName(name)
                .WithAllowUnsafe(false)
                .WithOptimizationLevel(OptimizationLevel.Release);

            CSharpCompilation compilation = CSharpCompilation.Create(
                name,
                new[] { tree },
                references,
                options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    string[] failures = result.Diagnostics
                        .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(d => d.ToString())
                        .ToArray();

                    throw new FlatSharpCompilationException(failures, formattedTextFactory());
                }

                var metadataRef = compilation.ToMetadataReference();
                ms.Position = 0;
                byte[] assemblyData = ms.ToArray();

                ResolveEventHandler handler = null;
                if (enableAppDomainIntercept)
                {
                    handler = (s, e) =>
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        if (AssemblyNameReferenceMapping.TryGetValue(requestedName.Name, out var value))
                        {
                            return value.Item1;
                        }

                        return null;
                    };

                    AppDomain.CurrentDomain.AssemblyResolve += handler;
                }

                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(assemblyData);
                    assembly.GetTypes();
                }
                finally
                {
                    if (handler != null)
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= handler;
                    }
                }

                AssemblyNameReferenceMapping[name] = (assembly, assemblyData);

                return (assembly, formattedTextFactory, assemblyData);
            }
        }

        /// <summary>
        /// Recursively crawls through the object graph and looks for methods to define.
        /// </summary>
        private void DefineMethods(ITypeModel model)
        {
            HashSet<Type> types = new HashSet<Type>();
            model.TraverseObjectGraph(types);

            foreach (var type in types)
            {
                string nameBase = Guid.NewGuid().ToString("n");
                this.writeMethods[type] = $"WriteInlineValueOf_{nameBase}";
                this.maxSizeMethods[type] = $"GetMaxSizeOf_{nameBase}";
                this.readMethods[type] = $"Read_{nameBase}";
            }
        }

        private void ImplementInterfaceMethod(Type rootType)
        {
            {
                string methodText =
$@"
                public void Write<TSpanWriter>(TSpanWriter writer, Span<byte> target, {CSharpHelpers.GetCompilableTypeName(rootType)} root, int offset, SerializationContext context)
                    where TSpanWriter : ISpanWriter
                {{
                    {this.writeMethods[rootType]}(writer, target, root, offset, context);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }

            {
                string methodText =
$@"
                public int GetMaxSize({CSharpHelpers.GetCompilableTypeName(rootType)} root)
                {{
                    return {this.maxSizeMethods[rootType]}(root);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }

            {
                string methodText =
$@"
                public {CSharpHelpers.GetCompilableTypeName(rootType)} Parse<TInputBuffer>(TInputBuffer buffer, int offset) 
                    where TInputBuffer : IInputBuffer
                {{
                    return {this.readMethods[rootType]}(buffer, offset);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }
        }

        private void ImplementMethods()
        {
            foreach (var type in this.writeMethods.Keys)
            {
                ITypeModel typeModel = this.typeModelContainer.CreateTypeModel(type);
                var maxSizeContext = new GetMaxSizeCodeGenContext("value", this.maxSizeMethods, this.options);
                var parseContext = new ParserCodeGenContext("buffer", "offset", "TInputBuffer", this.readMethods, this.options);
                var serializeContext = new SerializationCodeGenContext("context", "span", "spanWriter", "value", "offset", this.writeMethods, this.options);

                var maxSizeMethod = typeModel.CreateGetMaxSizeMethodBody(maxSizeContext);
                var parseMethod = typeModel.CreateParseMethodBody(parseContext);
                var writeMethod = typeModel.CreateSerializeMethodBody(serializeContext);

                this.GenerateGetMaxSizeMethod(type, maxSizeMethod, maxSizeContext);
                this.GenerateParseMethod(typeModel, parseMethod, parseContext);
                this.GenerateSerializeMethod(typeModel, writeMethod, serializeContext);
            }
        }

        /// <summary>
        /// Surrounds all properties/methods/constructors with checked syntax.
        /// </summary>
        private static SyntaxNode ApplySyntaxTransformations(SyntaxNode rootNode)
        {
            rootNode = rootNode.ReplaceNodes(
               rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>(),
               (a, b) =>
               {
                   if (a.Body != null)
                   {
                       return b.WithBody(SyntaxFactory.Block(SyntaxFactory.CheckedStatement(SyntaxKind.CheckedStatement, a.Body)));
                   }

                   return a;
               });

            rootNode = rootNode.ReplaceNodes(
                rootNode.DescendantNodes().OfType<ConstructorDeclarationSyntax>(),
                (a, b) =>
                {
                    return b.WithBody(SyntaxFactory.Block(SyntaxFactory.CheckedStatement(SyntaxKind.CheckedStatement, a.Body)));
                });

            rootNode = rootNode.ReplaceNodes(
                rootNode.DescendantNodes().OfType<AccessorDeclarationSyntax>(),
                (a, b) =>
                {
                    if (b.Body == null)
                    {
                        return a;
                    }

                    return b.WithBody(SyntaxFactory.Block(SyntaxFactory.CheckedStatement(SyntaxKind.CheckedStatement, b.Body)));
                });

            return rootNode;
        }

        /// <summary>
        /// Gets a method to serialize the given type with the given body.
        /// </summary>
        private void GenerateGetMaxSizeMethod(Type type, CodeGeneratedMethod method, GetMaxSizeCodeGenContext context)
        {
            string inlineDeclaration = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";
            if (!method.IsMethodInline)
            {
                inlineDeclaration = string.Empty;
            }

            string declaration =
$@"
            {inlineDeclaration}
            private static int {this.maxSizeMethods[type]}({CSharpHelpers.GetCompilableTypeName(type)} {context.ValueVariableName})
            {{
                {method.MethodBody}
            }}";

            var node = CSharpSyntaxTree.ParseText(declaration, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());
            
            if (!string.IsNullOrEmpty(method.ClassDefinition))
            {
                node = CSharpSyntaxTree.ParseText(method.ClassDefinition, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }
        }

        private void GenerateParseMethod(ITypeModel typeModel, CodeGeneratedMethod method, ParserCodeGenContext context)
        {
            string inlineDeclaration = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";
            if (!method.IsMethodInline)
            {
                inlineDeclaration = string.Empty;
            }
            string declaration =
$@"
            {inlineDeclaration}
            private static {CSharpHelpers.GetCompilableTypeName(typeModel.ClrType)} {this.readMethods[typeModel.ClrType]}<TInputBuffer>(
                TInputBuffer {context.InputBufferVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName}) where TInputBuffer : IInputBuffer
            {{
                {method.MethodBody}
            }}";

            var node = CSharpSyntaxTree.ParseText(declaration, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());

            if (!string.IsNullOrEmpty(method.ClassDefinition))
            {
                node = CSharpSyntaxTree.ParseText(method.ClassDefinition, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }
        }

        private void GenerateSerializeMethod(ITypeModel typeModel, CodeGeneratedMethod method, SerializationCodeGenContext context)
        {
            string inlineDeclaration = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";
            if (!method.IsMethodInline)
            {
                inlineDeclaration = string.Empty;
            }

            string declaration =
$@"
            {inlineDeclaration}
            private static void {this.writeMethods[typeModel.ClrType]}<TSpanWriter>(
                TSpanWriter {context.SpanWriterVariableName}, 
                Span<byte> {context.SpanVariableName}, 
                {CSharpHelpers.GetCompilableTypeName(typeModel.ClrType)} {context.ValueVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName}, 
                {nameof(SerializationContext)} {context.SerializationContextVariableName}) where TSpanWriter : ISpanWriter
            {{
                {method.MethodBody}
            }}";

            var node = CSharpSyntaxTree.ParseText(declaration, ParseOptions);
            this.methodDeclarations.Add(node.GetRoot());

            if (!string.IsNullOrEmpty(method.ClassDefinition))
            {
                node = CSharpSyntaxTree.ParseText(method.ClassDefinition, ParseOptions);
                this.methodDeclarations.Add(node.GetRoot());
            }
        }

        /// <summary>
        /// Returns a flat int for single-entry vtables, and a ref tuple otherwise.
        /// </summary>
        private static string GetVTableOffsetVariableType(int vtableLength)
        {
            if (vtableLength == 1)
            {
                return "int";
            }
            else
            {
                return $"ref ({string.Join(", ", Enumerable.Range(0, vtableLength).Select(x => $"int offset{x}"))})";
            }
        }
    }
}
