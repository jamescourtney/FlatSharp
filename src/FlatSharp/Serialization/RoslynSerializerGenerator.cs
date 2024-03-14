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

using FlatSharp.TypeModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Formatting;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FlatSharp.CodeGen;

/// <summary>
/// Generates a collection of methods to help serialize the given root type.
/// Does recursive traversal of the object graph and builds a set of methods to assist with populating vtables and writing values.
/// 
/// Eventually, everything must reduce to a built in type of string / scalar, which this will then call out to.
/// </summary>
internal class RoslynSerializerGenerator
{
    private static IReadOnlyList<FlatBufferDeserializationOption> DistinctDeserializationOptions = 
        Enum.GetValues(typeof(FlatBufferDeserializationOption))
            .Cast<FlatBufferDeserializationOption>()
            .Distinct()
            .ToList();

#if NET8_0_OR_GREATER
    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
        LanguageVersion.CSharp11,
        preprocessorSymbols: new[] { CSharpHelpers.Net7PreprocessorVariable, CSharpHelpers.Net8PreprocessorVariable });
#elif NET7_0_OR_GREATER
    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
        LanguageVersion.CSharp11,
        preprocessorSymbols: new[] { CSharpHelpers.Net7PreprocessorVariable });
#else
    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
        LanguageVersion.CSharp11);
