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
    public class Benchmark
    {
        private static ISerializer<Table> Serializer;
        //private static Table Table;
        private static byte[] Buffer;

        [GlobalSetup]
        public void Setup()
        {
            List<Vector3> vector3s = new List<Vector3>();
            List<FlatBufferVec3> fbVec3s = new List<FlatBufferVec3>();
            for (int i = 0; i < 300; ++i)
            {
                vector3s.Add(new Vector3(i));
                fbVec3s.Add(new FlatBufferVec3 { X = i, Y = i, Z = i });
            }

            Serializer = Table.Serializer.WithSettings(new() { DeserializationMode = FlatBufferDeserializationOption.Lazy });

            Table t = new Table { Items = vector3s, NormalItems = fbVec3s };

            Buffer = new byte[Serializer.GetMaxSize(t)];
            Serializer.Write(Buffer, t);
        }

        [Benchmark]
        public int ParseItem_Vec3()
        {
            Table t = Serializer.Parse(Buffer);

            Vector3 sum = new();
            var vector = t.Items;
            int count = vector.Count;

            for (int i = 0; i < count; ++i)
            {
                sum += vector[i];
            }

            return (int)Vector3.Dot(sum, Vector3.One);
        }

        [Benchmark]
        public int ParseItem_Normal()
        {
            Table t = Serializer.Parse(Buffer);

            float x = 0;
            float y = 0;
            float z = 0;

            var vector = t.NormalItems;
            int count = vector.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vector[i];

                x += item.X;
                y += item.Y;
                z += item.Z;
            }

            return (int)(x + y + z);
        }

        [Benchmark]
        public int ParseItem_Normal_ToVec3()
        {
            Table t = Serializer.Parse(Buffer);

            float x = 0;
            float y = 0;
            float z = 0;

            var vector = t.NormalItems;
            int count = vector.Count;
            var vec3 = Vector3.Zero;

            for (int i = 0; i < count; ++i)
            {
                var item = vector[i];
                ref Vector3 v3 = ref Unsafe.As<FlatBufferVec3, Vector3>(ref item);
                vec3 += v3;
            }

            return (int)(x + y + z);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Job job = Job.ShortRun
                .WithAnalyzeLaunchVariance(true)
                .WithLaunchCount(1)
                .WithWarmupCount(3)
                .WithIterationCount(5)
                .WithRuntime(CoreRuntime.Core60)
                .WithEnvironmentVariable(new EnvironmentVariable("DOTNET_TieredPGO", "1"));

            var config = DefaultConfig.Instance
                 .AddDiagnoser(MemoryDiagnoser.Default)
                 .AddJob(job);

            BenchmarkRunner.Run(typeof(Benchmark), config);
        }
    }
}
