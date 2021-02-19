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
    using System.Linq;
    using System.Reflection;

    internal class FieldDefinition
    {
        public FieldDefinition(
            TableOrStructDefinition parentDefinition,
            string name,
            string fieldType)
        {
            this.Parent = parentDefinition;
            this.Name = name;
            this.FbsFieldType = fieldType;
        }

        public int Index { get; set; }

        public bool IsIndexSetManually { get; set; }

        public string Name { get; set; }

        public string FbsFieldType { get; set; }

        public bool Deprecated { get; set; }

        public bool SortedVector { get; set; }

        public bool IsKey { get; set; }

        public bool? NonVirtual { get; set; }

        public string? DefaultValue { get; set; }

        public bool IsOptionalScalar { get; set; }

        public VectorType VectorType { get; set; }

        public SetterKind SetterKind { get; set; } = SetterKind.Public;

        public bool SharedString { get; set; }

        public TableOrStructDefinition Parent { get; }


        public void WriteDefaultConstructorLine(CodeWriter writer, CompileContext context)
        {
            FieldDefinitionEmitter emitter = new FieldDefinitionEmitter();
            emitter.EmitDefaultInitializationLine(writer, this, context);
        }

        public void WriteCopyConstructorLine(CodeWriter writer, string sourceName, BaseSchemaMember parent, CompileContext context)
        {
            FieldDefinitionEmitter emitter = new FieldDefinitionEmitter();
            emitter.EmitCloneLine(writer, sourceName, this, context);
        }

        public void WriteField(CodeWriter writer, TableOrStructDefinition parent, CompileContext context)
        {
            ErrorContext.Current.WithScope(this.Name, () =>
            {
                FieldDefinitionEmitter emitter = new FieldDefinitionEmitter();
                emitter.EmitDefinition(writer, this, context);
            });
        }
    }
}
