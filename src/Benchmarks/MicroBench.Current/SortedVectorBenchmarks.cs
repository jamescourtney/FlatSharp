/*
 * Copyright 2022 James Courtney
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

namespace Microbench
{
    using BenchmarkDotNet.Attributes;
    using System;
    using FlatSharp;
    using System.Linq;

    public class SortedVectorBenchmarks
    {
        [Benchmark]
        public void Serialize_SortedVector_StringKey()
        {
            SortedTable.Serializer.Write(Constants.SortedVectorTables.Buffer_StringKey, Constants.SortedVectorTables.SortedStrings);
        }

        [Benchmark]
        public void Serialize_SortedVector_IntKey()
        {
            SortedTable.Serializer.Write(Constants.SortedVectorTables.Buffer_IntKey, Constants.SortedVectorTables.SortedInts);
        }

        [Benchmark]
        public int Parse_SortedVector_StringKey()
        {
            SortedTable sortedStrings = SortedTable.Serializer.Parse(Constants.SortedVectorTables.Buffer_StringKey);

            int count = 0;
            foreach (var key in Constants.SortedVectorTables.AllStringKeys)
            {
                if (sortedStrings.Strings!.TryGetValue(key.Key, out _))
                {
                    count++;
                }
            }

            return count;
        }

        [Benchmark]
        public int Parse_SortedVector_IntKey()
        {
            SortedTable sortedInts = SortedTable.Serializer.Parse(Constants.SortedVectorTables.Buffer_IntKey);

            int count = 0;
            foreach (var key in Constants.SortedVectorTables.AllIntKeys)
            {
                if (sortedInts.Ints!.TryGetValue(key.Key, out _))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
