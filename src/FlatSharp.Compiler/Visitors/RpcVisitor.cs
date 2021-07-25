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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parses an enum definition.
    /// </summary>
    internal class RpcVisitor : FlatBuffersBaseVisitor<RpcDefinition?>
    {
        private readonly BaseSchemaMember parent;

        private RpcDefinition? rpcDefinition;

        public RpcVisitor(BaseSchemaMember parent)
        {
            this.parent = parent;
        }

        public override RpcDefinition? VisitRpc_decl([NotNull] FlatBuffersParser.Rpc_declContext context)
        {
            this.rpcDefinition = new RpcDefinition(context.IDENT().GetText(), this.parent);
            var metadata = new MetadataVisitor().Visit(context.metadata());

            if (metadata.TryGetValue(MetadataKeys.RpcInterface, out string? value))
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = $"I{this.rpcDefinition.Name}";
                }

                this.rpcDefinition.GeneratedInterfaceName = value;
            }

            base.VisitRpc_decl(context);
            return this.rpcDefinition;
        }

        public override RpcDefinition? VisitRpc_method([NotNull] FlatBuffersParser.Rpc_methodContext context)
        {
            FlatSharpInternal.Assert(this.rpcDefinition != null, "Failed to initialize RPC definition");

            string name = context.IDENT().GetText();
            string requestType = context.type()[0].GetText();
            string responseType = context.type()[1].GetText();
            Dictionary<string, string?> metadata = new MetadataVisitor().Visit(context.metadata());

            var streamingType = RpcStreamingType.Unary;
            if (metadata.TryGetValue("streaming", out string? value))
            {
                streamingType = ParseStreamingType(value);
            }

            this.rpcDefinition.AddRpcMethod(name, requestType, responseType, streamingType);
            return null;
        }

        private static RpcStreamingType ParseStreamingType(string? type)
        {
            if (!Enum.TryParse<RpcStreamingType>(type, true, out var result))
            {
                ErrorContext.Current.RegisterError($"Unable to parse '{type}' as a streaming type. Valid values are '{string.Join("', '", Enum.GetNames(typeof(RpcStreamingType)))}'.");
                result = RpcStreamingType.Unary;
            }

            return result;
        }
    }
}
