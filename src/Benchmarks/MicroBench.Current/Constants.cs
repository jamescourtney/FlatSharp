/*
 * Copyright 2022 James Courtney
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

namespace Microbench
{
    using System;
    using System.Linq;
    using FlatSharp;

    public static class Constants
    {
        public const int VectorLength = 30;

        public static class StringTables
        {
            public static StringTable Empty = new StringTable();
            public static StringTable WithString = new StringTable { SingleString = Guid.NewGuid().ToString() };
            public static StringTable WithVector = new StringTable { Vector = Enumerable.Range(1, VectorLength).Select(x => Guid.NewGuid().ToString()).ToList() };
        }

        public static class PrimitiveTables
        {
            public static PrimitivesTable Empty = new PrimitivesTable();

            public static PrimitivesTable Full = new PrimitivesTable
            {
                Bool = true,
                Byte = 1,
                SByte = 1,
                Short = 1,
                UShort = 1,
                Int = 1,
                UInt = 1,
                Long = 1,
                ULong = 1,
                Float = 1,
                Double = 1,
            };
        }

        public static class StructTables
        {
            public static StructsTable SingleRef = new StructsTable { SingleRef = new RefStruct { Value = 1 } };
            public static StructsTable SingleValue = new StructsTable { SingleValue = new ValueStruct { Value = 1 } };
            public static StructsTable VectorRef = new StructsTable { VecRef = Enumerable.Range(1, 30).Select(x => new RefStruct { Value = x }).ToList() };
            public static StructsTable VectorValue = new StructsTable { VecValue = Enumerable.Range(1, 30).Select(x => new ValueStruct { Value = x }).ToList() };
        }

        public static class Buffers
        {
            public static byte[] Stringtable_Empty;
            public static byte[] StringTable_WithString;
            public static byte[] StringTable_WithVector;

            public static byte[] PrimitivesTable_Empty;
            public static byte[] PrimitivesTable_Full;

            public static byte[] StructTable_SingleRef;
            public static byte[] StructTable_SingleValue;
            public static byte[] StructTable_VecRef;
            public static byte[] StructTable_VecValue;

            static Buffers()
            {
                AllocateAndSerialize(StringTables.Empty, out Stringtable_Empty);
                AllocateAndSerialize(StringTables.WithString, out StringTable_WithString);
                AllocateAndSerialize(StringTables.WithVector, out StringTable_WithVector);

                AllocateAndSerialize(PrimitiveTables.Empty, out PrimitivesTable_Empty);
                AllocateAndSerialize(PrimitiveTables.Full, out PrimitivesTable_Full);

                AllocateAndSerialize(StructTables.SingleRef, out StructTable_SingleRef);
                AllocateAndSerialize(StructTables.SingleValue, out StructTable_SingleValue);
                AllocateAndSerialize(StructTables.VectorRef, out StructTable_VecRef);
                AllocateAndSerialize(StructTables.VectorValue, out StructTable_VecValue);
            }

            private static void AllocateAndSerialize<T>(T value, out byte[] buffer) where T : class, IFlatBufferSerializable<T>, new()
            {
                buffer = new byte[value.Serializer.GetMaxSize(value)];
                int length = value.Serializer.Write(buffer, value);
            }
        }
    }
}
