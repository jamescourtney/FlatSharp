using FlatSharp;
using Google.FlatBuffers;
using System;
using System.IO;

namespace Benchmark.FBBench;

internal class GoogleHelper 
{
    private static FlatBufferBuilder builder = new FlatBufferBuilder(1024 * 1024);
    private static ByteBuffer readBuffer;
    private static Offset<GFB.FooBarContainer> offset;
    private static GFB.FooBarContainerT container;
    private static string[] randomStrings;
    private static int[] randomInts;

    public static int Prepare(int length)
    {
        BenchmarkUtilities.Prepare(length, out container);

        int offset = Serialize();

        byte[] sizedArray = builder.SizedByteArray();
        readBuffer = new(sizedArray);

        randomStrings = new string[length];
        randomInts = new int[length];
        
        for (int i = 0; i < length; ++i)
        {
            randomStrings[i] = Guid.NewGuid().ToString();
            randomInts[i] = Random.Shared.Next();
        }

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
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer);

        int sum = 0;
        for (int i = 0; i < iterations; ++i)
        {
            sum += parsed.Initialized ? 1 : 0;
            sum += parsed.Location.Length;
            sum += parsed.Fruit;

            int count = parsed.ListLength;
            for (int j = 0; j < count; ++j)
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

    public static int ParseAndTraversePartial(int iterations)
    {
        var parsed = GFB.FooBarContainer.GetRootAsFooBarContainer(readBuffer);

        int sum = 0;
        for (int i = 0; i < iterations; ++i)
        {
            sum += parsed.Initialized ? 1 : 0;
            sum += parsed.Location.Length;
            sum += parsed.Fruit;

            int count = parsed.ListLength;
            for (int j = 0; j < count; ++j)
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

    public static int SerializeSortedStringVector()
    {
        var fbb = builder;
        fbb.Clear();

        var offsets = CreateSortedStringVectorOffsets(fbb);
        var vectorOffset = GFB.SortedVectorStringKey.CreateSortedVectorOfSortedVectorStringKey(fbb, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(fbb, StringVectorOffset: vectorOffset);
        fbb.Finish(tableOffset.Value);


        return fbb.Offset;
    }

    public static int SerializeUnsortedStringVector()
    {
        var fbb = builder;
        fbb.Clear();

        var offsets = CreateSortedStringVectorOffsets(fbb);
        var vectorOffset = GFB.SortedVectorContainer.CreateStringVectorVector(fbb, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(fbb, StringVectorOffset: vectorOffset);
        fbb.Finish(tableOffset.Value);

        return fbb.Offset;
    }

    private static Offset<GFB.SortedVectorStringKey>[] CreateSortedStringVectorOffsets(FlatBufferBuilder builder)
    {
        var stringVector = randomStrings;
        int stringVectorLength = stringVector.Length;

        var offsets = new Offset<GFB.SortedVectorStringKey>[stringVectorLength];
        for (int i = 0; i < stringVectorLength; ++i)
        {
            offsets[i] = GFB.SortedVectorStringKey.CreateSortedVectorStringKey(
                builder,
                builder.CreateString(stringVector[i]));
        }

        return offsets;
    }

    public static int SerializeSortedIntVector()
    {
        var fbb = builder;
        fbb.Clear();

        var offsets = CreateSortedIntVectorOffsets(fbb);
        var vectorOffset = GFB.SortedVectorIntKey.CreateSortedVectorOfSortedVectorIntKey(builder, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(fbb, IntVectorOffset: vectorOffset);
        fbb.Finish(tableOffset.Value);

        return fbb.Offset;
    }

    public static int SerializeUnsortedIntVector()
    {
        var fbb = builder;
        fbb.Clear();

        var offsets = CreateSortedIntVectorOffsets(fbb);
        var vectorOffset = GFB.SortedVectorContainer.CreateIntVectorVector(fbb, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(fbb, IntVectorOffset: vectorOffset);
        fbb.Finish(tableOffset.Value);

        return fbb.Offset;
    }

    private static Offset<GFB.SortedVectorIntKey>[] CreateSortedIntVectorOffsets(FlatBufferBuilder builder)
    {
        var intVector = randomInts;
        int intVectorLength = intVector.Length;

        var offsets = new Offset<GFB.SortedVectorIntKey>[intVectorLength];
        for (int i = 0; i < intVectorLength; ++i)
        {
            offsets[i] = GFB.SortedVectorIntKey.CreateSortedVectorIntKey(
                builder,
                intVector[i]);
        }

        return offsets;
    }
}
