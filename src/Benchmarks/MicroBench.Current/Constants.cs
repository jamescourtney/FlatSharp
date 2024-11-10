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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Internal;

    public static class Constants
    {
        public const int VectorLength = 30;
        private static readonly Random Random = new Random();

        static Constants()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
#if POOLABLE
            ObjectPool.MaxToRetain = VectorLength;
#endif
        }

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
            public static StructsTable SingleRef = new StructsTable { SingleRef = new RefStruct { Value = 1 }, SingleValue = default, };
            public static StructsTable SingleValue = new StructsTable { SingleValue = new ValueStruct { Value = 1 } };
            public static StructsTable VectorRef = new StructsTable { VecRef = Enumerable.Range(1, VectorLength).Select(x => new RefStruct { Value = x }).ToList(), SingleValue = default, };
            public static StructsTable VectorValue = new StructsTable { VecValue = Enumerable.Range(1, VectorLength).Select(x => new ValueStruct { Value = x }).ToList(), SingleValue = default, };
        }

        public static class SortedVectorTables
        {
            public static readonly List<StringKey> AllStringKeys = Enumerable.Range(1, VectorLength).Select(x => new StringKey { Key = Guid.NewGuid().ToString() }).ToList();

            public static readonly List<IntKey> AllIntKeys = Enumerable.Range(1, VectorLength).Select(x => new IntKey { Key = Random.Next() }).ToList();

            public static readonly SortedTable SortedStrings = new SortedTable
            {
                Strings = new IndexedVector<string, StringKey>(
                    AllStringKeys,
                    false)
            };

            public static readonly SortedTable SortedInts = new SortedTable
            {
                Ints = new IndexedVector<int, IntKey>(
                    AllIntKeys,
                    false)
            };

            public static readonly byte[] Buffer_StringKey = AllocateAndSerialize(SortedStrings);
            public static readonly byte[] Buffer_IntKey = AllocateAndSerialize(SortedInts);
        }

        public static class UnionTables
        {
            public static readonly UnionTable Safe = new()
            {
                Safe = Enumerable.Range(1, VectorLength).Select(x =>
                {
                    int random = Random.Next() % 3;
                    switch (random)
                    {
                        case 0: return new SafeUnion(new ValueStructA());
                        case 1: return new SafeUnion(new ValueStructB());
                        case 2: return new SafeUnion(new ValueStructC());
                    }

                    throw new Exception();
                }).ToList()
            };

            public static readonly UnionTable Unsafe = new()
            {
                Unsafe = Enumerable.Range(1, VectorLength).Select(x =>
                {
                    int random = Random.Next() % 3;
                    switch (random)
                    {
                        case 0: return new UnsafeUnion(new ValueStructA());
                        case 1: return new UnsafeUnion(new ValueStructB());
                        case 2: return new UnsafeUnion(new ValueStructC());
                    }

                    throw new Exception();
                }).ToList()
            };


            public static readonly UnionTable Mixed = new()
            {
                Mixed = Enumerable.Range(1, VectorLength).Select(x =>
                {
                    int random = Random.Next() % 3;
                    switch (random)
                    {
                        case 0: return new MixedUnion(new ValueStructA());
                        case 1: return new MixedUnion(new ValueStructB());
                        case 2: return new MixedUnion(new ValueStructC());
                    }

                    throw new Exception();
                }).ToList()
            };
        }

        public static class Buffers
        {
            public static readonly byte[] Stringtable_Empty = AllocateAndSerialize(StringTables.Empty);
            public static readonly byte[] StringTable_WithString = AllocateAndSerialize(StringTables.WithString);
            public static readonly byte[] StringTable_WithVector = AllocateAndSerialize(StringTables.WithVector);

            public static readonly byte[] PrimitivesTable_Empty = AllocateAndSerialize(PrimitiveTables.Empty);
            public static readonly byte[] PrimitivesTable_Full = AllocateAndSerialize(PrimitiveTables.Full);

            public static readonly byte[] StructTable_SingleRef = AllocateAndSerialize(StructTables.SingleRef);
            public static readonly byte[] StructTable_SingleValue = AllocateAndSerialize(StructTables.SingleValue);
            public static readonly byte[] StructTable_VecRef = AllocateAndSerialize(StructTables.VectorRef);
            public static readonly byte[] StructTable_VecValue = AllocateAndSerialize(StructTables.VectorValue);

            public static readonly byte[] UnionTable_Safe = AllocateAndSerialize(UnionTables.Safe);
            public static readonly byte[] UnionTable_Unsafe = AllocateAndSerialize(UnionTables.Unsafe);
            public static readonly byte[] UnionTable_Mixed = AllocateAndSerialize(UnionTables.Mixed);
        }

        private static byte[] AllocateAndSerialize<T>(T value) where T : class, IFlatBufferSerializable<T>
        {
            byte[] buffer = new byte[value.Serializer.GetMaxSize(value)];
            long length = value.Serializer.Write(buffer, value);

            return buffer;
        }
    }
}
