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

    public class Program
    {
        public static void Main(string[] args)
        {
            FBDeserializeBench bench = new FBDeserializeBench();
            bench.VectorLength = 30;
            bench.DeserializeOption = FlatSharp.FlatBufferDeserializationOption.Lazy;
            bench.TraversalCount = 5;
            bench.GlobalSetup();

            while (true)
            {
                bench.FlatSharp_ParseAndTraverse();
            }
        }
    }
}
