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

        public string FormatTypeName(Schema root, out bool isVector, out bool isArray)
        {
            isVector = this.BaseType == BaseType.Vector;
            isArray = this.BaseType == BaseType.Array;

            BaseType toFormat = this.BaseType;
            if (isVector || isArray)
            {
                toFormat = this.ElementType;
            }

            if (this.Index == -1 && toFormat.TryGetBuiltInTypeName(out string? typeName))
            {
                return typeName;
            }

            if (toFormat == BaseType.Obj) // table or struct
            {
                return root.Objects[this.Index].Name;
            }
            else if (toFormat == BaseType.UType || toFormat == BaseType.Union || (this.Index != -1 && toFormat.IsScalar()))
            {
                return root.Enums[this.Index].Name;
            }

            throw new InvalidOperationException();
        }
    }
}
