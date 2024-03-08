using FlatSharp;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class FlatSharpValueStructs 
{
    private static byte[] buffer;
    private static FS.FooBarContainerValue container;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        buffer = new byte[GetMaxSize()];
        return Serialize();
    }

    public static int GetMaxSize()
    {
        return FS.FooBarContainerValue.Serializer.GetMaxSize(container);
    }

    public static int Serialize()
    {
        return FS.FooBarContainerValue.Serializer.Write(new SpanWriter(), buffer, container);
    }

    public static int ParseAndTraverse(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = FS.FooBarContainerValue.Serializer.Parse(buffer, option);
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = FS.FooBarContainerValue.Serializer.Parse(buffer, option);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }
}
