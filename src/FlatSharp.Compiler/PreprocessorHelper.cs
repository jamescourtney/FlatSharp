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

namespace FlatSharp.Compiler;

/// <summary>
/// Preprocessor helper utility.
/// </summary>
public class PreprocessorHelper
{
    private (string condition, string code) ifCondition;
    private List<(string condition, string code)> elseIfConditions;
    private string? elseCondition;
    private CodeWriter writer;

    public PreprocessorHelper(string condition, string code, CodeWriter writer)
    {
        this.ifCondition = (condition, code);
        this.elseIfConditions = new();
        this.writer = writer;
    }

    public PreprocessorHelper ElseIf(string condition, string code)
    {
        this.elseIfConditions.Add((condition, code));
        return this;
    }

    public PreprocessorHelper Else(string code)
    {
        FlatSharpInternal.Assert(this.elseCondition is null, "Duplicate else condition");
        this.elseCondition = code;
        return this;
    }

    public void Flush()
    {
        using (this.writer.WithNoIndent())
        {
            this.writer.AppendLine($"#if {this.ifCondition.condition}");
        }

        this.writer.AppendLine(this.ifCondition.code);

        foreach (var elseif in this.elseIfConditions)
        {
            using (this.writer.WithNoIndent())
            {
                this.writer.AppendLine($"#elif {elseif.condition}");
            }

            this.writer.AppendLine(elseif.code);
        }

        if (this.elseCondition is not null)
        {
            using (this.writer.WithNoIndent())
            {
                this.writer.AppendLine($"#else");
            }

            this.writer.AppendLine(this.elseCondition);
        }

        using (this.writer.WithNoIndent())
        {
            this.writer.AppendLine($"#endif");
        }
    }
}
