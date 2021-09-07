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
    using FlatSharp.TypeModel;
    using System.Reflection;

    /// <summary>
    /// Compilation context
    /// </summary>
    public record CompileContext
    {
        /// <summary>
        /// The current compilation pass.
        /// </summary>
        public CodeWritingPass CompilePass { get; init; }

        /// <summary>
        /// The assembly from the previous step, if any.
        /// </summary>
        public Assembly? PreviousAssembly { get; init; }

        /// <summary>
        /// The input hash.
        /// </summary>
        public string InputHash { get; init; } = string.Empty;

        /// <summary>
        /// The Root schema.
        /// </summary>
        public Schema.Schema Root { get; init; } = new();

        /// <summary>
        /// The root FBS file in the compilation.
        /// </summary>
        public string RootFile { get; init; } = string.Empty;

        /// <summary>
        /// The fully qualified name of a static method to deep-clone an item.
        /// </summary>
        public string? FullyQualifiedCloneMethodName { get; set; }

        /// <summary>
        /// The command line options.
        /// </summary>
        public CompilerOptions Options { get; init; } = new CompilerOptions();

        /// <summary>
        /// Resolves type models.
        /// </summary>
        public TypeModelContainer TypeModelContainer { get; init; } = TypeModelContainer.CreateDefault();
    }
}
