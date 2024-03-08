using System;
using System.IO;

namespace Benchmark.FBBench;

#if !AOT

internal class PbdnHelper
{
    private static MemoryStream outputBuffer = new MemoryStream(1024 * 1024);
    private static byte[] inputBuffer;
    private static ReflectionBased.FooBarListContainer container;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        int bytesWritten = Serialize();

        inputBuffer = new byte[bytesWritten];
        Array.Copy(outputBuffer.GetBuffer(), inputBuffer, bytesWritten);

        return bytesWritten;
    }

    public static int Serialize()
    {
        MemoryStream ms = outputBuffer;
        ms.Position = 0;
        ProtoBuf.Serializer.Serialize(outputBuffer, container);

        return (int)ms.Position;
    }

    public static int ParseAndTraverse(int iterations)
    {
        var parsed = ProtoBuf.Serializer.Deserialize<ReflectionBased.FooBarListContainer>(inputBuffer.AsSpan());
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        var parsed = ProtoBuf.Serializer.Deserialize<ReflectionBased.FooBarListContainer>(inputBuffer.AsSpan());
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }
}

#endif