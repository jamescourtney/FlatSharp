/*
 * Copyright 2018 James Courtney
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

namespace BenchmarkCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using benchfb;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Unsafe;

    [FlatBufferTable]
    public class Test1<T>
    {
        [FlatBufferItem(0)]
        public virtual T Item { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"x64={Environment.Is64BitProcess}");

            FooBar[] fooBars = new FooBar[5];
            for (int i = 0; i < fooBars.Length; i++)
            {
                var foo = new Foo
                {
                    id = 0xABADCAFEABADCAFE + (ulong)i,
                    count = (short)(10000 + i),
                    prefix = (sbyte)('@' + i),
                    length = (uint)(1000000 + i)
                };

                var bar = new Bar
                {
                    parent = foo,
                    ratio = 3.14159f + i,
                    size = (ushort)(10000 + i),
                    time = 123456 + i
                };

                var fooBar = new FooBar
                {
                    name = Guid.NewGuid().ToString(),
                    postfix = (byte)('!' + i),
                    rating = 3.1415432432445543543 + i,
                    sibling = bar,
                };

                fooBars[i] = fooBar;
            }

            var defaultContainer = new FooBarContainer
            {
                fruit = benchfb.Enum.Bananas,
                initialized = true,
                location = "http://google.com/flatbuffers/",
                list = fooBars,
            };

            var serializer = FooBarContainer.Serializer;
            //serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(FlatBufferSerializerFlags.GenerateMutableObjects)).Compile<FooBarContainer>();
            SpanWriter writer = new SpanWriter();
            byte[] buffer = new byte[serializer.GetMaxSize(defaultContainer)];
            InputBuffer inputBuffer = new ArrayInputBuffer(buffer);
            int traversalCount = 5;

            (string, Action)[] items = new (string, Action)[]
            {
                ("GetMaxSize",       () => serializer.GetMaxSize(defaultContainer)),
                ("Serialize",        () => serializer.Write(writer, buffer, defaultContainer)),
                ("Parse",            () => serializer.Parse(inputBuffer)),
                ("ParseAndTraverse x1", () => ParseAndTraverse(serializer, inputBuffer, 1)),
                ($"ParseAndTraverse x{traversalCount}", () => ParseAndTraverse(serializer, inputBuffer, traversalCount)),
                ("ParseAndTraversePartial x1", () => ParseAndTraversePartial(serializer, inputBuffer, 1)),
                ($"ParseAndTraversePartial x{traversalCount}", () => ParseAndTraversePartial(serializer, inputBuffer, traversalCount)),
            };

            foreach (var tuple in items)
            {
                for (int loop = 0; loop < 5; ++loop)
                {
                    var action = tuple.Item2;
                    var name = tuple.Item1;

                    for (int i = 0; i < 10000; ++i)
                    {
                        action();
                    }

                    var sw = Stopwatch.StartNew();
                    int count = 1000000;
                    for (int i = 0; i < count; ++i)
                    {
                        action();
                    }
                    sw.Stop();

                    Console.WriteLine($"{name}: Took {sw.ElapsedMilliseconds} ({sw.ElapsedMilliseconds * 1000 / ((double)count)} us per op)");
                    System.Threading.Thread.Sleep(250);
                    GC.Collect();
                }

                Console.WriteLine();
            }
        }

        private static void ParseAndTraverse(ISerializer<FooBarContainer> serializer, InputBuffer inputBuffer, int traversalCount)
        {
            FooBarContainer container = serializer.Parse(inputBuffer);
            for (int i = 0; i < traversalCount; ++i)
            {
                var fruit = container.fruit;
                var initialized = container.initialized;
                var location = container.location;
                var items = container.list;
                int count = items.Count;

                for (int j = 0; j < count; ++j)
                {
                    var item = items[j];
                    var name = item.name;
                    var postfix = item.postfix;
                    var rating = item.rating;
                    var sibling = item.sibling;

                    var ratio = sibling.ratio;
                    var size = sibling.size;
                    var time = sibling.time;
                    var parent = sibling.parent;
                    var count2 = parent.count;
                    var id = parent.id;
                    var length = parent.length;
                    var prefix = parent.prefix;
                }
            }
        }

        private static void ParseAndTraversePartial(ISerializer<FooBarContainer> serializer, InputBuffer inputBuffer, int traversalCount)
        {
            FooBarContainer container = serializer.Parse(inputBuffer);
            for (int i = 0; i < traversalCount; ++i)
            {
                var fruit = container.fruit;
                var initialized = container.initialized;
                var location = container.location;
                var items = container.list;
                int count = items.Count;

                for (int j = 0; j < count; ++j)
                {
                    var item = items[j];
                    var name = item.name;
                    //var postfix = item.postfix;
                    //var rating = item.rating;
                    //var sibling = item.sibling;

                    var ratio = item.sibling.ratio;
                    //var size = sibling.size;
                    //var time = sibling.time;
                    //var parent = sibling.parent;
                    //var count2 = parent.count;
                    //var id = parent.id;
                    //var length = parent.length;
                    //var prefix = parent.prefix;
                }
            }
        }
    }
}
