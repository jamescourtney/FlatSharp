using FlatSharp;
using System.Diagnostics;
using System.Text;

namespace NativeAot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IndexedVector<string, KeyValuePair> indexedVector = new();
            for (int i = 0; i < 1000; ++i)
            {
                indexedVector.Add(new KeyValuePair { Key = Guid.NewGuid().ToString(), Value = i });
            }

            Root root = new()
            {
                IndexedVector = indexedVector,
                IntVector = new int[] { 1, 2, 3 },
                StructVector = new List<Vec3>
                {
                    new Vec3 { X = 1, Y = 2, Z = 3 },
                    new Vec3 { X = 4, Y = 5, Z = 6 },
                    new Vec3 { X = 7, Y = 8, Z = 9 },
                }
            };

            int maxSize = Root.Serializer.GetMaxSize(root);
            Console.WriteLine("Max size: " + maxSize);

            byte[] buffer = new byte[maxSize];
            int bytesWritten = 0;

            for (int i = 0; i < 10; ++i)
            {
                bytesWritten = Root.Serializer.Write(buffer, root);
            }

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
            {
                bytesWritten = Root.Serializer.Write(buffer, root);
            }
            sw.Stop();

            Console.WriteLine($"Serialization complete. Bytes written = {bytesWritten}. TotalTime = {sw.Elapsed.TotalMicroseconds:N0}us");
            Console.WriteLine();

            foreach (var option in Enum.GetValues<FlatBufferDeserializationOption>())
            {
                Traverse<ArrayInputBuffer>(root, new(buffer), option);
                Traverse<MemoryInputBuffer>(root, new(buffer), option);
                Traverse<ReadOnlyMemoryInputBuffer>(root, new(buffer), option);
                Traverse<ArraySegmentInputBuffer>(root, new(buffer), option);
                Traverse<CustomInputBuffer>(root, new(new ArrayInputBuffer(buffer)), option);
                Console.WriteLine();
            }
        }

        public static void Traverse<TInputBuffer>(Root original, TInputBuffer buffer, FlatBufferDeserializationOption option)
            where TInputBuffer : IInputBuffer
        {
            for (int i = 0; i < 10; ++i)
            {
                ParseAndTraverse(original, buffer, option);
            }

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; ++i)
            {
                ParseAndTraverse(original, buffer, option);
            }
            sw.Stop();

            Console.WriteLine($"Parsing [ {option} ][ {typeof(TInputBuffer).Name} ]. TotalTime = {sw.Elapsed.TotalMicroseconds:N0}us");

            static void ParseAndTraverse(Root original, TInputBuffer buffer, FlatBufferDeserializationOption option)
            {
                Root parsed = Root.Serializer.Parse(buffer, option);

                for (int i = 0; i < original.IntVector.Count; ++i)
                {
                    if (original.IntVector[i] != parsed.IntVector[i])
                    {
                        throw new Exception();
                    }
                }

                foreach (var kvp in original.IndexedVector)
                {
                    string key = kvp.Key;
                    int value = kvp.Value.Value;

                    if (!parsed.IndexedVector.TryGetValue(key, out var parsedValue) || parsedValue.Value != value)
                    {
                        throw new Exception();
                    }
                }

                for (int i = 0; i < original.StructVector.Count; ++i)
                {
                    Vec3 originalItem = original.StructVector[i];
                    Vec3 parsedItem = parsed.StructVector[i];

                    if (originalItem.X != parsedItem.X || originalItem.Y != parsedItem.Y || originalItem.Z != parsedItem.Z)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        private class CustomInputBuffer : IInputBuffer
        {
            private readonly IInputBuffer inner;

            public CustomInputBuffer(IInputBuffer inner)
            {
                this.inner = inner;
            }

            public bool IsReadOnly => inner.IsReadOnly;

            public bool IsPinned => inner.IsPinned;

            public int Length => inner.Length;

            public Memory<byte> GetMemory()
            {
                return inner.GetMemory();
            }

            public ReadOnlyMemory<byte> GetReadOnlyMemory()
            {
                return inner.GetReadOnlyMemory();
            }

            public ReadOnlySpan<byte> GetReadOnlySpan()
            {
                return inner.GetReadOnlySpan();
            }

            public Span<byte> GetSpan()
            {
                return inner.GetSpan();
            }

            public byte ReadByte(int offset)
            {
                return inner.ReadByte(offset);
            }

            public double ReadDouble(int offset)
            {
                return inner.ReadDouble(offset);
            }

            public float ReadFloat(int offset)
            {
                return inner.ReadFloat(offset);
            }

            public int ReadInt(int offset)
            {
                return inner.ReadInt(offset);
            }

            public long ReadLong(int offset)
            {
                return inner.ReadLong(offset);
            }

            public sbyte ReadSByte(int offset)
            {
                return inner.ReadSByte(offset);
            }

            public short ReadShort(int offset)
            {
                return inner.ReadShort(offset);
            }

            public string ReadString(int offset, int byteLength, Encoding encoding)
            {
                return inner.ReadString(offset, byteLength, encoding);
            }

            public uint ReadUInt(int offset)
            {
                return inner.ReadUInt(offset);
            }

            public ulong ReadULong(int offset)
            {
                return inner.ReadULong(offset);
            }

            public ushort ReadUShort(int offset)
            {
                return inner.ReadUShort(offset);
            }
        }
    }
}
