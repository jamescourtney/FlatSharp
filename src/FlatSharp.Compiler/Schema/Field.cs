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
table Field {
    name:string (required, key);
    type:Type (required);
    id:ushort;
    offset:ushort;  // Offset into the vtable for tables, or into the struct.
    default_integer:long = 0;
    default_real:double = 0.0;
    deprecated:bool = false;
    required:bool = false;
    key:bool = false;
    attributes:[KeyValue];
    documentation:[string];
    optional:bool = false;
}
*/
    [FlatBufferTable]
    public class Field
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual FlatBufferType Type { get; set; } = new();

        [FlatBufferItem(2)]
        public virtual ushort Id { get; set; }

        // Offset into the vtable for tables, or into the struct.
        [FlatBufferItem(3)]
        public virtual ushort Offset { get; set; }

        [FlatBufferItem(4)]
        public virtual long DefaultInteger { get; set; }
        
        [FlatBufferItem(5)]
        public virtual double DefaultDouble { get; set; }

        [FlatBufferItem(6, DefaultValue = false)]
        public bool Deprecated { get; set; } = false;

        [FlatBufferItem(7, DefaultValue = false)]
        public virtual bool Required { get; set; } = false;

        [FlatBufferItem(8, DefaultValue = false)]
        public virtual bool Key { get; set; } = false;

        [FlatBufferItem(9)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(10)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(11, DefaultValue = false)]
        public virtual bool Optional { get; set; } = false;
    }
}
