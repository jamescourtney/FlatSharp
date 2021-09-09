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
    using System.Globalization;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Utility class for generating reasonably formatted code.
    /// </summary>
    public class CodeWriter
    {
        private const string OneIndent = "    ";
        private int indent;
        private readonly StringBuilder builder = new StringBuilder();

        public void AppendLine(string line)
        {
            for (int i = 0; i < this.indent; ++i)
            {
                this.builder.Append(OneIndent);
            }

            this.builder.AppendLine(line);
        }

        public void AppendMethodSummaryComment(string comment)
        {
            this.AppendLine("/// <summary>");
            this.AppendLine($"/// {comment}");
            this.AppendLine("/// </summary>");
        }

        public void AppendLine()
        {
            for (int i = 0; i < this.indent; ++i)
            {
                this.builder.Append(OneIndent);
            }

            this.builder.AppendLine();
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }

        public IDisposable IncreaseIndent()
        {
            this.indent++;
            return new FakeDisposable(() => this.indent--);
        }

        public IDisposable WithBlock()
        {
            this.AppendLine("{");
            this.indent++;
            return new FakeDisposable(() =>
            {
                this.indent--;
                this.AppendLine("}");
                this.AppendLine(string.Empty);
            });
        }

        private class FakeDisposable : IDisposable
        {
            private readonly Action onDispose;

            public FakeDisposable(Action onDispose)
            {
                this.onDispose = onDispose;
            }

            public void Dispose()
            {
                this.onDispose();
            }
        }
    }
}
