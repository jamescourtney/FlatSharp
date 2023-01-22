/*
 * Copyright 2018 James Courtney
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

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FlatSharp;
using FlatSharp.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BenchmarkCore
{
    public class IterationTests
    {
        public const int Length = 1000;
        public byte[] buffer;
        public Outer outer;

        [GlobalSetup]
        public void Setup()
        {
            outer = new Outer { RefItems = new List<RefStruct>(), ValueItems = new List<ValueStruct>(), };

            for (int i = 0; i < Length; ++i)
            {
                outer.RefItems.Add(new RefStruct { A = i });
                outer.ValueItems.Add(new ValueStruct { A = i });
            }


            buffer = new byte[Outer.Serializer.GetMaxSize(outer)];
            Outer.Serializer.Write(buffer, outer);
            outer = Outer.Serializer.Parse(buffer, FlatBufferDeserializationOption.Lazy);
        }

        //[Benchmark]
        public int RefNormalTraversal()
        {
            int sum = 0;
            IList<RefStruct> items = outer.RefItems!;

            if (items is null)
            {
                return sum;
            }

            for (int i = 0; i < Length; ++i)
            {
                sum += items[i].A;
            }

            return sum;
        }

        //[Benchmark]
        public int ValueNormalTraversal()
        {
            int sum = 0;
            IList<ValueStruct> items = outer.ValueItems!;

            if (items is null)
            {
                return sum;
            }

            for (int i = 0; i < Length; ++i)
            {
                sum += items[i].A;
            }

            return sum;
        }

        [Benchmark]
        public int RefFancyTraversal()
        {
            IVisitableReferenceVector<RefStruct> vec = (IVisitableReferenceVector<RefStruct>)outer.RefItems;
            return vec.Visit<Visitor, int>(new Visitor());
        }

        //[Benchmark]
        public int ValueFancyTraversal()
        {
            IVisitableValueVector<ValueStruct> vec = (IVisitableValueVector<ValueStruct>)outer.ValueItems;
            return vec.Visit<Visitor, int>(new Visitor());
        }

        private struct Visitor : IReferenceVectorVisitor<RefStruct, int>, IValueVectorVisitor<ValueStruct, int>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            int IValueVectorVisitor<ValueStruct, int>.Visit<TVector>(TVector vector)
            {
                int sum = 0;
                for (int i = 0; i < vector.Count; ++i)
                {
                    sum += vector[i].A;
                }

                return sum;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Visit<TDerived, TVector>(TVector vector) 
                where TDerived : RefStruct 
                where TVector : struct, ISimpleVector<TDerived>
            {
                int sum = 0;
                for (int i = 0; i < vector.Count; ++i)
                {
                    TDerived item = vector[i];
                    sum += item.A;
                    sum += 3;
                }

                return sum;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
