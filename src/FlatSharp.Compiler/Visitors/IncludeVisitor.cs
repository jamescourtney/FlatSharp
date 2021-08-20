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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime.Misc;

    internal class IncludeVisitor : FlatBuffersBaseVisitor<object?>
    {
        private readonly HashSet<string> uniqueIncludes;
        private readonly Queue<string> toVisit;
        private readonly string currentDirectory;

        public IncludeVisitor(string currentFilePath, HashSet<string> uniqueIncludes, Queue<string> toVisit)
        {
            this.currentDirectory = Path.GetDirectoryName(currentFilePath) ?? throw new InvalidFbsFileException($"Unable to get path from file '{currentFilePath}'");
            this.uniqueIncludes = uniqueIncludes;
            this.toVisit = toVisit;
        }

        public override object? VisitInclude([NotNull] FlatBuffersParser.IncludeContext context)
        {
            string? includeFile = context.STRING_CONSTANT()?.GetText()?.Trim('"').Trim();
            if (string.IsNullOrEmpty(includeFile))
            {
                ErrorContext.Current.RegisterError("Include directive was empty.");
            }
            else
            {
                string includedPath = Path.Combine(this.currentDirectory, includeFile);
                if (this.uniqueIncludes.Add(includedPath))
                {
                    this.toVisit.Enqueue(includedPath);
                }
            }

            return null;
        }
    }
}
