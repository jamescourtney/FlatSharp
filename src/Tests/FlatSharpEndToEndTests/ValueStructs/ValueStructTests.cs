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

namespace FlatSharpEndToEndTests.ValueStructs
{
    using FlatSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Xunit;

    public class ValueStructTestCases
    {
        [Fact]
        public void Basics()
        {
            Assert.Equal(148, Marshal.SizeOf<ValueStruct>());
        }

        [Fact]
        public void ExtensionMethod_WorksForVectors()
        {
            ValueStruct v = default;

            Assert.Equal(128, v.D_Length);

            for (int i = 0; i < v.D_Length; ++i)
            {
                v.D(i) = (byte)i;
            }

            for (int i = 0; i < v.D_Length; ++i)
            {
                Assert.Equal((byte)i, v.D(i));
            }
        }

        [Fact]
        public void SerializeAndParse_Full()
        {
            ValueStruct vs = new ValueStruct
            {
                A = 1,
                B = 2,
                C = 3,
                Inner = new InnerValueStruct { A = 3.14f }
            };

            for (int i = 0; i < vs.D_Length; ++i)
            {
                vs.D(i) = (byte)i;
            }

            RefStruct rs = new()
            {
                A = 1,
                VS = vs,
            };

            RootTable table = new()
            {
                RefStruct = rs,
                Union = new TestUnion(vs),
                ValueStruct = vs,
                ValueStructVector = Enumerable.Range(0, 10).Select(x => vs).ToList(),
                VectorOfUnion = Enumerable.Range(0, 10).Select(x => new TestUnion(vs)).ToList(),
            };

            ISerializer<RootTable> serializer = RootTable.Serializer;
            int maxBytes = serializer.GetMaxSize(table);
            byte[] buffer = new byte[maxBytes];
            int written = serializer.Write(buffer, table);
            RootTable parsed = serializer.Parse(buffer[..written]);

            Assert.NotNull(parsed.RefStruct);
            Assert.NotNull(parsed.ValueStruct);
            Assert.NotNull(parsed.ValueStructVector);
            Assert.NotNull(parsed.Union);
            Assert.NotNull(parsed.VectorOfUnion);

            Assert.Equal(table.VectorOfUnion.Count, parsed.VectorOfUnion.Count);
            Assert.Equal(table.ValueStructVector.Count, parsed.ValueStructVector.Count);

            Assert.Equal(table.RefStruct.A, parsed.RefStruct.A);
            AssertStructsEqual(table.RefStruct.VS, parsed.RefStruct.VS);

            AssertStructsEqual(table.ValueStruct.Value, parsed.ValueStruct.Value);
            AssertStructsEqual(table.Union.ValueStruct, parsed.Union.ValueStruct);

            for (int i = 0; i < table.VectorOfUnion.Count; ++i)
            {
                var t = table.VectorOfUnion[i];
                var p = parsed.VectorOfUnion[i];
                AssertStructsEqual(t.ValueStruct, p.ValueStruct);
            }

            for (int i = 0; i < table.ValueStructVector.Count; ++i)
            {
                var t = table.ValueStructVector[i];
                var p = parsed.ValueStructVector[i];
                AssertStructsEqual(t, p);
            }
        }

        [Fact]
        public void SerializeAndParse_Empty()
        {
            RootTable t = new();

            Assert.Null(t.ValueStruct);

            ISerializer<RootTable> serializer = RootTable.Serializer;
            int maxBytes = serializer.GetMaxSize(t);
            byte[] buffer = new byte[maxBytes];
            int written = serializer.Write(buffer, t);
            RootTable parsed = serializer.Parse(buffer[..written]);

            Assert.Null(parsed.RefStruct);
            Assert.Null(parsed.ValueStruct);
        }

        private static void AssertStructsEqual(ValueStruct a , ValueStruct b)
        {
            Assert.Equal(a.A, b.A);
            Assert.Equal(a.B, b.B);
            Assert.Equal(a.C, b.C);
            Assert.Equal(a.Inner.A, b.Inner.A);

            for (int i = 0; i < a.D_Length; ++i)
            {
                Assert.Equal(a.D(i), b.D(i));
            }

            Span<byte> scratchA = stackalloc byte[Marshal.SizeOf<ValueStruct>()];
            Span<byte> scratchB = stackalloc byte[Marshal.SizeOf<ValueStruct>()];

            MemoryMarshal.Cast<byte, ValueStruct>(scratchA)[0] = a;
            MemoryMarshal.Cast<byte, ValueStruct>(scratchB)[0] = b;

            Assert.True(scratchA.SequenceEqual(scratchB));
        }
    }
}