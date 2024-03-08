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

namespace Benchmark.FBBench;

using BenchmarkDotNet.Attributes;
using global::FlatSharp;
using System;

public abstract class FBBenchCore
{
    [Params(30)]
    public virtual int VectorLength { get; set; }

    public virtual int TraversalCount { get; set; }

    public virtual FlatBufferDeserializationOption DeserializeOption { get; set; } = FlatBufferDeserializationOption.Default;

    [GlobalSetup]
    public virtual void GlobalSetup()
    {
#if !AOT
        Console.WriteLine($"Pbdn Size = {PbdnHelper.Prepare(this.VectorLength)}");
        Console.WriteLine($"MsgPack Size = {MsgPackHelper.Prepare(this.VectorLength)}");
#endif

        Console.WriteLine($"FlatSharp Size = {FlatSharpHelper.Prepare(this.VectorLength)}");
        Console.WriteLine($"FlatSharpValue Size = {FlatSharpValueStructs.Prepare(this.VectorLength)}");

        Console.WriteLine($"GoogleFlatBuffersObjectApi Size = {GoogleObjectApiHelper.Prepare(this.VectorLength)}");
        Console.WriteLine($"GoogleFlatBuffers Size = {GoogleHelper.Prepare(this.VectorLength)}");
        Console.WriteLine($"Protobuf Size = {ProtobufHelper.Prepare(this.VectorLength)}");
    }

    public virtual void Google_FlatBuffers_Serialize()
        => GoogleObjectApiHelper.Serialize();

    public virtual int Google_Flatbuffers_ParseAndTraverse() 
        => GoogleHelper.ParseAndTraverse(this.TraversalCount);

    public virtual int Google_Flatbuffers_ParseAndTraverse_ObjectApi() 
        => GoogleObjectApiHelper.ParseAndTraverse(this.TraversalCount);

    public virtual int Google_Flatbuffers_ParseAndTraversePartial() 
        => GoogleHelper.ParseAndTraversePartial(this.TraversalCount);

    public virtual int Google_Flatbuffers_ParseAndTraversePartial_ObjectApi() 
        => GoogleObjectApiHelper.ParseAndTraversePartial(this.TraversalCount);

    public virtual void Google_Flatbuffers_StringVector_Sorted()
        => GoogleHelper.SerializeSortedStringVector();

    public virtual void Google_Flatbuffers_IntVector_Sorted()
        => GoogleHelper.SerializeSortedIntVector();

    public virtual void Google_Flatbuffers_StringVector_Unsorted()
        => GoogleHelper.SerializeUnsortedStringVector();

    public virtual void Google_Flatbuffers_IntVector_Unsorted()
        => GoogleHelper.SerializeUnsortedIntVector();

    public virtual void FlatSharp_GetMaxSize() 
        => FlatSharpHelper.GetMaxSize();

    public virtual void FlatSharp_Serialize() 
        => FlatSharpHelper.Serialize();

    public virtual void FlatSharp_Serialize_ValueStructs() 
        => FlatSharpValueStructs.Serialize();

    public virtual void FlatSharp_ParseAndTraverse() 
        => FlatSharpHelper.ParseAndTraverse(this.TraversalCount, this.DeserializeOption);

    public virtual void FlatSharp_ParseAndTraversePartial() 
        => FlatSharpHelper.ParseAndTraversePartial(this.TraversalCount, this.DeserializeOption);

    public virtual void FlatSharp_ParseAndTraverse_ValueStructs()
        => FlatSharpValueStructs.ParseAndTraverse(this.TraversalCount, this.DeserializeOption);

    public virtual void FlatSharp_ParseAndTraversePartial_ValueStructs()
        => FlatSharpValueStructs.ParseAndTraversePartial(this.TraversalCount, this.DeserializeOption);

    public virtual void FlatSharp_Serialize_StringVector_Sorted()
        => FlatSharpHelper.SerializeSortedStrings();

    public virtual void FlatSharp_Serialize_IntVector_Sorted()
        => FlatSharpHelper.SerializeSortedInts();

    public virtual void FlatSharp_Serialize_StringVector_Unsorted()
        => FlatSharpHelper.SerializeUnsortedStrings();

    public virtual void FlatSharp_Serialize_IntVector_Unsorted()
        => FlatSharpHelper.SerializeUnsortedInts();

    public virtual void Protobuf_Serialize()
        => ProtobufHelper.Serialize();

    public virtual void Protobuf_ParseAndTraverse()
        => ProtobufHelper.ParseAndTraverse(this.TraversalCount);

    public virtual void Protobuf_ParseAndTraversePartial()
        => ProtobufHelper.ParseAndTraversePartial(this.TraversalCount);

#if !AOT

    public virtual void PBDN_Serialize() 
        => PbdnHelper.Serialize();

    public virtual void PBDN_ParseAndTraverse()
        => PbdnHelper.ParseAndTraverse(this.TraversalCount);

    public virtual void PBDN_ParseAndTraversePartial()
        => PbdnHelper.ParseAndTraversePartial(this.TraversalCount);

    public virtual void MsgPack_Serialize()
        => MsgPackHelper.Serialize();

    public virtual void MsgPack_ParseAndTraverse()
        => MsgPackHelper.ParseAndTraverse(this.TraversalCount);

    public virtual void MsgPack_ParseAndTraversePartial()
        => MsgPackHelper.ParseAndTraversePartial(this.TraversalCount);

#endif

}
