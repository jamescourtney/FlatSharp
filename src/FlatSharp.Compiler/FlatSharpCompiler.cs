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

using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using CommandLine;
using FlatSharp.CodeGen;
using FlatSharp.Compiler.SchemaModel;
using FlatSharp.TypeModel;

namespace FlatSharp.Compiler;

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
        if (options.InputFiles?.Any() != true)
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
        string outputFileName = "FlatSharp.generated.cs";
        string outputFullPath = Path.Combine(options.OutputDirectory, outputFileName);

        try
        {
            int attemptCount = 0;
            while (attemptCount++ <= 5)
            {
                try
                {
                    string inputHash = AssemblyVersion;

                    List<byte[]> bfbs = GetBfbs(options);

                    using (var hash = SHA256.Create())
                    {
                        byte[] hashBytes = new byte[32];
                        foreach (var schema in bfbs)
                        {
                            var tempHash = hash.ComputeHash(schema);
                            for (int i = 0; i < 32; ++i)
                            {
                                hashBytes[i] ^= tempHash[i];
                            }
                        }

                        inputHash += "." + Convert.ToBase64String(hashBytes);
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
        catch (FileNotFoundException fex)
        {
            Console.Error.WriteLine($"File '{fex.FileName}' was not found");
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
        string temp = Path.GetTempFileName() + ".fbs";
        File.WriteAllText(temp, fbsSchema);

        try
        {
            return CompileAndLoadAssemblyWithCode(new[] { new FileInfo(temp) }, options, additionalReferences);
        }
        finally
        {
            File.Delete(temp);
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

    // Test hook
    internal static Assembly CompileAndLoadAssembly(
        IEnumerable<(string FileName, string Content)> fbsSchemas,
        CompilerOptions options,
        IEnumerable<Assembly>? additionalReferences = null)
    {
        Assembly[] additionalRefs = additionalReferences?.ToArray() ?? Array.Empty<Assembly>();

        var tempDir = Path.GetFileNameWithoutExtension(Path.GetTempFileName());

        if (!string.IsNullOrEmpty(options.IncludesDirectory))
        {
            // Convert includes directories into absolute paths as would be done by the targets file
            var paths = options.IncludesDirectory.Split(';', StringSplitOptions.RemoveEmptyEntries);
            options.IncludesDirectory = string.Join(';', paths.Select(path => Path.GetFullPath(Path.Combine(tempDir, path))));
        }

        try
        {
            Directory.CreateDirectory(tempDir);

            var fbsFiles = new List<FileInfo>();

            foreach (var fbsSchema in fbsSchemas)
            {
                var fbsFile = new FileInfo(Path.Combine(tempDir, fbsSchema.FileName));
                fbsFile.Directory?.Create();
                File.WriteAllText(fbsFile.FullName, fbsSchema.Content);

                fbsFiles.Add(fbsFile);
            }

            var (asm, _) = CompileAndLoadAssemblyWithCode(fbsFiles, options, additionalRefs);

            return asm;
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    // Test hook
    private static (Assembly, string) CompileAndLoadAssemblyWithCode(
        IEnumerable<FileInfo> fbsFiles,
        CompilerOptions options,
        IEnumerable<Assembly>? additionalReferences = null)
    {
        try
        {
            Assembly[] additionalRefs = additionalReferences?.ToArray() ?? Array.Empty<Assembly>();

            options.InputFiles = fbsFiles.Select(x => x.FullName);

            List<byte[]> bfbs = GetBfbs(options);
            CreateCSharp(bfbs, "hash", options, out string cSharp);

            var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true, additionalRefs);
            string debugText = formattedText();
            return (assembly, cSharp);
        }
        finally
        {
            ErrorContext.Current.Clear();
        }
    }

    private static List<byte[]> GetBfbs(CompilerOptions options)
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

        using var p = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = flatcPath,
            }
        };

        var args = new List<string>
        {
            "-b",
            "--schema",
            "--bfbs-comments",
            "--bfbs-builtins",
            "--no-warnings",
            "-o",
            outputDir,
        };

        if (!string.IsNullOrEmpty(options.IncludesDirectory))
        {
            // One or more includes directory has been specified
            foreach (var includePath in options.IncludesDirectory.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                args.AddRange(new[]
                {
                    "-I",
                    new DirectoryInfo(includePath).FullName,
                });
            }
        }

        foreach (var item in options.InputFiles)
        {
            args.Add(new FileInfo(item).FullName);
        }

        foreach (var arg in args)
        {
            p.StartInfo.ArgumentList.Add(arg);
        }

        try
        {
            p.EnableRaisingEvents = true;

            var lines = new List<string>();

            void OnDataReceived(object sender, DataReceivedEventArgs args)
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    lines.Add(args.Data);
                }
            }

            p.OutputDataReceived += OnDataReceived;
            p.ErrorDataReceived += OnDataReceived;

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();

            if (p.ExitCode == 0)
            {
                List<byte[]> bfbs = new();
                foreach (string output in Directory.GetFiles(outputDir, "*.bfbs"))
                {
                    bfbs.Add(File.ReadAllBytes(output));
                }

                return bfbs;
            }
            else
            {
                foreach (var line in lines)
                {
                    ErrorContext.Current.RegisterError(line);
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
        List<byte[]> bfbs,
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

            RootModel rootModel = new(Schema.AdvancedFeatures.None);
            foreach (var s in bfbs)
            {
                rootModel.UnionWith(ParseSchema(s, options).ToRootModel());
            }

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
                        InputHash = inputHash,
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

    private static Schema.Schema ParseSchema(byte[] bfbs, CompilerOptions options)
    {
        ISerializer<Schema.Schema> mutableSerializer = FlatBufferSerializer.Default.Compile<Schema.Schema>();
        ISerializer<Schema.Schema> immutableSerializer = mutableSerializer
            .WithSettings(new SerializerSettings 
            { 
                DeserializationMode = FlatBufferDeserializationOption.Greedy 
            });

        // Mutable
        var schema = mutableSerializer.Parse(bfbs);

        // Modify
        if (options.NormalizeFieldNames == true)
        {
            foreach (Schema.FlatBufferObject item in schema.Objects)
            {
                bool? preserveFieldCasingParent = item.Attributes != null ? new FlatSharpAttributes(item.Attributes).PreserveFieldCasing : default;
                foreach (Schema.Field field in item.Fields)
                {
                    bool? preserveFieldCasing = field.Attributes != null ? preserveFieldCasing = new FlatSharpAttributes(field.Attributes).PreserveFieldCasing : default;

                    var preserve = (preserveFieldCasing ?? preserveFieldCasingParent) switch
                    {
                        false or null => false,
                        true => true,
                    };

                    if (!preserve)
                    {
                        field.Name = NormalizeFieldName(field.Name);
                    }
                }
            }
        }

        // Serialize
        byte[] temp = new byte[mutableSerializer.GetMaxSize(schema)];
        mutableSerializer.Write(temp, schema);

        // Immutable.
        return immutableSerializer.Parse(temp);
    }

    private static string NormalizeFieldName(string name)
    {
        StringBuilder sb = new();
        string[] parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            sb.Append(char.ToUpperInvariant(part[0]));
            if (part.Length > 1)
            {
                sb.Append(part.AsSpan()[1..]);
            }
        }

        return sb.ToString();
    }
}
