/*
 * Copyright 2020 James Courtney
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

namespace Samples.Vectors;

public class VectorsSample : IFlatSharpSample
{
    public bool HasConsoleOutput => false;

    public void Run()
    {
        LotsOfLists table = new()
        {
            // IList<T>. You can use anything implementing IList, even arrays!
            // This vector contains other tables. Those tables contain a vector themselves.
            ListVectorOfTable = new[]
            {
                new SimpleTable { A = 0, InnerVector = new[] { "A", "B", "C" } },
                new SimpleTable { A = 1, InnerVector = new[] { "D", "E", "F" } },
            },

            // Vectors can also contain unions. Vectors of unions
            // are a newer FlatBuffers feature.
            ListVectorOfUnion = new List<SimpleUnion>()
            {
                new SimpleUnion(new SimpleTable { A = 2, InnerVector = new[] { "G", "H", "I", } }),
                new SimpleUnion(new SimpleStruct { A = 3, B = 3.14f, }),
            },

            // This vector is IReadOnlyList<T>.
            ReadOnlyListVectorOfStruct = new[]
            {
                new SimpleStruct { A = 4, B = (float)Math.E },
            },

            /// Memory<byte>. Memory<byte> Vectors are special because when you use lazy
            /// or progressive parsing, they reference a literal chunk of the underlying
            /// <see cref="IInputBuffer"/>.
            VectorOfUbyte = new byte[] { 1, 2, 3, 4, 5, },

            /// ReadOnlyMemory<byte> is also supported if you want to prevent
            /// changes to the underlying <see cref="IInputBuffer"/>.
            ReadOnlyVectorOfUbyte = new byte[] { 6, 7, 8, 9, 10 },
        };

        Memory<byte> buffer = new byte[LotsOfLists.Serializer.GetMaxSize(table)];
        int bytesWritten = LotsOfLists.Serializer.Write(buffer, table);

        // Trim the buffer before we try parsing.
        buffer = buffer[..bytesWritten];

        this.UseLazy(buffer);
        this.UseProgressive(buffer);
        this.UseGreedy(buffer);
        this.UseGreedyMutable(buffer);
    }

    private void UseLazy(Memory<byte> serialized)
    {
        // Parse the buffer lazily
        LotsOfLists lazyParsed = LotsOfLists.Serializer.Parse(serialized, FlatBufferDeserializationOption.Lazy);

        {
            IList<SimpleTable>? vectorOfTable = lazyParsed.ListVectorOfTable;
            Debug.Assert(vectorOfTable is not null);

            Type actualType = vectorOfTable.GetType();

            // Lazy Vectors return a different instance each time:
            SimpleTable? first = vectorOfTable[0];
            SimpleTable? second = vectorOfTable[0];
            Assert.NotSameObject(first, second, "Lazy vectors return a different object each time.");
        }

        {
            Memory<byte> vectorOfByte = lazyParsed.VectorOfUbyte!.Value;

            // The vector overlaps the original memory. No copies are made.
            Assert.True(vectorOfByte.Span.Overlaps(serialized.Span), "The parsed memory vector overlaps the original data.");
        }
    }

    private void UseProgressive(Memory<byte> serialized)
    {
        // Parse the buffer using progressive mode
        LotsOfLists progressiveParsed = LotsOfLists.Serializer.Parse(serialized, FlatBufferDeserializationOption.Progressive);

        {
            IList<SimpleTable>? vectorOfTable = progressiveParsed.ListVectorOfTable;
            Debug.Assert(vectorOfTable is not null);

            Type actualType = vectorOfTable.GetType();

            // Progressive vectors return the same instance each time.
            SimpleTable? first = vectorOfTable[0];
            SimpleTable? second = vectorOfTable[0];
            Assert.SameObject(first, second, "First and second are the same reference.");
        }

        {
            Memory<byte> vectorOfByte = progressiveParsed.VectorOfUbyte!.Value;

            // The vector overlaps the original memory. No copies are made.
            Assert.True(
                vectorOfByte.Span.Overlaps(serialized.Span),
                "The parsed memory vector overlaps the original data.");
        }
    }

    private void UseGreedy(Memory<byte> serialized)
    {
        // Parse the buffer greedily
        LotsOfLists greedyParsed = LotsOfLists.Serializer.Parse(serialized, FlatBufferDeserializationOption.Greedy);

        {
            IList<SimpleTable>? vectorOfTable = greedyParsed.ListVectorOfTable!;

            Type actualType = vectorOfTable.GetType();

            // Greedy Vectors return a different instance each time:
            SimpleTable? first = vectorOfTable[0];
            SimpleTable? second = vectorOfTable[0];
            Assert.SameObject(first, second, "First and second are the same reference.");
        }

        {
            Memory<byte> vectorOfByte = greedyParsed.VectorOfUbyte!.Value;

            // The vector does not overlap the original memory, meaning a copy is made.
            Assert.True(
                !vectorOfByte.Span.Overlaps(serialized.Span),
                "The memory vector does not overlap the original data.");
        }
    }

    private void UseGreedyMutable(Memory<byte> serialized)
    {
        // Parse the buffer using GreedyMutable mode
        LotsOfLists greedyParsed = LotsOfLists.Serializer.Parse(serialized, FlatBufferDeserializationOption.GreedyMutable);

        {
            IList<SimpleTable>? vectorOfTable = greedyParsed.ListVectorOfTable!;

            Type actualType = vectorOfTable.GetType();

            // GreedyMutable Vectors return the same instance each time:
            SimpleTable? first = vectorOfTable[0];
            SimpleTable? second = vectorOfTable[0];
            Assert.SameObject(first, second, "First and second are the same reference.");
        }

        {
            Memory<byte> vectorOfByte = greedyParsed.VectorOfUbyte!.Value;

            // The vector does not overlap the original memory, meaning a copy is made.
            Assert.True(
                !vectorOfByte.Span.Overlaps(serialized.Span),
                "The memory vector does not overlap the original data.");
        }
    }

}