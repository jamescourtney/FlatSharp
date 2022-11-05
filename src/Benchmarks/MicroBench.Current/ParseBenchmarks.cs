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
    using System.Runtime.CompilerServices;

    public class ParseBenchmarks
    {
        [Benchmark]
        public int Parse_StringTable_SingleString()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithString));
        }

        // Subtracting the vector results below from this should give the approximate overhead of using a vector.
        [Benchmark]
        public int Parse_StringTable_SingleString_Repeated()
        {
            StringTable table = StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithString);

            int length = 0;
            for (int i = 0; i < Constants.VectorLength; ++i)
            {
                length += table.SingleString!.Length;
            }

            return length;
        }

        [Benchmark]
        public int Parse_StringTable_Vector()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithVector));
        }

        [Benchmark]
        public int Parse_StringTable_Empty()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.Stringtable_Empty));
        }

        [Benchmark]
        public int Parse_PrimitivesTable_Empty()
        {
            return ParseAndTraverse(PrimitivesTable.Serializer.Parse(Constants.Buffers.PrimitivesTable_Empty));
        }

        [Benchmark]
        public int Parse_PrimitivesTable_Full()
        {
            return ParseAndTraverse(PrimitivesTable.Serializer.Parse(Constants.Buffers.PrimitivesTable_Full));
        }

        [Benchmark]
        public int Parse_StructTable_SingleRef()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleRef);
            return st.SingleRef!.Value;
        }

        [Benchmark]
        public void Parse_StructTable_SingleRef_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleRef);
            st.SingleRef!.Value = 3;
        }

        [Benchmark]
        public int Parse_StructTable_SingleValue()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleValue);
            return st.SingleValue.Value;
        }

        [Benchmark]
        public void Parse_StructTable_SingleValue_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleValue);
            st.SingleValue = new ValueStruct { Value = 3 };
        }

        [Benchmark]
        public int Parse_StructTable_VecRef()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef);
            int sum = 0;

            var vecRef = st.VecRef!;
            int count = vecRef.Count;

            for (int i = 0; i < count; ++i)
            {
                sum += vecRef[i].Value;
            }

            return sum;
        }

        [Benchmark]
        public void Parse_StructTable_VecRef_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef);

            var vecRef = st.VecRef!;
            int count = vecRef.Count;

            for (int i = 0; i < count; ++i)
            {
                vecRef[i].Value++;
            }
        }

        [Benchmark]
        public int Parse_StructTable_VecValue()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue);
            int sum = 0;

            var vecValue = st.VecValue!;
            int count = vecValue.Count;

            for (int i = 0; i < count; ++i)
            {
                sum += vecValue[i].Value;
            }

            return sum;
        }

        [Benchmark]
        public void Parse_StructTable_VecValue_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue);

            var vecValue = st.VecValue!;
            int count = vecValue.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vecValue[i];
                item.Value++;
                vecValue[i] = item;
            }
        }

        [Benchmark]
        public int ParseAndTraverse_SafeUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Safe);
            var vector = st.Safe;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_UnsafeUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Unsafe);
            var vector = st.Unsafe;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_MixedUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Mixed);
            var vector = st.Mixed;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            return sum;
        }

        private struct UnionVisitor : UnsafeUnion.Visitor<int>, SafeUnion.Visitor<int>, MixedUnion.Visitor<int>
        {
            public int Visit(ValueStructA item)
            {
                return 0;
            }

            public int Visit(ValueStructB item)
            {
                return 1;
            }

            public int Visit(ValueStructC item)
            {
                return 2;
            }

            public int Visit(string item)
            {
                return 10;
            }
        }

        private static int ParseAndTraverse(PrimitivesTable table)
        {
            int sum = 0;
            sum += table.Bool ? 1 : 0;
            sum += table.SByte;
            sum += table.Byte;
            sum += (int)table.Double;
            sum += (int)table.Float;
            sum += table.Int;
            sum += (int)table.Long;
            sum += table.SByte;
            sum += table.Short;
            sum += (int)table.UInt;
            sum += (int)table.ULong;
            sum += table.UShort;

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseAndTraverse(StringTable table)
        {
            string? single = table.SingleString;
            IList<string>? vec = table.Vector;

            int length = 0;

            if (single is not null)
            {
                length += single.Length;
            }

            if (vec is not null)
            {
                int count = vec.Count;
                for (int i = 0; i < count; ++i)
                {
                    length += vec[i].Length;
                }
            }

            return length;
        }
    }
}
