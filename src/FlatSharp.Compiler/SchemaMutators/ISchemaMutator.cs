﻿/*
 * Copyright 2022 James Courtney
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

using FlatSharp.Compiler;
using FlatSharp.Compiler.Schema;

/// <summary>
/// Mutates a FlatBuffer FBS Schema prior to the flatsharp compiler running
/// </summary>
public interface ISchemaMutator
{
    /// <summary>
    /// Mutates the given schema.
    /// </summary>
    /// <param name="options">The compiler options</param>
    /// <param name="postProcessors">A list of delegates to invoke on the generated C#.</param>
    /// <param name="schema">The schema.</param>
    void Mutate(Schema schema, CompilerOptions options, List<Func<string, string>> postProcessors);
}