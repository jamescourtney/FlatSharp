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
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Parses metadata as a set of key value pairs. Case insensitive on keys.
    /// </summary>
    internal class MetadataVisitor : FlatBuffersBaseVisitor<Dictionary<string, string?>>
    {
        public override Dictionary<string, string?> VisitMetadata([NotNull] FlatBuffersParser.MetadataContext context)
        {
            return this.VisitMetadata_list(context.metadata_list());
        }

        public override Dictionary<string, string?> VisitMetadata_list([NotNull] FlatBuffersParser.Metadata_listContext context)
        {
            var pairs = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            if (context?.metadata_item() != null)
            {
                foreach (var item in context.metadata_item())
                {
                    string identifier = item.Key.Text;
                    string? value = item.Value?.GetText()?.Trim('"');

                    pairs[identifier] = value;
                }
            }

            foreach (var unsupportedAttribute in MetadataKeys.UnsupportedStandardAttributes)
            {
                if (pairs.ContainsKey(unsupportedAttribute))
                {
                    ErrorContext.Current?.RegisterError($"FlatSharpCompiler does not support the '{unsupportedAttribute}' attribute in FBS files.");
                }
            }

            return pairs;
        }
    }
}
