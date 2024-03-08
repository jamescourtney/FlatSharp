using FlatSharp;
using Google.FlatBuffers;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class GoogleObjectApiHelper
{
    private static FlatBufferBuilder builder = new FlatBufferBuilder(1024 * 1024);
    private static ByteBuffer readBuffer;
    private static Offset<GFB.FooBarContainer> offset;
    private static GFB.FooBarContainerT container;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        int offset = Serialize();
        byte[] sizedArray = builder.SizedByteArray();
        readBuffer = new(sizedArray);

        return sizedArray.Length;
    }

    public static int Serialize()
    {
        var fbb = builder;
        fbb.Clear();
        offset = GFB.FooBarContainer.Pack(fbb, container);
        fbb.Finish(offset.Value);
        return fbb.Offset;
    }

    public static int ParseAndTraverse(int iterations)
    {
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer).UnPack();
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer).UnPack();
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }
}
