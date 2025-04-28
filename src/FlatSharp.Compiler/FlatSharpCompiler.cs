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
using System.Text.RegularExpressions;
using System.Threading;

using CommandLine;
using FlatSharp.CodeGen;
using FlatSharp.Compiler.SchemaModel;
using FlatSharp.TypeModel;

namespace FlatSharp.Compiler;

public class FlatSharpCompiler
{
    public const string FailureMessage = "// !! FLATSHARP CODE GENERATION FAILED. THIS FILE MAY CONTAIN INCOMPLETE OR INACCURATE DATA !!";

    internal static CompilerOptions? CommandLineOptions { get; private set; }

    private static string AssemblyVersion => typeof(ISchemaMutator).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";

    [ExcludeFromCodeCoverage]
    static int Main(string[] args)
    {
        int exitCode = -1;

        try
        {
            Parser parser = new(with =>
            {
                with.HelpWriter = Console.Error;
                with.CaseInsensitiveEnumValues = true;
            });

            parser.ParseArguments<CompilerOptions>(args)
                  .WithParsed(x =>
                   {
                       CommandLineOptions = x;
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
        if (options.Debug)
        {
            Debugger.Launch();
        }

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

        // Create the output directory if it doesn't exist.
        if (!Directory.Exists(options.OutputDirectory))
        {
            Directory.CreateDirectory(options.OutputDirectory);
        }

        if (!string.IsNullOrEmpty(options.UnityAssemblyPath) && !File.Exists(options.UnityAssemblyPath))
        {
            Console.Error.WriteLine("FlatSharp compiler: Unity assembly path specified does not exist.");
            return -1;
        }

        string outputFileName = "FlatSharp.generated.cs";
        if (options.MutationTestingMode)
        {
            StrykerSuppressor.IsEnabled = true;
            outputFileName = "FlatSharp.cs";
        }

        string outputFullPath = Path.Combine(options.OutputDirectory, outputFileName);

        try
        {
            int attemptCount = 0;
            while (attemptCount++ <= 5)
            {
                try
                {
                    var bfbs = GetBfbs(options);

                    string inputHash = ComputeInputHash(bfbs, options);

                    if (IsInputUnchanged(outputFullPath, inputHash))
                    {
                        return 0;
                    }

                    string cSharp = string.Empty;
                    Exception? exception = null;
                    try
                    {
                        cSharp = CreateCSharp(bfbs, inputHash, options);
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

    [ExcludeFromCodeCoverage]
    private static bool IsInputUnchanged(string outputFullPath, string inputHash)
    {
        if (!File.Exists(outputFullPath))
        {
            return false;
        }

        using StreamReader reader = File.OpenText(outputFullPath);

        bool hasFailureMessage = false;
        bool containsInputHash = false;

        for (int i = 0; i < 100; ++i)
        {
            string? line = reader.ReadLine();
            if (line is null)
            {
                break;
            }

            hasFailureMessage |= line.Contains(FailureMessage);
            containsInputHash |= line.Contains(inputHash);
        }

        if (hasFailureMessage)
        {
            return false;
        }

        if (containsInputHash)
        {
            return true;
        }

        return false;
    }

    [ExcludeFromCodeCoverage]
    private static string ComputeInputHash(List<(byte[] bfbs, string _)> bfbs, CompilerOptions options)
    {
        static void MergeHashes(byte[] hash, byte[] temp)
        {
            for (int i = 0; i < temp.Length; ++i)
            {
                hash[i] ^= temp[i];
            }
        }

        string inputHash = AssemblyVersion;

        using (var hash = SHA256.Create())
        {
            // Use the assembly hash as the base; this means each build will change the hash for the same schema.
            byte[] hashBytes = hash.ComputeHash(File.ReadAllBytes(typeof(FlatSharpCompiler).Assembly.Location));

            // Supplement with command line options.
            MergeHashes(hashBytes, hash.ComputeHash(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(options))));

            // Merge each of the schema files.
            foreach (var schema in bfbs)
            {
                MergeHashes(hashBytes, hash.ComputeHash(schema.bfbs));
            }

            inputHash += "." + Convert.ToBase64String(hashBytes);
        }

        return inputHash;
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
            Assembly[] additionalRefs = BuildAdditionalReferences(options, additionalReferences);

            options.InputFiles = fbsFiles.Select(x => x.FullName);

            List<(byte[], string)> bfbs = GetBfbs(options);
            string cSharp = CreateCSharp(bfbs, "hash", options);

            var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true, additionalRefs);
            string debugText = formattedText();
            return (assembly, cSharp);
        }
        finally
        {
            ErrorContext.Current.Clear();
        }
    }

    private static List<(byte[] bfbs, string fbsPath)> GetBfbs(CompilerOptions options)
    {
        string flatcPath;

        if (options.FlatcPath is null)
        {
            flatcPath = GetFlatcPath();
        }
        else
        {
            flatcPath = options.FlatcPath;
        }

        List<(byte[] bfbs, string fbsPath)> results = new();
        string temp = Path.GetTempPath();
        string dirName = $"flatsharpcompiler_temp_{Guid.NewGuid():n}";
        string outputDir = Path.Combine(temp, dirName);
        Directory.CreateDirectory(outputDir);

        try
        {
            foreach (var inputFile in options.InputFiles)
            {
                string inputFullPath = PathHelpers.NormalizePathName(inputFile);

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
                    "--bfbs-filenames",
                    Environment.CurrentDirectory,
                    "--bfbs-absolute-paths",
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

                foreach (var arg in args)
                {
                    p.StartInfo.ArgumentList.Add(arg);
                }

                p.StartInfo.ArgumentList.Add(inputFullPath);

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
                    results.Add((
                        File.ReadAllBytes(Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputFullPath) + ".bfbs")),
                        inputFullPath));
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
        }
        finally
        {
            Directory.Delete(outputDir, recursive: true);
        }

        return results;
    }

    [ExcludeFromCodeCoverage]
    private static string GetFlatcPath()
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
            if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            {
                os = "macos_arm";
                name = "flatc";
            }
            else
            {
                os = "macos_intel";
                name = "flatc";
            }
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
        string flatcPath = Path.Combine(currentDirectory, "flatc", os, name);

        return flatcPath;
    }

    private static string CreateCSharp(
        List<(byte[] bfbs, string inputPath)> bfbs,
        string inputHash,
        CompilerOptions options)
    {
        string csharp = string.Empty;

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
            List<Func<string, string>> postProcessTransforms = new();

            var mutators = new ISchemaMutator[]
            {
                new FieldNameNormalizerSchemaMutator(),
                new ExternalTypeSchemaMutator(),
                new PathNormalizerSchemaMutator(),
            };

            Stopwatch sw = Stopwatch.StartNew();
            ISerializer<Schema.Schema> mutableSerializer = Instrument("CompileReflectionFbs", options, FlatBufferSerializer.Default.Compile<Schema.Schema>);

            foreach ((byte[] s, string fbsPath) in bfbs)
            {
                rootModel.UnionWith(ParseSchema(mutableSerializer, s, options, postProcessTransforms, mutators).ToRootModel(options, fbsPath));
            }

            ErrorContext.Current.ThrowIfHasErrors();

            Assembly[] additionalRefs = BuildAdditionalReferences(options);

            foreach (var step in steps)
            {
                var localOptions = options;
                if (step <= CodeWritingPass.PropertyModeling)
                {
                    localOptions = localOptions with { NullableWarnings = false };
                }

                if (step > CodeWritingPass.Initialization)
                {
                    csharp = Instrument($"{step}.CodeWriterToString", options, writer.ToString);
                    (assembly, _, _) = Instrument($"{step}.CompilePreviousAssembly", options, () => RoslynSerializerGenerator.CompileAssembly(csharp, true, additionalRefs));
                }

                writer = new CodeWriter();

                Instrument(
                    $"{step}.WriteCode",
                    options,
                    () =>
                    {
                        rootModel.WriteCode(
                            writer,
                            new CompileContext
                            {
                                CompilePass = step,
                                Options = localOptions,
                                InputHash = inputHash,
                                PreviousAssembly = assembly,
                                TypeModelContainer = TypeModelContainer.CreateDefault().WithUnitySupport(true),
                            });

                        return true;
                    });

                ErrorContext.Current.ThrowIfHasErrors();
            }

            csharp = Instrument($"FinalStep.CodeWriterToString", options, writer.ToString);

            csharp = Instrument($"PostProcessTransforms", options, () =>
            {
                foreach (var transform in postProcessTransforms)
                {
                    csharp = transform(csharp);
                }

                return csharp;
            });
        }
        finally
        {
            if (csharp is not null && options.PrettyPrint)
            {
                csharp = Instrument("PrettyPrint", options, () => RoslynSerializerGenerator.GetFormattedText(csharp));
            }
        }

        return csharp;
    }

    private static Schema.Schema ParseSchema(
        ISerializer<Schema.Schema> serializer,
        byte[] bfbs,
        CompilerOptions options,
        List<Func<string, string>> postProcessTransforms,
        params ISchemaMutator[] mutators)
    {
        // Mutable
        var schema = serializer.Parse(bfbs, FlatBufferDeserializationOption.GreedyMutable);

        foreach (var mutator in mutators)
        {
            mutator.Mutate(schema, options, postProcessTransforms);
        }

        // Serialize
        byte[] temp = new byte[serializer.GetMaxSize(schema)];
        serializer.Write(temp, schema);

        // Immutable.
        return serializer.Parse(temp, FlatBufferDeserializationOption.Greedy);
    }

    [ExcludeFromCodeCoverage]
    private static T Instrument<T>(string step, CompilerOptions opts, Func<T> callback)
    {
        if (opts.Instrument)
        {
            Stopwatch sw = Stopwatch.StartNew();
            T result = callback();
            sw.Stop();
            Console.WriteLine($"FlatSharp compiler instrumentation: {step} took {sw.Elapsed.TotalMilliseconds}ms.");
            return result;
        }
        else
        {
            return callback();
        }
    }

    private static Assembly[] BuildAdditionalReferences(CompilerOptions options, IEnumerable<Assembly>? additionalReferences = null)
    {
        var references = new List<Assembly>();

        if (additionalReferences is not null)
        {
            references.AddRange(additionalReferences);
        }

        if (options.UnityAssemblyPath is not null)
        {
            references.Add(Assembly.LoadFrom(options.UnityAssemblyPath));
        }
        else
        {
            references.Add(typeof(Unity.Collections.NativeArray<>).Assembly);
        }

        return references.ToArray();
    }
}
