namespace Samples.SchemaFilesExample
{
    using System;
    using System.Collections.Generic;
    using FlatSharp;

    /// <summary>
    /// This sample shows the usage of FlatSharp when declaring types in an FBS schema file.
    /// </summary>
    /// <remarks>
    /// Based on the monster sample here:
    /// https://google.github.io/flatbuffers/flatbuffers_guide_tutorial.html
    /// </remarks>
    public class SchemaFilesExample
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

                            // Null structs are replaced with a new instance.
                            parent = null,
                        }
                    }
                },
            };

            byte[] destination = new byte[1024];

            // Generated types can still use a runtime-generated serializer
            int maxBytes = FlatBufferSerializer.Default.GetMaxSize(container);
            int bytesWritten = FlatBufferSerializer.Default.Serialize(container, destination);
            var parsed = FlatBufferSerializer.Default.Parse<FooBarContainer>(destination);
        }
    }
}
