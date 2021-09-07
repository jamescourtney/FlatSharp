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
table RPCCall {
    name:string (required, key);
    request:Object (required);      // must be a table (not a struct)
    response:Object (required);     // must be a table (not a struct)
    attributes:[KeyValue];
    documentation:[string];
}
*/
    [FlatBufferTable]
    public class RpcCall
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        // Must be a table.
        [FlatBufferItem(1, Required = true)]
        public virtual FlatBufferObject Request { get; set; } = new FlatBufferObject();

        // Must be a table.
        [FlatBufferItem(2, Required = true)]
        public virtual FlatBufferObject Response { get; set; } = new FlatBufferObject();

        [FlatBufferItem(3)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(4)]
        public virtual IList<string>? Documentation { get; set; }
    }
}
