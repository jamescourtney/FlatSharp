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
        public int Offset;

        [Params(1, 2, 4, 8)]
        public int Alignment;

        [GlobalSetup]
        public void Setup()
        {
            this.Offset = new Random().Next(0, 256);
        }

        [Benchmark]
        public int NegPlusOne()
        {
            return ((~this.Offset) + 1) & (this.Alignment - 1);
        }

        [Benchmark]
        public int NotAnd()
        {
            return (-this.Offset) & (this.Alignment - 1);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StructVectorClone>();
            //StructVectorClone cloner = new StructVectorClone();
            //cloner.Setup();

            //for (int i = 0; i < 10_000_000; ++i)
            //{
            //    cloner.Parse();
            //}

            //for (int i = 0; i < 10_000_000; ++i)
            //{
            //    cloner.Serialize();
            //}
        }
    }
}
