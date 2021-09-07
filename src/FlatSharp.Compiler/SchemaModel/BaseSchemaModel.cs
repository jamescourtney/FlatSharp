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
        protected BaseSchemaModel(Schema.Schema schema, FlatSharpAttributes attributes)
        {
            this.Attributes = attributes;
            this.Schema = schema;
        }

        public Schema.Schema Schema { get; }

        public FlatSharpAttributes Attributes { get; }

        private void Validate()
        {
            this.Attributes.Validate(this);
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

        public string Namespace => this.FullName.Substring(0, this.FullName.LastIndexOf('.'));

        public string Name => this.FullName.Substring(this.FullName.LastIndexOf('.') + 1);

        public abstract string FullName { get; }

        public abstract string DeclaringFile { get; }

        public abstract FlatBufferSchemaElementType ElementType { get; }

        public virtual bool SupportsNonVirtual(bool nonVirtualValue) => false;

        public virtual bool SupportsDeserializationOption(FlatBufferDeserializationOption option) => false;

        public virtual bool SupportsSortedVector(bool sortedVectorOption) => false;

        public virtual bool SupportsSharedString(bool sharedStringOption) => false;

        public virtual bool SupportsDefaultCtorKindOption(DefaultConstructorKind kind) => false;

        public virtual bool SupportsSetterKind(SetterKind setterKind) => false;

        public virtual bool SupportsForceWrite(bool forceWriteOption) => false;

        public virtual bool SupportsUnsafeStructVector(bool unsafeStructVector) => false;

        public virtual bool SupportsMemoryMarshal(MemoryMarshalBehavior option) => false;

        public virtual bool SupportsWriteThrough(bool writeThroughOption) => false;

        public virtual bool SupportsRpcInterface(bool rpcInterface) => false;

        public virtual bool SupportsVectorType(VectorType vectorType) => false;
    }
}
