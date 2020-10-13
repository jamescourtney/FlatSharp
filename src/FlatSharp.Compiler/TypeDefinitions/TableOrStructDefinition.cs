﻿/*
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
    using System.Linq;

    internal class TableOrStructDefinition : BaseSchemaMember
    {
        public TableOrStructDefinition(
            string name, 
            BaseSchemaMember parent) : base(name, parent)
        {
        }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
        
        public bool IsTable { get; set; }

        public bool? NonVirtual { get; set; }

        public bool ObsoleteDefaultConstructor { get; set; }

        public FlatBufferDeserializationOption? RequestedSerializer { get; set; }

        protected override bool SupportsChildren => false;

        public void AssignIndexes()
        {
            ErrorContext.Current.WithScope(this.Name, () =>
            {
                int nextIndex = 0;
                for (int i = 0; i < this.Fields.Count; ++i)
                {
                    this.Fields[i].Index = nextIndex;

                    nextIndex++;

                    if (this.TryResolveName(this.Fields[i].FbsFieldType, out var typeDef) && typeDef is UnionDefinition)
                    {
                        // Unions are double-wide.
                        nextIndex++;
                    }
                }
            });
        }

        protected override void OnWriteCode(CodeWriter writer, CodeWritingPass pass, string forFile, IReadOnlyDictionary<string, string> precompiledSerailizers)
        {
            this.AssignIndexes();

            string attribute = this.IsTable ? "[FlatBufferTable]" : "[FlatBufferStruct]";

            writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {this.Name} : object");
            writer.AppendLine($"{{");

            using (writer.IncreaseIndent())
            {
                writer.AppendLine($"partial void OnInitialized();");

                // default ctor.
                string obsolete = this.ObsoleteDefaultConstructor ? $"[Obsolete]" : string.Empty;
                writer.AppendLine($"{obsolete} public {this.Name}() {{ this.OnInitialized(); }}");

                writer.AppendLine($"public {this.Name}({this.Name} source)");
                using (writer.WithBlock())
                {
                    foreach (var field in this.Fields)
                    {
                        field.WriteCopyConstructorLine(writer, "source", this);
                    }

                    writer.AppendLine("this.OnInitialized();");
                }

                foreach (var field in this.Fields)
                {
                    if (!this.IsTable && field.Deprecated)
                    {
                        ErrorContext.Current?.RegisterError($"FlatBuffer structs may not have deprecated fields.");
                    }

                    field.WriteField(writer, this);
                }

                if (pass == CodeWritingPass.SecondPass && precompiledSerailizers != null && this.RequestedSerializer != null)
                {
                    if (precompiledSerailizers.TryGetValue(this.FullName, out string serializer))
                    {
                        writer.AppendLine($"public static ISerializer<{this.FullName}> Serializer {{ get; }} = new {RoslynSerializerGenerator.GeneratedSerializerClassName}().AsISerializer();");
                        writer.AppendLine(string.Empty);
                        writer.AppendLine($"#region Serializer for {this.FullName}");
                        writer.AppendLine(serializer);
                        writer.AppendLine($"#endregion");
                    }
                    else
                    {
                        ErrorContext.Current.RegisterError($"Table {this.FullName} requested serializer, but none was found.");
                    }
                }
            }

            writer.AppendLine($"}}");
        }

        protected override string OnGetCopyExpression(string source)
        {
            return $"{source} != null ? new {this.FullName}({source}) : null";
        }
    }
}
