/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Compiler.SchemaModel
{
    using FlatSharp;
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class BaseSchemaModel : IFlatSharpAttributeSupportTester
    {
        protected BaseSchemaModel(Schema.Schema schema, string name, FlatSharpAttributes attributes)
        {
            this.Attributes = attributes;
            this.Schema = schema;
            this.FullName = name;

            (string ns, string typeName) = Helpers.ParseName(name);

            this.Namespace = ns;
            this.Name = typeName;
        }

        public Schema.Schema Schema { get; }

        public FlatSharpAttributes Attributes { get; }

        private void Validate()
        {
            this.ValidateAttributes(this.Attributes);
            this.OnValidate();
        }

        public void WriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass < CodeWritingPass.LastPass || context.RootFile == this.DeclaringFile)
            {
                ErrorContext.Current.WithScope(this.Name, () =>
                {
                    this.Validate();

                    writer.AppendLine($"namespace {this.Namespace}");
                    using (writer.WithBlock())
                    {
                        this.OnWriteCode(writer, context);
                    }
                });
            }
        }

        protected abstract void OnWriteCode(CodeWriter writer, CompileContext context);

        protected virtual void OnValidate()
        {
        }

        public string Namespace { get; }

        public string Name { get; }

        public string FullName { get; }

        public abstract string DeclaringFile { get; }

        public abstract FlatBufferSchemaElementType ElementType { get; }

        public virtual SupportTestResult SupportsNonVirtual(bool nonVirtualValue) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsDeserializationOption(FlatBufferDeserializationOption option) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsSortedVector(bool sortedVectorOption) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsSharedString(bool sharedStringOption) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsSetterKind(SetterKind setterKind) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsForceWrite(bool forceWriteOption) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsUnsafeStructVector(bool unsafeStructVector) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsMemoryMarshal(MemoryMarshalBehavior option) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsWriteThrough(bool writeThroughOption) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsRpcInterface(bool rpcInterface) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsVectorType(VectorType vectorType) => SupportTestResult.NeverValid;

        public virtual SupportTestResult SupportsStreamingType(RpcStreamingType streamingType) => SupportTestResult.NeverValid;
    }
}
