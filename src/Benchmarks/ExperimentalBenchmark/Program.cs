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

using FlatSharp;
using FlatSharp.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BenchmarkCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var table = new StructsTable
            {
                VecValue = Enumerable.Range(1, 300).Select(x => new ValueStruct() { Value = 1 }).ToArray(),
                SingleValue = new(),
            };
            /*
            var serializer = StructsTable.Serializer;
            byte[] buffer = new byte[serializer.GetMaxSize(table)];
            serializer.Write(buffer, table);

            for (int i = 0; i < 10000; ++i)
            {
                var item = serializer.Parse(buffer);
                TraverseAndUpdate(item);
            }

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000000; ++i)
            {
                var item = serializer.Parse(buffer);
                TraverseAndUpdate(item);
            }

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void TraverseAndUpdate(StructsTable table)
        {
            var vector = table.VecValue;
            int count = vector.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = vector[i];
                item.Value++;
                vector[i] = item;
            }*/
        }

    }
}
