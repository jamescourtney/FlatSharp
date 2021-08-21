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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Runtime.InteropServices;
    using FlatSharp.Attributes;

    [FlatBufferTable]
    public class FiveByteStructTable
    {
        [FlatBufferItem(0)]
        public virtual FiveByteStruct[]? Vector { get; set; }
    }

    [FlatBufferStruct]
    public class FiveByteStruct
    {
        [FlatBufferItem(0)]
        public virtual int Int { get; set; }

        [FlatBufferItem(1)]
        public virtual byte Byte { get; set; }
    }
  
    [FlatBufferStruct, StructLayout(LayoutKind.Explicit, Size = 5)]
    public struct ValueFiveByteStruct 
    {
        [FieldOffset(0)] public int Int;
        [FieldOffset(4)] public byte Byte;
    }

    [FlatBufferStruct, StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ValueFiveByteStructSequential
    {
        public byte b1;
        public long Long;
        public byte b2;
        public int Int;
        public byte b3;
        public InnerStructSequential Inner;
    }

    [FlatBufferStruct, StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct InnerStructSequential
    {
        public ushort UShort;
        public long Long;
    }

    [FlatBufferStruct, StructLayout(LayoutKind.Sequential, Pack = 128)]
    public struct ValueFiveByteStructSequential128
    {
        public byte b1;
        public long Long;
        public byte b2;
        public int Int;
        public byte b3;
        public InnerStructSequential128 Inner;
    }

    [FlatBufferStruct, StructLayout(LayoutKind.Sequential, Pack = 128)]
    public struct InnerStructSequential128
    {
        public ushort UShort;
        public long Long;
    }
}
