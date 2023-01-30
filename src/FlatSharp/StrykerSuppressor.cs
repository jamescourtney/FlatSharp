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

using FlatSharp.Attributes;
using System.Linq;

namespace FlatSharp;

public static class StrykerSuppressor
{
    public static bool IsEnabled 
    {
        get;
        set;
    }

    public static string BitConverterTypeName => IsEnabled ? "BitConverter" : "MockBitConverter";

    public static string SuppressNextLine(string condition = "all")
    {
        if (IsEnabled)
        {
            return $"//Stryker disable once {condition}";
        }

        return string.Empty;
    }

    public static string ExcludeFromCodeCoverage()
    {
        if (IsEnabled)
        {
            return $"[{typeof(ExcludeFromCodeCoverageAttribute).GetCompilableTypeName()}]";
        }

        return string.Empty;
    }
}
