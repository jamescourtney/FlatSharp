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

namespace Samples.CopyConstructorsExample
{
    using FlatSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// This sample shows the implementation and usage of a simple FlatBuffers GRPC service.
    /// </summary>
    public class CopyConstructorsExample
    {
        public static void Run()
        {
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

            // Simple use case: make a deep copy of an object you're using.
            var copy = new FooBarContainer(container);
            Debug.Assert(!object.ReferenceEquals(copy.list, container.list), "A new list is created");
            for (int i = 0; i < container.list.Count; ++i)
            {
                var originalItem = container.list[i];
                var copyItem = copy.list[i];
                Debug.Assert(!object.ReferenceEquals(copyItem, originalItem));
            }

            // Now let's look at how this can be useful when operating on deserialized objects.
            var serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.Lazy);
            byte[] data = new byte[1024];
            serializer.Serialize(container, data);
            var deserialized = serializer.Parse<FooBarContainer>(data);

            // Take a deserialized item and "upcast" it back to the original type.
            // This performs a full traversal of the object and allows the underlying buffer to be reused.
            Debug.Assert(deserialized.GetType() != container.GetType(), "The deserialized type is a subclass of the FooBarContainer type");
            copy = new FooBarContainer(deserialized);
            Debug.Assert(copy.GetType() == container.GetType(), "By using the copy constructor, we can get an instance of the original type.");

            // Next: Some deserialization modes, such as Lazy, don't permit mutation of the object.
            // Using the copy constructor can convert this to an object that we can mutate!
            try
            {
                // will throw
                deserialized.fruit = Fruit.Apples;
                Debug.Assert(false);
            }
            catch
            {
            }

            // Modifying the copy is just fine, though.
            copy.fruit = Fruit.Apples;
        }
    }
}
