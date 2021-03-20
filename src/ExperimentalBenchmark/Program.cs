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
using System.Diagnostics;

namespace BenchmarkCore
{
    [DisassemblyDiagnoser(maxDepth: 30, printSource: true, exportHtml: true, printInstructionAddresses: true)]
    [ShortRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class StructVectorClone
    {
        private readonly byte[] data = new byte[10240];
        private SomeTable? table;

        private ArrayInputBuffer inputBuffer;
        //private UnsafeSpanWriter2 spanWriter;

        [GlobalSetup]
        public void Setup()
        {
            this.inputBuffer = new ArrayInputBuffer(this.data);
            //this.spanWriter = UnsafeSpanWriter2.Instance;

            this.table = new SomeTable
            {
                Int = new int[100]
                //String = "foobar",
                //Struct = new SomeStruct[100]
            };

            for (int i = 0; i < this.table.Int.Count; ++i)
            {
                table.Int[i] = i;
            }

            this.Serialize();
        }

        [Benchmark]
        public void Serialize()
        {
            SomeTable.Serializer.Write(SpanWriter.Instance, this.data, this.table!);
        }

        [Benchmark]
        public void Parse()
        {
            SomeTable.Serializer.Parse(this.inputBuffer);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<StructVectorClone>();
            StructVectorClone cloner = new StructVectorClone();
            cloner.Setup();

            const int Count = 3_000_000;

            for (int iter = 0; iter < 3; ++iter)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < Count; ++i)
                {
                    cloner.Parse();
                }
                sw.Stop();
                Console.WriteLine($"Parse: {Count / sw.ElapsedMilliseconds} items per ms");

                sw.Restart();
                for (int i = 0; i < Count; ++i)
                {
                    cloner.Serialize();
                }

                sw.Stop();
                Console.WriteLine($"Serialize: {Count / sw.ElapsedMilliseconds} items per ms");
            }
        }
    }
}
