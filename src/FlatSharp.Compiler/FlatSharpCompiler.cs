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
    using CommandLine;
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Threading;

    public class FlatSharpCompiler
    {
        public const string FailureMessage = "// !! FLATSHARP CODE GENERATION FAILED. THIS FILE MAY CONTAIN INCOMPLETE OR INACCURATE DATA !!";

        private static string AssemblyVersion => typeof(FlatSharpCompiler).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";

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
                int attemptCount = 0;
                while (attemptCount++ <= 5)
                {
                    try
                    {
                        string inputHash = AssemblyVersion;

                        byte[] bfbs = GetBfbs(options);

                        using (var hash = SHA256Managed.Create())
                        {
                            inputHash += "." + Convert.ToBase64String(hash.ComputeHash(bfbs));
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
                            CreateCSharp(bfbs, inputHash, options, out cSharp);
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

                            File.WriteAllText(outputFullPath, cSharp);
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
            catch (InvalidFlatBufferDefinitionException ex)
            {
                Console.Error.WriteLine(ex.Message);
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
                ErrorContext.Current.Clear();
            }

            return 0;
        }

        // Test hook
        internal static (Assembly, string) CompileAndLoadAssemblyWithCode(
            string fbsSchema,
            CompilerOptions options,
            IEnumerable<Assembly>? additionalReferences = null)
        {
            string fbsFile = Path.GetTempFileName() + ".fbs";
            try
            {
                Assembly[] additionalRefs = additionalReferences?.ToArray() ?? Array.Empty<Assembly>();

                File.WriteAllText(fbsFile, fbsSchema);
                options.InputFile = fbsFile;

                byte[] bfbs = GetBfbs(options);
                CreateCSharp(bfbs, "hash", options, out string cSharp);

                var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true, additionalRefs);
                string debugText = formattedText();
                return (assembly, cSharp);
            }
            finally
            {
                ErrorContext.Current.Clear();
                File.Delete(fbsFile);
            }
        }

        // Test hook
        internal static Assembly CompileAndLoadAssembly(
            string fbsSchema,
            CompilerOptions options,
            IEnumerable<Assembly>? additionalReferences = null)
        {
            (Assembly asm, _) = CompileAndLoadAssemblyWithCode(
                fbsSchema,
                options,
                additionalReferences);

            return asm;
        }

        internal static byte[] GetBfbs(CompilerOptions options)
        {
            string flatcPath;

            if (options.FlatcPath is null)
            {
                string os;
                string name;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    os = "windows";
                    name = "flatc.exe";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    os = "macos";
                    name = "flatc";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    os = "linux";
                    name = "flatc";
                }
                else
                {
                    throw new InvalidOperationException("FlatSharp compiler is not supported on this operating system.");
                }

                string currentProcess = typeof(FlatSharpCompiler).Assembly.Location;
                string currentDirectory = Path.GetDirectoryName(currentProcess)!;
                flatcPath = Path.Combine(currentDirectory, "flatc", os, name);
            }
            else
            {
                flatcPath = options.FlatcPath;
            }

            string temp = Path.GetTempPath();
            string dirName = $"flatsharpcompiler_temp_{Guid.NewGuid():n}";
            string outputDir = Path.Combine(temp, dirName);

            Directory.CreateDirectory(outputDir);
            FileInfo info = new FileInfo(options.InputFile);

            System.Diagnostics.Process p = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    FileName = flatcPath,
                    Arguments = $"-b --schema --bfbs-comments --bfbs-builtins --bfbs-filenames \"{info.DirectoryName}\" --no-warnings -o \"{outputDir}\" \"{info.FullName}\"",
                }
            };

            try
            {
                p.Start();
                string stdout = p.StandardOutput.ReadToEnd();
                string stderr = p.StandardError.ReadToEnd();

                p.WaitForExit();

                if (p.ExitCode == 0)
                {
                    string output = Directory.GetFiles(outputDir, "*.bfbs").Single();
                    return File.ReadAllBytes(output);
                }
                else
                {
                    string[] lines = stdout.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            ErrorContext.Current.RegisterError(line);
                        }
                    }

                    ErrorContext.Current.ThrowIfHasErrors();
                    throw new InvalidFbsFileException("Unknown error when invoking flatc. Process exited with error, but didn't write any errors.");
                }
            }
            finally
            {
                Directory.Delete(outputDir, recursive: true);
            }
        }

        private static void CreateCSharp(
            byte[] bfbs,
            string inputHash,
            CompilerOptions options,
            out string csharp)
        {
            csharp = string.Empty;

            try
            {
                Assembly? assembly = null;
                CodeWriter writer = new CodeWriter();

                // FlatSharp is a three pass compiler.
                // Pass 1: We write the initial class definitions, etc.
                // Pass 2: We reflect on what we wrote to fill in a few pieces of missing data.
                // Pass 3: We generate serializers and RPC definitions.
                var steps = new[]
                {
                    CodeWritingPass.Initialization,
                    CodeWritingPass.PropertyModeling,
                    CodeWritingPass.SerializerAndRpcGeneration,
                };

                FlatBufferSerializer serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Greedy); // immutable.
                var schema = serializer.Parse<Schema.Schema>(bfbs);
                var rootModel = schema.ToRootModel();

                ErrorContext.Current.ThrowIfHasErrors();

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

                    rootModel.WriteCode(
                        writer,
                        new CompileContext
                        {
                            CompilePass = step,
                            Options = localOptions,
                            RootFile = $"//{Path.GetFileName(options.InputFile)}",
                            InputHash = inputHash,
                            Root = schema,
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
