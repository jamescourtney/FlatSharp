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
using BenchmarkDotNet.Running;
using FlatSharp;
using FlatSharp.Unsafe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BenchmarkCore
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    [MediumRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class Benchmark
    {
        private readonly byte[] data = new byte[10 * 1024 * 1024];

        private ArrayInputBuffer inputBuffer;
        //private UnsafeSpanWriter2 spanWriter;

        [GlobalSetup]
        public void Setup()
        {
            ValueTable t = new ValueTable();
            t.Points = new List<Vec3Value>();

            for (int i = 0; i < 20_000; ++i)
            {
                var value = new Vec3Value();

                for (int ii = 0; ii < value.X_Length; ++ii)
                {
                    value.X(ii) = (byte)ii;
                }

                t.Points.Add(value);
            }

            ValueTable.Serializer.Write(data, t);
            inputBuffer = new ArrayInputBuffer(data);
        }

        [Benchmark]
        public int ParseAndTraverse_Value()
        {
            var t = ValueTable.Serializer.Parse(this.inputBuffer);

            int sum = 0;

            var points = t.Points;
            if (points is null)
            {
                return 0;
            }

            int count = points.Count;
            for (int i = 0; i < count; ++i)
            {
                var point = points[i];
                for (int ii = 0; ii < point.X_Length; ++ii)
                {
                    sum += (int)point.X(ii);
                }
            }

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_Value_Unsafe()
        {
            var t = ValueTable_Unsafe.Serializer.Parse(this.inputBuffer);

            int sum = 0;

            var points = t.Points;
            if (points is null)
            {
                return 0;
            }

            int count = points.Count;
            for (int i = 0; i < count; ++i)
            {
                var point = points[i];
                for (int ii = 0; ii < point.X_Length; ++ii)
                {
                    sum += (int)point.X(ii);
                }
            }

            return sum;
        }

        [Benchmark]
        public int ParseAndTraverse_Ref()
        {
            var t = Table.Serializer.Parse(this.inputBuffer);

            int sum = 0;

            var points = t.Points;
            if (points is null)
            {
                return 0;
            }

            int count = points.Count;
            for (int i = 0; i < count; ++i)
            {
                var vec = points[i].X;
                for (int ii = 0; ii < vec.Count; ++ii)
                {
                    sum += (int)vec[ii];
                }
            }

            return sum;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //Vec3Value v = default;
            //v.X(3) = 5;

            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
