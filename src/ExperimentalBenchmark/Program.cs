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

        [GlobalSetup]
        public void Setup()
        {
            this.table = new SomeTable
            {
                Int = 1,
                String = "foobar",
                Struct = new SomeStruct()
            };

            for (int i = 0; i < this.table.Struct.Hash.Count; ++i)
            {
                this.table.Struct.Hash[i] = (byte)i;
            }

            this.Serialize();
        }

        [Benchmark]
        public void Serialize()
        {
            SomeTable.Serializer.Write(this.data, this.table!);
        }

        [Benchmark]
        public void Parse()
        {
            SomeTable.Serializer.Parse<SomeTable>(this.data);
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
