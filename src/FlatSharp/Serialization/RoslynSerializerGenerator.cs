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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
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

        private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.CSharp8);
        private static readonly ConcurrentDictionary<string, (Assembly, byte[])> AssemblyNameReferenceMapping = new ConcurrentDictionary<string, (Assembly, byte[])>();

        private readonly Dictionary<Type, string> maxSizeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> writeMethods = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> readMethods = new Dictionary<Type, string>();

        private readonly FlatBufferSerializerOptions options;
        private readonly TypeModelContainer typeModelContainer;

        private readonly List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        private static readonly List<MetadataReference> commonReferences;

        static RoslynSerializerGenerator()
        {
            [ExcludeFromCodeCoverage]
            static Assembly TryLoadFromFile(string assemblyName)
            {
                try
                {
                    return Assembly.Load(assemblyName);
                }
                catch (FileNotFoundException)
                {
                    try
                    {
                        // For .NET 47, NetStandard may not be present in the GAC. Try to expand to see if we can grab it locally.
                        return Assembly.LoadFile(Path.Combine(typeof(RoslynSerializerGenerator).Assembly.Location, $"{assemblyName}.dll"));
                    }
                    catch (FileNotFoundException)
                    {
                        // Method of last resort: Load our embedded resource.
                        var embeddedResourceName = typeof(RoslynSerializerGenerator).Assembly.GetManifestResourceNames().Single(
                            x => x.IndexOf(assemblyName, StringComparison.OrdinalIgnoreCase) >= 0);

                        using var resourceStream = typeof(RoslynSerializerGenerator).Assembly.GetManifestResourceStream(embeddedResourceName);
                        using var memoryStream = new MemoryStream();

                        resourceStream!.CopyTo(memoryStream);
                        return Assembly.Load(memoryStream.ToArray());
                    }
                }
            }

            commonReferences = new List<MetadataReference>();

            foreach (string name in new[] { "netstandard", "System.Collections", "System.Runtime" })
            {
                Assembly assembly = TryLoadFromFile(name);
                var reference = MetadataReference.CreateFromFile(assembly.Location);
                commonReferences.Add(reference);
            }
        }

        /// <summary>
        /// Enables "treat warnings as errors" functionality. This is great for unit test contexts, but 
        /// less great for real-life scenarios. 
        /// </summary>
        internal static bool EnableStrictValidation { get; set; }

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

            var externalRefs = this.TraverseAssemblyReferenceGraph<TRoot>();

            (Assembly assembly, Func<string> formattedTextFactory, byte[] assemblyData) =
                CompileAssembly(
                    template,
                    this.options.EnableAppDomainInterceptOnAssemblyLoad,
                    externalRefs.ToArray());

            Type? type = assembly.GetType($"Generated.{GeneratedSerializerClassName}");
            FlatSharpInternal.Assert(type is not null, "Generated assembly did not contain serializer type.");

            object? item = Activator.CreateInstance(type);

            FlatSharpInternal.Assert(
                item is IGeneratedSerializer<TRoot>,
                $"Compilation succeeded, but created instance {item}, Type = {assembly.GetTypes()[0]}");

            return new GeneratedSerializerWrapper<TRoot>(
                (IGeneratedSerializer<TRoot>)item,
                assembly,
                formattedTextFactory,
                assemblyData);
        }

        internal string GenerateCSharp<TRoot>(string visibility = "public")
        {
            ITypeModel rootModel = this.typeModelContainer.CreateTypeModel(typeof(TRoot));
            if (rootModel.SchemaType != FlatBufferSchemaType.Table)
            {
                throw new InvalidFlatBufferDefinitionException($"Can only compile [FlatBufferTable] elements as root types. Type '{typeof(TRoot).GetCompilableTypeName()}' is a {rootModel.SchemaType}.");
            }

            this.DefineMethods(rootModel);
            this.ImplementInterfaceMethod(typeof(TRoot));
            this.ImplementMethods(rootModel);

            string code = $@"
                [{nameof(FlatSharpGeneratedSerializerAttribute)}({nameof(FlatBufferDeserializationOption)}.{this.options.DeserializationOption})]
                {visibility} sealed class {GeneratedSerializerClassName} : {nameof(IGeneratedSerializer<byte>)}<{typeof(TRoot).GetGlobalCompilableTypeName()}>
                {{    
                    // Method generated to help AOT compilers make good decisions about generics.
                    public void __AotHelper()
                    {{
                        this.Write<ISpanWriter>(default!, new byte[10], default!, default!, default!);
                        this.Write<SpanWriter>(default!, new byte[10], default!, default!, default!);

                        this.Parse<IInputBuffer>(default!, 0);
                        this.Parse<MemoryInputBuffer>(default!, 0);
                        this.Parse<ReadOnlyMemoryInputBuffer>(default!, 0);
                        this.Parse<ArrayInputBuffer>(default!, 0);
                        this.Parse<ArraySegmentInputBuffer>(default!, 0);

                        throw new InvalidOperationException(""__AotHelper is not intended to be invoked"");
                    }}

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
            var rootNode = ApplySyntaxTransformations(CSharpSyntaxTree.ParseText(sourceCode, ParseOptions).GetRoot());
            SyntaxTree tree = SyntaxFactory.SyntaxTree(rootNode);
            Func<string> formattedTextFactory = GetFormattedTextFactory(tree);

#if DEBUG
            string actualCSharp = tree.ToString();
            var debugCSharp = formattedTextFactory();

            rootNode = ApplySyntaxTransformations(CSharpSyntaxTree.ParseText(debugCSharp, ParseOptions).GetRoot());
            tree = SyntaxFactory.SyntaxTree(rootNode);
            formattedTextFactory = GetFormattedTextFactory(tree);
#endif

            string name = $"FlatSharpDynamicAssembly_{Guid.NewGuid():n}";
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithModuleName(name)
                .WithAllowUnsafe(false)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithNullableContextOptions(NullableContextOptions.Enable);

            CSharpCompilation compilation = CSharpCompilation.Create(
                name,
                new[] { tree },
                GetMetadataReferences(additionalReferences),
                options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                ThrowOnEmitFailure(
                    result, 
                    tree,
#if DEBUG
                    debugCSharp
#else
                    formattedTextFactory
#endif
                );

                var metadataRef = compilation.ToMetadataReference();
                ms.Position = 0;
                byte[] assemblyData = ms.ToArray();

                ResolveEventHandler? handler = null;
                if (enableAppDomainIntercept)
                {
                    handler = (s, e) =>
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        if (requestedName.Name is not null && AssemblyNameReferenceMapping.TryGetValue(requestedName.Name, out var value))
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
                    if (handler is not null)
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= handler;
                    }
                }

                AssemblyNameReferenceMapping[name] = (assembly, assemblyData);

                return (assembly, formattedTextFactory, assemblyData);
            }
        }

        private static List<MetadataReference> GetMetadataReferences(Assembly[] additionalReferences)
        {
            List<MetadataReference> references = new List<MetadataReference>(commonReferences);
            foreach (Assembly additionalReference in additionalReferences)
            {
                var referenceName = additionalReference.GetName().Name;

                if (referenceName is not null && AssemblyNameReferenceMapping.TryGetValue(referenceName, out var compilationRef))
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
                MetadataReference.CreateFromFile(typeof(Unsafe).Assembly.Location),
            });

            return references;
        }

        private static void ThrowOnEmitFailure(
            EmitResult result,
            SyntaxTree syntaxTree,
#if DEBUG
            string cSharp
#else
            Func<string> cSharpFactory
#endif
            )
        {
            if (!result.Success || EnableStrictValidation)
            {
                var failures = result.Diagnostics
                    .Where(d => d.Id != "CS8019") // unnecessary using directive.
                    .Where(d => d.Id != "CS1701") // DLL version mismatch
                    .ToArray();

                if (failures.Length > 0)
                {
                    using var workspace = new AdhocWorkspace();
                    List<string> errors = new List<string>();

                    foreach (var failure in failures)
                    {
                        string error = failure.ToString();
                        SyntaxNode node = syntaxTree.GetRoot().FindNode(failure.Location.SourceSpan);
                        var formattedNode = Formatter.Format(node, workspace);
                        string formatted = formattedNode.ToFullString();
                        formatted = formatted.Trim().Replace('\r', ' ').Replace('\n', ' ');

                        errors.Add($"FlatSharp compilation error: {error}, Context = \"{formatted}\"");
                    }

#if !DEBUG
                    string cSharp = cSharpFactory();
#endif
                    
                    throw new FlatSharpCompilationException(errors.ToArray(), cSharp);
                }
            }
        }

        /// <summary>
        /// Recursively crawls through the object graph and looks for methods to define.
        /// </summary>
        private void DefineMethods(ITypeModel rootModel)
        {
            HashSet<Type> types = new HashSet<Type>();
            rootModel.TraverseObjectGraph(types);

            foreach (var type in types)
            {
                string nameBase = Guid.NewGuid().ToString("n");
                this.writeMethods[type] = $"WriteInlineValueOf_{nameBase}";
                this.maxSizeMethods[type] = $"GetMaxSizeOf_{nameBase}";
                this.readMethods[type] = $"Read_{nameBase}";
            }
        }

        private HashSet<Assembly> TraverseAssemblyReferenceGraph<TRoot>()
        {
            var rootModel = this.typeModelContainer.CreateTypeModel(typeof(TRoot));

            // all type model types.
            HashSet<Type> types = new HashSet<Type>();
            rootModel.TraverseObjectGraph(types);

            foreach (var type in types.ToArray())
            {
                ITypeModel typeModel = this.typeModelContainer.CreateTypeModel(type);
                types.UnionWith(typeModel.GetReferencedTypes());
            }

            Queue<Assembly> pendingAssemblies = new Queue<Assembly>(types.Select(x => x.Assembly));
            HashSet<Assembly> seenAssemblies = new HashSet<Assembly>();

            while (pendingAssemblies.Count > 0)
            {
                var assembly = pendingAssemblies.Dequeue();

                if (seenAssemblies.Add(assembly))
                {
                    foreach (var assemblyName in assembly.GetReferencedAssemblies())
                    {
                        try
                        {
                            pendingAssemblies.Enqueue(Assembly.Load(assemblyName));
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return seenAssemblies;
        }

        private void ImplementInterfaceMethod(Type rootType)
        {
            {
                string methodText =
$@"
                public void Write<TSpanWriter>(TSpanWriter writer, Span<byte> target, {CSharpHelpers.GetGlobalCompilableTypeName(rootType)} root, int offset, SerializationContext context)
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
                public int GetMaxSize({CSharpHelpers.GetGlobalCompilableTypeName(rootType)} root)
                {{
                    return {this.maxSizeMethods[rootType]}(root);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }

            {
                string methodText =
$@"
                public {CSharpHelpers.GetGlobalCompilableTypeName(rootType)} Parse<TInputBuffer>(TInputBuffer buffer, int offset) 
                    where TInputBuffer : IInputBuffer
                {{
                    return {this.readMethods[rootType]}(buffer, offset);
                }}
";
                this.methodDeclarations.Add(CSharpSyntaxTree.ParseText(methodText, ParseOptions).GetRoot());
            }
        }

        private void ImplementMethods(ITypeModel rootTypeModel)
        {
            List<(ITypeModel, TableFieldContext)> allContexts = rootTypeModel.GetAllTableFieldContexts();

            Dictionary<ITypeModel, List<TableFieldContext>> allContextsMap = new();
            foreach (var item in allContexts)
            {
                if (!allContextsMap.TryGetValue(item.Item1, out List<TableFieldContext>? list))
                {
                    list = new();
                    allContextsMap[item.Item1] = list;
                }

                list.Add(item.Item2);
            }

            foreach (var type in this.writeMethods.Keys)
            {
                ITypeModel typeModel = this.typeModelContainer.CreateTypeModel(type);
                bool isOffsetByRef = typeModel.PhysicalLayout.Length > 1;

                var requirements = typeModel.TableFieldContextRequirements;

                string getMaxSizeFieldContextVariableName = requirements.HasFlag(TableFieldContextRequirements.GetMaxSize)
                    ? "fieldContext"
                    : string.Empty;

                string parseFieldContextVariableName = requirements.HasFlag(TableFieldContextRequirements.Parse)
                    ? "fieldContext"
                    : string.Empty;

                string serializeFieldContextVariableName = requirements.HasFlag(TableFieldContextRequirements.Serialize)
                    ? "fieldContext"
                    : string.Empty;

                var maxSizeContext = new GetMaxSizeCodeGenContext("value", getMaxSizeFieldContextVariableName, this.maxSizeMethods, this.options, this.typeModelContainer, allContextsMap);
                var parseContext = new ParserCodeGenContext("buffer", "offset", "TInputBuffer", isOffsetByRef, parseFieldContextVariableName, this.readMethods, this.writeMethods, this.options, this.typeModelContainer, allContextsMap);
                var serializeContext = new SerializationCodeGenContext("context", "span", "spanWriter", "value", "offset", serializeFieldContextVariableName, isOffsetByRef, this.writeMethods, this.typeModelContainer, this.options, allContextsMap);

                var maxSizeMethod = typeModel.CreateGetMaxSizeMethodBody(maxSizeContext);
                var parseMethod = typeModel.CreateParseMethodBody(parseContext);
                var writeMethod = typeModel.CreateSerializeMethodBody(serializeContext);

                this.GenerateGetMaxSizeMethod(typeModel, maxSizeMethod, maxSizeContext);
                this.GenerateParseMethod(typeModel, parseMethod, parseContext);
                this.GenerateSerializeMethod(typeModel, writeMethod, serializeContext);

                string? extraClasses = typeModel.CreateExtraClasses();
                if (extraClasses is not null)
                {
                    var node = CSharpSyntaxTree.ParseText(extraClasses, ParseOptions);
                    this.methodDeclarations.Add(node.GetRoot());
                }
            }
        }

        /// <summary>
        /// Surrounds all properties/methods/constructors with checked syntax.
        /// </summary>
        private static SyntaxNode ApplySyntaxTransformations(SyntaxNode rootNode)
        {
            // Add checked{} to methods.
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

            // Add checked{} to constructors.
            rootNode = rootNode.ReplaceNodes(
                rootNode.DescendantNodes().OfType<ConstructorDeclarationSyntax>(),
                (a, b) =>
                {
                    return b.WithBody(SyntaxFactory.Block(SyntaxFactory.CheckedStatement(SyntaxKind.CheckedStatement, a.Body)));
                });

            // Add checked{} to property accessors.
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
        private void GenerateGetMaxSizeMethod(ITypeModel typeModel, CodeGeneratedMethod method, GetMaxSizeCodeGenContext context)
        {
            string tableFieldContextParameter = string.Empty;
            if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.GetMaxSize))
            {
                tableFieldContextParameter = $", {nameof(TableFieldContext)} {context.TableFieldContextVariableName}";
            }

            string declaration =
$@"
            {method.GetMethodImplAttribute()}
            private static int {this.maxSizeMethods[typeModel.ClrType]}({typeModel.GetGlobalCompilableTypeName()} {context.ValueVariableName}{tableFieldContextParameter})
            {{
                {method.MethodBody}
            }}";

            this.AddMethod(method, declaration);
        }

        private void GenerateParseMethod(ITypeModel typeModel, CodeGeneratedMethod method, ParserCodeGenContext context)
        {
            string tableFieldContextParameter = string.Empty;
            if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.Parse))
            {
                tableFieldContextParameter = $", {nameof(TableFieldContext)} {context.TableFieldContextVariableName}";
            }

            string clrType = typeModel.GetGlobalCompilableTypeName();

            string declaration =
            $@"
            {method.GetMethodImplAttribute()}
            private static {clrType} {this.readMethods[typeModel.ClrType]}<TInputBuffer>(
                TInputBuffer {context.InputBufferVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName}
                {tableFieldContextParameter}) where TInputBuffer : IInputBuffer
            {{
                {method.MethodBody}
            }}";

            this.AddMethod(method, declaration);
        }

        private void GenerateSerializeMethod(ITypeModel typeModel, CodeGeneratedMethod method, SerializationCodeGenContext context)
        {
            string serializationContextParameter = string.Empty;
            string tableFieldContextParameter = string.Empty;

            if (typeModel.SerializeMethodRequiresContext)
            {
                serializationContextParameter = $", {nameof(SerializationContext)} {context.SerializationContextVariableName}";
            }

            if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.Serialize))
            {
                tableFieldContextParameter = $", {nameof(TableFieldContext)} {context.TableFieldContextVariableName}";
            }

            string declaration =
$@"
            {method.GetMethodImplAttribute()}
            private static void {this.writeMethods[typeModel.ClrType]}<TSpanWriter>(
                TSpanWriter {context.SpanWriterVariableName}, 
                Span<byte> {context.SpanVariableName}, 
                {CSharpHelpers.GetGlobalCompilableTypeName(typeModel.ClrType)} {context.ValueVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName}
                {serializationContextParameter}
                {tableFieldContextParameter}) where TSpanWriter : ISpanWriter
            {{
                {method.MethodBody}
            }}";

            this.AddMethod(method, declaration);
        }

        private void AddMethod(CodeGeneratedMethod method, string fullText)
        {
            var node = CSharpSyntaxTree.ParseText(fullText, ParseOptions);
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
