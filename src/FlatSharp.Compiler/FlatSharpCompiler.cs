/*
 * Copyright 2020 James Courtney
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
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;

    public static class FlatSharpCompiler
    {
        static int Main(string[] args)
        {
            using (var context = ErrorContext.Current)
            {
                try
                {
                    context.PushScope("$");

                    // Read existing file to see if we even need to do any work.
                    string fbsFileName = Path.GetFileName(args[0]);
                    string outputFileName = fbsFileName + ".generated.cs";
                    string outputPath = args.Length < 2 || string.IsNullOrEmpty(args[1]) ? Path.GetDirectoryName(args[0]) : args[1];
                    string outputFullPath = Path.Combine(outputPath, outputFileName);
                    // string fbsText = File.ReadAllText(args[0]);

                    int attemptCount = 0;
                    while (attemptCount++ <= 5)
                    {
                        try
                        {
                            RootNodeDefinition rootNode = ParseSyntax(args[0], new IncludeFileLoader());

                            if (File.Exists(outputFullPath))
                            {
                                string existingOutput = File.ReadAllText(outputFullPath);
                                if (existingOutput.Contains(rootNode.InputHash))
                                {
                                    // Input file unchanged.
                                    return 0;
                                }
                            }

                            string cSharp = CreateCSharp(rootNode);
                            File.WriteAllText(outputFullPath, cSharp);
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
                catch (FlatSharpCompilationException ex)
                {
                    foreach (var message in ex.CompilerErrors)
                    {
                        Console.Error.WriteLine(message);
                    }

                    return -1;
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine($"File '{args[0]}' was not found");
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
            IEnumerable<Assembly> additionalReferences = null,
            Dictionary<string, string> additionalIncludes = null)
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
                    string cSharp = CreateCSharp(rootNode);
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

        internal static RootNodeDefinition TestHookParseSyntax(string fbsSchema, Dictionary<string, string> includes = null)
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
                includeVisitor.Visit(GetParser(fbs).schema());
                schemaVisitor.Visit(GetParser(fbs).schema());
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

        internal static string TestHookCreateCSharp(string fbsSchema, Dictionary<string, string> includes = null)
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
                return CreateCSharp(ParseSyntax("root.fbs", includeLoader));
            }
        }

        private static string CreateCSharp(BaseSchemaMember rootNode)
        {
            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            var tablesNeedingSerializers = new List<TableOrStructDefinition>();
            var rpcDefinitions = new List<RpcDefinition>();
            FindItemsRequiringSecondCodePass(rootNode, tablesNeedingSerializers, rpcDefinitions);

            if (tablesNeedingSerializers.Count == 0 && rpcDefinitions.Count == 0)
            {
                // Hey, no serializers or RPCs. We're all done. Go ahead and return the code we already generated.
                CodeWriter tempWriter = new CodeWriter();
                rootNode.WriteCode(tempWriter, CodeWritingPass.SecondPass, rootNode.DeclaringFile, new Dictionary<string, string>());

                if (ErrorContext.Current.Errors.Any())
                {
                    throw new InvalidFbsFileException(ErrorContext.Current.Errors);
                }

                return tempWriter.ToString();
            }

            // Compile the assembly so that we may generate serializers for the data contracts defined in this FBS file.;
            // Compile with firstpass here to include all data (even stuff from includes).
            CodeWriter writer = new CodeWriter();
            rootNode.WriteCode(writer, CodeWritingPass.FirstPass, rootNode.DeclaringFile, new Dictionary<string, string>());
            string code = writer.ToString();
            var (assembly, _, _) = RoslynSerializerGenerator.CompileAssembly(code, true);

            Dictionary<string, string> generatedSerializers = new Dictionary<string, string>();
            foreach (var definition in tablesNeedingSerializers)
            {
                generatedSerializers[definition.FullName] = GenerateSerializerForType(assembly, definition);
            }

            writer = new CodeWriter();
            rootNode.WriteCode(writer, CodeWritingPass.SecondPass, rootNode.DeclaringFile, generatedSerializers);

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            string rawCode = writer.ToString();
            string formattedCode = RoslynSerializerGenerator.GetFormattedText(rawCode);
            return formattedCode;
        }

        // TODO: consider moving to TableOrStructDefinition.
        private static string GenerateSerializerForType(Assembly assembly, TableOrStructDefinition tableOrStruct)
        {
            Type type = assembly.GetType(tableOrStruct.FullName);
            var options = new FlatBufferSerializerOptions(tableOrStruct.RequestedSerializer.Value);
            var generator = new RoslynSerializerGenerator(options, SchemaDefinition.BuiltInTypeModelProvider);

            var method = generator
                .GetType()
                .GetMethod(nameof(RoslynSerializerGenerator.GenerateCSharp), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .MakeGenericMethod(type);

            try
            {
                string code = (string)method.Invoke(generator, new[] { "private" });
                return code;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Recursively find tables for which the schema has asked for us to generate serializers.
        /// </summary>
        private static void FindItemsRequiringSecondCodePass(
            BaseSchemaMember node,
            List<TableOrStructDefinition> tables,
            List<RpcDefinition> rpcs)
        {
            if (node is TableOrStructDefinition tableOrStruct)
            {
                if (tableOrStruct.RequestedSerializer != null)
                {
                    tables.Add(tableOrStruct);
                }
            }
            else if (node is RpcDefinition rpc)
            {
                rpcs.Add(rpc);
            }

            foreach (var childNode in node.Children.Values)
            {
                FindItemsRequiringSecondCodePass(childNode, tables, rpcs);
            }
        }
    }

    internal class CustomErrorListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(
            [NotNull] IRecognizer recognizer,
            [Nullable] IToken offendingSymbol,
            int line,
            int charPositionInLine,
            [NotNull] string msg,
            [Nullable] RecognitionException e)
        {
            ErrorContext.Current?.RegisterError($"Syntax error FBS file: Token='{offendingSymbol.Text}', Msg='{msg}' Line='{line}:{charPositionInLine}");
        }
    }
}
