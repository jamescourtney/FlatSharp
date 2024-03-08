using Benchmark.FBBench.FS;
using FlatSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Benchmark.FBBench;

internal class FlatSharpHelper
{
    private static byte[] buffer;
    private static FS.FooBarContainer container;

    private static FS.SortedVectorContainer sortedStrings;
    private static FS.SortedVectorContainer unsortedStrings;
    private static FS.SortedVectorContainer sortedInts;
    private static FS.SortedVectorContainer unsortedInts;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);
        buffer = new byte[GetMaxSize()];
        int bytesWritten = Serialize();

        sortedStrings = new() { SortedStrings = new List<SortedVectorStringKey>() };
        unsortedStrings = new() { UnsortedStrings = new List<SortedVectorStringKey>() };
        sortedInts = new() { SortedInts = new List<SortedVectorIntKey>() };
        unsortedInts = new() { UnsortedInts = new List<SortedVectorIntKey>() };

        for (int i = 0; i < length; ++i)
        {
            string str = Guid.NewGuid().ToString();
            int randomInt = Random.Shared.Next();

            sortedStrings.SortedStrings.Add(new() { Key = str });
            unsortedStrings.UnsortedStrings.Add(new() { Key = str });
            sortedInts.SortedInts.Add(new() { Key = randomInt });
            unsortedInts.UnsortedInts.Add(new() { Key = randomInt });
        }

        return bytesWritten;
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
        return BenchmarkUtilities.TraverseFooBarContainer(parsed, iterations);
    }

    public static int ParseAndTraversePartial(int iterations, FlatBufferDeserializationOption option)
    {
        var parsed = FS.FooBarContainer.Serializer.Parse(buffer, option);
        return BenchmarkUtilities.TraverseFooBarContainerPartial(parsed, iterations);
    }

    public static int SerializeSortedStrings()
        => FS.SortedVectorContainer.Serializer.Write(new SpanWriter(), buffer, sortedStrings);

    public static int SerializeUnsortedStrings()
        => FS.SortedVectorContainer.Serializer.Write(new SpanWriter(), buffer, unsortedStrings);

    public static int SerializeSortedInts()
        => FS.SortedVectorContainer.Serializer.Write(new SpanWriter(), buffer, sortedInts);

    public static int SerializeUnsortedInts()
        => FS.SortedVectorContainer.Serializer.Write(new SpanWriter(), buffer, unsortedInts);
}
