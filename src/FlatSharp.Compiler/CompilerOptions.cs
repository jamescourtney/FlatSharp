﻿/*
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

using CommandLine;

namespace FlatSharp.Compiler;

public record CompilerOptions
{
    [Option('i', "input", HelpText = "FBS input file", Required = true, Separator = ';')]
    public IEnumerable<string> InputFiles { get; set; } = Array.Empty<string>();

    [Option('o', "output", HelpText = "Output directory", Required = true)]
    public string? OutputDirectory { get; set; }

    [Option('I', "includes", HelpText = "Includes search directory path(s)")]
    public string? IncludesDirectory { get; set; }

    [Option("normalize-field-names", Default = true, HelpText = "Normalize snake_case and lowerPascalCase field names to UpperPascalCase.")]
    public bool? NormalizeFieldNames { get; set; }

    [Option("nullable-warnings", Default = false, HelpText = "Emit full nullable annotations and enable warnings.")]
    public bool? NullableWarnings { get; set; }

    [Option("gen-poolable", Hidden = false, Default = false, HelpText = "EXPERIMENTAL: Generate extra code to enable object pooling for allocation reductions.")]
    public bool? GeneratePoolableObjects { get; set; }

    [Option("flatc-path", Hidden = true)]
    public string? FlatcPath { get; set; }

    [Option("debug", Hidden = true, Default = false)]
    public bool Debug { get; set; }
    
    [Option("unity-assembly-path", HelpText = "Path to assembly (e.g. UnityEngine.dll) which enables Unity support.")]
    public string? UnityAssemblyPath { get; set; }

    [Option("instrument", Hidden = true, Default = false)]
    public bool Instrument { get; set; }

    // Suppress auto generated markers for mutation testing.
    [Option("mutation-testing-mode", Hidden = true, Default = false)]
    public bool MutationTestingMode { get; set; }
}