#endif

    private static readonly ConcurrentDictionary<string, (Assembly, byte[])> AssemblyNameReferenceMapping = new ConcurrentDictionary<string, (Assembly, byte[])>();

    private readonly FlatBufferSerializerOptions options;
    private readonly TypeModelContainer typeModelContainer;

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

    internal static bool AllowUnsafeBlocks { get; set; }

    public RoslynSerializerGenerator(FlatBufferSerializerOptions options, TypeModelContainer typeModelContainer)
    {
        this.options = options;
        this.typeModelContainer = typeModelContainer;
    }

    public ISerializer<TRoot> Compile<TRoot>() where TRoot : class
    {
        var (code, typeName) = this.GenerateCSharpRecursive<TRoot>();

        string template =
$@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Runtime.CompilerServices;
            using FlatSharp;
            using FlatSharp.Attributes;
            using FlatSharp.Internal;

            {code}";

        var externalRefs = this.TraverseAssemblyReferenceGraph<TRoot>();

        (Assembly assembly, Func<string> formattedTextFactory, byte[] assemblyData) =
            CompileAssembly(
                template,
                this.options.EnableAppDomainInterceptOnAssemblyLoad,
                externalRefs.ToArray());

        Type? type = assembly.GetType(typeName);
        FlatSharpInternal.Assert(type is not null, "Generated assembly did not contain serializer type.");

        object? item = Activator.CreateInstance(type);

        FlatSharpInternal.Assert(
            item is IGeneratedSerializer<TRoot>,
            $"Compilation succeeded, but created instance {item}, Type = {assembly.GetTypes()[0]}");

        return new GeneratedSerializerWrapper<TRoot>(
            this.options.DeserializationOption,
            (IGeneratedSerializer<TRoot>)item,
            formattedTextFactory);
    }

    internal (string text, string serializerTypeName) GenerateCSharpRecursive<TRoot>()
    {
        ITypeModel rootModel = this.typeModelContainer.CreateTypeModel(typeof(TRoot));

        FlatSharpInternal.Assert(
            rootModel.SchemaType == FlatBufferSchemaType.Table,
            $"Can only compile [FlatBufferTable] elements as root types. Type '{typeof(TRoot).GetCompilableTypeName()}' is a {rootModel.SchemaType}.");

        HashSet<Type> dependencies = new();
        rootModel.TraverseObjectGraph(dependencies);

        List<string> parts = new();
        foreach (Type type in dependencies)
        {
            parts.Add(this.ImplementHelperClass(this.typeModelContainer.CreateTypeModel(type), DistinctDeserializationOptions));
        }

        var serializerParts = DefaultMethodNameResolver.ResolveGeneratedSerializerClassName(this.typeModelContainer.CreateTypeModel(typeof(TRoot)));
        string fullName = $"{serializerParts.@namespace}.{serializerParts.name}";
        return (string.Join("\r\n\r\n", parts), fullName);
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
            .WithAllowUnsafe(AllowUnsafeBlocks)
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

    private HashSet<Assembly> TraverseAssemblyReferenceGraph<TRoot>()
    {
        var rootModel = this.typeModelContainer.CreateTypeModel(typeof(TRoot));

        // all type model types.
        HashSet<Type> types = new();
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

    private (string body, string fullName) ImplementInterfaceMethod(TableTypeModel typeModel, IEnumerable<FlatBufferDeserializationOption> deserializationOptions)
    {
        Type rootType = typeModel.ClrType;
        List<string> bodyParts = new();

        {
            var parts = DefaultMethodNameResolver.ResolveSerialize(typeModel);

            // Reserve first 4 bytes for offset to first table.
            string writeFileId = $"context.Offset = 4;";

            // Or if there is a file ID, reserve the first 8 bytes.
            if (typeModel.TryGetFileIdentifier(out string? fileId))
            {
                writeFileId = $"""
                    context.Offset = 8;
                    target[7] = {(byte)fileId[3]};
                    target[6] = {(byte)fileId[2]};
                    target[5] = {(byte)fileId[1]};
                    target[4] = {(byte)fileId[0]};
                """;
            }

            string methodText =
$@"
                public void Write<TSpanWriter>(TSpanWriter writer, Span<byte> target, {CSharpHelpers.GetGlobalCompilableTypeName(rootType)} root, SerializationContext context)
                    where TSpanWriter : ISpanWriter
                {{
                    {writeFileId}
                    {parts.@namespace}.{parts.className}.{parts.methodName}(writer, target, root, 0, context);
                }}
";
            bodyParts.Add(methodText);
        }

        {
            string fileIdSize = string.Empty;
            if (typeModel.TryGetFileIdentifier(out _))
            {
                fileIdSize = "maxSize += 4; // file id";
            }

            var parts = DefaultMethodNameResolver.ResolveGetMaxSize(typeModel);
            string methodText =
$@"
                public int GetMaxSize({CSharpHelpers.GetGlobalCompilableTypeName(rootType)} root)
                {{
                    int maxSize = 0;
                    {fileIdSize}
                    maxSize += {parts.@namespace}.{parts.className}.{parts.methodName}(root);
                    return maxSize;
                }}
";
            bodyParts.Add(methodText);
        }

        var pairs = new[]
        {
            (nameof(IGeneratedSerializer<bool>.ParseGreedy), FlatBufferDeserializationOption.Greedy),
            (nameof(IGeneratedSerializer<bool>.ParseGreedyMutable), FlatBufferDeserializationOption.GreedyMutable),
            (nameof(IGeneratedSerializer<bool>.ParseProgressive), FlatBufferDeserializationOption.Progressive),
            (nameof(IGeneratedSerializer<bool>.ParseLazy), FlatBufferDeserializationOption.Lazy),
        };

        foreach (var pair in pairs)
        {
            var parts = DefaultMethodNameResolver.ResolveParse(pair.Item2, typeModel);

            string body;

            if (deserializationOptions.Contains(pair.Item2))
            {
                body = $"return {parts.@namespace}.{parts.className}.{parts.methodName}(buffer, args.{nameof(GeneratedSerializerParseArguments.Offset)}, args.{nameof(GeneratedSerializerParseArguments.DepthLimit)});";
            }
            else
            {
                body = $"throw new NotImplementedException(\"Deserializer type '{pair.Item2}' was excluded from generation at compile time.\");";
            }

            string methodText =
$@"
                public {CSharpHelpers.GetGlobalCompilableTypeName(rootType)} {pair.Item1}<TInputBuffer>(TInputBuffer buffer, in {typeof(GeneratedSerializerParseArguments).GetGlobalCompilableTypeName()} args) 
                    where TInputBuffer : IInputBuffer
                {{
                    {body}
                }}
";
            bodyParts.Add(methodText);
        }

        string? compilerVersion = typeof(RoslynSerializerGenerator).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

        var resolvedName = DefaultMethodNameResolver.ResolveGeneratedSerializerClassName(typeModel);

        string code = $@"
        namespace {resolvedName.@namespace}
        {{
            {(this.options.EnableFileVisibility ? "file" : "internal")} class {resolvedName.name} : {nameof(IGeneratedSerializer<byte>)}<{rootType.GetGlobalCompilableTypeName()}>
            {{    
                // Method generated to help AOT compilers make good decisions about generics.
                [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                public void __AotHelper()
                {{
                    this.Write<ISpanWriter>(default!, new byte[10], default!, default!);
                    this.Write<SpanWriter>(default!, new byte[10], default!, default!);

                    this.ParseLazy<IInputBuffer>(default!, default);
                    this.ParseLazy<MemoryInputBuffer>(default!, default);
                    this.ParseLazy<ReadOnlyMemoryInputBuffer>(default!, default);
                    this.ParseLazy<ArrayInputBuffer>(default!, default);
                    this.ParseLazy<ArraySegmentInputBuffer>(default!, default);

                    this.ParseProgressive<IInputBuffer>(default!, default);
                    this.ParseProgressive<MemoryInputBuffer>(default!, default);
                    this.ParseProgressive<ReadOnlyMemoryInputBuffer>(default!, default);
                    this.ParseProgressive<ArrayInputBuffer>(default!, default);
                    this.ParseProgressive<ArraySegmentInputBuffer>(default!, default);

                    this.ParseGreedy<IInputBuffer>(default!, default);
                    this.ParseGreedy<MemoryInputBuffer>(default!, default);
                    this.ParseGreedy<ReadOnlyMemoryInputBuffer>(default!, default);
                    this.ParseGreedy<ArrayInputBuffer>(default!, default);
                    this.ParseGreedy<ArraySegmentInputBuffer>(default!, default);

                    this.ParseGreedyMutable<IInputBuffer>(default!, default);
                    this.ParseGreedyMutable<MemoryInputBuffer>(default!, default);
                    this.ParseGreedyMutable<ReadOnlyMemoryInputBuffer>(default!, default);
                    this.ParseGreedyMutable<ArrayInputBuffer>(default!, default);
                    this.ParseGreedyMutable<ArraySegmentInputBuffer>(default!, default);

                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.InvalidOperation_AotHelper)}();
                }}

                [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                public {resolvedName.name}()
                {{
                    {typeof(FlatSharpInternal).GGCTN()}.{nameof(FlatSharpInternal.AssertFlatSharpRuntimeVersionMatches)}(""{compilerVersion}"");
                }}

                {string.Join("\r\n", bodyParts)}
            }}
        }}
";

        return (code, $"{resolvedName.@namespace}.{resolvedName.name}");
    }

    /// <summary>
    /// Implements methods for a single type.
    /// </summary>
    /// <returns>The c# code</returns>
    internal string ImplementHelperClass(ITypeModel typeModel, IEnumerable<FlatBufferDeserializationOption> deserializationOptions)
    {
        bool requiresDepthTracking = typeModel.IsDeepEnoughToRequireDepthTracking();
        
        // Find all table field contexts in the whole graph. This can probably
        // be cached...
        Dictionary<ITypeModel, HashSet<TableFieldContext>> allContextsMap = new();
        foreach (ITypeModel model in this.typeModelContainer.GetEnumerator())
        {
            var contexts = model.GetAllTableFieldContexts();
            foreach (var ctx in contexts)
            {
                if (!allContextsMap.TryGetValue(ctx.Item1, out HashSet<TableFieldContext>? set))
                {
                    set = new();
                    allContextsMap[ctx.Item1] = set;
                }

                set.Add(ctx.Item2);
            }
        }

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

        var maxSizeContext = new GetMaxSizeCodeGenContext("value", getMaxSizeFieldContextVariableName, this.options, this.typeModelContainer, allContextsMap);
        var serializeContext = new SerializationCodeGenContext("context", "span", "spanWriter", "value", "offset", serializeFieldContextVariableName, isOffsetByRef, this.typeModelContainer, this.options, allContextsMap);
        var parseContext = new ParserCodeGenContext("buffer", "offset", "remainingDepth", "TInputBuffer", isOffsetByRef, parseFieldContextVariableName, options, this.typeModelContainer, allContextsMap);

        CodeGeneratedMethod maxSizeMethod = typeModel.CreateGetMaxSizeMethodBody(maxSizeContext);
        CodeGeneratedMethod writeMethod = typeModel.CreateSerializeMethodBody(serializeContext);

        List<string> methods = new();

        methods.Add(this.GenerateGetMaxSizeMethod(typeModel, maxSizeMethod, maxSizeContext));
        methods.Add(maxSizeMethod.ClassDefinition ?? string.Empty);

        methods.Add(this.GenerateSerializeMethod(typeModel, writeMethod, serializeContext));
        methods.Add(writeMethod.ClassDefinition ?? string.Empty);

        if (typeModel.IsParsingInvariant)
        {
            var parseMethod = typeModel.CreateParseMethodBody(parseContext);

            methods.Add(this.GenerateParseMethod(requiresDepthTracking, typeModel, parseMethod, parseContext));
            methods.Add(parseMethod.ClassDefinition ?? string.Empty);
        }
        else
        {
            foreach (var option in DistinctDeserializationOptions.Intersect(deserializationOptions))
            {
                parseContext = parseContext with { Options = this.options with { DeserializationOption = option } };
                var parseMethod = typeModel.CreateParseMethodBody(parseContext);
                methods.Add(this.GenerateParseMethod(requiresDepthTracking, typeModel, parseMethod, parseContext));
                methods.Add(parseMethod.ClassDefinition ?? string.Empty);
            }
        }

        methods.Add(typeModel.CreateExtraClasses() ?? string.Empty);

        (string ns, string name) = DefaultMethodNameResolver.ResolveHelperClassName(typeModel);

        string serializerBody = string.Empty;
        if (typeModel.SchemaType == FlatBufferSchemaType.Table)
        {
            TableTypeModel? tableModel = typeModel as TableTypeModel;
            FlatSharpInternal.Assert(tableModel is not null, "expecting table");

            if (tableModel.ShouldBuildISerializer)
            {
                // Generate a serializer as well.
                (serializerBody, _) = ImplementInterfaceMethod(tableModel, deserializationOptions);
            }
        }

        string @class =
$@"
            namespace {ns}
            {{
                // Make sure we can reference the namespace of the type we are using.
                // Ensures that extension methods, etc are available.
                using {typeModel.ClrType.Namespace};

                {(this.options.EnableFileVisibility ? "file" : "internal")} static class {name}
                {{
                    {string.Join("\r\n", methods)}
                }}
            }}

            {serializerBody}
";

        return @class;
    }

    /// <summary>
    /// Surrounds all properties/methods/constructors with checked syntax.
    /// </summary>
    private static SyntaxNode ApplySyntaxTransformations(SyntaxNode rootNode)
    {
        // Add checked() to multiplications.
        rootNode = rootNode.ReplaceNodes(
            rootNode.DescendantNodes().OfType<BinaryExpressionSyntax>().Where(bes => bes.Kind() == SyntaxKind.MultiplyExpression),
            (a, _) =>
            {
                return SyntaxFactory.CheckedExpression(SyntaxKind.CheckedExpression, a);
            });

        rootNode = rootNode.ReplaceNodes(
            rootNode.DescendantNodes().OfType<BinaryExpressionSyntax>().Where(bes => bes.Kind() == SyntaxKind.LeftShiftExpression),
            (a, _) =>
            {
                return SyntaxFactory.CheckedExpression(SyntaxKind.CheckedExpression, a);
            });

        FlatSharpInternal.Assert(
            !rootNode.DescendantNodes().OfType<BinaryExpressionSyntax>().Where(bes => bes.Kind() == SyntaxKind.MultiplyAssignmentExpression).Any(),
            "No *= operators allowed");

        FlatSharpInternal.Assert(
            !rootNode.DescendantNodes().OfType<BinaryExpressionSyntax>().Where(bes => bes.Kind() == SyntaxKind.LeftShiftAssignmentExpression).Any(),
            "No <<= operators allowed");

        return rootNode;
    }

    /// <summary>
    /// Gets a method to serialize the given type with the given body.
    /// </summary>
    private string GenerateGetMaxSizeMethod(ITypeModel typeModel, CodeGeneratedMethod method, GetMaxSizeCodeGenContext context)
    {
        string tableFieldContextParameter = string.Empty;
        if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.GetMaxSize))
        {
            tableFieldContextParameter = $", {nameof(TableFieldContext)} {context.TableFieldContextVariableName}";
        }

        string declaration =
$@"
            {method.GetMethodImplAttribute()}
            internal static int {DefaultMethodNameResolver.ResolveGetMaxSize(typeModel).methodName}({typeModel.GetGlobalCompilableTypeName()} {context.ValueVariableName}{tableFieldContextParameter})
            {{
                {method.MethodBody}
            }}";

        return declaration;
    }

    private string GenerateParseMethod(bool requiresDepthTracking, ITypeModel typeModel, CodeGeneratedMethod method, ParserCodeGenContext context)
    {
        string tableFieldContextParameter = string.Empty;
        if (typeModel.TableFieldContextRequirements.HasFlag(TableFieldContextRequirements.Parse))
        {
            tableFieldContextParameter = $", {nameof(TableFieldContext)} {context.TableFieldContextVariableName}";
        }

        string clrType = typeModel.GetGlobalCompilableTypeName();
        string parsedType = typeModel.GetDeserializedTypeName(context.Options.DeserializationOption, context.InputBufferTypeName);

        // If we require depth tracking due to the schema, inject the if statement and the decrement instruction.
        string depthCheck = string.Empty;
        if (requiresDepthTracking)
        {
            depthCheck = $@"
                --{context.RemainingDepthVariableName};
                {typeof(SerializationHelpers).GetGlobalCompilableTypeName()}.{nameof(SerializationHelpers.EnsureDepthLimit)}({context.RemainingDepthVariableName});
            ";
        }

        string fullText =
        $@"
            {method.GetMethodImplAttribute()}
            internal static {parsedType} {DefaultMethodNameResolver.ResolveParse(context.Options.DeserializationOption, typeModel).methodName}<TInputBuffer>(
                TInputBuffer {context.InputBufferVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName},
                short {context.RemainingDepthVariableName}
                {tableFieldContextParameter}) where TInputBuffer : IInputBuffer
            {{
                {depthCheck}
                {method.MethodBody}
            }}";

        return fullText;
    }

    private string GenerateSerializeMethod(ITypeModel typeModel, CodeGeneratedMethod method, SerializationCodeGenContext context)
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

        string fullText =
        $@"
            {method.GetMethodImplAttribute()}
            internal static void {DefaultMethodNameResolver.ResolveSerialize(typeModel).methodName}<TSpanWriter>(
                TSpanWriter {context.SpanWriterVariableName}, 
                Span<byte> {context.SpanVariableName}, 
                {CSharpHelpers.GetGlobalCompilableTypeName(typeModel.ClrType)} {context.ValueVariableName}, 
                {GetVTableOffsetVariableType(typeModel.PhysicalLayout.Length)} {context.OffsetVariableName}
                {serializationContextParameter}
                {tableFieldContextParameter}) where TSpanWriter : ISpanWriter
            {{
                {method.MethodBody}
            }}
        ";

        return fullText;
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
