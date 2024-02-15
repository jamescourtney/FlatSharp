/*
 * Copyright 2021 James Courtney
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
    using FlatSharp.Attributes;
    using System.Runtime.InteropServices;

    public class WriteThroughBench
    {
        private FlatBufferSerializer serializer;
        private byte[] data;

        [GlobalSetup]
        public void Setup()
        {
            this.serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            Table t = new()
            {
                RefStructLarge = new(),
                RefStructSmall = new(),
                ValueStructLarge = new(),
                ValueStructSmall = new(),
            };

            this.data = new byte[this.serializer.GetMaxSize(t)];
            this.serializer.Serialize(t, this.data);
        }

        [Benchmark]
        public void RefStructSmall()
        {
            Table t = this.serializer.Parse<Table>(this.data);
            RefStruct_Small s = t.RefStructSmall;

            for (int i = 0; i < 100; ++i)
            {
                s.Foo = i;
            }
        }

        [Benchmark]
        public void RefStructLarge()
        {
            Table t = this.serializer.Parse<Table>(this.data);
            RefStruct_Large s = t.RefStructLarge;

            for (int i = 0; i < 100; ++i)
            {
                s.A = i;
                s.B = i;
                s.C = i;
                s.D = i;
                s.E = i;
                s.F = i;
                s.G = i;
                s.H = i;
                s.I = i;
                s.J = i;
            }
        }

        [Benchmark]
        public void ValueStructSmall()
        {
            Table t = this.serializer.Parse<Table>(this.data);

            for (int i = 0; i < 100; ++i)
            {
                t.ValueStructSmall = new() { Foo = i };
            }
        }

        [Benchmark]
        public void ValueStructLarge()
        {
            Table t = this.serializer.Parse<Table>(this.data);

            for (int i = 0; i < 100; ++i)
            {
                t.ValueStructLarge = new()
                {
                    A = i,
                    B = i,
                    C = i,
                    D = i,
                    E = i,
                    F = i,
                    G = i,
                    H = i,
                    I = i,
                    J = i,
                };
            }
        }

        [FlatBufferTable]
        public class Table
        {
            [FlatBufferItem(0)]
            public virtual RefStruct_Small RefStructSmall { get; set; }

            [FlatBufferItem(1, Required = true, WriteThrough = true)]
            public virtual ValueStruct_Small ValueStructSmall { get; set; }

            [FlatBufferItem(2)]
            public virtual RefStruct_Large RefStructLarge { get; set; }

            [FlatBufferItem(3, Required = true, WriteThrough = true)]
            public virtual ValueStruct_Large ValueStructLarge { get; set; }
        }

        [StructLayout(LayoutKind.Explicit, Size = 4)]
        [FlatBufferStruct]
        public struct ValueStruct_Small
        {
            [FieldOffset(0)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "Foo")]
            public int Foo;
        }

        [StructLayout(LayoutKind.Explicit, Size = 40)]
        [FlatBufferStruct]
        public struct ValueStruct_Large
        {
            [FieldOffset(0)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "A")]
            public int A;
            [FieldOffset(4)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "B")]
            public int B;
            [FieldOffset(8)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "C")]
            public int C;
            [FieldOffset(12)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "D")]
            public int D;
            [FieldOffset(16)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "E")]
            public int E;
            [FieldOffset(20)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "F")]
            public int F;
            [FieldOffset(24)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "G")]
            public int G;
            [FieldOffset(28)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "H")]
            public int H;
            [FieldOffset(32)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "I")]
            public int I;
            [FieldOffset(36)]
            [FlatBufferMetadata(FlatBufferMetadataKind.Accessor, "", "J")]
            public int J;
        }

        [FlatBufferStruct]
        public class RefStruct_Small
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int Foo { get; set; }
        }

        [FlatBufferStruct]
        public class RefStruct_Large
        {
            [FlatBufferItem(0, WriteThrough = true)]
            public virtual int A { get; set; }

            [FlatBufferItem(1, WriteThrough = true)]
            public virtual int B { get; set; }

            [FlatBufferItem(2, WriteThrough = true)]
            public virtual int C { get; set; }

            [FlatBufferItem(3, WriteThrough = true)]
            public virtual int D { get; set; }

            [FlatBufferItem(4, WriteThrough = true)]
            public virtual int E { get; set; }

            [FlatBufferItem(5, WriteThrough = true)]
            public virtual int F { get; set; }

            [FlatBufferItem(6, WriteThrough = true)]
            public virtual int G { get; set; }

            [FlatBufferItem(7, WriteThrough = true)]
            public virtual int H { get; set; }

            [FlatBufferItem(8, WriteThrough = true)]
            public virtual int I { get; set; }

            [FlatBufferItem(9, WriteThrough = true)]
            public virtual int J { get; set; }
        }
    }
}