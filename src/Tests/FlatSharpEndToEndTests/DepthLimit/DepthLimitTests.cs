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

using System.IO;

namespace FlatSharpEndToEndTests.DepthLimit;

public class DepthLimitTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void DepthLimit_OneOver(FlatBufferDeserializationOption option)
    {
        const int Limit = 50;

        var serializer = LinkedListNode.Serializer.WithSettings(opt => opt.UseDeserializationMode(option).WithObjectDepthLimit(Limit));

        LinkedListNode head = CreateList(Limit + 1);

        var ex = Assert.Throws<InvalidDataException>(
            () => ParseAndTraverseList(head, serializer));

        Assert.Contains("FlatSharp passed the configured depth limit when deserializing.", ex.Message);
    }

    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void DepthLimit_Exact(FlatBufferDeserializationOption option)
    {
        const int Limit = 50;

        var serializer = LinkedListNode.Serializer.WithSettings(opt => opt.UseDeserializationMode(option).WithObjectDepthLimit(Limit));

        LinkedListNode head = CreateList(Limit);
        ParseAndTraverseList(head, serializer);
    }

    private static void ParseAndTraverseList(LinkedListNode head, ISerializer<LinkedListNode> serializer)
    {
        LinkedListNode current = head.SerializeAndParse(serializer);
        while (current != null)
        {
            current = current.Next;
        }
    }

    private static LinkedListNode CreateList(int length)
    {
        LinkedListNode head = new() { Value = 0 };
        LinkedListNode current = head;

        for (int i = 1; i < length; ++i)
        {
            LinkedListNode next = new() { Value = i };
            current.Next = next;
            current = next;
        }

        return head;
    }
}