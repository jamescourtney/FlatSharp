using FlatSharp;

namespace NativeAot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Root root = new()
            {
                IndexedVector = new IndexedVector<string, KeyValuePair>
                {
                    new KeyValuePair { Key = "a", Value = 0 },
                    new KeyValuePair { Key = "b", Value = 1 },
                    new KeyValuePair { Key = "c", Value = 2 },
                },
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
            int bytesWritten = Root.Serializer.Write(buffer, root);

            Console.WriteLine("Serialization complete. Bytes written = " + bytesWritten);

            foreach (var option in Enum.GetValues<FlatBufferDeserializationOption>())
            {
                Traverse<ArrayInputBuffer>(root, new(buffer), option);
                Traverse<MemoryInputBuffer>(root, new(buffer), option);
                Traverse<ReadOnlyMemoryInputBuffer>(root, new(buffer), option);
                Traverse<ArraySegmentInputBuffer>(root, new(buffer), option);
            }
        }

        public static void Traverse<TInputBuffer>(Root original, TInputBuffer buffer, FlatBufferDeserializationOption option)
            where TInputBuffer : IInputBuffer
        {
            Console.WriteLine($"Parsing [ {option} ][ {typeof(TInputBuffer).Name} ]");

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
}
