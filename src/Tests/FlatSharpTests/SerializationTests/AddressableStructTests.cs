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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies the IFlatBufferAddressableStruct interface is implemented correctly.
    /// </summary>
    [TestClass]
    public class AddressableStructTests
    {
        [TestMethod]
        public void GreedyStructs_NotAddressable()
        {
            Table table = new()
            {
                JaggedList = new List<JaggedStruct>
                {
                    new() { AlignmentImp = 1, Value = 1 },
                },

                Points = new List<Vec3>
                {
                    new() { X = 1, Y = 1, Z = 1 },
                }
            };

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.GreedyMutable);

            byte[] buffer = new byte[1024];
            serializer.Serialize(table, buffer);
            var parsed = serializer.Parse<Table>(buffer);

            for (int i = 0; i < parsed.Points.Count; ++i)
            {
                Assert.IsNotInstanceOfType(parsed.Points[i], typeof(IFlatBufferAddressableStruct));
            }

            for (int i = 0; i < parsed.JaggedList.Count; ++i)
            {
                Assert.IsNotInstanceOfType(parsed.JaggedList[i], typeof(IFlatBufferAddressableStruct));
            }
        }

        [TestMethod]
        public void AddressableStructTest_AccessEachItem()
        {
            Table table = new()
            {
                JaggedList = new List<JaggedStruct>
                { 
                    new() { AlignmentImp = 1, Value = 1 },
                    new() { AlignmentImp = 2, Value = 2 },
                    new() { AlignmentImp = 3, Value = 3 },
                },

                Points = new List<Vec3>
                { 
                    new() { X = 1, Y = 1, Z = 1 },
                    new() { X = 2, Y = 2, Z = 2 },
                    new() { X = 3, Y = 3, Z = 3 },
                }
            };

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            byte[] buffer = new byte[1024];
            serializer.Serialize(table, buffer);
            var parsed = serializer.Parse<Table>(buffer);

            for (int i = 0; i < parsed.Points.Count; ++i)
            {
                Assert.AreEqual(i + 1, parsed.Points[i].X);
                Assert.AreEqual(i + 1, parsed.Points[i].Y);
                Assert.AreEqual(i + 1, parsed.Points[i].Z);

                Assert.IsTrue(parsed.Points[i].TryGetVec3(out var vec3));

                Assert.AreEqual(i + 1, vec3.X);
                Assert.AreEqual(i + 1, vec3.Y);
                Assert.AreEqual(i + 1, vec3.Z);
            }

            for (int i = 0; i < parsed.JaggedList.Count; ++i)
            {
                Assert.AreEqual(i + 1, parsed.JaggedList[i].AlignmentImp);
                Assert.AreEqual(i + 1, parsed.JaggedList[i].Value);

                Assert.IsTrue(parsed.JaggedList[i].TryGetStruct(out var s));

                Assert.AreEqual(i + 1, s.AlignmentImp);
                Assert.AreEqual(i + 1, s.Value);
            }
        }

        [TestMethod]
        public void AddressableStructTest_ManualTraversal()
        {
            Table table = new()
            {
                JaggedList = new List<JaggedStruct>
                {
                    new() { AlignmentImp = 1, Value = 1 },
                    new() { AlignmentImp = 2, Value = 2 },
                    new() { AlignmentImp = 3, Value = 3 },
                },

                Points = new List<Vec3>
                {
                    new() { X = 1, Y = 1, Z = 1 },
                    new() { X = 2, Y = 2, Z = 2 },
                    new() { X = 3, Y = 3, Z = 3 },
                }
            };

            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);

            byte[] buffer = new byte[1024];
            serializer.Serialize(table, buffer);
            var parsed = serializer.Parse<Table>(buffer);

            {
                var first = parsed.Points[0] as IFlatBufferAddressableStruct;
                int alignment = first.Alignment;
                int offset = first.Offset;
                int size = first.Size;

                for (int i = 0; i < parsed.Points.Count; ++i)
                {
                    System.Numerics.Vector3 vec3 = MemoryMarshal.Cast<byte, System.Numerics.Vector3>(
                        buffer.AsSpan().Slice(offset, size))[0];

                    Assert.AreEqual(i + 1, vec3.X);
                    Assert.AreEqual(i + 1, vec3.Y);
                    Assert.AreEqual(i + 1, vec3.Z);

                    offset += size;
                    offset += SerializationHelpers.GetAlignmentError(offset, alignment);
                }
            }

            {
                var first = parsed.JaggedList[0] as IFlatBufferAddressableStruct;
                int alignment = first.Alignment;
                int offset = first.Offset;
                int size = first.Size;

                for (int i = 0; i < parsed.JaggedList.Count; ++i)
                {
                    JaggedValueStruct vec3 = MemoryMarshal.Cast<byte, JaggedValueStruct>(
                        buffer.AsSpan().Slice(offset))[0];

                    Assert.AreEqual(i + 1, vec3.AlignmentImp);
                    Assert.AreEqual(i + 1, vec3.Value);

                    offset += size;
                    offset += SerializationHelpers.GetAlignmentError(offset, alignment);
                }
            }
        }

        [FlatBufferTable]
        public class Table
        {
            [FlatBufferItem(0)]
            public virtual IList<Vec3>? Points { get; set; }

            [FlatBufferItem(1)]
            public virtual IList<JaggedStruct>? JaggedList { get; set; }
        }
        
        [FlatBufferStruct]
        public class Vec3
        {
            [FlatBufferItem(0)]
            public virtual float X { get; set; }

            [FlatBufferItem(1)]
            public virtual float Y { get; set; }

            [FlatBufferItem(2)]
            public virtual float Z { get; set; }

            public bool TryGetVec3(out System.Numerics.Vector3 vec3)
            {
                if (this is IFlatBufferAddressableStruct @struct)
                {
                    Assert.AreEqual(12, @struct.Size);
                    Assert.AreEqual(4, @struct.Alignment);
                    Assert.AreEqual(0, @struct.Offset % 4);
                    Assert.AreEqual(12, Marshal.SizeOf<System.Numerics.Vector3>());

                    var span = MemoryMarshal.Cast<byte, System.Numerics.Vector3>(
                        @struct.InputBuffer.GetByteMemory(@struct.Offset, @struct.Size).Span);

                    Assert.AreEqual(1, span.Length);

                    vec3 = span[0];
                    return true;
                }

                vec3 = default;
                return false;
            }
        }

        [FlatBufferStruct]
        public class JaggedStruct
        {
            [FlatBufferItem(0)]
            public virtual long Value { get; set; }

            [FlatBufferItem(1)]
            public virtual byte AlignmentImp { get; set; }

            public bool TryGetStruct(out JaggedValueStruct value)
            {
                if (this is IFlatBufferAddressableStruct @struct)
                {
                    Assert.AreEqual(9, @struct.Size);
                    Assert.AreEqual(8, @struct.Alignment);
                    Assert.AreEqual(0, @struct.Offset % 8);

                    int nativeSize = Marshal.SizeOf<JaggedValueStruct>();

                    int nextItem = @struct.Offset + @struct.Size;
                    nextItem += SerializationHelpers.GetAlignmentError(nextItem, @struct.Alignment);
                    int paddedSize = nextItem - @struct.Offset;

                    Assert.AreEqual(nativeSize, paddedSize);

                    var span = MemoryMarshal.Cast<byte, JaggedValueStruct>(
                        @struct.InputBuffer.GetByteMemory(@struct.Offset, nativeSize).Span);

                    Assert.AreEqual(1, span.Length);

                    value = span[0];
                    return true;
                }

                value = default;
                return false;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct JaggedValueStruct
        {
            [FieldOffset(0)]
            public long Value;

            [FieldOffset(8)]
            public byte AlignmentImp;
        }
    }
}
