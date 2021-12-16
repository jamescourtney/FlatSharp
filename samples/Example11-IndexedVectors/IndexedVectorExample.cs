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
/// Indexed Vectors have all of the limitations of sorted vectors (only strings and scalars supported as keys). They can be declared both
/// in C# code (below) and in FBS files (IndexedVectors.fbs).
/// </summary>
public class IndexedVectorsExample
{
    public static void Run()
    {
        IndexedVectorTable table = new IndexedVectorTable
        {
            Users = new IndexedVector<string, User>
            {
                { new User("1") { FirstName = "Charlie", LastName = "Kelly" } },
                { new User("2") { FirstName = "Dennis", LastName = "Reynolds" } },
                { new User("3") { FirstName = "Ronald", LastName = "McDonald" } },
                { new User("4") { FirstName = "Frank", LastName = "Reynolds" } },
                { new User("5") { FirstName = "Deeandra", LastName = "Reynolds" } },
            }
        };

        // Indexed vectors look and act like dictionaries. The main difference
        // is that adding arbitrary keys is not supported.
        Debug.Assert(table.Users.TryGetValue("5", out var bird));
        Debug.Assert(bird.UserId == "5");
        Debug.Assert(table.Users["5"].FirstName == "Deeandra");

        byte[] data = new byte[1024];
        int bytesWritten = FlatBufferSerializer.Default.Serialize(table, data);

        IndexedVectorTable parsedTable = FlatBufferSerializer.Default.Parse<IndexedVectorTable>(data);

        Debug.Assert(parsedTable.Users is not null);

        // Performs binary search
        Debug.Assert(parsedTable.Users.TryGetValue("2", out var dennis));
        Debug.Assert(parsedTable.Users.TryGetValue("3", out var mac));
        Debug.Assert(parsedTable.Users.TryGetValue("1", out var charlie));
        Debug.Assert(parsedTable.Users.TryGetValue("5", out bird));
        Debug.Assert(parsedTable.Users.TryGetValue("4", out var frank));
    }

    [FlatBufferTable]
    public class IndexedVectorTable
    {
        /// <summary>
        /// Indexed vectors have two generic parameters: key type and value type. The type of the key must match 
        /// the "key" in the value.
        /// </summary>
        [FlatBufferItem(0)]
        public virtual IIndexedVector<string, User>? Users { get; set; }
    }

    [FlatBufferTable]
    public class User
    {
        public User(string id)
        {
            this.UserId = id;
        }

        /// <summary>
        /// This constructor is used by Flatsharp when it is defined.
        /// The context parameter gives a little information about the deserialization,
        /// but it is not required to do anything with it.
        /// </summary>
        protected User(FlatBufferDeserializationContext context)
        {
        }

        /// <summary>
        /// This is the key. The type here (string) matches the key in the IIndexedVector up above.
        /// It is recommended to keep key properties immutable when using Indexed Vectors.
        /// </summary>
        [FlatBufferItem(0, Key = true)]
        public virtual string? UserId { get; init; }

        [FlatBufferItem(1)]
        public virtual string? FirstName { get; set; }

        [FlatBufferItem(2)]
        public virtual string? LastName { get; set; }
    }
}
