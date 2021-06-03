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

namespace FlatSharp
{
    using System;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.TypeModel;

    internal class PoolingDeserializeClassDefinition : DeserializeClassDefinition
    {
        private const string PoolVariableName = "__Pool";
        private const string AllocationStackVariableName = "__poolDiag_allocationStack";
        private const string ReleaseStackVariableName = "__poolDiag_releaseStack";
        private const string ReleasedVariableName = "__poolDiag_released";
        private readonly int poolSize;

        public PoolingDeserializeClassDefinition(
            string className, 
            MethodInfo? onDeserializeMethod, 
            ITypeModel typeModel, 
            FlatBufferSerializerOptions options,
            int poolSize) : base(className, onDeserializeMethod, typeModel, options)
        {
            this.poolSize = poolSize;

            base.instanceFieldDefinitions[AllocationStackVariableName] = $"private string {AllocationStackVariableName};";
            base.instanceFieldDefinitions[ReleaseStackVariableName] = $"private string {ReleaseStackVariableName};";
            base.instanceFieldDefinitions[ReleasedVariableName] = $"private bool {ReleasedVariableName};";

            base.staticFieldDefinitions[PoolVariableName] =
                $@"
                    private static readonly System.Collections.Concurrent.ConcurrentBag<{this.ClassName}<TInputBuffer>> {PoolVariableName}
                        = new System.Collections.Concurrent.ConcurrentBag<{this.ClassName}<TInputBuffer>>();
                ";
        }

        protected override string GetGetterBody(ItemMemberModel itemModel)
        {
            return $@"
                if (this.{ReleasedVariableName})
                {{
                    throw new InvalidOperationException(
                        {this.CreatePoolErrorMessage("FlatSharp pooled object used after release.")});
                }}
                {base.GetGetterBody(itemModel)}
            ";
        }

        protected override string GetSetterBody(ItemMemberModel itemModel)
        {
            return $@"
                if (this.{ReleasedVariableName})
                {{
                    throw new InvalidOperationException(
                        {this.CreatePoolErrorMessage("FlatSharp pooled object used after release.")});
                }}
                {base.GetSetterBody(itemModel)}
            ";
        }

        protected override string GetReleaseMethodBody()
        {
            var releaseStatements = base.instanceFieldDefinitions.Keys
                .Where(f => f != ReleasedVariableName) // don't reset our metadata.
                .Where(f => f != AllocationStackVariableName)
                .Where(f => f != ReleaseStackVariableName)
                .Select(f => $"this.{f} = default;");

            return $@"
                if (this.{ReleasedVariableName})
                {{
                    throw new InvalidOperationException(
                        {this.CreatePoolErrorMessage("FlatSharp pooled object released twice.")});
                }}

                this.{ReleasedVariableName} = true;
                if ({nameof(FlatSharpGlobalSettings)}.{nameof(FlatSharpGlobalSettings.CollectPooledObjectStackTraces)})
                {{
                    this.{ReleaseStackVariableName} = Environment.StackTrace;
                }}

                {string.Join("\r\n", releaseStatements)}
                {PoolVariableName}.Add(this);
            ";
        }

        protected override string GetGetOrCreateMethodBody()
        {
            return $@"                
                if (!{PoolVariableName}.TryTake(out var item))
                {{
                    item = new {this.ClassName}<TInputBuffer>();
                }}

                item.Initialize(buffer, offset);

                item.{ReleasedVariableName} = false;
                item.{AllocationStackVariableName} = null;
                item.{ReleaseStackVariableName} = null;

                if ({nameof(FlatSharpGlobalSettings)}.{nameof(FlatSharpGlobalSettings.CollectPooledObjectStackTraces)})
                {{
                    item.{AllocationStackVariableName} = Environment.StackTrace;
                }}

                return item;
            ";
        }

        protected override string GetCtorMethodDefinition(string onDeserializedStatement, string baseCtorParams)
        {
            return $@"
                private {this.ClassName}() : base({baseCtorParams}) 
                {{
                }}

                private void Initialize(TInputBuffer buffer, int offset)
                {{
                    {string.Join("\r\n", this.initializeStatements)}
                    {onDeserializedStatement}
                }}
            ";
        }

        private string CreatePoolErrorMessage(string message)
        {
            string type = this.typeModel.GetCompilableTypeName();
            return $@"$""{message} Type = '{type}', \r\n\r\n AllocationStack = '{{this.{AllocationStackVariableName}}}', \r\n\r\n ReleaseStack = '{{this.{ReleaseStackVariableName}}}'""";
        }
    }
}
