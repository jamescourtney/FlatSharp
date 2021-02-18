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

#nullable enable

namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    internal class TableOrStructDefinition : BaseSchemaMember
    {
        internal const string SerializerPropertyName = "Serializer";

        public TableOrStructDefinition(
            string name, 
            BaseSchemaMember parent) : base(name, parent)
        {
        }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
        
        public bool IsTable { get; set; }

        public bool? NonVirtual { get; set; }

        public bool ObsoleteDefaultConstructor { get; set; }

        public FlatBufferDeserializationOption? RequestedSerializer { get; set; }

        protected override bool SupportsChildren => false;

        private void AssignIndexes()
        {
            ErrorContext.Current.WithScope(
                this.Name,
                () =>
                {
                    if (this.Fields.Any(field => field.IsIndexSetManually))
                    {
                        if (!this.IsTable)
                        {
                            ErrorContext.Current.RegisterError("Structure fields can not have 'id' attribute set.");
                        }

                        if (!this.Fields.TrueForAll(field => field.IsIndexSetManually))
                        {
                            ErrorContext.Current.RegisterError("All or none fields should have 'id' attribute set.");
                        }

                        return;
                    }

                    int nextIndex = 0;
                    foreach (var field in this.Fields)
                    {
                        field.Index = nextIndex;

                        nextIndex++;

                        if (this.TryResolveName(field.FbsFieldType, out var typeDef) &&
                            typeDef is UnionDefinition)
                        {
                            // Unions are double-wide.
                            nextIndex++;
                        }
                    }
                });
        }

        protected override void OnWriteCode(CodeWriter writer, CompileContext context)
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
                if (this.ObsoleteDefaultConstructor)
                {
                    writer.AppendLine("[Obsolete]");
                }

                writer.AppendLine($"public {this.Name}()");
                using (writer.WithBlock())
                {
                    foreach (var field in this.Fields)
                    {
                        field.WriteDefaultConstructorLine(writer, context);
                    }

                    writer.AppendLine("this.OnInitialized();");
                }

                writer.AppendLine($"public {this.Name}({this.Name} source)");
                using (writer.WithBlock())
                {
                    foreach (var field in this.Fields)
                    {
                        field.WriteCopyConstructorLine(writer, "source", this, context);
                    }

                    writer.AppendLine("this.OnInitialized();");
                }

                foreach (var field in this.Fields)
                {
                    if (!this.IsTable && field.Deprecated)
                    {
                        ErrorContext.Current?.RegisterError($"FlatBuffer structs may not have deprecated fields.");
                    }

                    field.WriteField(writer, this, context);
                }

                if (context.CompilePass >= CodeWritingPass.SerializerGeneration && this.RequestedSerializer is not null)
                {
                    // generate the serializer.
                    string serializer = this.GenerateSerializerForType(
                        context,
                        this.RequestedSerializer.Value);

                    writer.AppendLine($"public static ISerializer<{this.FullName}> {SerializerPropertyName} {{ get; }} = new {RoslynSerializerGenerator.GeneratedSerializerClassName}().AsISerializer();");
                    writer.AppendLine(string.Empty);
                    writer.AppendLine($"#region Serializer for {this.FullName}");
                    writer.AppendLine(serializer);
                    writer.AppendLine($"#endregion");
                }
            }

            writer.AppendLine($"}}");
        }

        private string GenerateSerializerForType(
            CompileContext context,
            FlatBufferDeserializationOption deserializationOption)
        {
            try
            {
                CSharpHelpers.ConvertProtectedInternalToProtected = false;

                Type? type = context.PreviousAssembly?.GetType(this.FullName);
                if (type is null)
                {
                    ErrorContext.Current.RegisterError($"Flatsharp failed to find expected type '{this.FullName}' in assembly.");
                    return string.Empty;
                }

                var options = new FlatBufferSerializerOptions(deserializationOption);
                var generator = new RoslynSerializerGenerator(options, context.TypeModelContainer);

                MethodInfo method = generator.GetType()
                                             .GetMethod(nameof(RoslynSerializerGenerator.GenerateCSharp), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                                             .MakeGenericMethod(type);

                try
                {
                    string code = (string)method.Invoke(generator, new[] { "private" })!;
                    return code;
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                    throw;
                }
            }
            finally
            {
                CSharpHelpers.ConvertProtectedInternalToProtected = true;
            }
        }
    }
}
