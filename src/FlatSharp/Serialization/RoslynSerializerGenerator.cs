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

        private readonly SerializerCodeGenerator serializerCodeGenerator;
        private readonly ParserCodeGenerator parserCodeGenerator;
        private readonly GetMaxSizeCodeGenerator maxSizeCodeGenerator;
        private readonly FlatBufferSerializerOptions options;
        private readonly List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public RoslynSerializerGenerator(FlatBufferSerializerOptions options)
        {
            this.options = options;
            this.serializerCodeGenerator = new SerializerCodeGenerator(options, this.writeMethods);
            this.parserCodeGenerator = new ParserCodeGenerator(options, this.readMethods);
            this.maxSizeCodeGenerator = new GetMaxSizeCodeGenerator(options, this.maxSizeMethods);
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
            var runtimeModel = RuntimeTypeModel.CreateFrom(typeof(TRoot));
            if (runtimeModel.SchemaType != FlatBufferSchemaType.Table)
            {
                throw new InvalidFlatBufferDefinitionException($"Can only compile [FlatBufferTable] elements as root types. Type '{typeof(TRoot).Name}' is a '{runtimeModel.SchemaType}'.");
            }

            this.DefineMethods(RuntimeTypeModel.CreateFrom(typeof(TRoot)));
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
            List<MetadataReference> references = new List<MetadataReference>();
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
                MetadataReference.CreateFromFile(typeof(IGeneratedSerializer<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                MetadataReference.CreateFromFile(typeof(System.IO.InvalidDataException).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
            });

            var rootNode = ApplySyntaxTransformations(CSharpSyntaxTree.ParseText(sourceCode, ParseOptions).GetRoot());
            SyntaxTree tree = SyntaxFactory.SyntaxTree(rootNode);
            Func<string> formattedTextFactory = GetFormattedTextFactory(tree);

#if DEBUG
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
        private void DefineMethods(RuntimeTypeModel model)
        {
            if (model.IsBuiltInType)
            {
                // Built in; we can call out to those when we need to.
                return;
            }

            if (this.writeMethods.ContainsKey(model.ClrType))
            {
                // Already done.
                return;
            }

            this.DefineNameBase(model);

            if (model is TableTypeModel tableModel)
            {
                this.DefineTableMethods(tableModel);
            }
            else if (model is StructTypeModel structModel)
            {
                this.DefineStructMethods(structModel);
            }
            else if (model is VectorTypeModel vectorModel)
            {
                this.DefineVectorMethods(vectorModel);
            }
            else if (model is UnionTypeModel unionModel)
            {
                this.DefineUnionMethods(unionModel);
            }
            else if (model is EnumTypeModel)
            {
                // Nothing to define for enums as they don't
                // contain references to other types.
            }
            else
            {
                throw new InvalidOperationException("Unexepcted type model: " + model?.GetType());
            }
        }

        private void DefineNameBase(RuntimeTypeModel model)
        {
            string nameBase = Guid.NewGuid().ToString("n");

            this.writeMethods[model.ClrType] = $"WriteInlineValueOf_{nameBase}";
            this.maxSizeMethods[model.ClrType] = $"GetMaxSizeOf_{nameBase}";
            this.readMethods[model.ClrType] = $"Read_{nameBase}";
        }

        private void DefineTableMethods(TableTypeModel tableModel)
        {
            foreach (var member in tableModel.IndexToMemberMap.Values)
            {
                this.DefineMethods(member.ItemTypeModel);
            }
        }

        private void DefineStructMethods(StructTypeModel structModel)
        {
            foreach (var member in structModel.Members)
            {
                this.DefineMethods(member.ItemTypeModel);
            }
        }

        private void DefineVectorMethods(VectorTypeModel vectorModel)
        {
            this.DefineMethods(vectorModel.ItemTypeModel);
        }

        private void DefineUnionMethods(UnionTypeModel unionModel)
        {
            foreach (var member in unionModel.UnionElementTypeModel)
            {
                this.DefineMethods(member);
            }
        }

        private void ImplementInterfaceMethod(Type rootType)
        {
            {
                string methodText =
$@"
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void Write(SpanWriter writer, Span<byte> target, {CSharpHelpers.GetCompilableTypeName(rootType)} root, int offset, SerializationContext context)
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
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public {CSharpHelpers.GetCompilableTypeName(rootType)} Parse(InputBuffer buffer, int offset)
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

            this.maxSizeCodeGenerator.ImplementMethods();
            this.methodDeclarations.AddRange(this.maxSizeCodeGenerator.MethodDeclarations);
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
    }
}
