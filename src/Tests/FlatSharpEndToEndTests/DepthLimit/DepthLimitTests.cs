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

[TestClass]
public class DepthLimitTests
{
    [TestMethod]
    public void DepthLimit_Negative()
    {
        var ex = Assert.ThrowsException<ArgumentException>(
            () => LLNode.Serializer.WithSettings(opt => opt.WithObjectDepthLimit(-1)));

        Assert.IsTrue(ex.Message.Contains("ObjectDepthLimit must be nonnegative."));
    }


    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void DepthLimit_OneOver(FlatBufferDeserializationOption option)
    {
        const int Limit = 50;

        var serializer = LLNode.Serializer.WithSettings(opt => opt.UseDeserializationMode(option).WithObjectDepthLimit(Limit));

        LLNode head = CreateList(Limit + 1);

        var ex = Assert.ThrowsException<InvalidDataException>(
            () => ParseAndTraverseList(head, serializer));

        Assert.IsTrue(ex.Message.Contains("FlatSharp passed the configured depth limit when deserializing."));
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void DepthLimit_Exact(FlatBufferDeserializationOption option)
    {
        const int Limit = 50;

        var serializer = LLNode.Serializer.WithSettings(opt => opt.UseDeserializationMode(option).WithObjectDepthLimit(Limit));

        LLNode head = CreateList(Limit);
        ParseAndTraverseList(head, serializer);
    }

    private static void ParseAndTraverseList(LLNode head, ISerializer<LLNode> serializer)
    {
        LLNode current = head.SerializeAndParse(serializer);
        while (current != null)
        {
            current = current.Next;
        }
    }

    private static LLNode CreateList(int length)
    {
        LLNode head = new() { Value = 0 };
        LLNode current = head;

        for (int i = 1; i < length; ++i)
        {
            LLNode next = new() { Value = i };
            current.Next = next;
            current = next;
        }

        return head;
    }
}