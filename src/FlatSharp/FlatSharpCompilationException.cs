/*
 * Copyright 2018 James Courtney
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

namespace FlatSharp
{
    using System;

    /// <summary>
    /// An exception thrown when Roslyn fails to compile the generated C# code.
    /// </summary>
    public sealed class FlatSharpCompilationException : Exception
    {
        public FlatSharpCompilationException(string[] compilerErrors, string cSharp) 
            : base("FlatSharp failed to generate proper C# for your schema. Please see the list of errors for precise errors.")
        {
            this.CompilerErrors = compilerErrors;
            this.CSharp = cSharp;
        }

        /// <summary>
        /// The list of individual errors from the C# compiler.
        /// </summary>
        public string[] CompilerErrors { get; }

        /// <summary>
        /// The generated C# that failed to compile.
        /// </summary>
        public string CSharp { get; }
    }
}
