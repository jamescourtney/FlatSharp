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

using System.Diagnostics.CodeAnalysis;

namespace Samples.IndexedVectors;

/// <summary>
/// FlatSharp provides a feature called "Indexed Vectors". Indexed Vectors are just fancy sorted vectors.
/// The difference between indexed vectors and sorted vectors is the programming model that FlatSharp exposes.
/// 
/// Indexed Vectors look and act like dictionaries for the most part, while vectors look and act like arrays. 
/// Indexed vectors are *always* sorted, even if the Sorted option is not specified.
/// 
/// The main issue with sorted vectors is that they are only sorted at serialization time. So objects that the programmer
/// constructs will not be sorted until they are serialized, which means that it is not always clear when Binary Search
/// can be used to get results:
/// 
///    myVector.BinarySearchByFlatBufferKey("foobar") // sorted or not??
///    
/// Indexed Vectors solve this problem. FlatSharp uses the IIndexedVector{TKey, TValue} interface to identify indexed vectors.
/// FlatSharp provides two implementations of this interface: IndexedVector{TKey, TValue}, which is backed by a dictionary and
/// is intended for use when creating a new object. The other implementation wraps a FlatBuffer vector and performs binary search
/// to access items. This implementation is hidden and not intended for use outside generated code.
/// 
/// Indexed Vectors have all of the limitations of sorted vectors (only strings and scalars supported as keys).
/// </summary>
public class IndexedVectorsExample : IFlatSharpSample
{
    public void Run()
    {
        IndexedVectorTable table = new IndexedVectorTable
        {
            Users = new IndexedVector<string, User>
            {
                { new User { Id = "1", FirstName = "Charlie", LastName = "Kelly" } },
                { new User { Id = "2", FirstName = "Dennis", LastName = "Reynolds" } },
                { new User { Id = "3", FirstName = "Ronald", LastName = "McDonald" } },
                { new User { Id = "4", FirstName = "Frank", LastName = "Reynolds" } },
                { new User { Id = "5", FirstName = "Deeandra", LastName = "Reynolds" } },
            }
        };

        // Indexed vectors look and act like dictionaries. The main difference
        // is that adding arbitrary keys is not supported.
        Assert.True(table.Users.TryGetValue("5", out var bird), "Found the bird!");
        Assert.True(bird.Id == "5", "The ID matches");
        Assert.True(table.Users["5"].FirstName == "Deeandra", "We can also use an indexer.");

        byte[] data = new byte[1024];
        int bytesWritten = IndexedVectorTable.Serializer.Write(data, table);

        IndexedVectorTable parsedTable = IndexedVectorTable.Serializer.Parse(data, FlatBufferDeserializationOption.Lazy);

        // After parsing, the API of indexed vectors is the same, but the underlying class is different.
        // Note that Progressive, Lazy, and Greedy will all parse IndexedVectors to different
        // concrete types.
        Assert.True(
            parsedTable.Users!.GetType().GetGenericTypeDefinition() == typeof(FlatSharp.Internal.FlatBufferIndexedVector<,>).GetGenericTypeDefinition(),
            "Parsed indexed vectors have a different implementation");

        foreach (string key in new[] { "1", "2", "3", "4", "5" })
        {
            // Performs binary search
            Assert.True(
                parsedTable.Users!.TryGetValue(key, out var person),
                "We can use an indexed vector like a dictionary");
        }
    }
}