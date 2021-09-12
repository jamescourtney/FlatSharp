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

namespace Benchmark.FBBench
{
    using BenchmarkDotNet.Attributes;
    using FlatSharp;
    using System;

    public class FBDeserializeBench : FBBenchCore
    {
        [Params(1, 5)]
        public override int TraversalCount { get; set; }

        [Params(
            //FlatBufferDeserializationOption.Lazy,
#if !FLATSHARP_6_0_0_OR_GREATER
            FlatBufferDeserializationOption.PropertyCache,
            FlatBufferDeserializationOption.VectorCache,
            FlatBufferDeserializationOption.VectorCacheMutable
#else
            FlatBufferDeserializationOption.Progressive
#endif
            //FlatBufferDeserializationOption.Greedy,
            //FlatBufferDeserializationOption.GreedyMutable
        )]
        public override FlatBufferDeserializationOption DeserializeOption { get; set; }

        [Benchmark]
        public override void FlatSharp_ParseAndTraverse() => base.FlatSharp_ParseAndTraverse();

        [Benchmark]
        public override void FlatSharp_ParseAndTraversePartial() => base.FlatSharp_ParseAndTraversePartial();

        //[Benchmark]
        public override void FlatSharp_ParseAndTraverse_NonVirtual() => base.FlatSharp_ParseAndTraverse_NonVirtual();

        //[Benchmark]
        public override void FlatSharp_ParseAndTraversePartial_NonVirtual() => base.FlatSharp_ParseAndTraversePartial_NonVirtual();

#if FLATSHARP_5_7_1_OR_GREATER
        //[Benchmark]
        public override void FlatSharp_ParseAndTraverse_ValueStructs() => base.FlatSharp_ParseAndTraverse_ValueStructs();

        //[Benchmark]
        public override void FlatSharp_ParseAndTraversePartial_ValueStructs() => base.FlatSharp_ParseAndTraversePartial_ValueStructs();
#endif
    }
}
