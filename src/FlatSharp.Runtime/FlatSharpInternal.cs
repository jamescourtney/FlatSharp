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

namespace FlatSharp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    internal static class FlatSharpInternal
    {
        [ExcludeFromCodeCoverage]
        public static void Assert(
            [DoesNotReturnIf(false)] bool condition, 
            string message, 
            [CallerMemberName] string memberName = "", 
            [CallerFilePath] string fileName = "", 
            [CallerLineNumber] int lineNumber = -1)
        {
            if (!condition)
            {
                throw new FlatSharpInternalException(message, memberName, fileName, lineNumber);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class FlatSharpInternalException : Exception
    {
        public FlatSharpInternalException(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = -1) : base($"FlatSharp Internal Error! Message = '{message}'.File = '{System.IO.Path.GetFileName(fileName)}', Member = '{memberName}:{lineNumber}'")
        {
        }
    }
}
