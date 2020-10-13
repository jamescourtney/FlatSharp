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
    using Antlr4.Runtime.Misc;
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeVisitor : FlatBuffersBaseVisitor<TableOrStructDefinition>
    {
        private readonly BaseSchemaMember parent;

        public TypeVisitor(BaseSchemaMember parent)
        {
            this.parent = parent;
        }

        public override TableOrStructDefinition VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            Dictionary<string, string> metadata = new MetadataVisitor().Visit(context.metadata());

            TableOrStructDefinition definition = new TableOrStructDefinition(
                context.IDENT().GetText(),
                this.parent);

            ErrorContext.Current.WithScope(definition.Name, () =>
            {
                definition.IsTable = context.GetChild(0).GetText() == "table";

                definition.NonVirtual = metadata.ParseNullableBooleanMetadata("nonVirtual");
                definition.ObsoleteDefaultConstructor = metadata.ParseBooleanMetadata("obsoleteDefaultConstructor");

                definition.RequestedSerializer = metadata.ParseMetadata<FlatBufferDeserializationOption?>(
                    "PrecompiledSerializer",
                    ParseSerailizerFlags,
                    FlatBufferDeserializationOption.Default,
                    null);

                if (!definition.IsTable && definition.RequestedSerializer != null)
                {
                    ErrorContext.Current.RegisterError("Structs may not have precompiled serializers.");
                }

                var fields = context.field_decl();
                if (fields != null)
                {
                    definition.Fields = fields.Select(x => new FieldVisitor().VisitField_decl(x)).ToList();
                }
            });

            return definition;
        }

        private static bool ParseSerailizerFlags(string value, out FlatBufferDeserializationOption? result)
        {
            var success = Enum.TryParse<FlatBufferDeserializationOption>(value, true, out var tempResult);
            result = tempResult;
            return success;
        }
    }
}
