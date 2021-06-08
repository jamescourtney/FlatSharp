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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp.TypeModel;

    internal class PoolingDeserializeClassDefinition : DeserializeClassDefinition
    {
        private const string PoolVariableName = "__Pool";
        private const string AllocationStackVariableName = "__poolDiag_allocationStack";
        private const string ReleaseStackVariableName = "__poolDiag_releaseStack";
        private const string ReleasedVariableName = "__poolDiag_released";

        public PoolingDeserializeClassDefinition(
            string className, 
            MethodInfo? onDeserializeMethod, 
            ITypeModel typeModel, 
            FlatBufferSerializerOptions options,
            int poolSize) : base(className, onDeserializeMethod, typeModel, options)
        {
            FlatSharpInternal.Assert(poolSize != 0, "Pool size must not be 0.");

            base.instanceFieldDefinitions[AllocationStackVariableName] = $"private string? {AllocationStackVariableName};";
            base.instanceFieldDefinitions[ReleaseStackVariableName] = $"private string? {ReleaseStackVariableName};";
            base.instanceFieldDefinitions[ReleasedVariableName] = $"private bool {ReleasedVariableName};";

            string actualPoolSize = poolSize < 0 ? "null" : poolSize.ToString();

            base.staticFieldDefinitions[PoolVariableName] =
                $@"
                    private static readonly IFlatSharpObjectPool<{this.ClassName}<TInputBuffer>> {PoolVariableName} =
                        {nameof(FlatSharpRuntimeSettings)}.{nameof(FlatSharpRuntimeSettings.ObjectPoolFactory)}.{nameof(IFlatSharpObjectPoolFactory.Create)}<{this.ClassName}<TInputBuffer>>({actualPoolSize});
                ";
        }

        protected override string GetGetterBody(ItemMemberModel itemModel)
        {
            return $@"
                if (this.{ReleasedVariableName})
                {{
                    throw new InvalidOperationException(
                        {this.CreatePoolErrorMessage("FlatSharp object used after recycle.")});
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
                        {this.CreatePoolErrorMessage("FlatSharp object used after recycle.")});
                }}
                {base.GetSetterBody(itemModel)}
            ";
        }

        protected override string GetDangerousReleaseMethodBody()
        {
            return $@"
                if (this.{ReleasedVariableName})
                {{
                    throw new InvalidOperationException(
                        {this.CreatePoolErrorMessage("FlatSharp object recycled twice.")});
                }}

                this.{ReleasedVariableName} = true;
                if ({nameof(FlatSharpRuntimeSettings)}.{nameof(FlatSharpRuntimeSettings.EnableRecyclingDiagnostics)})
                {{
                    this.{ReleaseStackVariableName} = Environment.StackTrace;
                }}
                else
                {{
                    {string.Join("\r\n", this.GetResetStatements())}
                    {PoolVariableName}.Return(this);
                }}
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

                if ({nameof(FlatSharpRuntimeSettings)}.{nameof(FlatSharpRuntimeSettings.EnableRecyclingDiagnostics)})
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
                    {string.Join("\r\n", this.GetResetStatements())}
                }}

                private void Initialize(TInputBuffer buffer, int offset)
                {{
                    {string.Join("\r\n", this.initializeStatements)}
                    {onDeserializedStatement}
                }}
            ";
        }

        private IEnumerable<string> GetResetStatements() => 
            base.instanceFieldDefinitions.Keys
                .Where(f => f != ReleasedVariableName) // don't reset our metadata.
                .Where(f => f != AllocationStackVariableName)
                .Where(f => f != ReleaseStackVariableName)
                .Select(f => $"this.{f} = default!;");

        private string CreatePoolErrorMessage(string message)
        {
            string type = this.typeModel.GetCompilableTypeName();
            return $@"$""{message} Type = '{type}', \r\n\r\n AllocationStack = '{{this.{AllocationStackVariableName}}}', \r\n\r\n ReleaseStack = '{{this.{ReleaseStackVariableName}}}'""";
        }
    }
}
