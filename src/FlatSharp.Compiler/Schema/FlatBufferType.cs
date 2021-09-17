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
    using System;

    /*
    table Type
    {
        base_type:BaseType;
        element:BaseType = None;  // Only if base_type == Vector
                                  // or base_type == Array.
        index:int = -1;  // If base_type == Object, index into "objects" below.
                         // If base_type == Union, UnionType, or integral derived
                         // from an enum, index into "enums" below.
        fixed_length:uint16 = 0;  // Only if base_type == Array.
    }
    */

    [FlatBufferTable]
    public class FlatBufferType
    {
        [FlatBufferItem(0)]
        public virtual BaseType BaseType { get; set; }

        /// <summary>
        /// Only set if BaseType == Array or Vector.
        /// </summary>
        [FlatBufferItem(1, DefaultValue = BaseType.None)]
        public virtual BaseType ElementType { get; set; } = BaseType.None;

        [FlatBufferItem(2, DefaultValue = -1)]
        public virtual int Index { get; set; } = -1;

        [FlatBufferItem(3)]
        public virtual ushort FixedLength { get; set; }

        public string ResolveTypeOrElementTypeName(Schema schema, IFlatSharpAttributes? attributes)
        {
            BaseType baseType = this.BaseType;

            bool isVector = baseType == BaseType.Vector;
            bool isArray = baseType == BaseType.Array;

            if (isVector || isArray)
            {
                baseType = this.ElementType;
            }

            string typeName;
            if (this.Index == -1)
            {
                if (baseType == BaseType.String && attributes?.SharedString == true)
                {
                    typeName = "string";
                }
                else
                {
                    // Default value. This means that this is a simple built-in type.
                    FlatSharpInternal.Assert(baseType.TryGetBuiltInTypeName(out string? temp), "Failed to get type name");
                    typeName = temp;
                }
            }
            else if (baseType == BaseType.Obj)
            {
                // table or struct.
                typeName = schema.Objects[this.Index].Name;
            }
            else
            {
                // enum (or union).
                typeName = schema.Enums[this.Index].Name;
            }

            return typeName;
        }
    }
}
