using FlatSharp;
using MessagePack;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class FlatSharpHelper 
{
    private static byte[] buffer;
    private static FS.FooBarContainer container;

    public static void Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        buffer = new byte[GetMaxSize()];
        Serialize();
    }

    public static int GetMaxSize()
    {
        return FS.FooBarContainer.Serializer.GetMaxSize(container);
    }

    public static int Serialize()
    {
        return FS.FooBarContainer.Serializer.Write(new SpanWriter(), buffer, container);
    }

    public static int ParseAndTraverse(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = FS.FooBarContainer.Serializer.Parse(buffer, option);
        return BenchmarkUtilities.TraverseFooBarContainer(container, iterations);
    }

    public static int ParseAndTraversePartial(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = FS.FooBarContainer.Serializer.Parse(buffer, option);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(container, iterations);
    }
}
