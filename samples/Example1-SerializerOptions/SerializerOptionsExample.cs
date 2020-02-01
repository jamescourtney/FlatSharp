namespace Samples.SerializerOptions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;

    /// <summary>
    /// This sample shows some of the different serializer options in FlatSharp and discusses the tradeoffs with them.
    /// </summary>
    /// <remarks>
    /// FlatSharp exposes 4 core deserialization modes, in order from most greedy to laziest:
    /// 1) Greedy / GreedyMutable: 
    ///    Pre-parse everything and release references to the underlying buffer. 
    ///    This is the safest option, and therefore the default. It is usually not the fastest option.
    ///            
    /// 2) VectorCache / VectorCacheMutable: 
    ///    Data is read progressively and cached, and vectors are pre-allocated upon initial access. Guarantees
    ///    each element is only read once from the underlying buffer.
    /// 
    /// 4) PropertyCache: 
    ///    Properties of tables and structs are cached, but vector elements are not. Does not have any array allocations behind
    ///    the scenes.
    /// 
    /// 5) Lazy: 
    ///    Nothing is ever cached, and data is reconstituted from the underlying buffer each time. This option is fastest when you access each item
    ///    no more than once, but gets expensive very quickly if you repeatedly access items.
    ///
    /// Unless in Lazy mode, FlatBuffer tables and structs always have their non-vector elements cached as they are accessed, 
    /// so re-reading the same property returns the same instance. Vectors are special, since they can be large, and array 
    /// allocations can be expensive. In general, your choice of deserialization method will be informed by your answers to these 
    /// questions:
    /// 
    /// Question 1: Am I managing the lifetime of my input buffers? 
    ///    Greedy deserialization guarantees deserialized objects hold no more reference to the input buffer (literally, the generated code
    ///    does not even have a variable declared for the input buffer), so you are free to immediately recycle/reuse the buffer. 
    ///    If you are pooling or doing your own lifetime management of these objects, then Greedy deserialization may make sense so the 
    ///    buffer can be immediately reused. Otherwise, you will likely see better performance from another option.
    ///    
    /// Question 2: Will I read the entire object graph?
    ///    If you're not reading all properties of the object, then Greedy deserialization will waste cycles preparsing data you will not use.
    ///    If you plan to touch each field no more than once, then a lazier parsing option will be the best approach. 
    ///    
    /// Question 3: Do I have large vectors?
    ///    Array allocations can be expensive, especially for large arrays. If you are deserializing large vectors, you should use some form of lazy parsing
    ///    (options.Lazy or options.PropertyCache). These are the only configurations that don't induce array allocations behind the scenes for vectors.
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
            PropertyCacheDeserialization(demo);
            VectorCacheDeserialization(demo);
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
            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];
            Debug.Assert(!object.ReferenceEquals(index0_1, index0_2), "A different instance is returned each time from lazy vectors");

            // Properties from tables and structs are cached after they are read.
            string name = parsed.Name;
            string name2 = parsed.Name;

            Debug.Assert(
                !object.ReferenceEquals(name, name2),
                "When reading table/struct properties Lazy parsing returns a different instance each time.");

            // Invalidate the whole buffer. Undefined behavior past here!
            Array.Fill(buffer, (byte)0);

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
        /// The next step up in greediness is PropertyCache mode. In this mode, Flatsharp will cache the results of property accesses.
        /// So, if you read the results of FooObject.Property1 multiple times, the same value comes back each time. What this mode
        /// does not do is cache vectors. So reding FooObject.Vector[0] multiple times re-visits the buffer each time.
        /// </summary>
        public static void PropertyCacheDeserialization(DemoTable demo)
        {
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.PropertyCache));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);
            var parsed = serializer.Parse<DemoTable>(buffer);

            // Properties from tables and structs are cached after they are read.
            string name = parsed.Name;
            string name2 = parsed.Name;

            Debug.Assert(
                object.ReferenceEquals(name, name2),
                "When reading table/struct properties, PropertyCache mode returns the same instance.");

            // PropertyCache deserialization doesn't cache the results of vector lookups.
            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];

            Debug.Assert(!object.ReferenceEquals(index0_1, index0_2), "A different instance is returned each time from vectors in PropertyCache mode.");
            Debug.Assert(object.ReferenceEquals(parsed.ListVector, parsed.ListVector), "But the vector instance itself is the cached.");
            Debug.Assert(object.ReferenceEquals(index0_1.Fruit, index0_1.Fruit), "And the items returned from each vector exhibit property cache behavior");

            // Invalidate the whole buffer. Undefined behavior past here!
            Array.Fill(buffer, (byte)0);

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
        /// Vector cache is a superset of PropertyCache. The difference is that when deserializing in VectorCache mode, FlatSharp
        /// will allocate a vector for you that gets lazily filled in as elements are accessed. This leads to some array allocations
        /// behind the scenes, since FlatSharp needs to know what objects have been returned for what indices.
        /// </summary>
        public static void VectorCacheDeserialization(DemoTable demo)
        {
            var serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferDeserializationOption.VectorCache));

            byte[] buffer = new byte[1024];
            serializer.Serialize(demo, buffer);
            var parsed = serializer.Parse<DemoTable>(buffer);

            // Properties from tables and structs are cached after they are read.
            string name = parsed.Name;
            string name2 = parsed.Name;

            Debug.Assert(
                object.ReferenceEquals(name, name2),
                "When reading table/struct properties, PropertyCache mode returns the same instance.");

            // VectorCache deserialization guarantees only one object per index.
            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];

            Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "The same instance is returned each time from vectors in VectorCache mode.");
            Debug.Assert(object.ReferenceEquals(parsed.ListVector, parsed.ListVector), "And the vector instance itself is the same.");
            Debug.Assert(object.ReferenceEquals(index0_1.Fruit, index0_1.Fruit), "And the items returned from each vector exhibit property cache behavior");
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
            Array.Fill(buffer, (byte)0);

            InnerTable index0_1 = parsed.ListVector[0];
            InnerTable index0_2 = parsed.ListVector[0];
            Debug.Assert(object.ReferenceEquals(index0_1, index0_2), "Greedy deserialization returns you the same instance each time");

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
            parsed.ListVector.Clear();
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
        public virtual string Name { get; set; }

        [FlatBufferItem(1)]
        public virtual IList<InnerTable> ListVector { get; set; }
    }

    [FlatBufferTable]
    public class InnerTable
    {
        [FlatBufferItem(0)]
        public virtual string Fruit { get; set; }
    }
}
