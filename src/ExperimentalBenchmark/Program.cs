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

namespace BenchmarkCore
{
    [DisassemblyDiagnoser(maxDepth: 30, printSource: true, exportHtml: true, printInstructionAddresses: true)]
    [ShortRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class StructVectorClone
    {
        public byte[] Buffer;
        public ScalarTable table;

        [GlobalSetup]
        public void Setup()
        {
            this.Buffer = new byte[1024];
            this.table = new ScalarTable
            {
                A = 1,
                B = 2,
                C = 3,
                Struct = new Struct
                {
                }
            };

            this.Serialize();
        }

        [Benchmark]
        public int Serialize()
        {
            return ScalarTable.Serializer.Write(this.Buffer, this.table);
        }

        [Benchmark]
        public void Parse()
        {
            ScalarTable.Serializer.Parse(new ArrayInputBuffer(this.Buffer));
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<StructVectorClone>();
            StructVectorClone cloner = new StructVectorClone();
            cloner.Setup();

            for (int i = 0; i < 10_000_000; ++i)
            {
                cloner.Parse();
            }

            //for (int i = 0; i < 10_000_000; ++i)
            //{
            //    cloner.Serialize();
            //}
        }
    }
}
