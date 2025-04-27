/*
 * Copyright 2022 James Courtney
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

namespace Microbench
{
    using BenchmarkDotNet.Attributes;
    using System;
    using FlatSharp;
    using System.Linq;

    public class SerializeBenchmarks
    {
        [Benchmark]
        public long Serialize_StringTable_SingleString()
        {
            return StringTable.Serializer.Write(Constants.Buffers.StringTable_WithString, Constants.StringTables.WithString);
        }

        [Benchmark]
        public long Serialize_StringTable_Vector()
        {
            return StringTable.Serializer.Write(Constants.Buffers.StringTable_WithVector, Constants.StringTables.WithVector);
        }

        [Benchmark]
        public long Serialize_StringTable_EmptyTable()
        {
            return StringTable.Serializer.Write(Constants.Buffers.Stringtable_Empty, Constants.StringTables.Empty);
        }

        [Benchmark]
        public long Serialize_PrimitivesTable_Empty()
        {
            return PrimitivesTable.Serializer.Write(Constants.Buffers.PrimitivesTable_Empty, Constants.PrimitiveTables.Empty);
        }

        [Benchmark]
        public long Serialize_PrimitivesTable_Full()
        {
            return PrimitivesTable.Serializer.Write(Constants.Buffers.PrimitivesTable_Full, Constants.PrimitiveTables.Full);
        }

        [Benchmark]
        public long Serialize_PrimitivesTable_RandomlyPopulated()
        {
            return NestedTable.Serializer.Write(Constants.Buffers.PrimitivesTable_RandomEntries, Constants.PrimitiveTables.RandomEntries);
        }

        [Benchmark]
        public long Serialize_StructTable_SingleRef()
        {
            return StructsTable.Serializer.Write(Constants.Buffers.StructTable_SingleRef, Constants.StructTables.SingleRef);
        }

        [Benchmark]
        public long Serialize_StructTable_SingleValue()
        {
            return StructsTable.Serializer.Write(Constants.Buffers.StructTable_SingleValue, Constants.StructTables.SingleValue);
        }

        [Benchmark]
        public long Serialize_StructTable_VecRef()
        {
            return StructsTable.Serializer.Write(Constants.Buffers.StructTable_VecRef, Constants.StructTables.VectorRef);
        }

        [Benchmark]
        public long Serialize_StructTable_VecValue()
        {
            return StructsTable.Serializer.Write(Constants.Buffers.StructTable_VecValue, Constants.StructTables.VectorValue);
        }

        [Benchmark]
        public long Serialize_SafeUnion()
        {
            return UnionTable.Serializer.Write(Constants.Buffers.UnionTable_Safe, Constants.UnionTables.Safe);
        }

        [Benchmark]
        public long Serialize_UnsafeUnion()
        {
            return UnionTable.Serializer.Write(Constants.Buffers.UnionTable_Unsafe, Constants.UnionTables.Unsafe);
        }

        [Benchmark]
        public long Serialize_MixedUnion()
        {
            return UnionTable.Serializer.Write(Constants.Buffers.UnionTable_Mixed, Constants.UnionTables.Mixed);
        }
    }
}
