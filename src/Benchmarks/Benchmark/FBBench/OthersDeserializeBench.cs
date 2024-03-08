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
    using global::FlatSharp;

    public class OthersDeserializeBench : FBBenchCore
    {
        public override FlatBufferDeserializationOption DeserializeOption { get; set; }

        [Params(1, 5)]
        public override int TraversalCount { get; set; }

        [Benchmark]
        public override int Google_Flatbuffers_ParseAndTraverse() => base.Google_Flatbuffers_ParseAndTraverse();

        [Benchmark]
        public override int Google_Flatbuffers_ParseAndTraversePartial() => base.Google_Flatbuffers_ParseAndTraversePartial();

        [Benchmark]
        public override int Google_Flatbuffers_ParseAndTraverse_ObjectApi() => base.Google_Flatbuffers_ParseAndTraverse_ObjectApi();

        [Benchmark]
        public override int Google_Flatbuffers_ParseAndTraversePartial_ObjectApi() => base.Google_Flatbuffers_ParseAndTraversePartial_ObjectApi();

        [Benchmark]
        public override void Protobuf_ParseAndTraverse() => base.Protobuf_ParseAndTraverse();

        [Benchmark]
        public override void Protobuf_ParseAndTraversePartial() => base.Protobuf_ParseAndTraversePartial();

#if !AOT

        [Benchmark]
        public override void PBDN_ParseAndTraverse() => base.PBDN_ParseAndTraverse();

        [Benchmark]
        public override void PBDN_ParseAndTraversePartial() => base.PBDN_ParseAndTraversePartial();

        [Benchmark]
        public override void MsgPack_ParseAndTraverse() => base.MsgPack_ParseAndTraverse();

        [Benchmark]
        public override void MsgPack_ParseAndTraversePartial() => base.MsgPack_ParseAndTraversePartial();

#endif
    }
}
