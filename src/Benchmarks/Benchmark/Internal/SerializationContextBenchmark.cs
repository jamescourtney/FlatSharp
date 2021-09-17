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

namespace Benchmark
{
    using BenchmarkDotNet.Attributes;
    using FlatSharp;
    using System;
    using System.Collections.Generic;

    public class SerializationContextBenchmark
    {
        private static readonly SerializationContext context = new SerializationContext();

        private const int ScratchLength = 10 * 1024 * 1024;
        private readonly byte[] Scratch = new byte[ScratchLength];

        [Params(4, 8, 32, 64)]
        public int VTableLength { get; set; }

        [Params(10, 100, 200, 400)]
        public int VTableCount { get; set; }

        // random.
        private byte[][] RandomVTables;

        // non random with repeated elements.
        private byte[][] GuassianVTables;

        private byte[] ZeroByteVTable;

        [GlobalSetup]
        public void Setup()
        {
            this.ZeroByteVTable = new byte[this.VTableLength];

            Random r = new Random();

            List<byte[]> allVtables = new List<byte[]>();
            for (int i = 0; i < 2 * this.VTableCount; ++i)
            {
                byte[] vtable = new byte[this.VTableLength];
                r.NextBytes(vtable);
                allVtables.Add(vtable);
            }

            this.RandomVTables = new byte[this.VTableCount][];
            this.GuassianVTables = new byte[this.VTableCount][];

            for (int i = 0; i < this.VTableCount; ++i)
            {
                this.RandomVTables[i] = allVtables[i];
                this.GuassianVTables[i] = allVtables[i % ((int)Math.Sqrt(this.VTableCount))];
            }

            for (int i = 0; i < this.VTableCount / 2; ++i)
            {
                this.GuassianVTables[r.Next(this.VTableCount)] = allVtables[r.Next(this.VTableCount)];
            }
        }

        [Benchmark]
        public void FinishVTables_Random()
        {
            context.Reset(ScratchLength);

            var random = this.RandomVTables;
            for (int i = 0; i < random.Length; ++i)
            {
                context.FinishVTable(Scratch, random[i]);
            }
        }

        [Benchmark]
        public void FinishVTables_Guassian()
        {
            context.Reset(ScratchLength);

            var random = this.GuassianVTables;
            for (int i = 0; i < random.Length; ++i)
            {
                context.FinishVTable(Scratch, random[i]);
            }
        }
    }
}
