namespace Samples.SerializerOptions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;

    /// <summary>
    /// This sample shows some of the different serializer options in FlatSharp and discusses the tradeoffs with them.
    /// </summary>
    /// <remarks>
    /// FlatSharp exposes 4 core deserialization modes, in order from most greedy to laziest:
    /// 1) Greedy: 
    ///    Pre-parse everything and release references to the underlying buffer. 
    ///    This is the safest option, and therefore the default. It is usually not the fastest option.
    ///            
    /// 2) Mutable objects: 
    ///    Generated objects are fully mutable, but vectors are allocated ahead of time (however vector elements are not)
    ///    Imagine a buffer had a vector of 5 objects. This deserialization mode would allocate a vector of length 5,
    ///    but fill each slot with a lazily-populated FlatBuffer object. When this option is not set, then returned objects are immutable.
    ///                     
    /// 3) Vector Cache: 
    ///    Operates the same Mutable objects, but all vector items are wrapped in ReadOnlyCollection, which adds some latency.
    /// 
    /// 4) Lazy: None of these behaviors. Everything is allocated on-demand. FlatSharp never allocates an array for you.
    /// 
    /// FlatBuffer tables and structs always have their non-vector elements cached as they are accessed, so re-reading the same property returns 
    /// the same object. Vectors are special, since they can be large, and array allocations can be expensive. In general, your choice of 
    /// deserialization method will be informed by your answers to these questions:
    /// 
    /// Question 1: Am I managing the lifetime of my input buffers? 
    ///    Greedy deserialization guarantees deserialized objects hold no more reference to the input buffer, so you are free to
    ///    immediately recycle/reuse the buffer. If you are pooling or doing your own lifetime management of these objects,
    ///    then Greedy deserialization may make sense so the buffer can be immediately reused. Otherwise, you will likely
    ///    see better performance from another option.
    ///    
    /// Question 2: Will I read the entire object graph?
    ///    If you're not reading all properties of the object, then Greedy deserialization will waste cycles preparsing data you will not use.
    ///    If you plan to touch each field no more than once, then lazy parsing will be the best approach. Mutable objects
    ///    and VectorCache options are somewhere in the middle (both provide progressive caching of list vectors), the only difference
    ///    being mutability of items in the list.
    ///    
    /// Question 3: Do I have large vectors?
    ///    Array allocations can be expensive, especially for large arrays. If you are deserializing large vectors, you should use pure lazy parsing
    ///    (options.Lazy). This the only configuration that does not introduce extra array allocations for list vector types.
    /// 
    /// In order of laziest parsing to most greedy:
    /// 1) Lazy. No allocations you don't ask for. Everything is parsed on-demand.
    /// 2) VectorCache/Mutable. Vectors are allocated once you ask for them, but the items in the vector are parsed on-demand. Often the fastest option.
    /// 4) Greedy. Everything parsed and pre-allocated. Usually the slowest option.
    /// 
    /// Some combinations like (Greedy | Mutable) are possible, which generates mutable objects that are pre-allocated.
    /// 
    /// The right way to handle this is to benchmark, and make your choices based on that. What performs best depends on your access patterns.
    /// </remarks>
    public class SerializerOptionsExample
    {
        public static void Run()
        {
            DemoTable demo = new DemoTable
            {
                Name = "My demo table",
                MemoryVector = new[] { 1, 2, 3, 4, 5, },
                ListVector = new List<InnerTable>
                {
                    new InnerTable { Fruit = "Apple" },
                    new InnerTable { Fruit = "Banana" },
                    new InnerTable { Fruit = "Pear" }
                }
            };

            GreedyDeserialization(demo);
            LazyDeserialization(demo);
            MutableDeserialization(demo);
            ComboDeserialization(demo);
        }

        /// <summary>
        /// Greedy deserialization operates the same way that conventional serializers do. The entire buffer is traversed
        /// and the structure is copied into the deserialized object. This is the most straightforward way of using FlatSharp,
        /// because the results it gives are predictable, and require no developer cognitive overhead. However, it can be less efficient
        /// in cases where you do not need to access all data in the buffer.
        /// 
        /// Greedy Deserialization is used by FlatBufferSerializer.Default.
        /// </summary>
        public static void GreedyDeserialization(DemoTable demo)
        {
            // Same as FlatBufferSerializer.Default
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GreedyDeserialize));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);
            long originalSum = buffer.Sum(x => (long)x);

            var parsed = serializer.Parse<DemoTable>(buffer);

            // Fill array with 0. Source data is gone now.
            Array.Fill(buffer, (byte)0);

            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];
            Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "Greedy deserialization returns you the same instance each time");

            // Memory vectors are copied when deserializing greedily
            Span<byte> parsedSpan = MemoryMarshal.Cast<int, byte>(parsed.MemoryVector.Span);
            Debug.Assert(!parsedSpan.Overlaps(buffer), "The parsed vector does not overlap with the original buffer");

            // We cleared the data, but can still read the name. Greedy deserialization is easy!
            string name = parsed.Name;

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
        /// Lazy deserialization! This is the opposite of the above, and is to show you how FlatSharp behaves without any special options
        /// specified. 
        /// </summary>
        public static void LazyDeserialization(DemoTable demo)
        {
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferSerializerFlags.Lazy));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);

            long originalSum = buffer.Sum(x => (long)x);

            var parsed = serializer.Parse<DemoTable>(buffer);

            // Lazy deserialization reads objects from vectors each time you ask for them.
            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];
            Debug.Assert(!object.ReferenceEquals(index0_1, index0_2), "A different instance is returned each time from lazy vectors");
            
            // Properties from tables and structs are cached after they are read.
            string name = parsed.Name;
            string name2 = parsed.Name;

            Debug.Assert(
                object.ReferenceEquals(name, name2), 
                "When reading table/struct properties, FlatSharp caches the result for you even in lazy mode.");

            // Memory vectors reference the underlying buffer directly when not deserialized greedily.
            Span<byte> parsedSpan = MemoryMarshal.Cast<int, byte>(parsed.MemoryVector.Span);
            bool overlaps = parsedSpan.Overlaps(buffer);
            parsedSpan[0] = byte.MaxValue;
            long newSum = buffer.Sum(x => (long)x);

            Debug.Assert(overlaps, "The parsed vector should reference the underlying buffer");
            Debug.Assert(newSum != originalSum, "Modifying either vector changes data in both places");

            // Invalidate the whole buffer. Undefined behavior past here!
            Array.Fill(buffer, (byte)0);

            try
            {
                var whoKnows = parsed.ListVector[1];
                Debug.Assert(false);
            }
            catch
            {
                // This can be any sort of exception.
            }
        }

        /// <summary>
        /// This example shows the flag that exposes mutable objects. Note that mutable objects forces FlatSharp to allocate a List{T} for vector types.
        /// However, unlike greedy, it does not eagerly parse the contents of the list. This is simply a mutable version of VectorCache.
        /// </summary>
        public static void MutableDeserialization(DemoTable demo)
        {
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GenerateMutableObjects));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);

            long originalSum = buffer.Sum(x => (long)x);

            var parsed = serializer.Parse<DemoTable>(buffer);

            // Same reference returned.
            var index0_1 = parsed.ListVector[0];
            var index0_2 = parsed.ListVector[0];
            Debug.Assert(object.ReferenceEquals(index0_1, index0_2));

            parsed.Name = "Benjamin Franklin";
            parsed.ListVector.Clear();
            parsed.ListVector.Add(new InnerTable());

            long newSum = buffer.Sum(x => (long)x);
            Debug.Assert(
                newSum == originalSum, 
                "Changes to the deserialized objects are not written back to the buffer. You'll need to re-serialize it to a new buffer for that. ");

            // If you are not using greedy deserialization, then you need to write back to a new buffer. If you
            // try to serialize into the same buffer you're reading from, things will get corrupted.
            byte[] newBuffer = new byte[1024];
            serializer.Serialize(parsed, newBuffer);
        }
        
        /// <summary>
        /// This example shows Greedy + Mutable deserialization. This creates a "normal" .NET object that behaves just as you'd expect.
        /// Other combinations are permitted, but this is the most interesting one.
        /// </summary>
        public static void ComboDeserialization(DemoTable demo)
        {
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(
                FlatBufferSerializerFlags.GreedyDeserialize | FlatBufferSerializerFlags.GenerateMutableObjects));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);

            long originalSum = buffer.Sum(x => (long)x);

            var parsed = serializer.Parse<DemoTable>(buffer);
            parsed.Name = "James Adams";

            parsed.ListVector.Clear();
            parsed.ListVector.Add(new InnerTable());

            long newSum = buffer.Sum(x => (long)x);
            Debug.Assert(
                newSum == originalSum,
                "Changes to the deserialized objects are not written back to the buffer. You'll need to re-serialize it to a new buffer for that. ");

            // Unlike the example above, here we can be sure that the original buffer is not used
            // so we're free to immediately reserialize our data into it.
            serializer.Serialize(parsed, buffer);
        }
    }

    [FlatBufferTable]
    public class DemoTable : object
    {
        [FlatBufferItem(0)]
        public virtual Memory<int> MemoryVector { get; set; }

        [FlatBufferItem(1)]
        public virtual string Name { get; set; }

        [FlatBufferItem(2)]
        public virtual IList<InnerTable> ListVector { get; set; }
    }

    [FlatBufferTable]
    public class InnerTable
    {
        [FlatBufferItem(0)]
        public virtual string Fruit { get; set; }
    }
}
