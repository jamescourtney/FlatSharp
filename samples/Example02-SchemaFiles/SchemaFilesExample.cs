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

namespace Samples.SchemaFilesExample;

/// <summary>
/// This sample shows the usage of FlatSharp when declaring types in an FBS schema file.
/// Note: It does *not* show how to use FlatSharp metadata attributes to precompile your serializers.
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
                            parent = new Foo(), // structs are nullable types, but are not meant to be null.
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
