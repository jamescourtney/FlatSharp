/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    using Antlr4.Runtime;
    using CommandLine;
    using FlatSharp.TypeModel;

    public class FlatSharpCompiler
    {
        [ExcludeFromCodeCoverage]
        static int Main(string[] args)
        {
            int exitCode = -1;

            try
            {
                CommandLine.Parser.Default.ParseArguments<CompilerOptions>(args)
                    .WithParsed(x =>
                    {
                        exitCode = RunCompiler(x);
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return exitCode;
        }

        [ExcludeFromCodeCoverage]
        private static int RunCompiler(CompilerOptions options)
        {
            using (var context = ErrorContext.Current)
            {
                if (string.IsNullOrEmpty(options.InputFile))
                {
                    Console.Error.WriteLine($"FlatSharp Compiler: No input file specified.");
                    return -1;
                }

                if (string.IsNullOrEmpty(options.OutputDirectory))
                {
                    Console.Error.WriteLine("FlatSharp compiler: No output directory specified.");
                    return -1;
                }

                // Read existing file to see if we even need to do any work.
                string fbsFileName = Path.GetFileName(options.InputFile);
                string outputFileName = fbsFileName + ".generated.cs";
                string outputFullPath = Path.Combine(options.OutputDirectory, outputFileName);

                try
                {
                    context.PushScope("$");

                    int attemptCount = 0;
                    while (attemptCount++ <= 5)
                    {
                        try
                        {
                            RootNodeDefinition rootNode = ParseSyntax(options.InputFile, new IncludeFileLoader());

                            if (string.IsNullOrEmpty(rootNode.InputHash))
                            {
                                throw new InvalidFbsFileException("Failed to compute input hash");
                            }

                            if (File.Exists(outputFullPath))
                            {
                                string existingOutput = File.ReadAllText(outputFullPath);
                                if (existingOutput.Contains(rootNode.InputHash))
                                {
                                    // Input file unchanged.
                                    return 0;
                                }
                            }

                            string cSharp = string.Empty;
                            try
                            {
                                CreateCSharp(rootNode, options, out cSharp);
                            }
                            finally
                            {
                                if (cSharp is not null)
                                {
                                    File.WriteAllText(outputFullPath, cSharp);
                                }
                            }
                        }
                        catch (IOException)
                        {
                            // Some projects built multiple targets at once, and this can
                            // cause contention between different invocations of the compiler.
                            // Usually, one will succeed, the others will fail, then they'll wake up and
                            // see that the file looks right to them.
                            Thread.Sleep(TimeSpan.FromMilliseconds(new Random().Next(20, 200)));
                        }
                    }
                }
                catch (InvalidFbsFileException ex)
                {
                    foreach (var message in ex.Errors)
                    {
                        Console.Error.WriteLine(message);
                    }

                    return -1;
                }
                catch (FlatSharpCompilationException)
                {
                    Console.Error.WriteLine(
                        $"FlatSharp failed to generate valid C# output. \r\n" + 
                        $"This is commonly caused by the fbs schema using a C# keyword, for example: \r\n" + 
                        $"\ttable SomeTable {{\r\n\t\tclass : string\r\n\t}}\r\n" +
                        "\r\n" +
                        $"The output can be viewed in: '{Path.GetFullPath(outputFullPath)}'.");

                    return -1;
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine($"File '{options.InputFile}' was not found");
                    return -1;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.Error.WriteLine($"No file specified");
                    return -1;
                }
                finally
                {
                    context.PopScope();
                }

                return 0;
            }
        }

        internal static Assembly CompileAndLoadAssembly(
            string fbsSchema,
            CompilerOptions options,
            IEnumerable<Assembly>? additionalReferences = null,
            Dictionary<string, string>? additionalIncludes = null)
        {
            InMemoryIncludeLoader includeLoader = new InMemoryIncludeLoader
            {
                { "root.fbs", fbsSchema }
            };

            if (additionalIncludes != null)
            {
                foreach (var kvp in additionalIncludes)
                {
                    includeLoader[kvp.Key] = kvp.Value;
                }
            }

            using (var context = ErrorContext.Current)
            {
                context.PushScope("$");
                try
                {
                    Assembly[] additionalRefs = additionalReferences?.ToArray() ?? Array.Empty<Assembly>();
                    var rootNode = ParseSyntax("root.fbs", includeLoader);
                    CreateCSharp(rootNode, options, out string cSharp);
                    var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true, additionalRefs);
                    string debugText = formattedText();
                    return assembly;
                }
                finally
                {
                    context.PopScope();
                }
            }
        }

        internal static RootNodeDefinition TestHookParseSyntax(string fbsSchema, Dictionary<string, string>? includes = null)
        {
            InMemoryIncludeLoader includeLoader = new InMemoryIncludeLoader
            {
                { "root.fbs", fbsSchema }
            };

            if (includes != null)
            {
                foreach (var kvp in includes)
                {
                    includeLoader[kvp.Key] = kvp.Value;
                }
            }

            using (ErrorContext.Current)
            {
                return ParseSyntax("root.fbs", includeLoader);
            }
        }

        private static RootNodeDefinition ParseSyntax(
            string fbsPath,
            IIncludeLoader includeLoader)
        {
            string rootPath = Path.GetFullPath(fbsPath);

            // First, visit includes. We need to figure out which files warrant a thorough look.
            HashSet<string> includes = new HashSet<string>() { rootPath };
            Queue<string> visitOrder = new Queue<string>();
            visitOrder.Enqueue(rootPath);

            var rootNode = new RootNodeDefinition(rootPath);
            var schemaVisitor = new SchemaVisitor(rootNode);

            // SHA256 -> 32 bytes.
            byte[] hash = new byte[32];

            string asmVersion = typeof(FlatSharpCompiler).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";
            Encoding.UTF8.GetBytes(asmVersion, 0, asmVersion.Length, hash, 0);

            while (visitOrder.Count > 0)
            {
                string next = visitOrder.Dequeue();
                string fbs = includeLoader.LoadInclude(next);

                using (var sha256 = SHA256Managed.Create())
                {
                    byte[] componentHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(fbs));
                    for (int i = 0; i < hash.Length; ++i)
                    {
                        hash[i] ^= componentHash[i];
                    }
                }

                schemaVisitor.CurrentFileName = next;

                // Traverse the graph of includes.
                var includeVisitor = new IncludeVisitor(next, includes, visitOrder);

                ErrorContext.Current.WithScope(next, () =>
                {
                    var schema = GetParser(fbs).schema();
                    ErrorContext.Current.ThrowIfHasErrors();

                    includeVisitor.Visit(schema); 
                    ErrorContext.Current.ThrowIfHasErrors();

                    schemaVisitor.Visit(schema); 
                    ErrorContext.Current.ThrowIfHasErrors();
                });
            }

            rootNode.InputHash = $"{asmVersion}.{Convert.ToBase64String(hash)}";
            return rootNode;
        }

        private static FlatBuffersParser GetParser(string fbs)
        {
            AntlrInputStream input = new AntlrInputStream(fbs);
            FlatBuffersLexer lexer = new FlatBuffersLexer(input);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            FlatBuffersParser parser = new FlatBuffersParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());
            return parser;
        }

        internal static string TestHookCreateCSharp(
            string fbsSchema,
            CompilerOptions options,
            Dictionary<string, string>? includes = null)
        {
            InMemoryIncludeLoader includeLoader = new InMemoryIncludeLoader
            {
                { "root.fbs", fbsSchema }
            };

            if (includes != null)
            {
                foreach (var pair in includes)
                {
                    includeLoader[pair.Key] = pair.Value;
                }
            }

            using (ErrorContext.Current)
            {
                CreateCSharp(ParseSyntax("root.fbs", includeLoader), options, out string csharp);
                return csharp;
            }
        }

        private static void CreateCSharp(
            BaseSchemaMember rootNode,
            CompilerOptions options,
            out string csharp)
        {
            csharp = string.Empty;

            try
            {
                ErrorContext.Current.ThrowIfHasErrors();
                FlatSharpInternal.Assert(!string.IsNullOrEmpty(rootNode.DeclaringFile), "RootNode missing declaring file");

                Assembly? assembly = null;
                CodeWriter writer = new CodeWriter();
                var steps = new[]
                {
                CodeWritingPass.Initialization,
                CodeWritingPass.PropertyModeling,
                CodeWritingPass.SerializerGeneration,
                CodeWritingPass.RpcGeneration,
            };

                foreach (var step in steps)
                {
                    var localOptions = options;

                    if (step <= CodeWritingPass.PropertyModeling)
                    {
                        localOptions = localOptions with { NullableWarnings = false };
                    }

                    if (step > CodeWritingPass.Initialization)
                    {
                        csharp = writer.ToString();
                        (assembly, _, _) = RoslynSerializerGenerator.CompileAssembly(csharp, true);
                    }

                    writer = new CodeWriter();

                    rootNode.WriteCode(
                        writer,
                        new CompileContext
                        {
                            CompilePass = step,
                            Options = localOptions,
                            RootFile = rootNode.DeclaringFile,
                            PreviousAssembly = assembly,
                            TypeModelContainer = TypeModelContainer.CreateDefault(),
                        });

                    ErrorContext.Current.ThrowIfHasErrors();
                }

                csharp = writer.ToString();

            }
            finally
            {
                if (csharp is not null)
                {
                    csharp = RoslynSerializerGenerator.GetFormattedText(csharp);
                }
            }
        }
    }
}
