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

    public class FBSharedStringBench : FBBenchCore
    {
        public VectorTable<string> nonSharedStringVector;
        public VectorTable<SharedString> randomSharedStringVector;
        public VectorTable<SharedString> nonRandomSharedStringVector;

        private ISerializer<VectorTable<string>> regularStringSerializer;
        private ISerializer<VectorTable<SharedString>> sharedStringSerializer;
        private ISerializer<VectorTable<SharedString>> sharedStringThreadSafeSerializer;

        public byte[] serializedGuassianStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedRandomStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedNonSharedStringVector = new byte[10 * 1024 * 1024];

        [Params(100, 200, 400, 800)]
        public int CacheSize { get; set; }

        [Params(1000)]
        public override int VectorLength { get; set; }

        public override void GlobalSetup()
        {
            this.regularStringSerializer = FlatBufferSerializer.Default.Compile<VectorTable<string>>();

            this.sharedStringSerializer = FlatBufferSerializer.Default.Compile<VectorTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringReaderFactory = () => SharedStringReader.Create(this.CacheSize),
                    SharedStringWriterFactory = () => new SharedStringWriter(this.CacheSize),
                });

            this.sharedStringThreadSafeSerializer = FlatBufferSerializer.Default.Compile<VectorTable<SharedString>>()
                .WithSettings(new SerializerSettings
                {
                    SharedStringReaderFactory = () => SharedStringReader.CreateThreadSafe(this.CacheSize),
                    SharedStringWriterFactory = () => new SharedStringWriter(this.CacheSize),
                });

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
            nonRandomSharedStringVector = new VectorTable<SharedString> { Vector = guassianGuids.Select(SharedString.Create).ToList() };

            int nonSharedSize = this.Serialize_RandomStringVector_WithRegularString();
            int cacheSize = this.Serialize_NonRandomStringVector_WithSharing();
        }
                
        [Benchmark]
        public int Serialize_RandomStringVector_WithRegularString()
        {
            return this.regularStringSerializer.Write(
                SpanWriter.Instance,
                serializedNonSharedStringVector,
                nonSharedStringVector);
        }

        [Benchmark]
        public int Serialize_RandomStringVector_WithSharing()
        {
            return this.sharedStringSerializer.Write(
                SpanWriter.Instance,
                this.serializedRandomStringVector,
                randomSharedStringVector);
        }

        [Benchmark]
        public int Serialize_NonRandomStringVector_WithSharing()
        {
            return this.sharedStringSerializer.Write(
                SpanWriter.Instance, 
                this.serializedGuassianStringVector, 
                this.nonRandomSharedStringVector);
        }

        [Benchmark]
        public void Parse_RepeatedStringVector_WithRegularString()
        {
            this.regularStringSerializer.Parse(this.serializedNonSharedStringVector);
        }

        [Benchmark]
        public void Parse_RepeatedStringVector_WithSharedStrings()
        {
            this.sharedStringSerializer.Parse(this.serializedNonSharedStringVector);
        }

        [Benchmark]
        public void Parse_NonRandomSharedStringVector_WithSharedStrings_NonThreadSafe()
        {
            this.sharedStringSerializer.Parse(this.serializedGuassianStringVector);
        }

        [Benchmark]
        public void Parse_NonRandomSharedStringVector_WithSharedStrings_ThreadSafe()
        {
            this.sharedStringThreadSafeSerializer.Parse(this.serializedGuassianStringVector);
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
