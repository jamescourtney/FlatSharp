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

namespace BenchmarkCore
{
    using System;
    using System.Diagnostics;
    using Benchmark.FBBench;
    using FlatSharp;
    using FlatSharp.Attributes;

    [FlatBufferTable]
    public class Test1<T>
    {
        [FlatBufferItem(0)]
        public virtual T Item { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            FlatSharp.FlatBufferSerializer.Default.Compile<Test1<byte>>();
            sw.Stop();
            Console.WriteLine("First compilation took: " + sw.Elapsed.TotalSeconds + " seconds");
            
            sw = Stopwatch.StartNew();
            FlatSharp.FlatBufferSerializer.Default.Compile<Test1<sbyte>>();
            sw.Stop();
            Console.WriteLine("Second compilation took: " + sw.Elapsed.TotalSeconds + " seconds");

            sw = Stopwatch.StartNew();
            FlatSharp.FlatBufferSerializer.Default.Compile<Test1<bool>>();
            sw.Stop();
            Console.WriteLine("Third compilation took: " + sw.Elapsed.TotalSeconds + " seconds");

            Google_FBBench b = new Google_FBBench();
            const int count = 1000000;
            b.TraversalCount = 5;
            b.VectorLength = 30;
            b.GlobalSetup();
            Container = b.defaultContainer;
            sw.Stop();

            var s = new ExperimentalBenchmark.Generated.Serializer();

            Action[] items = new Action[]
            {
                b.FlatSharp_Serialize,
                SerializeManual,
                //b.FlatSharp_Serialize_Unsafe,
                // b.ZF_Serialize
            };

            for (int loop = 0; loop < 2; ++loop)
            {
                foreach (var action in items)
                {
                    for (int i = 0; i < 10000; ++i)
                    {
                        action();
                    }
                    GC.Collect();

                    sw = Stopwatch.StartNew();
                    for (int i = 0; i < count; ++i)
                    {
                        action();
                    }
                    sw.Stop();
                    Console.WriteLine($"{action.Method.Name}: Took {sw.ElapsedMilliseconds} ({sw.ElapsedMilliseconds * 1000 / ((double)count)} us per op)");
                    System.Threading.Thread.Sleep(1000);
                    GC.Collect();
                }
            }
        }

        private static readonly ExperimentalBenchmark.Generated.Serializer ManualSerializer = new ExperimentalBenchmark.Generated.Serializer();
        private static readonly SpanWriter spanWriter = new SpanWriter();
        private static readonly byte[] writeBuffer = new byte[128 * 1024];
        private static FooBarListContainer Container;
        private static readonly SerializationContext Context = new SerializationContext();

        private static void SerializeManual()
        {
            Context.Reset(writeBuffer.Length);
            ManualSerializer.Write(spanWriter, writeBuffer, Container, 0, Context);
        }
    }
}
