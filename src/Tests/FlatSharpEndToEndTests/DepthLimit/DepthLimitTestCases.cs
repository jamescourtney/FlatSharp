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

namespace FlatSharpEndToEndTests.DepthLimit;

public class DepthLimitTestCases
{
    [Fact]
    public void GreedyLinkedList()
    {
        GreedyLinkedListNode head = new GreedyLinkedListNode { Value = 0 };
        GreedyLinkedListNode current = head;

        for (int i = 0; i < 999; ++i)
        {
            var node = new GreedyLinkedListNode { Value = i + 1 };
            current.Next = node;
            current = node;
        }

        byte[] buffer = new byte[10 * 1024 * 1024];
        GreedyLinkedListNode.Serializer.Write(buffer, head);

        var serializer = GreedyLinkedListNode.Serializer.WithSettings(new SerializerSettings { ObjectDepthLimit = 999 });

        var result = serializer.Parse(buffer);
    }

    [Fact]
    public void LazyLinkedList()
    {
        LazyLinkedListNode head = new LazyLinkedListNode { Value = 0 };
        LazyLinkedListNode current = head;

        for (int i = 0; i < 999; ++i)
        {
            var node = new LazyLinkedListNode { Value = i + 1 };
            current.Next = node;
            current = node;
        }

        byte[] buffer = new byte[10 * 1024 * 1024];
        LazyLinkedListNode.Serializer.Write(buffer, head);

        var serializer = LazyLinkedListNode.Serializer.WithSettings(new SerializerSettings { ObjectDepthLimit = 999 });

        var result = serializer.Parse(buffer);

        while (result.Next is not null)
        {
            result = result.Next;
        }
    }
}
