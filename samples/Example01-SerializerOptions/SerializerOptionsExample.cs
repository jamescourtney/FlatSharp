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

using System.Security.Cryptography;

namespace Samples.SerializerOptions;

/// <summary>
/// This sample shows some of the different serializer options in FlatSharp and discusses the tradeoffs with them.
/// </summary>
/// <remarks>
/// FlatSharp exposes 4 core deserialization modes, in order from most greedy to laziest:
/// 1) Greedy / GreedyMutable: 
///    Pre-parse everything and release references to the underlying buffer. 
///    This is the safest option, and therefore the default. When in Greedy mode, FlatSharp behaves like other .NET serializers (JSON.NET, Protobuf, etc).
/// 
/// 2) Progressive: 
///    The FlatBuffer is read on-demand. As pieces are read, they are cached. Each logical element of the buffer will be accessed at most once.
///    Progressive is a great choice when access patterns are not predictable. It's also a great one-size-fits-most solution.
/// 
/// 3) Lazy: 
///    Nothing is ever cached, and data is reconstituted from the underlying buffer each time it is accessed. This option is fastest when you access each item
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
        Person person = new Person
        {
            Name = "James",
            FavoriteFruits = new List<Fruit>
            { 
                new Fruit { Name = "Banana", Reason = "Pretty good" },
                new Fruit { Name = "Apple", Reason = "put it in pie" },
                new Fruit { Name = "Strawberry", Reason = "Blend them up" }
            }
        };

        // In order of greediness
        LazyDeserialization(person);
        ProgressiveDeserialization(person);
        GreedyDeserialization(person);
        GreedyMutableDeserialization(person);
    }

    /// <summary>
    /// In lazy deserialization, FlatSharp reads from the underlying buffer each time. No caching is done. This will be
    /// the fastest option if your access patterns are sparse and you touch each element only once.
    /// </summary>
    public static void LazyDeserialization(Person person)
    {
        byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];
        Person.Serializer.Write(buffer, person);
        Person parsed = Person.Serializer.Parse(buffer, FlatBufferDeserializationOption.Lazy);

        // Lazy deserialization reads objects from vectors each time you ask for them.
        Fruit index0_1 = parsed.FavoriteFruits![0];
        Fruit index0_2 = parsed.FavoriteFruits[0];
        Debug.Assert(!object.ReferenceEquals(index0_1, index0_2), "A different instance is returned each time from lazy vectors");

        // Lazy mode is immutable. Writes will never succeed unless using write through.
        try
        {
            parsed.Name = "Bob";
            Debug.Assert(false); // the above will throw.
        }
        catch (NotMutableException)
        {
        }

        // Properties from tables and reference structs are different instances each time. Lazy consults the
        // buffer each time.
        string? name = parsed.Name;
        string? name2 = parsed.Name;

        Debug.Assert(
            !object.ReferenceEquals(name, name2),
            "When reading table/struct properties Lazy parsing returns a different instance each time.");

        // Invalidate the whole buffer. Undefined behavior past here!
        buffer.AsSpan().Fill(0);

        try
        {
            var whoKnows = parsed.FavoriteFruits[1];
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
    public static void ProgressiveDeserialization(Person person)
    {
        byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];
        Person.Serializer.Write(buffer, person);
        Person parsed = Person.Serializer.Parse(buffer, FlatBufferDeserializationOption.Progressive);

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
        Fruit index0_1 = parsed.FavoriteFruits![0];
        Fruit index0_2 = parsed.FavoriteFruits[0];

        Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "The same instance is also returned from vectors.");

        // Invalidate the whole buffer. Undefined behavior past here!
        buffer.AsSpan().Fill(0);

        try
        {
            var whoKnows = parsed.FavoriteFruits[1]; // we haven't accessed element 1 before, so this will lead to issues since Progressive still reads
                                                     // from the underlying buffer for things it hasn't read before.
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
    public static void GreedyDeserialization(Person person)
    {
        byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];
        Person.Serializer.Write(buffer, person);
        Person parsed = Person.Serializer.Parse(buffer, FlatBufferDeserializationOption.Greedy);

        // Fill array with 0. Source data is gone now, but we can still read the buffer because we were greedy!
        buffer.AsSpan().Fill(0);

        Fruit index0_1 = parsed.FavoriteFruits![0];
        Fruit index0_2 = parsed.FavoriteFruits[0];
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
            parsed.FavoriteFruits.Clear();
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
    public static void GreedyMutableDeserialization(Person person)
    {
        byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];
        Person.Serializer.Write(buffer, person);
        Person parsed = Person.Serializer.Parse(buffer, FlatBufferDeserializationOption.GreedyMutable);

        using SHA256 sha = SHA256.Create();

        // Get the hash of the buffer.
        byte[] hash = sha.ComputeHash(buffer);

        // Change some stuff.
        parsed.Name = "James Adams";
        parsed.FavoriteFruits!.Clear();
        parsed.FavoriteFruits.Add(new Fruit());

        byte[] hash2 = sha.ComputeHash(buffer);

        // They are the same -- greedy and greedymutable don't store a reference to the buffer.
        Debug.Assert(
            hash.AsSpan().SequenceEqual(hash2),
            "Changes to the deserialized objects are not written back to the buffer. You'll need to re-serialize it to a new buffer for that.");
    }
}