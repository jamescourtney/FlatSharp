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

        [Params(FlatBufferDeserializationOption.Lazy, FlatBufferDeserializationOption.Progressive)]
        public FlatBufferDeserializationOption Option { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var outer = new Outer { RefItems = new List<RefStruct>(), ValueItems = new List<ValueStruct>(), };

            for (int i = 0; i < Length; ++i)
            {
                outer.RefItems.Add(new RefStruct { A = i });
                outer.ValueItems.Add(new ValueStruct { A = i });
            }


            buffer = new byte[Outer.Serializer.GetMaxSize(outer)];
            Outer.Serializer.Write(buffer, outer);
        }

        [Benchmark]
        public int RefNormalTraversal()
        {
            var outer = Outer.Serializer.Parse(buffer, this.Option);

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

        [Benchmark]
        public int ValueNormalTraversal()
        {
            var outer = Outer.Serializer.Parse(buffer, this.Option);

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
            var outer = Outer.Serializer.Parse(buffer, this.Option);
            IVisitableReferenceVector<RefStruct> vec = (IVisitableReferenceVector<RefStruct>)outer.RefItems;
            return vec.Accept<RefVisitor, int>(new RefVisitor());
        }

        [Benchmark]
        public int ValueFancyTraversal()
        {
            var outer = Outer.Serializer.Parse(buffer, this.Option);
            IVisitableValueVector<ValueStruct> vec = (IVisitableValueVector<ValueStruct>)outer.ValueItems;
            return vec.Accept<ValueVisitor, int>(new ValueVisitor());
        }

        private struct RefVisitor : IReferenceVectorVisitor<RefStruct, int>
        {
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

        private struct ValueVisitor : IValueVectorVisitor<ValueStruct, int>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Visit<TVector>(TVector vector)
                where TVector : struct, ISimpleVector<ValueStruct>
            {
                int sum = 0;
                for (int i = 0; i < vector.Count; ++i)
                {
                    sum += vector[i].A;
                }

                return sum;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run(typeof(Program).Assembly);
            IterationTests t = new();

            t.Option = FlatBufferDeserializationOption.Progressive;
            t.Setup();

            while (true)
            {
                t.RefFancyTraversal();
            }
        }
    }
}
