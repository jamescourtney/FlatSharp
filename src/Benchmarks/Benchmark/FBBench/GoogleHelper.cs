using FlatSharp;
using Google.FlatBuffers;
using MessagePack;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class GoogleHelper 
{
    private static FlatBufferBuilder builder = new FlatBufferBuilder(1024 * 1024);
    private static ByteBuffer readBuffer;
    private static Offset<GFB.FooBarContainer> offset;
    private static GFB.FooBarContainerT container;

    public static void Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        int offset = Serialize();
        readBuffer = new(builder.SizedByteArray());
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
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer);

        int sum = 0;
        for (int i = 0; i < iterations; ++i)
        {
            sum += parsed.Initialized ? 1 : 0;
            sum += parsed.Location.Length;
            sum += parsed.Fruit;

            int count = parsed.ListLength;
            for (int j = 0; j < count; ++i)
            {
                var item = parsed.List(j).Value;

                sum += item.Name.Length;
                sum += item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling.Value;
                sum += (int)bar.Ratio;
                sum += bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }

    public static int ParseAndTraversePartial(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer);

        int sum = 0;
        for (int i = 0; i < iterations; ++i)
        {
            sum += parsed.Initialized ? 1 : 0;
            sum += parsed.Location.Length;
            sum += parsed.Fruit;

            int count = parsed.ListLength;
            for (int j = 0; j < count; ++i)
            {
                var item = parsed.List(j).Value;

                sum += item.Name.Length;

                var bar = item.Sibling.Value;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }
}
