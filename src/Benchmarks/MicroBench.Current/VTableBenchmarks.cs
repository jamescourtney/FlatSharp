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
    using BenchmarkDotNet.Attributes;
    using System;
    using FlatSharp;
    using System.Linq;
    using System.Collections.Generic;
    using System.Buffers.Binary;
    using FlatSharp.Internal;
    using System.Runtime.CompilerServices;

    public class VTableBenchmarks
    {
#if PUBLIC_IVTABLE
        const int MaxField = 10;

        private static Random random = new();

        private static byte[][] RandomVTables = Enumerable.Range(1, 1024).Select(x => CreateRandomVTable(MaxField)).ToArray();

        private static byte[] CreateRandomVTable(int numFields)
        {
            byte[] vtable = new byte[4 + 4 + (2 * numFields)];
            Span<byte> span = vtable;

            BinaryPrimitives.WriteInt32LittleEndian(span, -4); // offset to vtable.

            int maxField = random.Next(-1, numFields);

            for (int i = 0; i <= maxField; ++i)
            {
                bool present = random.Next() % 2 == 0;
                if (present)
                {
                    BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(8 + (2 * i)), 1);
                }
            }

            ushort length = (ushort)(6 + (2 * maxField));
            BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(4), length); // vtable length
            return span.Slice(0, 4 + length).ToArray();
        }

        private int i;

        [Benchmark]
        public int VTable4_ParseAndUse()
        {
            byte[][] tables = RandomVTables;
            byte[] vtable = tables[(i++) % tables.Length];

            var buffer = new ArrayInputBuffer(vtable);
            VTable4.Create(buffer, 0, out var item);

            return Use(item, buffer);
        }

        [Benchmark]
        public int VTable4_BE_ParseAndUse()
        {
            byte[][] tables = RandomVTables;
            byte[] vtable = tables[(i++) % tables.Length];

            var buffer = new ArrayInputBuffer(vtable);
            VTable4.CreateBigEndian(buffer, 0, out var item);

            return Use(item, buffer);
        }

        [Benchmark]
        public int VTableGeneric_ParseAndUse()
        {
            byte[][] tables = RandomVTables;
            byte[] vtable = tables[(i++) % tables.Length];

            var buffer = new ArrayInputBuffer(vtable);
            VTableGeneric.Create(buffer, 0, out var item);

            return Use(item, buffer);
        }

        [Benchmark]
        public int VTable8_ParseAndUse()
        {
            byte[][] tables = RandomVTables;
            byte[] vtable = tables[(i++) % tables.Length];

            var buffer = new ArrayInputBuffer(vtable);
            VTable8.Create(buffer, 0, out var item);

            return Use(item, buffer);
        }

        [Benchmark]
        public int VTable8_BE_ParseAndUse()
        {
            byte[][] tables = RandomVTables;
            byte[] vtable = tables[(i++) % tables.Length];

            var buffer = new ArrayInputBuffer(vtable);
            VTable8.CreateBigEndian(buffer, 0, out var item);

            return Use(item, buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Use<TVTable, TInputBuffer>(TVTable table, TInputBuffer buffer) 
            where TVTable : IVTable
            where TInputBuffer : IInputBuffer
        {
            int sum = 0;

            sum += table.OffsetOf(buffer, 0);
            sum += table.OffsetOf(buffer, 1);
            sum += table.OffsetOf(buffer, 2);
            sum += table.OffsetOf(buffer, 3);
            sum += table.OffsetOf(buffer, 4);
            sum += table.OffsetOf(buffer, 5);
            sum += table.OffsetOf(buffer, 6);
            sum += table.OffsetOf(buffer, 7);
            sum += table.OffsetOf(buffer, 8);

            return sum;
        }
#endif
    }
}
