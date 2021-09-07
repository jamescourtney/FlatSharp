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
    using FlatSharp.Compiler.SchemaModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /*
    table Enum {
        name:string (required, key);
        values:[EnumVal] (required);  // In order of their values.
        is_union:bool = false;
        underlying_type:Type (required);
        attributes:[KeyValue];
        documentation:[string];
        /// File that this Enum is declared in.
        declaration_file: string;
    }
    */

    [FlatBufferTable]
    public class FlatBufferEnum
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual IIndexedVector<long, EnumVal> Values { get; set; } = new IndexedVector<long, EnumVal>();

        [FlatBufferItem(2)]
        public virtual bool IsUnion { get; set; }

        [FlatBufferItem(3, Required = true)]
        public virtual FlatBufferType UnderlyingType { get; set; } = new FlatBufferType();

        [FlatBufferItem(4)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(5)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(6, Required = true)]
        public virtual string DeclarationFile { get; set; } = string.Empty;

        public BaseSchemaModel ToSchemaModel(Schema schema)
        {
            if (EnumSchemaModel.TryCreate(schema, this, out var enumModel))
            {
                return enumModel;
            }

            throw new NotImplementedException();
        }
    }
}
