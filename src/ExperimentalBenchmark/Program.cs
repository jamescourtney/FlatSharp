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
    using Benchmark.FBBench;
    using BenchmarkDotNet.Running;
    using System;
    using JobKind = BenchmarkDotNet.Attributes.MediumRunJobAttribute;

    public class Program
    {
        public static void Main(string[] args)
        {
            //    FBSharedStringBench bench = new FBSharedStringBench();
            //    bench.CacheSize = 800;
            //    bench.VectorLength = 1000;
            //    bench.GlobalSetup();

            //    while (true)
            //    {
            //        bench.Parse_RepeatedStringVector_WithSharedStrings();
            //    }
            BenchmarkRunner.Run<TestBench>();
        }
    }

    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp31)]
    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp21)]
    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.Net47)]
    //[CsvExporter(BenchmarkDotNet.Exporters.Csv.CsvSeparator.Comma)]
    public class TestBench
    {
        private readonly TestClassDelegate @delegate = new TestClassDelegate(() => { });
        private readonly TestClassVirtual @virtual = new TestClassFinal();

        [BenchmarkDotNet.Attributes.Benchmark]
        public void TestWithDelegate()
        {
            this.@delegate.DoWork();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void TestWithOverride()
        {
            this.@virtual.DoWork();
        }
    }

    public sealed class TestClassDelegate
    {
        private readonly Action action;

        public TestClassDelegate(Action action)
        {
            this.action = action;
        }

        public void DoWork()
        {
            Action a = this.action;
            for (int i = 0; i < 100; ++i)
            {
                a();
            }
        }
    }


    public abstract class TestClassVirtual
    {
        public void DoWork()
        {
            for (int i = 0; i < 100; ++i)
            {
                this.DoThing();
            }
        }

        protected abstract void DoThing();
    }

    public sealed class TestClassFinal : TestClassVirtual
    {
        protected override void DoThing()
        {
        }
    }
}
