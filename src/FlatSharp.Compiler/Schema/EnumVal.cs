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

using FlatSharp.Attributes;

namespace FlatSharp.Compiler.Schema;

/*
table EnumVal {
    name:string (required);
    value:long (key);
    object:Object (deprecated);
    union_type:Type;
    documentation:[string];
    attributes:[KeyValue];
}
*/

[FlatBufferTable]
public class EnumVal : ISortableTable<long>
{
    static EnumVal() => SortedVectorHelpers.RegisterKeyLookup<EnumVal, long>(ev => ev.Value, 1);

    [FlatBufferItem(0, Required = true)]
    public virtual string Key { get; set; } = string.Empty;

    [FlatBufferItem(1, Key = true)]
    public virtual long Value { get; set; }

    [FlatBufferItem(2, Deprecated = true)]
    public virtual bool Object { get; set; }

    [FlatBufferItem(3)]
    public virtual FlatBufferType? UnionType { get; set; }

    [FlatBufferItem(4)]
    public virtual IList<string>? Documentation { get; set; }

    [FlatBufferItem(5)]
    public virtual IList<KeyValue>? Attributes { get; set; }
}
