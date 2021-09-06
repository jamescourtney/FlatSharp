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
        public const string FailureMessage = "// !! FLATSHARP CODE GENERATION FAILED. THIS FILE MAY CONTAIN INCOMPLETE OR INACCURATE DATA !!";

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
                            string inputHash = typeof(FlatSharpCompiler).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";

                            byte[] fileData = File.ReadAllBytes(options.InputFile);
                            using (var hash = SHA256Managed.Create())
                            {
                                inputHash += "." + Convert.ToBase64String(hash.ComputeHash(fileData));
                            }

                            if (File.Exists(outputFullPath))
                            {
                                string existingOutput = File.ReadAllText(outputFullPath);
                                if (existingOutput.Contains(inputHash) && !existingOutput.StartsWith(FailureMessage))
                                {
                                    // Input file unchanged.
                                    return 0;
                                }
                            }

                            string cSharp = string.Empty;
                            Exception? exception = null;
                            try
                            {
                                CreateCSharp(fileData, inputHash, options, out cSharp);
                            }
                            catch (Exception ex)
                            {
                                exception = ex;
                                throw;
                            }
                            finally
                            {
                                if (exception is not null)
                                {
                                    cSharp = $"{FailureMessage}\r\n/* Error: \r\n{exception}\r\n*/\r\n\r\n{cSharp}";
                                }

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

        private static void CreateCSharp(
            byte[] bfbs,
            string bfbsHash,
            CompilerOptions options,
            out string csharp)
        {
            csharp = string.Empty;

            try
            {
                var schema = FlatBufferSerializer.Default.Parse<Schema.Schema>(bfbs);

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

                    schema.WriteCode(writer, null!);

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
