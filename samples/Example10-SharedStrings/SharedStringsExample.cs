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

using FlatSharp;
using FlatSharp.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Samples.SharedStrings
{
    /// <summary>
    /// This file shows how to use FlatSharp to provide automatic string deduplication. In this example,
    /// we define a collection of rows where each value is a (Key, Value) pair. We use string deduplication
    /// to share the column names so that we don't serialize the column name for each cell.
    /// </summary>
    public class SharedStringsExample
    {
        public static void Run()
        {
            // Create a matrix of 1000 rows.
            Matrix matrix = new Matrix()
            {
                Rows = Enumerable.Range(0, 100).Select(x => CreateRow()).ToArray(),
            };

            // String deduplication is off by default.
            ISerializer<Matrix> defaultSerializer = Matrix.Serializer;

            // We can create a new serializer based on the current one with shared strings turned on.
            // These factory delegates configure the writer. FlatSharp deprecated reading shared strings 
            // in version 6.
            ISerializer<Matrix> sharedStringSerializer = Matrix.Serializer.WithSettings(
                new SerializerSettings
                {
                    // This can be null to disable shared string reading:
                    // SharedStringReaderFactory = null,
                    SharedStringWriterFactory = () => new SharedStringWriter(),
                });

            // We can also create our own shared string providers (defined at the bottom of this file).
            // These two use normal dictionaries internally.
            ISerializer<Matrix> customSharedStringSerializer = Matrix.Serializer.WithSettings(
                new SerializerSettings
                {
                    SharedStringWriterFactory = () => new PerfectSharedStringWriter(),
                });

            byte[] defaultBuffer = new byte[defaultSerializer.GetMaxSize(matrix)];
            byte[] sharedBuffer = new byte[sharedStringSerializer.GetMaxSize(matrix)];
            byte[] customBuffer = new byte[customSharedStringSerializer.GetMaxSize(matrix)];

            int defaultBytesWritten = defaultSerializer.Write(defaultBuffer, matrix);
            int sharedBytesWritten = sharedStringSerializer.Write(sharedBuffer, matrix);
            int customBytesWritten = customSharedStringSerializer.Write(customBuffer, matrix);

            Console.WriteLine($"Serialized size without shared strings: {defaultBytesWritten}");

            // These will be the same since there are so few shared strings. For large numbers,
            // the custom provider will give smaller outputs while being considerably slower.
            Console.WriteLine($"Serialized size with shared strings: {sharedBytesWritten}");
            Console.WriteLine($"Serialized size with custom shared strings: {customBytesWritten}");
        }

        /// <summary>
        /// Creates a row with three well-defined column names and random values.
        /// </summary>
        public static Row CreateRow()
        {
            return new Row()
            {
                Values = new Cell[]
                {
                    new Cell { ColumnName = "ColumnA", Value = Guid.NewGuid().ToString() },
                    new Cell { ColumnName = "ColumnB", Value = Guid.NewGuid().ToString() },
                    new Cell { ColumnName = "ColumnC", Value = Guid.NewGuid().ToString() }
                }
            };
        }
    }

    /// <summary>
    /// this is a "perfect" shared string writer implementation, which guarantees a single string is written only once.
    /// this class will give optimal compression results, but will be considerably slower than FlatSharp's default implementation,
    /// which uses a hashtable with flush-on-evict semantics and may write shared strings more than once.
    /// </summary>
    public class PerfectSharedStringWriter : ISharedStringWriter
    {
        private readonly Dictionary<string, List<int>> stringOffsetMap = new Dictionary<string, List<int>>();

        /// <summary>
        /// Called when FlatSharp has finished a serialize operation. This is the signal to flush any strings that the 
        /// string writer is hanging onto.
        /// </summary>
        public void FlushWrites<TSpanWriter>(TSpanWriter writer, Span<byte> data, SerializationContext context) where TSpanWriter : ISpanWriter
        {
            foreach (var kvp in this.stringOffsetMap)
            {
                string str = kvp.Key;
                List<int> offsets = kvp.Value;

                // Write the string.
                int stringOffset = writer.WriteAndProvisionString(data, str, context);

                // Update all the pointers that need to point to that string.
                foreach (var offset in offsets)
                {
                    writer.WriteUOffset(data, offset, stringOffset);
                }
            }
        }

        /// <summary>
        /// Prepares to write. In this case, we just need to clear the internal map for a new write operation,
        /// since the same SharedStringWriter is reused.
        /// </summary>
        public void PrepareWrite()
        {
            this.stringOffsetMap.Clear();
        }

        /// <summary>
        /// Writes a shared string by storing the string mapped to the offsets at which the string occurs in the buffer.
        /// </summary>
        public void WriteSharedString<TSpanWriter>(TSpanWriter spanWriter, Span<byte> data, int offset, string value, SerializationContext context)
            where TSpanWriter : ISpanWriter
        {
            if (!this.stringOffsetMap.TryGetValue(value, out List<int>? offsets))
            {
                offsets = new List<int>();
                this.stringOffsetMap[value] = offsets;
            }

            offsets.Add(offset);
        }
    }
}
