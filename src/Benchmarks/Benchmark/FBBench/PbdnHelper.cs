using MessagePack;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class PbdnHelper
{
    private static MemoryStream buffer = new MemoryStream(1024 * 1024);
    private static RefelctionBased.FooBarListContainer container;

    public static void Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        Serialize();
    }

    public static int Serialize()
    {
        MemoryStream ms = buffer;
        ms.Position = 0;
        ProtoBuf.Serializer.Serialize(buffer, container);

        return (int)ms.Position;
    }

    public static int ParseAndTraverse(int iterations)
    {
        MemoryStream ms = buffer;
        ms.Position = 0;
        ProtoBuf.Serializer.Deserialize<RefelctionBased.FooBarListContainer>(ms.GetBuffer().AsSpan());

        return BenchmarkUtilities.TraverseFooBarContainer(container, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        MemoryStream ms = buffer;
        ms.Position = 0;
        ProtoBuf.Serializer.Deserialize<RefelctionBased.FooBarListContainer>(ms.GetBuffer().AsSpan());

        return BenchmarkUtilities.TraverseFooBarContainerPartial(container, iterations);
    }
}
