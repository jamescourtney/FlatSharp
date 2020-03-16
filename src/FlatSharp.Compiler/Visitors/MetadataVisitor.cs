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
    internal class MetadataVisitor : FlatBuffersBaseVisitor<Dictionary<string, string>>
    {
        public override Dictionary<string, string> VisitMetadata([NotNull] FlatBuffersParser.MetadataContext context)
        {
            return this.VisitCommasep_ident_with_opt_single_value(context.commasep_ident_with_opt_single_value());
        }

        public override Dictionary<string, string> VisitCommasep_ident_with_opt_single_value([NotNull] FlatBuffersParser.Commasep_ident_with_opt_single_valueContext context)
        {
            var pairs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (context?.ident_with_opt_single_value() != null)
            {
                foreach (var item in context.ident_with_opt_single_value())
                {
                    string identifier = item.IDENT().GetText();
                    string value = item.single_value()?.GetText()?.Trim('"');

                    pairs[identifier] = value;
                }
            }

            return pairs;
        }
    }
}
