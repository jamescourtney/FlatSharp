using FlatSharp;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace NativeAot
{
    internal class Program
    {
        static int ExitCode = 0;

        static int Main(string[] args)
        {
            Test_WriteThrough_ValueStruct();
            Test_WriteThrough_RefStruct();
            Test_IntVector();
            Test_IndexedVector();

            Console.WriteLine();
            Console.WriteLine("Benchmark -- first pass:");

            RunBenchmark();

            Console.WriteLine();
            Console.WriteLine("Benchmark -- second pass:");

            RunBenchmark();

            return ExitCode;
        }

        private static void RunBenchmark()
        {
            IndexedVector<string, KeyValuePair> indexedVector = new();
            for (int i = 0; i < 500; ++i)
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

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 200; ++i)
            {
                bytesWritten = Root.Serializer.Write(buffer, root);
            }
            sw.Stop();

            Console.WriteLine($"Serialization complete. Bytes written = {bytesWritten}. TotalTime = {sw.Elapsed.TotalMilliseconds:N0}ms");
            Console.WriteLine();

#if NETCOREAPP
            FlatBufferDeserializationOption[] options = Enum.GetValues<FlatBufferDeserializationOption>();
#else
            FlatBufferDeserializationOption[] options = (FlatBufferDeserializationOption[])Enum.GetValues(typeof(FlatBufferDeserializationOption));
#endif

            foreach (FlatBufferDeserializationOption option in options)
            {
                BenchmarkTraverse<ArrayInputBuffer>(root, new(buffer), option);
                BenchmarkTraverse<MemoryInputBuffer>(root, new(buffer), option);
                BenchmarkTraverse<ReadOnlyMemoryInputBuffer>(root, new(buffer), option);
                BenchmarkTraverse<ArraySegmentInputBuffer>(root, new(new ArraySegment<byte>(buffer)), option);
                BenchmarkTraverse<CustomInputBuffer>(root, new(new ArrayInputBuffer(buffer)), option);
                Console.WriteLine();
            }
        }

        private static void Test_IntVector()
        {
            RunTest(Test);

            static void Test(FlatBufferDeserializationOption option)
            {
                Root root = new()
                {
                    IntVector = new[] { 1, 2, 3, 4, 5, }
                };

                byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
                Root.Serializer.Write(buffer, root);

                Root parsed = Root.Serializer.Parse(buffer, option);

                Equal(5, parsed.IntVector.Count);
                Equal(1, parsed.IntVector[0]);
                Equal(2, parsed.IntVector[1]);
                Equal(3, parsed.IntVector[2]);
                Equal(4, parsed.IntVector[3]);
                Equal(5, parsed.IntVector[4]);
            }
        }

        private static void Test_IndexedVector()
        {
            RunTest(Test);

            static void Test(FlatBufferDeserializationOption option)
            {
                IndexedVector<string, KeyValuePair> sourceVector = new();
                for (int i = 0; i < 1000; ++i)
                {
                    sourceVector.Add(new() { Key = Guid.NewGuid().ToString(), Value = i });
                }

                Root root = new() { IndexedVector = sourceVector };

                byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
                Root.Serializer.Write(buffer, root);

                Root parsed = Root.Serializer.Parse(buffer, option);
                IIndexedVector<string, KeyValuePair> parsedVector = parsed.IndexedVector;

                Equal(sourceVector.Count, parsedVector.Count);

                foreach (var kvp in sourceVector)
                {
                    string key = kvp.Key;
                    KeyValuePair pair = kvp.Value;

                    Equal(true, parsedVector.ContainsKey(pair.Key));
                    Equal(true, parsedVector.TryGetValue(pair.Key, out _));
                    Equal(pair.Value, parsedVector[pair.Key].Value);
                }
            }
        }

        private static void Test_WriteThrough_ValueStruct()
        {
            RunTest(Test);

            static void Test(FlatBufferDeserializationOption option)
            {
                Root root = new()
                {
                    StructVector = new List<Vec3>
                    {
                        new() { X = 1, Y = 2, Z = 3 }
                    }
                };

                byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
                Root.Serializer.Write(buffer, root);

                Root parsed = Root.Serializer.Parse(buffer, option);
                Root parsed2 = Root.Serializer.Parse(buffer, option);

                if (option == FlatBufferDeserializationOption.Greedy || option == FlatBufferDeserializationOption.GreedyMutable)
                {
                    Throws(() => parsed.StructVector[0] = new() { X = 6, Y = 7, Z = 8 });
                    return;
                }

                parsed.StructVector[0] = new() { X = 6, Y = 7, Z = 8 };

                Equal(6, parsed.StructVector[0].X);
                Equal(7, parsed.StructVector[0].Y);
                Equal(8, parsed.StructVector[0].Z);

                Equal(6, parsed2.StructVector[0].X);
                Equal(7, parsed2.StructVector[0].Y);
                Equal(8, parsed2.StructVector[0].Z);
            }
        }

        private static void Test_WriteThrough_RefStruct()
        {
            RunTest(Test);

            static void Test(FlatBufferDeserializationOption option)
            {
                Root root = new()
                {
                    RefStruct = new() { X = 1, Y = 2, Z = 3 }
                };

                byte[] buffer = new byte[Root.Serializer.GetMaxSize(root)];
                Root.Serializer.Write(buffer, root);

                Root parsed = Root.Serializer.Parse(buffer, option);
                Root parsed2 = Root.Serializer.Parse(buffer, option);

                if (option == FlatBufferDeserializationOption.Greedy || option == FlatBufferDeserializationOption.GreedyMutable)
                {
                    Throws(() => parsed.RefStruct.X = 1);
                    Throws(() => parsed.RefStruct.Y = 1);
                    Throws(() => parsed.RefStruct.Z = 1);
                    return;
                }

                parsed.RefStruct.X = 5;
                parsed.RefStruct.Y = 6;
                parsed.RefStruct.Z = 7;

                Equal(5, parsed.RefStruct.X);
                Equal(6, parsed.RefStruct.Y);
                Equal(7, parsed.RefStruct.Z);

                Equal(5, parsed2.RefStruct.X);
                Equal(6, parsed2.RefStruct.Y);
                Equal(7, parsed2.RefStruct.Z);
            }
        }

        private static void Throws(Action action)
        {
            try
            {
                action();
                throw new Exception("Exception did not throw");
            }
            catch
            {
            }
        }

        private static void Equal<T>(T? expected, T? actual)
        {
            if (Comparer<T>.Default.Compare(expected, actual) != 0)
            {
                throw new Exception($"Assertion failed. Expected = '{expected}'. Actual = '{actual}'.");
            }
        }

        private static void RunTest(Action<FlatBufferDeserializationOption> test, [CallerMemberName] string caller = "")
        {
            Run(test, FlatBufferDeserializationOption.Lazy, caller);
            Run(test, FlatBufferDeserializationOption.Progressive, caller);
            Run(test, FlatBufferDeserializationOption.Greedy, caller);
            Run(test, FlatBufferDeserializationOption.GreedyMutable, caller);

            static void Run(Action<FlatBufferDeserializationOption> test, FlatBufferDeserializationOption option, string caller)
            {
                try
                {
                    test(option);
                    Console.WriteLine($"[Passed] {caller} ({option})");
                }
                catch (Exception ex)
                {
                    ExitCode = 1;
                    Console.WriteLine($"[Failed] {caller} ({option}): {ex.GetType().FullName} {ex.Message}");
                }
            }
        }

        public static void BenchmarkTraverse<TInputBuffer>(Root original, TInputBuffer buffer, FlatBufferDeserializationOption option)
            where TInputBuffer : IInputBuffer
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 200; ++i)
            {
                ParseAndTraverse(original, buffer, option);
            }
            sw.Stop();

            Console.WriteLine($"Parsing [ {option} ][ {typeof(TInputBuffer).Name} ]. TotalTime = {sw.Elapsed.TotalMilliseconds:N0}ms");

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
