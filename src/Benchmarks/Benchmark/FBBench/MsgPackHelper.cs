using MessagePack;

namespace Benchmark.FBBench;

internal class MsgPackHelper
{
    private static byte[] buffer;
    private static RefelctionBased.FooBarListContainer container;

    public static void Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
    }

    public static int Serialize()
    {
        buffer = MessagePackSerializer.Serialize(container);
        return buffer.Length;
    }

    public static int ParseAndTraverse(int iterations)
    {
        RefelctionBased.FooBarListContainer container = MessagePackSerializer.Deserialize<RefelctionBased.FooBarListContainer>(buffer);
        return BenchmarkUtilities.TraverseFooBarContainer(container, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        RefelctionBased.FooBarListContainer container = MessagePackSerializer.Deserialize<RefelctionBased.FooBarListContainer>(buffer);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(container, iterations);
    }
}
