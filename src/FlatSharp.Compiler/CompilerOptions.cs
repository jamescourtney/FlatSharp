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
    using CommandLine;

    public record CompilerOptions
    {
        [Option('i', "input", HelpText = "FBS input file", Required = true)]
        public string InputFile { get; set; } = string.Empty;

        [Option('o', "output", HelpText = "Output directory", Required = true)]
        public string? OutputDirectory { get; set; }

        [Option("nullable-warnings", Default = false)]
        public bool? NullableWarnings { get; set; }

        [Option("flatc-path", Hidden = true)]
        public string? FlatcPath { get; set; }
    }
}
