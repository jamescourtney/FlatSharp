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
    public void Run()
    {
        LotsOfLists lists = new()
        {
            // IList<string>
            ListVectorOfString = new List<string>
            {
                "Pig",
                "Cow",
                "Goat",
            },

            // IReadOnlyList<int>
            ReadOnlyListVectorOfString = new int[1] { 42 },

            // IList<Animal>
            ListOfUnion = new List<Animal>
            {
                new Animal(new Eagle { Age = 12, WingspanCm = 157 }),
                new Animal(new Dog { Name = "Rocket" }),
                new Animal(new Fish { FlipperCount = 4 })
            },

            // Memory<byte>?
            VectorOfUbyte = Guid.NewGuid().ToByteArray().AsMemory(),

            // ReadOnlyMemory<byte>?
            ReadOnlyVectorOfUbyte = Guid.NewGuid().ToByteArray().AsMemory(),

            // IList<KeyValuePair>
            // Sorted vectors are sorted when the buffer is serialized. There is no need to sort them yourself.
            // Sorted vectors are sorted by their key and can be binary searched after deserializing.
            SortedListOfTable = new KeyValuePair[]
            {
                new() { Key = "Pig", Value = "Wilbur" },
                new() { Key = "Deer", Value = "Bambi" },
                new() { Key = "Lion", Value = "Simba" },
                new() { Key = "Snowman", Value = "Olaf" },
            },
        };

        byte[] buffer = new byte[LotsOfLists.Serializer.GetMaxSize(lists)];
        int bytesWritten = LotsOfLists.Serializer.Write(buffer, lists);

        LotsOfLists parsedValue = LotsOfLists.Serializer.Parse(buffer);
    }
}