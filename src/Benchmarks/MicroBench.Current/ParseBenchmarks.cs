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
    using FlatSharp;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ParseBenchmarks
    {
        [Params(FlatBufferDeserializationOption.Lazy,
                FlatBufferDeserializationOption.Progressive,
                FlatBufferDeserializationOption.Greedy,
                FlatBufferDeserializationOption.GreedyMutable)]
        public FlatBufferDeserializationOption Option { get; set; }

        [Benchmark]
        public int Parse_StringTable_SingleString()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithString, this.Option));
        }

        // Subtracting the vector results below from this should give the approximate overhead of using a vector.
        [Benchmark]
        public int Parse_StringTable_SingleString_Repeated()
        {
            StringTable table = StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithString, this.Option);

            int length = 0;
            for (int i = 0; i < Constants.VectorLength; ++i)
            {
                length += table.SingleString!.Length;
            }

            table.TryReturnToPool();

            return length;
        }

        [Benchmark]
        public int Parse_StringTable_Vector()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.StringTable_WithVector, this.Option));
        }

        [Benchmark]
        public int Parse_StringTable_Empty()
        {
            return ParseAndTraverse(StringTable.Serializer.Parse(Constants.Buffers.Stringtable_Empty, this.Option));
        }

        [Benchmark]
        public int Parse_PrimitivesTable_Empty()
        {
            return ParseAndTraverse(PrimitivesTable.Serializer.Parse(Constants.Buffers.PrimitivesTable_Empty, this.Option));
        }

        [Benchmark]
        public int Parse_PrimitivesTable_Full()
        {
            return ParseAndTraverse(PrimitivesTable.Serializer.Parse(Constants.Buffers.PrimitivesTable_Full, this.Option));
        }

        [Benchmark]
        public int Parse_StructTable_SingleRef()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleRef, this.Option);
            var singleRef = st.SingleRef!;

            int result = singleRef.Value;

            //singleRef.TryReturnToPool();
            st.TryReturnToPool();

            return result;
        }

        [Benchmark]
        public void Parse_StructTable_SingleRef_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleRef, this.Option);
            var singleRef = st.SingleRef!;
            singleRef.Value = 3;

            //singleRef.TryReturnToPool();
            st.TryReturnToPool();
        }

        [Benchmark]
        public int Parse_StructTable_SingleValue()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleValue, this.Option);
            var value = st.SingleValue.Value;

            st.TryReturnToPool();

            return value;
        }

        [Benchmark]
        public void Parse_StructTable_SingleValue_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_SingleValue, this.Option);
            st.SingleValue = new ValueStruct { Value = 3 };

            st.TryReturnToPool();
        }

        [Benchmark]
        public int Parse_StructTable_VecRef()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef, this.Option);
            int sum = 0;

            var vecRef = st.VecRef!;
            int count = vecRef.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vecRef[i];
                sum += item.Value;
                //item.TryReturnToPool();
            }

            //vecRef.TryReturnToPool();
            st.TryReturnToPool();

            return sum;
        }

#if VECTOR_VISITORS
        [Benchmark]
        public int Parse_StructTable_VecRef_FastVisitor()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef, this.Option);
            var vecRef = (IVisitableReferenceVector<RefStruct>)st.VecRef!;
            return vecRef.Accept<VecRefVisitor, int>(new());
        }

        private struct VecRefVisitor : IReferenceVectorVisitor<RefStruct, int>
        {
            public int Visit<TDerived, TVector>(TVector vector)
                where TDerived : RefStruct
                where TVector : struct, ISimpleVector<TDerived>
            {
                int sum = 0;
                int count = vector.Count;

                for (int i = 0; i < count; ++i)
                {
                    var item = vector[i];
                    sum += item.Value;
                }

                return sum;
            }
        }
#endif

        [Benchmark]
        public void Parse_StructTable_VecRef_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef, this.Option);

            var vecRef = st.VecRef!;
            int count = vecRef.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vecRef[i];
                item.Value++;
                //item.TryReturnToPool();
            }

            //vecRef.TryReturnToPool();
            st.TryReturnToPool();
        }


