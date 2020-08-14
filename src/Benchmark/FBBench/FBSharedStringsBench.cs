/*
 * Copyright 2020 James Courtney
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

namespace Benchmark.FBBench
{
    using BenchmarkDotNet.Attributes;
    using FlatSharp;
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [MediumRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp21)]
    [MediumRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp31)]
    //[ShortRunJob]
    public class FBSharedStringBench
    {
        public VectorTable<string> nonSharedStringVector;
        public VectorTable<SharedString> randomSharedStringVector;
        public VectorTable<SharedString> guassianSharedStringVector;

        private SpanWriter AssociativeSpanWriter;

        public byte[] serializedGuassianStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedRandomStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedNonSharedStringVector = new byte[10 * 1024 * 1024];

        [Params(100, 200, 400, 800)]
        public int CacheSize { get; set; }

        [Params(1000)]
        public int VectorLength { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            this.AssociativeSpanWriter = new SpanWriter(new SharedStringWriter(this.CacheSize));

            Random random = new Random();

            List<string> guids =
                Enumerable.Range(0, this.VectorLength)
                .Select(i => Guid.NewGuid().ToString()).ToList();

            List<string> randomGuids = Enumerable.Range(0, this.VectorLength)
                .Select(i => guids[random.Next(0, guids.Count)]).ToList();

            List<string> guassianGuids = Enumerable.Range(0, this.VectorLength)
                .Select(i => guids[NextGaussianInt(random, 0, guids.Count - 1)]).ToList();

            nonSharedStringVector = new VectorTable<string> { Vector = randomGuids.ToList() };
            randomSharedStringVector = new VectorTable<SharedString> { Vector = randomGuids.Select(SharedString.Create).ToList() };
            guassianSharedStringVector = new VectorTable<SharedString> { Vector = guassianGuids.Select(SharedString.Create).ToList() };

            
            int nonSharedSize = this.Serialize_NonSharedString();
            int cacheSize = this.Serialize_GuassianStringVector_WithSharing();

            Console.WriteLine($"NonShared: {nonSharedSize}");
            Console.WriteLine($"Shared size: {cacheSize}");
        }

        
        [Benchmark]
        public int Serialize_NonSharedString()
        {
            return FlatBufferSerializer.Default.Serialize(
                nonSharedStringVector,
                serializedNonSharedStringVector,
                SpanWriter.Instance);
        }

        [Benchmark]
        public int Serialize_RandomSharedString_PLRU()
        {
            return FlatBufferSerializer.Default.Serialize(
                randomSharedStringVector,
                this.serializedRandomStringVector,
                this.AssociativeSpanWriter);
        }

        [Benchmark]
        public int Serialize_GuassianStringVector_WithSharing()
        {
            return FlatBufferSerializer.Default.Serialize(
                guassianSharedStringVector,
                this.serializedGuassianStringVector,
                this.AssociativeSpanWriter);
        }

        [Benchmark]
        public void Parse_NonSharedStringVector_WithRegularString()
        {
            InputBuffer buffer = new ArrayInputBuffer(this.serializedNonSharedStringVector);
            FlatBufferSerializer.Default.Parse<VectorTable<string>>(buffer);
        }

        [Benchmark]
        public void Parse_NonSharedStringVector_WithSharedString()
        {
            InputBuffer buffer = new ArrayInputBuffer(this.serializedNonSharedStringVector);
            FlatBufferSerializer.Default.Parse<VectorTable<SharedString>>(buffer);
        }

        [Benchmark]
        public void Parse_GuassianSharedStringVector_WithSharedStringWithCache_NonThreadSafe()
        {
            InputBuffer buffer = new ArrayInputBuffer(this.serializedGuassianStringVector); 
            buffer.SetSharedStringReader(new SharedStringReader(this.CacheSize, threadSafe: false));

            FlatBufferSerializer.Default.Parse<VectorTable<SharedString>>(buffer);
        }

        [Benchmark]
        public void Parse_GuassianSharedStringVector_WithSharedStringWithCache_ThreadSafe()
        {
            InputBuffer buffer = new ArrayInputBuffer(this.serializedGuassianStringVector);
            buffer.SetSharedStringReader(new SharedStringReader(this.CacheSize, threadSafe: true));

            FlatBufferSerializer.Default.Parse<VectorTable<SharedString>>(buffer);
        }

        [FlatBufferTable]
        public class VectorTable<T>
        {
            [FlatBufferItem(0)]
            public virtual IList<T> Vector { get; set; }
        }

        public static int NextGaussianInt(Random r, int min, int max)
        {
            double random = NextGaussian(r, (max - min) / 2.0, (max - min) / 6.0); // Gives a nice distribution.
            return Math.Max(min, Math.Min(max, (int)random));
        }

        public static double NextGaussian(Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}
