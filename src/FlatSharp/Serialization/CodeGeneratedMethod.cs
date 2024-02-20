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

namespace FlatSharp.CodeGen;

/// <summary>
/// Defines the result of code generating a method.
/// </summary>
public record CodeGeneratedMethod
{
    public CodeGeneratedMethod(string methodBody)
    {
        this.MethodBody = methodBody;
    }

    /// <summary>
    /// The body of the method.
    /// </summary>
    public string MethodBody { get; init; }

    /// <summary>
    /// A class definition.
    /// </summary>
    public string? ClassDefinition { get; init; }

    /// <summary>
    /// Indicates if the method should be marked with aggressive inlining.
    /// </summary>
    public bool? IsMethodInline { get; init; }

    public string GetMethodImplAttribute()
    {
        if (this.IsMethodInline == true)
        {
            return GetInlineAttribute();
        }
        else if (this.IsMethodInline == false)
        {
            return GetNonInlineAttribute();
        }
        else
        {
            return string.Empty;
        }
    }

    public static string GetInlineAttribute()
    {
        string inlining = $"System.Runtime.CompilerServices.MethodImplOptions.{nameof(MethodImplOptions.AggressiveInlining)}";
        return $"[{typeof(MethodImplAttribute).GetGlobalCompilableTypeName()}({inlining})]";
    }

    public static string GetNonInlineAttribute()
    {
        string inlining = $"System.Runtime.CompilerServices.MethodImplOptions.{nameof(MethodImplOptions.NoInlining)}";
        return $"[{typeof(MethodImplAttribute).GetGlobalCompilableTypeName()}({inlining})]";
    }
}