#if VECTOR_VISITORS
        [Benchmark]
        public void Parse_StructTable_VecRef_FastVisitor_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecRef, this.Option);
            var vecRef = (IVisitableReferenceVector<RefStruct>)st.VecRef!;
            vecRef.Accept<VecRefVisitor_WriteThrough, bool>(new());
            st.TryReturnToPool();
        }

        private struct VecRefVisitor_WriteThrough : IReferenceVectorVisitor<RefStruct, bool>
        {
            public bool Visit<TDerived, TVector>(TVector vector)
                where TDerived : RefStruct
                where TVector : struct, ISimpleVector<TDerived>
            {
                int count = vector.Count;

                for (int i = 0; i < count; ++i)
                {
                    TDerived item = vector[i];
                    item.Value++;
                }

                return true;
            }
        }
#endif

        [Benchmark]
        public int Parse_StructTable_VecValue()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue, this.Option);
            int sum = 0;

            var vecValue = st.VecValue!;
            int count = vecValue.Count;

            for (int i = 0; i < count; ++i)
            {
                sum += vecValue[i].Value;
            }

            //vecValue.TryReturnToPool();
            st.TryReturnToPool();

            return sum;
        }

#if VECTOR_VISITORS
        [Benchmark]
        public int Parse_StructTable_VecValue_FastVisitor()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue, this.Option);
            var vecValue = (IVisitableValueVector<ValueStruct>)st.VecValue!;
            return vecValue.Accept<VecValueVisitor, int>(new());
        }

        private struct VecValueVisitor : IValueVectorVisitor<ValueStruct, int>
        {
            public int Visit<TVector>(TVector vector)
                where TVector : struct, ISimpleVector<ValueStruct>
            {
                int sum = 0;
                int count = vector.Count;

                for (int i = 0; i < count; ++i)
                {
                    var item = vector[i];
                    sum += item.Value;
                }

                return sum;
            }
        }
#endif

        [Benchmark]
        public void Parse_StructTable_VecValue_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue, this.Option);

            var vecValue = st.VecValue!;
            int count = vecValue.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vecValue[i];
                item.Value++;
                vecValue[i] = item;
            }

            //vecValue.TryReturnToPool();
            st.TryReturnToPool();
        }

#if VECTOR_VISITORS
        [Benchmark]
        public void Parse_StructTable_VecValue_FastVisitor_WriteThrough()
        {
            var st = StructsTable.Serializer.Parse(Constants.Buffers.StructTable_VecValue, this.Option);
            var vecValue = (IVisitableValueVector<ValueStruct>)st.VecValue!;
            vecValue.Accept<VecValueVisitor_WriteThrough, bool>(new());
        }

        private struct VecValueVisitor_WriteThrough : IValueVectorVisitor<ValueStruct, bool>
        {
            public bool Visit<TVector>(TVector vector)
                where TVector : struct, ISimpleVector<ValueStruct>
            {
                int count = vector.Count;

                for (int i = 0; i < count; ++i)
                {
                    var item = vector[i];
                    item.Value++;
                    vector[i] = item;
                }

                return true;
            }
        }
#endif

        [Benchmark]
        public int ParseAndTraverse_SafeUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Safe, this.Option);
            var vector = st.Safe;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            //vector.TryReturnToPool();
            st.TryReturnToPool();

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_UnsafeUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Unsafe, this.Option);
            var vector = st.Unsafe;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            //vector.TryReturnToPool();
            st.TryReturnToPool();

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_MixedUnionVector()
        {
            var st = UnionTable.Serializer.Parse(Constants.Buffers.UnionTable_Mixed, this.Option);
            var vector = st.Mixed;
            int count = vector!.Count;

            var visitor = new UnionVisitor();
            int sum = 0;
            for (int i = 0; i < count; ++i)
            {
                sum += vector[i].Accept<UnionVisitor, int>(visitor);
            }

            //vector.TryReturnToPool();
            st.TryReturnToPool();

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

            table.TryReturnToPool();
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

                //vec.TryReturnToPool();
            }

            table.TryReturnToPool();

            return length;
        }
    }

    public static class Extensions
    {
#if POOLABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryReturnToPool<T>(this T obj) where T : IPoolableObject
        {
            obj.ReturnToPool();
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryReturnToPool<T>(this T obj)
        {
        }
#endif
    }
}
