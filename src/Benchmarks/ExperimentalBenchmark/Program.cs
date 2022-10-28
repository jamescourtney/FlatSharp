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

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using FlatSharp;
using FlatSharp.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BenchmarkCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IndexedVector<string, Item> values = new();
            for (int i = 0; i < 10000; ++i)
            {
                string key = Guid.NewGuid().ToString();
                Item item = new Item() { Key = key, Value = i, Vec3 = new Vector3(i), AVX = new Vector<float>(i) };
                values.Add(item);
            }

            Outer outer = new Outer { Items = values };

            byte[] buffer = new byte[Outer.Serializer.GetMaxSize(outer)];
            Outer.Serializer.Write(buffer, outer);

            var parsed = Outer.Serializer.Parse(buffer);

            int sum = 0;
            foreach (var kvp in values)
            {
                sum += parsed.Items[kvp.Key].Value;
            }

            Console.WriteLine(sum);
        }
    }
}
