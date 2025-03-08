using FlatSharp;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class FlatSharpValueStructs 
{
    private static byte[] buffer;
    private static FS.FooBarContainerValue container;

    public static long Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        buffer = new byte[GetMaxSize()];
        return Serialize();
    }

    public static long GetMaxSize()
    {
        return FS.FooBarContainerValue.Serializer.GetMaxSize(container);
    }

    public static long Serialize()
    {
        return FS.FooBarContainerValue.Serializer.Write(buffer, container);
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
