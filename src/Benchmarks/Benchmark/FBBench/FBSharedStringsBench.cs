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
#if FLATSHARP_6_0_0_OR_GREATER

        public NonSharedVectorTable nonSharedStringVector;
        public SharedVectorTable randomSharedStringVector;
        public SharedVectorTable nonRandomSharedStringVector;

        private ISerializer<NonSharedVectorTable> regularStringSerializer;
        private ISerializer<SharedVectorTable> sharedStringSerializer;

        public byte[] serializedGuassianStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedRandomStringVector = new byte[10 * 1024 * 1024];
        public byte[] serializedNonSharedStringVector = new byte[10 * 1024 * 1024];

        [Params(127, 1024)]
        public int CacheSize { get; set; }

        [Params(1000)]
        public override int VectorLength { get; set; }

        public override void GlobalSetup()
        {
            this.regularStringSerializer = FlatBufferSerializer.Default.Compile<NonSharedVectorTable>();

            this.sharedStringSerializer = FlatBufferSerializer.Default.Compile<SharedVectorTable>()
#if FLATSHARP_7_0_0_OR_GREATER
                .WithSettings(s => s.UseDefaultSharedStringWriter(this.CacheSize));
#else
                .WithSettings(new SerializerSettings { SharedStringWriterFactory = () => new SharedStringWriter(this.CacheSize) });
#endif

            Random random = new Random();

            List<string> guids =
                Enumerable.Range(0, this.VectorLength)
                .Select(i => Guid.NewGuid().ToString()).ToList();

            List<string> randomGuids = Enumerable.Range(0, this.VectorLength)
                .Select(i => guids[random.Next(0, guids.Count)]).ToList();

            List<string> guassianGuids = Enumerable.Range(0, this.VectorLength)
                .Select(i => guids[NextGaussianInt(random, 0, guids.Count - 1)]).ToList();

            nonSharedStringVector = new NonSharedVectorTable { Vector = randomGuids.ToList() };
            randomSharedStringVector = new SharedVectorTable { Vector = randomGuids.ToList() };
            nonRandomSharedStringVector = new SharedVectorTable { Vector = guassianGuids.ToList() };

            int nonSharedSize = this.Serialize_RandomStringVector_WithRegularString();
            int cacheSize = this.Serialize_NonRandomStringVector_WithSharing();

            Console.WriteLine($"NonSharedSize = {nonSharedSize}");
            Console.WriteLine($"DirectMapSize = {cacheSize}");
        }
                
        [Benchmark]
        public int Serialize_RandomStringVector_WithRegularString()
        {
            return this.regularStringSerializer.Write(
                default(SpanWriter),
                serializedNonSharedStringVector,
                nonSharedStringVector);
        }

        [Benchmark]
        public int Serialize_RandomStringVector_WithSharing()
        {
            return this.sharedStringSerializer.Write(
                default(SpanWriter),
                this.serializedRandomStringVector,
                randomSharedStringVector);
        }

        [Benchmark]
        public int Serialize_NonRandomStringVector_WithSharing()
        {
            return this.sharedStringSerializer.Write(
                default(SpanWriter), 
                this.serializedGuassianStringVector, 
                this.nonRandomSharedStringVector);
        }

        [FlatBufferTable]
        public class NonSharedVectorTable
        {
            [FlatBufferItem(0)]
            public virtual IList<string> Vector { get; set; }
        }

        [FlatBufferTable]
        public class SharedVectorTable
        {
            [FlatBufferItem(0, SharedString = true)]
            public virtual IList<string> Vector { get; set; }
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

#endif
    }
    }
