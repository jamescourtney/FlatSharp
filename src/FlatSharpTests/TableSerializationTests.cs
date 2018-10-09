///*
// * Copyright 2018 James Courtney
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//namespace FlatSharpTests
//{
//    using System;
//    using System.Buffers.Binary;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using FlatSharp;
//    using FlatSharp.Attributes;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;

//    [TestClass]
//    public class TableSerializationTests
//    {
//        private static readonly FlatBufferSerializer Serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions { ImplementIDeserializedObject = true });

//        /// <summary>
//        /// Serializes a series of linked list nodes and follows the offsets, verifying that the serializer is emitting correct data.
//        /// </summary>
//        [TestMethod]
//        public void TableSerialization_VerifyOffsets()
//        {
//            TestLinkedListNode node = new TestLinkedListNode
//            {
//                Value = "node 1",
//                Next = new TestLinkedListNode
//                {
//                    Value = "node 2",
//                    Next = new TestLinkedListNode
//                    {
//                        Value = "node 3",
//                        Next = null
//                    }
//                }
//            };

//            ByteArrayOutputBuffer memory = new ByteArrayOutputBuffer();
//            ReadOnlySpan<byte> buffer = memory.SizedSpan;
//            for (int i = 0; i < buffer.Length; ++i)
//            {
//                buffer[i] = byte.MaxValue;
//            }

//            int offset = Serializer.Serialize(node, memory);

//            var parsedNode = Serializer.Parse<TestLinkedListNode>(memory.SizedSpan.ToArray());

//            var node1 = parsedNode;
//            var node2 = parsedNode.Next;
//            var node3 = parsedNode.Next.Next;

//            buffer = memory.Memory.Span.Slice(0, offset);

//            IDeserializedObject node1Deserialized = (IDeserializedObject)node1;
//            IDeserializedObject node2Deserialized = (IDeserializedObject)node2;
//            IDeserializedObject node3Deserialized = (IDeserializedObject)node3;

//            // Make sure the root of the buffer points to the first table.
//            Assert.AreEqual(BinaryPrimitives.ReadInt32LittleEndian(buffer), node1Deserialized.GetOffset());

//            // First vtable always starts at 4 for FlatSharp
//            int vtableStart = node1Deserialized.GetOffset() - BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node1Deserialized.GetOffset()));
//            Assert.AreEqual(4, vtableStart);

//            int vtableLength = BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(vtableStart));
//            int tableLength = BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(vtableStart + 2));
//            int valueOffset = BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(vtableStart + 4));
//            int nextNodeOffset = BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(vtableStart + 6));
//            Assert.AreEqual(8, vtableLength); // vt length + table length + string offset + next offset
//            Assert.AreEqual(12, tableLength); // soffet + uoffset (string) + uoffset (next).

//            // Get the offset to the node 1 string:
//            int node1StringOffset = node1Deserialized.GetOffset() + valueOffset; // uoffset
//            node1StringOffset += BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node1StringOffset));

//            // Verify the length, then read the items, then check that the null terminator is present and accounted for.
//            Assert.AreEqual(node1.Value.Length, BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node1StringOffset)));
//            Assert.AreEqual(node1.Value, Encoding.UTF8.GetString(buffer.Slice(node1StringOffset + 4, 6).ToArray()));
//            Assert.AreEqual(0, buffer[node1StringOffset + 4 + node1.Value.Length]);

//            // Make sure node 1 points to node 2 correctly.
//            int node2Offset = node1Deserialized.GetOffset() + nextNodeOffset;
//            node2Offset += BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node2Offset));
//            Assert.AreEqual(node2Deserialized.GetOffset(), node2Offset);

//            // Make sure that nodes 1 and 2 share a vtable, since they are both fully formed nodes.
//            Assert.AreEqual(vtableStart, node2Offset - BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node2Offset)));
//            int node3Offset = node2Deserialized.GetOffset() + nextNodeOffset;
//            node3Offset += BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node3Offset));

//            Assert.AreEqual(node3Deserialized.GetOffset(), node3Offset);
//            int node3Vtable = node3Offset - BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(node3Offset));
//            Assert.AreNotEqual(vtableStart, node3Vtable);
//            int node3VtableLength = BinaryPrimitives.ReadInt16LittleEndian(buffer.Slice(node3Vtable));
//            int node3tableLength = BinaryPrimitives.ReadInt16LittleEndian(buffer.Slice(node3Vtable + 2));
//            Assert.AreEqual(node3VtableLength, 8);
//            Assert.AreEqual(node3tableLength, 8);
//        }

//        [FlatBufferTable]
//        public class SimpleTable
//        {
//            [FlatBufferItem(0)]
//            public virtual string String { get; set; }

//            [FlatBufferItem(1)]
//            public virtual SimpleStruct Struct { get; set; }

//            [FlatBufferItem(2)]
//            public virtual IList<SimpleStruct> StructVector { get; set; }

//            [FlatBufferItem(4)]
//            public virtual SimpleTable InnerTable { get; set; }
//        }

//        [FlatBufferStruct]
//        public class SimpleStruct
//        {
//            [FlatBufferItem(0)]
//            public double Double { get; set; }

//            [FlatBufferItem(1)]
//            public byte Byte { get; set; }

//            [FlatBufferItem(2)]
//            public uint Uint { get; set; }
//        }
//    }
//}
