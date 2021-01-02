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
    using System.Collections.Generic;

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

        protected override void OnWriteCode(CodeWriter writer, CodeWritingPass pass, string forFile, IReadOnlyDictionary<string, string> precompiledSerializers)
        {
            AssignIndexes();
            AppendTypeDefinition(writer);

            using (writer.IncreaseIndent())
            {
                AppendRequiredMembers(writer);
                AppendDefaultConstructor(writer);
                AppendCopyConstructor(writer);
                AppendFields(writer);
                AppendSerializer(writer, pass, precompiledSerializers);
            }

            writer.AppendLine("}");
        }

        protected override string OnGetCopyExpression(string source)
        {
            return $"{source} != null ? new {FullName}({source}) : null";
        }

        private void AssignIndexes()
        {
            ErrorContext.Current.WithScope(
                Name,
                () =>
                {
                    int nextIndex = 0;
                    foreach (var field in Fields)
                    {
                        field.Index = nextIndex;

                        nextIndex++;

                        if (TryResolveName(field.FbsFieldType, out var typeDef) &&
                            typeDef is UnionDefinition)
                        {
                            // Unions are double-wide.
                            nextIndex++;
                        }
                    }
                });
        }

        private void AppendTypeDefinition(CodeWriter writer)
        {
            string attribute = IsTable ? "[FlatBufferTable]" : "[FlatBufferStruct]";

            writer.AppendLine(attribute);
            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine($"public partial class {Name} : object");
            writer.AppendLine("{");
        }

        private void AppendDefaultConstructor(CodeWriter writer)
        {
            string obsolete = ObsoleteDefaultConstructor ? "[Obsolete]" : string.Empty;
            writer.AppendLine($"{obsolete} public {Name}() {{ this.OnInitialized(); }}");
        }

        private void AppendCopyConstructor(CodeWriter writer)
        {
            writer.AppendLine($"public {Name}({Name} source)");
            using (writer.WithBlock())
            {
                foreach (var field in Fields)
                {
                    field.WriteCopyConstructorLine(writer, "source", this);
                }

                writer.AppendLine("this.OnInitialized();");
            }
        }

        private static void AppendRequiredMembers(CodeWriter writer)
        {
            writer.AppendLine("partial void OnInitialized();");
        }

        private void AppendFields(CodeWriter writer)
        {
            foreach (var field in Fields)
            {
                if (!IsTable &&
                    field.Deprecated)
                {
                    ErrorContext.Current?.RegisterError("FlatBuffer structs may not have deprecated fields.");
                }

                field.WriteField(writer, this);
            }
        }

        private void AppendSerializer(CodeWriter writer, CodeWritingPass pass, IReadOnlyDictionary<string, string> precompiledSerializers)
        {
            if (pass != CodeWritingPass.SecondPass ||
                precompiledSerializers == null ||
                RequestedSerializer == null)
            {
                return;
            }

            if (precompiledSerializers.TryGetValue(FullName, out string serializer))
            {
                writer.AppendLine(
                    $"public static ISerializer<{FullName}> Serializer {{ get; }} = new {RoslynSerializerGenerator.GeneratedSerializerClassName}().AsISerializer();");

                writer.AppendLine(string.Empty);
                writer.AppendLine($"#region Serializer for {FullName}");
                writer.AppendLine(serializer);
                writer.AppendLine("#endregion");
            }
            else
            {
                ErrorContext.Current.RegisterError($"Table {FullName} requested serializer, but none was found.");
            }
        }
    }
}
