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

namespace FlatSharpTests;

public class DepthLimitTests
{
    [Theory]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable, 1000, 999, false)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable, 1000, 1000, true)]
    [InlineData(FlatBufferDeserializationOption.Lazy, 1000, 999, false)]
    [InlineData(FlatBufferDeserializationOption.Lazy, 1000, 1000, true)]
    public void LinkedListDepth(FlatBufferDeserializationOption option, short limit, int nodes, bool expectException)
    {
        FlatBufferSerializer fbs = new FlatBufferSerializer(option);
        ISerializer<LinkedListNode> serializer = fbs.Compile<LinkedListNode>().WithSettings(new SerializerSettings { ObjectDepthLimit = limit });

        LinkedListNode head = new LinkedListNode();
        LinkedListNode current = head;

        for (int i = 0; i < nodes; ++i)
        {
            current.Next = new LinkedListNode();
            current = current.Next;
        }

        byte[] buffer = new byte[serializer.GetMaxSize(head)];
        serializer.Write(buffer, head);

        Action callback = () =>
        {
            LinkedListNode node = serializer.Parse(buffer);
            while (node != null)
            {
                node = node.Next;
            }
        };

        if (expectException)
        {
            Assert.Throws<System.IO.InvalidDataException>(callback);
        }
        else
        {
            callback();
        }
    }

    [FlatBufferTable]
    public class LinkedListNode
    {
        [FlatBufferItem(0)]
        public virtual LinkedListNode? Next { get; set; }

        [FlatBufferItem(1)]
        public virtual int Value { get; set; }
    }
}