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
                    string outputFileName = args[0] + ".generated.cs";
                    string fbsText = File.ReadAllText(args[0]);

                    int attemptCount = 0;
                    while (attemptCount++ <= 5)
                    {
                        try
                        {
                            string inputHash;
                            using (var sha = System.Security.Cryptography.SHA256.Create())
                            {
                                inputHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(fbsText)));
                            }

                            if (File.Exists(outputFileName))
                            {
                                string existingOutput = File.ReadAllText(outputFileName);
                                if (existingOutput.Contains(inputHash))
                                {
                                    // Input file unchanged.
                                    return 0;
                                }
                            }

                            string cSharp = CreateCSharp(File.ReadAllText(args[0]), inputHash);
                            File.WriteAllText(outputFileName, cSharp);
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

        public static Assembly CompileAndLoadAssembly(string fbsSchema, string inputHash = "")
        {
            using (var context = ErrorContext.Current)
            {
                context.PushScope("$");
                try
                {
                    string cSharp = CreateCSharp(fbsSchema, inputHash);
                    var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true);
                    string debugText = formattedText();
                    return assembly;
                }
                finally
                {
                    context.PopScope();
                }
            }
        }

        internal static string CreateCSharp(string fbsSchema, string inputHash)
        {
            AntlrInputStream input = new AntlrInputStream(fbsSchema);
            FlatBuffersLexer lexer = new FlatBuffersLexer(input);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            FlatBuffersParser parser = new FlatBuffersParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            SchemaVisitor visitor = new SchemaVisitor(inputHash);
            BaseSchemaMember rootNode = visitor.Visit(parser.schema());

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            // Create the first pass of the code. This pass includes the data contracts from the FBS file.
            // If the schema requests a pregenerated serializer, then we'll need to load this code, generate
            // the serializer, and then rebuild it.
            CodeWriter writer = new CodeWriter();
            rootNode.WriteCode(writer, null);

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            string code = writer.ToString();

            var tablesNeedingSerializers = new List<TableOrStructDefinition>();
            FindTablesNeedingSerializers(rootNode, tablesNeedingSerializers);

            if (tablesNeedingSerializers.Count == 0)
            {
                // Hey, no serializers. We're all done. Go ahead and return the code we already generated.
                return code;
            }

            // Compile the assembly so that we may generate serializers for the data contracts defined in this FBS file.
            var (assembly, _, _) = RoslynSerializerGenerator.CompileAssembly(code, true);

            Dictionary<string, string> generatedSerializers = new Dictionary<string, string>();
            foreach (var definition in tablesNeedingSerializers)
            {
                generatedSerializers[definition.FullName] = GenerateSerializerForType(assembly, definition);
            }

            writer = new CodeWriter();
            rootNode.WriteCode(writer, generatedSerializers);

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            string rawCode = writer.ToString();
            string formattedCode = RoslynSerializerGenerator.GetFormattedTextFactory(rawCode)();
            return formattedCode;
        }

        // TODO: consider moving to TableOrStructDefinition.
        private static string GenerateSerializerForType(Assembly assembly, TableOrStructDefinition tableOrStruct)
        {
            Type type = assembly.GetType(tableOrStruct.FullName);
            var options = new FlatBufferSerializerOptions(tableOrStruct.RequestedSerializer.Value);
            var generator = new RoslynSerializerGenerator(options);

            var method = generator
                .GetType()
                .GetMethod(nameof(RoslynSerializerGenerator.GenerateCSharp), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .MakeGenericMethod(type);

            string code = (string)method.Invoke(generator, new[] { "private" });
            return code;
        }

        /// <summary>
        /// Recursively find tables for which the schema has asked for us to generate serializers.
        /// </summary>
        private static void FindTablesNeedingSerializers(BaseSchemaMember node, List<TableOrStructDefinition> items)
        {
            if (node is TableOrStructDefinition tableOrStruct)
            {
                if (tableOrStruct.RequestedSerializer != null)
                {
                    items.Add(tableOrStruct);
                }
            }

            foreach (var childNode in node.Children.Values)
            {
                FindTablesNeedingSerializers(childNode, items);
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
