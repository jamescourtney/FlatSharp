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

namespace Samples.CopyConstructorsExample;

/// <summary>
/// This sample shows the use cases for FlatSharp's auto-generated copy constructors.
/// </summary>
public class CopyConstructorsExample
{
    public static void Run()
    {
        FooBarContainer container = new FooBarContainer
        {
            Fruit = Fruit.Pears,
            Initialized = true,
            Location = "location",
            List = new List<FooBar>
            {
                new FooBar
                {
                    Name = "name",
                    Postfix = 1,
                    Rating = 3,
                    Sibling = new Bar
                    {
                        Ratio = 3.14f,
                        Size = ushort.MaxValue,
                        Time = int.MinValue,
                    }
                }
            },
        };

        // Simple use case: make a deep copy of an object you're using.
        var copy = new FooBarContainer(container);
        Debug.Assert(!object.ReferenceEquals(copy.List, container.List), "A new list is created");
        for (int i = 0; i < container.List.Count; ++i)
        {
            var originalItem = container.List[i];
            var copyItem = copy.List![i];
            Debug.Assert(!object.ReferenceEquals(copyItem, originalItem));
        }

        // Now let's look at how this can be useful when operating on deserialized objects.
        byte[] data = new byte[1024];
        FooBarContainer.Serializer.Write(data, container);
        var deserialized = FooBarContainer.Serializer.Parse(data);

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
            deserialized.Fruit = Fruit.Apples;
            Debug.Assert(false);
        }
        catch
        {
        }

        // Modifying the copy is just fine, though.
        copy.Fruit = Fruit.Apples;
    }
}
