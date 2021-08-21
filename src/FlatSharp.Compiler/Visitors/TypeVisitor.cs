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

    internal class TypeVisitor : FlatBuffersBaseVisitor<BaseTableOrStructDefinition>
    {
        private readonly BaseSchemaMember parent;
        private readonly string declaringFileName;

        public TypeVisitor(BaseSchemaMember parent, string declaringFileName)
        {
            this.parent = parent;
            this.declaringFileName = declaringFileName;
        }

        public override BaseTableOrStructDefinition VisitType_decl(
            [NotNull] FlatBuffersParser.Type_declContext context)
        {
            Dictionary<string, string?> metadata = new MetadataVisitor().Visit(context.metadata());

            BaseTableOrStructDefinition definition;
            if (context.GetChild(0).GetText() == "struct" && 
                metadata.ParseNullableBooleanMetadata(MetadataKeys.ValueStruct) == true)
            {
                definition = new ValueStructDefinition(
                    context.IDENT().GetText(),
                    this.parent);
            }
            else
            {
                definition = new TableOrStructDefinition(
                   context.IDENT().GetText(),
                   isTable: context.GetChild(0).GetText() == "table",
                   this.parent);
            }

            this.parent.AddChild(definition);

            definition.DeclaringFile = this.declaringFileName;

            ErrorContext.Current.WithScope(definition.Name, () =>
            {
                definition.ApplyMetadata(metadata);

                var fields = context.field_decl();
                if (fields != null)
                {
                    foreach (var f in fields)
                    {
                        new FieldVisitor(definition).VisitField_decl(f);
                    }
                }
            });

            return definition;
        }
    }
}
