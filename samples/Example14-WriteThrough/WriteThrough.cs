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

namespace Samples.WriteThrough
{
    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using FlatSharp;

    /// <summary>
    /// Write Through allows you to make updates to an already-serialized FlatBuffer in-place without a full parse or re-serialize.
    /// This is extremely performant, especially for large buffers as it avoids series of copies and allows in-place updates.
    /// 
    /// FlatSharp supports Write-Through in limited cases:
    /// 
    /// For reference structs, Write-Through is supported on fields inside the struct.
    /// - Serialization method is Progressive or Lazy.
    /// - Struct field has been opted into write-through.
    /// - Struct field is virtual.
    /// 
    /// For value structs, Write-Through is supported when:
    /// - Serialization method is Progressive or Lazy.
    /// - Table field is required and enabled for write through.
    /// - Vector field is enabled for write through.
    /// </summary>
    public class WriteThroughSample
    {
        public static void Run()
        {
            ReferenceStructWriteThrough();
            ValueStructWriteThrough();
        }

        /// <summary>
        /// Write through using reference structs. In this case, we use a Bloom Filter.
        /// </summary>
        public static void ReferenceStructWriteThrough()
        {
            byte[] rawData;

            {
                // Each block is 128 bytes. For a 1MB filter, we need 8192 blocks.
                BloomFilter filter = new BloomFilter(1024 * 1024 / 128);

                // Write our bloom filter out to an array. It should be about 1MB in size.
                rawData = new byte[BloomFilter.Serializer.GetMaxSize(filter)];
                BloomFilter.Serializer.Write(rawData, filter);
            }

            // This bloom filter will let us do in-place updates to the 'rawData' array,
            // eliminating the need to re-serialize on each update. If you want extra credit,
            // consider using memory mapped files here to automatically flush to disk!
            BloomFilter writeThroughFilter = BloomFilter.Serializer.Parse(rawData);

            // Add a bunch of random keys to our bloom filter. Keep in mind that we're
            // doing this *in place*. No Parse->Update->Re-serialize here. All of these operations
            // are happening directly into the 'rawData' array up above.
            List<string> keysInFilter = new List<string>();
            for (int i = 0; i < 25_000; ++i)
            {
                string key = Guid.NewGuid().ToString();
                keysInFilter.Add(key);

                Debug.Assert(!writeThroughFilter.MightContain(key), "Filter shouldn't contain any keys yet");
                writeThroughFilter.Add(key);
                Debug.Assert(writeThroughFilter.MightContain(key), "Filter should contain that key now.");
            }

            // Let's prove the writethrough worked now. Let's re-parse the data and verify that our keys
            // are still there!
            writeThroughFilter = BloomFilter.Serializer.Parse(rawData);
            foreach (var key in keysInFilter)
            {
                Debug.Assert(writeThroughFilter.MightContain(key), "Filter still contains the key! WriteThrough worked :)");
            }

            // For some fun, we can also test some keys that aren't in there.
            for (int i = 0; i < 1000; ++i)
            {
                Debug.Assert(!writeThroughFilter.MightContain(Guid.NewGuid().ToString()));
            }
        }

        /// <summary>
        /// Shows value struct write through. In this case, we define a path and add points to it.
        /// </summary>
        public static void ValueStructWriteThrough()
        {
            byte[] data;

            {
                // Build our "empty" path with capacity for 10K points.
                // Note that the vector has 10,000 items in it, but we
                // track the "Used" length ourselves. This is because
                // writethrough cannot add to the end of a list. You can
                // only update items that are already in the list.
                Path path = new Path()
                {
                    NumPoints = new MutableInt { Value = 0 },
                    Points = new List<Point>(),
                };

                // Give capacity for 10,000 points.
                for (int i = 0; i < 10000; ++i)
                {
                    path.Points.Add(new Point());
                }

                data = new byte[Path.Serializer.GetMaxSize(path)];
                Path.Serializer.Write(data, path);
            }

            Path parsed = Path.Serializer.Parse(data);
            
            // Add 100 points.
            for (int i = 0; i < 100; ++i)
            {
                int next = parsed.NumPoints.Value;

                // Update the vector and the length. Both of these write through to the underlying buffer.
                parsed.Points![next] = new Point { X = i, Y = i, Z = i };

                // This is inefficient -- it would be better to write the new length once after the end of the loop.
                parsed.NumPoints = new MutableInt { Value = next + 1 };
            }

            // Reparse the buffer to show that we actually wrote some points.
            parsed = Path.Serializer.Parse(data);
            Debug.Assert(parsed.NumPoints.Value == 100, "100 points, right?");
            
            // Print the last point we wrote, followed by the next empty slot in the vector.
            for (int i = 99; i <= 100; ++i)
            {
                Point point = parsed.Points![i];
                Console.WriteLine($"Point {i}: ({point.X}, {point.Y}, {point.Z})");
            }
        }
    }

    /// <summary>
    /// FlatSharp generates the data definitions. We are adding the methods in this partial declaration.
    /// </summary>
    public partial class BloomFilter
    {
        // Bloom filters use multiple hash functions to figure out which bits to set.
        // These were chosen arbitrarily by what was available in .NET. Ideally, you'd
        // use murmur3 or something appropriate for this use case :)
        private readonly HashAlgorithm[] hashAlgorithms = new HashAlgorithm[] { SHA256Managed.Create(), SHA1Managed.Create(), MD5.Create() };

        public BloomFilter(int blockCount)
        {
            this.Blocks = new List<Block>(blockCount);
            for (int i = 0; i < blockCount; ++i)
            {
                this.Blocks.Add(new Block());
            }
        }

        public int BlockCount => this.Blocks.Count;

        public bool MightContain(string key)
        {
            byte[] keyBytes = this.GetKeyBytes(key);

            for (int i = 0; i < this.hashAlgorithms.Length; ++i)
            {
                this.GetBlockAddress(keyBytes, this.hashAlgorithms[i], out Block block, out int blockIndex, out ulong blockMask);

                if ((block.Data[blockIndex] & blockMask) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Add(string key)
        {
            byte[] keyBytes = this.GetKeyBytes(key);

            for (int i = 0; i < this.hashAlgorithms.Length; ++i)
            {
                this.GetBlockAddress(keyBytes, this.hashAlgorithms[i], out Block block, out int blockIndex, out ulong blockMask);
                block.Data[blockIndex] |= blockMask;
            }

            return true;
        }

        private byte[] GetKeyBytes(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }

        private void GetBlockAddress(byte[] key, HashAlgorithm hashAlgorithm, out Block block, out int blockIndex, out ulong blockMask)
        {
            byte[] hash = hashAlgorithm.ComputeHash(key);

            // Use the hash to get the address of the bit we're looking for.
            // First 4 bytes finds the block.
            int hash1 = BinaryPrimitives.ReadInt32LittleEndian(hash) & int.MaxValue; // positive-valued int32 hash.

            // Second 4 bytes finds the offset within the block.
            int hash2 = BinaryPrimitives.ReadInt32LittleEndian(hash.AsSpan().Slice(4)) & int.MaxValue;

            // Third 4 bytes finds the bit mask within the ulong.
            int hash3 = BinaryPrimitives.ReadInt32LittleEndian(hash.AsSpan().Slice(8)) & int.MaxValue;

            block = this.Blocks[hash1 % this.BlockCount];
            blockIndex = hash2 % block.Data.Count;

            int positionInBlock = hash3 % 64;
            blockMask = ((ulong)1) << positionInBlock;
        }
    }
}
