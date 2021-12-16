/*
 * Copyright 2020 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Samples.SerializerOptions;

/// <summary>
/// This sample shows some of the different serializer options in FlatSharp and discusses the tradeoffs with them.
/// </summary>
/// <remarks>
/// FlatSharp exposes 4 core deserialization modes, in order from most greedy to laziest:
/// 1) Greedy / GreedyMutable: 
///    Pre-parse everything and release references to the underlying buffer. 
///    This is the "normal" option, and therefore the default. When in Greedy mode, FlatSharp behaves like other .NET serializers (JSON.NET, Protobuf, etc).
/// 
/// 2) Progressive: 
///    The FlatBuffer is read on-demand. As pieces are read, they are cached. Each logical element of the buffer will be accessed at most once.
///    Progressive is a great choice when access patterns are not predictable. It's also a great one-size-fits-most solution.
/// 
/// 3) Lazy: 
///    Nothing is ever cached, and data is reconstituted from the underlying buffer each time. This option is fastest when you access each item
///    no more than once, but gets expensive very quickly if you repeatedly access items.
///
/// In general, your choice of deserialization method will be informed by your answers to these 
/// questions:
/// 
/// Question 1: How complex is my object graph?
///    Greedy deserialization forces allocations of the entire object graph at once. This can put pressure on the garbage collector,
///    which may lead to surges in allocations, especially if your serialization workflow is in your program's inner loop. Using
///    a lazier approach will amortize these allocations out as you use the object, instead of putting them up front.
///    
/// Question 2: Will I read the entire object graph?
///    If you're not reading all properties of the object, then Greedy deserialization will waste cycles preparsing data you will not use.
///    If you plan to touch each field no more than once, then a lazier parsing option will be the best approach. 
///    
/// Question 3: Do I have large vectors?
///    Array allocations can be expensive, especially for large arrays. If you are deserializing large vectors, you should use some form of lazy parsing
///    (options.Lazy or options.Progressive). These options will not preallocate giant arrays.
/// 
/// The right way to handle this is to benchmark, and make your choices based on that. What performs best depends on your access patterns. Objectively,
/// all of these configurations are quite fast.
/// </remarks>
public class SerializerOptionsExample
{
    public static void Run()
    {
        DemoTable demo = new DemoTable
        {
            Name = "My demo table",
            ListVector = new List<InnerTable>
                {
                    new InnerTable { Fruit = "Apple" },
                    new InnerTable { Fruit = "Banana" },
                    new InnerTable { Fruit = "Pear" }
                }
        };

        // In order of greediness
        LazyDeserialization(demo);
        ProgressiveDeserialization(demo);
        GreedyDeserialization(demo);
        GreedyMutableDeserialization(demo);
    }

    /// <summary>
    /// In lazy deserialization, FlatSharp reads from the underlying buffer each time. No caching is done. This will be
    /// the fastest option if your access patterns are sparse and you touch each element only once.
    /// </summary>
    public static void LazyDeserialization(DemoTable demo)
    {
        var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Lazy));

        byte[] buffer = new byte[1024];
        serializer.Serialize(demo, buffer);
        var parsed = serializer.Parse<DemoTable>(buffer);

        // Lazy deserialization reads objects from vectors each time you ask for them.
        InnerTable index0_1 = parsed.ListVector![0];
        InnerTable index0_2 = parsed.ListVector[0];
        Debug.Assert(!object.ReferenceEquals(index0_1, index0_2), "A different instance is returned each time from lazy vectors");

        try
        {
            parsed.Name = "Bob";
            Debug.Assert(false); // the above will throw.
        }
        catch (NotMutableException)
        {
            // Lazy mode is immutable. Writes will never succeed unless using write through.
        }

        // Properties from tables and structs are cached after they are read.
        string? name = parsed.Name;
        string? name2 = parsed.Name;

        Debug.Assert(
            !object.ReferenceEquals(name, name2),
            "When reading table/struct properties Lazy parsing returns a different instance each time.");

        // Invalidate the whole buffer. Undefined behavior past here!
        buffer.AsSpan().Fill(0);

        try
        {
            var whoKnows = parsed.ListVector[1];
            Debug.Assert(false);
        }
        catch
        {
            // This can be any sort of exception. This behavior is undefined.
        }
    }

    /// <summary>
    /// The next step up in greediness is Progressive mode. In this mode, Flatsharp will cache the results of property and
    /// vector accesses. So, if you read the results of FooObject.Property1 multiple times, the same value comes back each time.
    /// </summary>
    public static void ProgressiveDeserialization(DemoTable demo)
    {
        var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Progressive));

        byte[] buffer = new byte[1024];
        serializer.Serialize(demo, buffer);
        var parsed = serializer.Parse<DemoTable>(buffer);

        try
        {
            parsed.Name = "Bob";
            Debug.Assert(false); // the above will throw.
        }
        catch (NotMutableException)
        {
            // Progressive mode is immutable. Writes will never succeed, unless using writethrough.
        }

        // Properties from tables and structs are cached after they are read.
        string? name = parsed.Name;
        string? name2 = parsed.Name;

        Debug.Assert(
            object.ReferenceEquals(name, name2),
            "When reading table/struct properties, Progressive mode returns the same instance.");

        // PropertyCache deserialization doesn't cache the results of vector lookups.
        InnerTable index0_1 = parsed.ListVector![0];
        InnerTable index0_2 = parsed.ListVector[0];

        Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "The same instances is also returned from vectors.");

        // Invalidate the whole buffer. Undefined behavior past here!
        buffer.AsSpan().Fill(0);

        try
        {
            var whoKnows = parsed.ListVector[1]; // we haven't accessed element 1 before, so this will lead to issues since Progressive still uses
                                                 // the underlying buffer.
            Debug.Assert(false);
        }
        catch
        {
            // This can be any sort of exception. This behavior is undefined.
        }
    }

    /// <summary>
    /// Greedy deserialization operates the same way that conventional serializers do. The entire buffer is traversed
    /// and the structure is copied into the deserialized object. This is the most straightforward way of using FlatSharp,
    /// because the results it gives are predictable, and require no developer cognitive overhead. However, it can be less efficient
    /// in cases where you do not need to access all data in the buffer.
    /// </summary>
    public static void GreedyDeserialization(DemoTable demo)
    {
        // Same as FlatBufferSerializer.Default
        var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy));

        byte[] buffer = new byte[1024];
        serializer.Serialize(demo, buffer);
        long originalSum = buffer.Sum(x => (long)x);

        var parsed = serializer.Parse<DemoTable>(buffer);

        // Fill array with 0. Source data is gone now, but we can still read the buffer because we were greedy!
        buffer.AsSpan().Fill(0);

        InnerTable index0_1 = parsed.ListVector![0];
        InnerTable index0_2 = parsed.ListVector[0];
        Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "Greedy deserialization returns you the same instance each time");

        // We cleared the data, but can still read the name. Greedy deserialization is easy!
        string? name = parsed.Name;

        // By default, Flatsharp will not allow mutations to properties. You can learn more about this in the mutable example below.
        try
        {
            parsed.Name = "George Washington";
            Debug.Assert(false);
        }
        catch (NotMutableException)
        {
        }

        try
        {
            parsed.ListVector.Clear();
            Debug.Assert(false);
        }
        catch (NotSupportedException)
        {
        }
    }

    /// <summary>
    /// This example shows GreedyMutable deserialization. This is exactly the same as Greedy deserialization, but setters are generated for
    /// the objects, so vectors and properties are mutable in a predictable way.
    /// </summary>
    public static void GreedyMutableDeserialization(DemoTable demo)
    {
        var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.GreedyMutable));

        byte[] buffer = new byte[1024];
        serializer.Serialize(demo, buffer);

        long originalSum = buffer.Sum(x => (long)x);

        var parsed = serializer.Parse<DemoTable>(buffer);

        parsed.Name = "James Adams";
        parsed.ListVector!.Clear();
        parsed.ListVector.Add(new InnerTable());

        long newSum = buffer.Sum(x => (long)x);
        Debug.Assert(
            newSum == originalSum,
            "Changes to the deserialized objects are not written back to the buffer. You'll need to re-serialize it to a new buffer for that.");
    }
}

[FlatBufferTable]
public class DemoTable : object
{
    [FlatBufferItem(0)]
    public virtual string? Name { get; set; }

    [FlatBufferItem(1)]
    public virtual IList<InnerTable>? ListVector { get; set; }
}

[FlatBufferTable]
public class InnerTable
{
    [FlatBufferItem(0)]
    public virtual string? Fruit { get; set; }
}
