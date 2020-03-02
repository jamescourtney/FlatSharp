namespace Samples.SchemaFilesExample2
{
    using System;
    using System.Collections.Generic;
    using FlatSharp;

    /// <summary>
    /// This sample shows the usage of FlatSharp when declaring types and serializers.
    /// </summary>
    /// <remarks>
    /// Based on the monster sample here:
    /// https://google.github.io/flatbuffers/flatbuffers_guide_tutorial.html
    /// </remarks>
    public class SchemaFilesExample2
    {
        public static void Run()
        {
            // These types are generated from SchemaFilesExample.fbs
            FooBarContainer container = new FooBarContainer
            {
                fruit = Fruit.Pears,
                initialized = true,
                location = "location",
                list = new List<FooBar>
                {
                    new FooBar 
                    { 
                        name = "name", 
                        postfix = 1, 
                        rating = 3, 
                        sibling = new Bar
                        {
                            ratio = 3.14f,
                            size = ushort.MaxValue,
                            time = int.MinValue,
                            parent = new Foo { count = 3, id = 4, length = 8, prefix = 10 },
                        }
                    }
                },
            };

            // Serializer is pregenerated, so no first-run penalty for FlatBufferSerializer.
            byte[] destination = new byte[1024];

            int maxBytes = FooBarContainer.Serializer.GetMaxSize(container);
            int bytesWritten = FooBarContainer.Serializer.Write(destination, container);
            var parsed = FooBarContainer.Serializer.Parse(destination);
        }
    }
}
