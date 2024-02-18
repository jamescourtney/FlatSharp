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

        private MultiTable multi;

        public static readonly int A = 3;
        public static readonly int B = 4;

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

            this.multi = new()
            {
                A = new() { Value = Guid.NewGuid().ToString() },
                B = new() { Value = Guid.NewGuid().ToString() },
                C = new() { Value = Guid.NewGuid().ToString() },
                D = new() { Value = Guid.NewGuid().ToString() },
                E = new() { Value = Guid.NewGuid().ToString() },
                F = new() { Value = Guid.NewGuid().ToString() },
                G = new() { Value = Guid.NewGuid().ToString() },
                H = new() { Value = Guid.NewGuid().ToString(), Value2 = Guid.NewGuid().ToString(), Value3 = Guid.NewGuid().ToString(), },
                I = new() { Value = Guid.NewGuid().ToString() },
                J = new() { Value = Guid.NewGuid().ToString() },
                K = new() { Value = Guid.NewGuid().ToString() },
            };
        }

        [Benchmark]
        public void Serialize()
        {
            FooBarContainer.Serializer.Write(this.buffer, this.container);
        }

        [Benchmark]
        public int MathMax()
        {
            return Math.Max(A, B);
        }

        [Benchmark]
        public int Shift()
        {
            int sign = (A - B) >>> 31;
            return A + ((B - A) * sign);
        }

        [Benchmark]
        public int NoOp()
        {
            return 0;
        }

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
