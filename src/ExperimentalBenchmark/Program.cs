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

            FooBar[] fooBars = new FooBar[3];
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
            SpanWriter writer = new SpanWriter();
            byte[] buffer = new byte[serializer.GetMaxSize(defaultContainer)];
            InputBuffer inputBuffer = new UnsafeArrayInputBuffer(buffer);

            (string, Action)[] items = new (string, Action)[]
            {
                ("GetMaxSize", () => serializer.GetMaxSize(defaultContainer)),
                ("Serialize",  () => serializer.Write(writer, buffer, defaultContainer)),
                ("Parse",      () => serializer.Parse(inputBuffer)),
            };

            for (int loop = 0; loop < 5; ++loop)
            {
                foreach (var tuple in items)
                {
                    var action = tuple.Item2;
                    var name = tuple.Item1;

                    for (int i = 0; i < 10000; ++i)
                    {
                        action();
                    }
                    GC.Collect();

                    var sw = Stopwatch.StartNew();
                    int count = 1000000;
                    for (int i = 0; i < count; ++i)
                    {
                        action();
                    }
                    sw.Stop();
                    Console.WriteLine($"{name}: Took {sw.ElapsedMilliseconds} ({sw.ElapsedMilliseconds * 1000 / ((double)count)} us per op)");
                    System.Threading.Thread.Sleep(1000);
                    GC.Collect();
                }
            }
        }
    }
}
