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
        private readonly GetMaxSizeCodeGenerator maxSizeCodeGenerator;

        private List<SyntaxNode> methodDeclarations = new List<SyntaxNode>();

        public RoslynSerializerGenerator(FlatBufferSerializerOptions options)
        {
            this.serializerCodeGenerator = new SerializerCodeGenerator(options, this.writeMethods);
            this.parserCodeGenerator = new ParserCodeGenerator(options, this.readMethods);
            this.maxSizeCodeGenerator = new GetMaxSizeCodeGenerator(options, this.maxSizeMethods);
        }

        public ISerializer<TRoot> Compile<TRoot>() where TRoot : class
        {
            this.DefineMethods(RuntimeTypeModel.CreateFrom(typeof(TRoot)));
            this.ImplementInterfaceMethod(typeof(TRoot));
            this.ImplementMethods();

            var runtime = typeof(System.Runtime.CompilerServices.MethodImplAttribute).Assembly;
            var sysRuntime = typeof(Span<byte>).Assembly;

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Span<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IList<byte>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(SerializationContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(TRoot).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<int>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.ArrayList).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ValueType).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                MetadataReference.CreateFromFile(typeof(System.IO.InvalidDataException).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
            };

            string template =
$@"
            namespace Generated
            {{
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Runtime.CompilerServices;
                using FlatSharp;
                
                public sealed class Serializer : {nameof(IGeneratedSerializer<byte>)}<{CSharpHelpers.GetCompilableTypeName(typeof(TRoot))}>
                {{
                    {string.Join("\r\n", this.methodDeclarations.Select(x => x.ToFullString()))}
                }}
            }}
";
            var node = CSharpSyntaxTree.ParseText(template, ParseOptions);

            Func<string> formattedTextFactory = () =>
            {
                using (var workspace = new AdhocWorkspace())
                {
                    var formattedNode = Formatter.Format(node.GetRoot(), workspace);
                    return formattedNode.ToString();
                }
            };

#if DEBUG
            var debugCSharp = formattedTextFactory();
#endif

            // StrongNameProvider snProvider = new DesktopStrongNameProvider();

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
                    formattedTextFactory,
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
    }
}
