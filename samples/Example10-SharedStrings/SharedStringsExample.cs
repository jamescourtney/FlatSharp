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

using FlatSharp.Internal;

namespace Samples.SharedStrings;

/// <summary>
/// This file shows how to use FlatSharp to provide automatic string deduplication. In this example,
/// we define a collection of rows where each value is a (Key, Value) pair. We use string deduplication
/// to share the column names so that we don't serialize the column name for each cell.
/// </summary>
public class SharedStringsExample : IFlatSharpSample
{
    public void Run()
    {
        // Create a Database of 10000 rows.
        Database Database = new Database()
        {
            Rows = Enumerable.Range(0, 10000).Select(CreateRow).ToArray(),
        };

        // Shared strings are enabled by default.
        ISerializer<Database> defaultSerializer = Database.Serializer;

        // We can create a new serializer based on the current one with shared strings turned off.
        // These factory delegates configure the writer.
        ISerializer<Database> noSharedStringsSerializer = Database.Serializer.WithSettings(s => s.DisableSharedStrings());

        // We can also create our own shared string providers (defined at the bottom of this file).
        ISerializer<Database> customSharedStringSerializer = Database.Serializer.WithSettings(s => s.UseSharedStringWriter(() => new PerfectSharedStringWriter()));

        byte[] unsharedBuffer = new byte[noSharedStringsSerializer.GetMaxSize(Database)];
        byte[] sharedBuffer = new byte[defaultSerializer.GetMaxSize(Database)];
        byte[] customBuffer = new byte[customSharedStringSerializer.GetMaxSize(Database)];

        int unsharedBytesWritten = noSharedStringsSerializer.Write(unsharedBuffer, Database);
        int defaultSharedBytesWritten = defaultSerializer.Write(sharedBuffer, Database);
        int customSharedBytesWritten = customSharedStringSerializer.Write(customBuffer, Database);

        Console.WriteLine($"Serialized size without shared strings: {unsharedBytesWritten}");

        // These will be the same since there are so few shared strings. For large numbers,
        // the custom provider will give smaller outputs while being considerably slower.
        Console.WriteLine($"Serialized size with shared strings: {defaultSharedBytesWritten}");
        Console.WriteLine($"Serialized size with custom shared strings: {customSharedBytesWritten}");
    }

    /// <summary>
    /// Creates a row with three well-defined column names and random values.
    /// </summary>
    public static Row CreateRow(int row)
    {
        return new Row()
        {
            Values = new Column[]
            {
                new Column { ColumnName = "Column" + (row++ % 500), Value = Guid.NewGuid().ToString() },
                new Column { ColumnName = "Column" + (row++ % 500), Value = Guid.NewGuid().ToString() },
                new Column { ColumnName = "Column" + (row++ % 500), Value = Guid.NewGuid().ToString() },
            }
        };
    }
}

/// <summary>
/// this is a "perfect" shared string writer implementation, which guarantees a single string is written only once.
/// this class will give optimal compression results, but will be considerably slower than FlatSharp's default implementation,
/// which uses a hashtable with flush-on-evict semantics and may write shared strings more than once.
/// </summary>
public sealed class PerfectSharedStringWriter : ISharedStringWriter
{
    private readonly Dictionary<string, List<int>> stringOffsetMap = new Dictionary<string, List<int>>();

    /// <summary>
    /// Must be true if there are any strings waiting to be flushed.
    /// </summary>
    public bool IsDirty => this.stringOffsetMap.Count > 0;

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
    public void Reset()
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
