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

namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System.Collections.Generic;

/*
table Service {
    name:string (required, key);
    calls:[RPCCall];
    attributes:[KeyValue];
    documentation:[string];
    /// File that this Service is declared in.
    declaration_file: string;
}
*/
    [FlatBufferTable]
    public class RpcService
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1)]
        public virtual IList<RpcCall>? Calls { get; set; }

        [FlatBufferItem(2)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(3)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(4, Required = true)]
        public virtual string DeclaringFile { get; set; } = string.Empty;
    }
}
