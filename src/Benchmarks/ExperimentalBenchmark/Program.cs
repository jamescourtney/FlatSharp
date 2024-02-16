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
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BenchmarkCore
{
    public class Program
    {
        private FooBarContainer container;
        private byte[] buffer;

        [GlobalSetup]
        public void Setup()
        {
            this.container = new()
            {
                Fruit = 1,
                Initialized = true,
                Location = "somewhere",
                List = Enumerable.Range(0, 30).Select(x => new FooBar
                {
                    Name = x.ToString(),
                    Postfix = (byte)x,
                    Rating = x,
                    Sibling = new Bar
                    {
                        Parent = new Foo
                        {
                            Count = 30,
                            Id = 10,
                            Length = 20,
                            Prefix = 40,
                        },
                        Ratio = 10,
                        Size = 10,
                        Time = 10,
                    }
                }).ToList(),
            };

            this.buffer = new byte[1024 * 1024];
        }

        [Benchmark]
        public void Serialize()
        {
            FooBarContainer.Serializer.Write(this.buffer, this.container);
        }

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
