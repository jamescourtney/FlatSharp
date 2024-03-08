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

    public class FBSerializeBench : FBBenchCore
    {
#if RUN_COMPARISON_BENCHMARKS
        [Benchmark]
        public override void Google_FlatBuffers_Serialize() => base.Google_FlatBuffers_Serialize();

        [Benchmark]
        public override void Google_Flatbuffers_StringVector_Sorted() => base.Google_Flatbuffers_StringVector_Sorted();

        [Benchmark]
        public override void Google_Flatbuffers_StringVector_Unsorted() => base.Google_Flatbuffers_StringVector_Unsorted();

        [Benchmark]
        public override void Google_Flatbuffers_IntVector_Sorted() => base.Google_Flatbuffers_IntVector_Sorted();

        [Benchmark]
        public override void Google_Flatbuffers_IntVector_Unsorted() => base.Google_Flatbuffers_IntVector_Unsorted();

        [Benchmark]
        public override void Protobuf_Serialize() => base.Protobuf_Serialize();

#if !AOT
        [Benchmark]
        public override void PBDN_Serialize() => base.PBDN_Serialize();

        [Benchmark]
        public override void MsgPack_Serialize() => base.MsgPack_Serialize();
#endif

#endif

        [Benchmark]
        public override void FlatSharp_GetMaxSize() => base.FlatSharp_GetMaxSize();

        [Benchmark]
        public override void FlatSharp_Serialize() => base.FlatSharp_Serialize();
        
        [Benchmark]
        public override void FlatSharp_Serialize_ValueStructs() => base.FlatSharp_Serialize_ValueStructs();

        [Benchmark]
        public override void FlatSharp_Serialize_StringVector_Sorted() => base.FlatSharp_Serialize_StringVector_Sorted();

        [Benchmark]
        public override void FlatSharp_Serialize_StringVector_Unsorted() => base.FlatSharp_Serialize_StringVector_Unsorted();

        [Benchmark]
        public override void FlatSharp_Serialize_IntVector_Sorted() => base.FlatSharp_Serialize_IntVector_Sorted();

        [Benchmark]
        public override void FlatSharp_Serialize_IntVector_Unsorted() => base.FlatSharp_Serialize_IntVector_Unsorted();
    }
}
