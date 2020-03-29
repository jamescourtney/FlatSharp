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
    using System.Linq;
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

            FlatBufferSerializer inPlaceSorter = new FlatBufferSerializer(new FlatBufferSerializerOptions());

            Random r = new Random();
            SortedVector<int> intVector = new SortedVector<int>
            {
                Item = Enumerable.Range(0, 25).Select(x => new SortedVectorItem<int> { Item = r.Next() }).ToList()
            };

            SortedVector<string> stringVector = new SortedVector<string>
            {
                Item = Enumerable.Range(0, 25).Select(x => new SortedVectorItem<string> { Item = Guid.NewGuid().ToString() }).ToList()
            };

            UnsortedVector<string> unsortedString = new UnsortedVector<string> { Item = stringVector.Item };
            UnsortedVector<int> unsortedInt = new UnsortedVector<int> { Item = intVector.Item };

            byte[] buffer = new byte[10 * 1024 * 1024];
            var items = new (string, Action)[]
            {
                //("ToArray", () => intVector.Item.Select(x => x.Item).ToArray()),
                //("ArrayStringSort", () => Array.Sort(stringVector.Item.Select(x => x.Item).ToArray())),
                //("ArrayIntSort", () => Array.Sort(intVector.Item.Select(x => x.Item).ToArray())),
                ("InPlaceSortString", () => inPlaceSorter.Serialize(stringVector, buffer)),
                ("UnsortedString", () => inPlaceSorter.Serialize(unsortedString, buffer)),
                ("InPlaceSortInt", () => inPlaceSorter.Serialize(intVector, buffer)),
                ("UnsortedInt", () => inPlaceSorter.Serialize(unsortedInt, buffer)),
            };

            RunBenchmarkSet(items);
        }

        private static void RunBenchmarkSet((string, Action)[] items)
        {
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

            Console.WriteLine();
        }

        private static (string, Action)[] GetBenchmarkSet(
            string runName, 
            ISerializer<FooBarContainer> serializer, 
            FooBarContainer container)
        {
            int traversalCount = 5;
            byte[] destination = new byte[serializer.GetMaxSize(container)];

            SpanWriter spanWriter = new SpanWriter();
            InputBuffer inputBuffer = new ArrayInputBuffer(destination);
            serializer.Write(spanWriter, destination, container);

            return new (string, Action)[]
            {
                ($"{runName}.GetMaxSize",       () => serializer.GetMaxSize(container)),
                ($"{runName}.Serialize",        () => serializer.Write(spanWriter, destination, container)),
                ($"{runName}.Parse",            () => serializer.Parse(inputBuffer)),
                ($"{runName}.ParseAndTraverse x1", () => ParseAndTraverse(serializer, inputBuffer, 1)),
                ($"{runName}.ParseAndTraverse x{traversalCount}", () => ParseAndTraverse(serializer, inputBuffer, traversalCount)),
                ($"{runName}.ParseAndTraversePartial x1", () => ParseAndTraversePartial(serializer, inputBuffer, 1)),
                ($"{runName}.ParseAndTraversePartial x{traversalCount}", () => ParseAndTraversePartial(serializer, inputBuffer, traversalCount)),
            };
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

        [FlatBufferTable]
        public class SortedVector<T>
        {
            [FlatBufferItem(0, SortedVector = true)]
            public virtual IList<SortedVectorItem<T>> Item { get; set; }
        }

        [FlatBufferTable]
        public class UnsortedVector<T>
        {
            [FlatBufferItem(0, SortedVector = false)]
            public virtual IList<SortedVectorItem<T>> Item { get; set; }
        }

        [FlatBufferTable]
        public class SortedVectorItem<T>
        {
            [FlatBufferItem(0, Key = true)]
            public virtual T Item { get; set; }
        }
    }
}
