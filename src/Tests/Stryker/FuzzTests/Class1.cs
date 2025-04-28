namespace FuzzTests;

using FlatSharp;
using FlatSharpStrykerTests;
using SharpFuzz;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        if (args[0] == "GenerateData")
        {
            Root root = Root.CreateTestRoot<SimpleListFactory>();
            byte[] temp = new byte[Root.Serializer.GetMaxSize(root)];
            int bytesWritten = Root.Serializer.Write(temp, root);

            Directory.CreateDirectory("TestCases");
            File.WriteAllBytes("TestCases/TestCase.bin", temp.AsSpan()[..bytesWritten]);
        }
        else
        {
            FlatBufferDeserializationOption option = Enum.Parse<FlatBufferDeserializationOption>(args[0]);

            Fuzzer.OutOfProcess.Run(stream =>
            {
                using MemoryStream ms = new();
                stream.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                var clone = new Root(Root.Serializer.Parse(bytes, option));
            });
        }
    }

    private class SimpleListFactory : ICreateCreateListFactory
    {
        public static CreateListCallback<T> GetCallback<T>() => items => new List<T>(items);
    }
}
