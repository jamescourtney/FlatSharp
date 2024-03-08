using Benchmark.FBBench.PB;
using FlatSharp;
using Google.FlatBuffers;
using Google.Protobuf;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class ProtobufHelper 
{
    private static byte[] outputBuffer = new byte[1024 * 1024];
    private static byte[] inputBuffer;
    private static FBBench.PB.FooBarContainer container;

    public static int Prepare(int length)
    {
        container = new()
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
        };

        for (int i = 0; i < length; ++i)
        {
            var foo = new Benchmark.FBBench.PB.Foo
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            var bar = new Benchmark.FBBench.PB.Bar
            {
                Parent = foo,
                Ratio = 3.14159f + i,
                Size = (ushort)(10000 + i),
                Time = 123456 + i
            };

            var fooBar = new Benchmark.FBBench.PB.FooBar
            {
                Name = System.Guid.NewGuid().ToString(),
                Postfix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = bar,
            };

            container.List.Add(fooBar);
        }

        int bytesWritten = Serialize();
        inputBuffer = new byte[bytesWritten];
        Array.Copy(outputBuffer, inputBuffer, bytesWritten);

        return inputBuffer.Length;
    }

    public static int Serialize()
    {
        var outputStream = new CodedOutputStream(outputBuffer);
        container.WriteTo(outputStream);
        return (int)outputStream.Position;
    }

    public static int ParseAndTraverse(int iterations)
    {
        var parsed = FooBarContainer.Parser.ParseFrom(inputBuffer);
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        var parsed = FooBarContainer.Parser.ParseFrom(inputBuffer);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }
}
