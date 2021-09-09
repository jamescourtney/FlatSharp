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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    internal class ErrorContext
    {
        private static readonly ThreadLocal<ErrorContext> ThreadLocalContext = new ThreadLocal<ErrorContext>(() => new ErrorContext());

        public static ErrorContext Current => ThreadLocalContext.Value!;

        private readonly LinkedList<(string scope, int? lineNumber, string? context)> contextStack = new LinkedList<(string, int?, string?)>();
        private readonly List<string> errors = new List<string>();

        private ErrorContext()
        {
        }

        public IEnumerable<string> Errors => this.errors;
         
        public void ThrowIfHasErrors()
        {
            if (this.Errors.Any())
            {
                throw new InvalidFbsFileException(this.Errors);
            }
        }

        public void Clear()
        {
            this.errors.Clear();
        }

        public void RegisterError(string message)
        {
            this.errors.Add(message);
        }
    }
}
