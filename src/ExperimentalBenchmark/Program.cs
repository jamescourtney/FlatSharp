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
        private byte[] data;
        private byte[] buffer;
        private Table table;

        [GlobalSetup]
        public void Setup()
        {
            this.data = new byte[32];
            this.buffer = new byte[1024];

            new Random().NextBytes(this.data);
            this.table = new Table
            {
                Hash = new Sha256()
            };

            this.table.Hash.Value.CopyFrom(this.data.AsSpan());
            this.Serialize();
        }

        //[Benchmark]
        public Table Clone() => new Table(this.table);

        //[Benchmark]
        public void CopyFrom() => this.table.Hash.Value.CopyFrom(this.data.AsSpan());

        [Benchmark]
        public int Serialize() => ((GeneratedSerializerWrapper<Table>)Table.Serializer).Write(SpanWriter.Instance, this.buffer, this.table);

        [Benchmark]
        public Table Parse() => ((GeneratedSerializerWrapper<Table>)Table.Serializer).Parse(new ArrayInputBuffer(this.buffer));
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StructVectorClone>();
        }
    }
}
