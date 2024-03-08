
namespace Benchmark.FBBench;

#if !AOT

using MessagePack;

internal class MsgPackHelper
{
    private static byte[] buffer;
    private static ReflectionBased.FooBarListContainer container;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        return Serialize();
    }

    public static int Serialize()
    {
        buffer = MessagePackSerializer.Serialize(container);
        return buffer.Length;
    }

    public static int ParseAndTraverse(int iterations)
    {
        ReflectionBased.FooBarListContainer parsed = MessagePackSerializer.Deserialize<ReflectionBased.FooBarListContainer>(buffer);
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations)
    {
        ReflectionBased.FooBarListContainer parsed = MessagePackSerializer.Deserialize<ReflectionBased.FooBarListContainer>(buffer);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }
}

#endif