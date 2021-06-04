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
using BenchmarkDotNet.Running;
using FlatSharp;
using FlatSharp.Unsafe;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BenchmarkCore
{
    [DisassemblyDiagnoser(maxDepth: 30, printSource: true, exportHtml: true, printInstructionAddresses: true)]
    [ShortRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class StructVectorClone
    {
        private readonly byte[] data = new byte[10 * 1024 * 1024];
        private SomeTable? table;

        private ArrayInputBuffer inputBuffer;
        //private UnsafeSpanWriter2 spanWriter;

        [GlobalSetup]
        public void Setup()
        {
            this.inputBuffer = new ArrayInputBuffer(this.data);
            //this.spanWriter = UnsafeSpanWriter2.Instance;

            this.table = new SomeTable
            {
                Points = new Vec3[20000]
            };

            for (int i = 0; i < 20_000; ++i)
            {
                this.table.Points[i] = (new Vec3 { X = 1, Y = 2, Z = 3 });
            }

            this.Serialize();
        }

        [Benchmark]
        public void Serialize()
        {
            int size = SomeTable.Serializer.Write(SpanWriter.Instance, this.data, this.table!);
        }

        [Benchmark]
        public int ParseAndTraverse()
        {
            var t = SomeTable.Serializer.Parse(this.inputBuffer);
            int sum = 0;

            var points = t.Points;
            int count = points.Count;
            for (int i = 0; i < count; ++i)
            {
                var item = points[i];
                sum += (int)(item.X + item.Y + item.Z);
            }

            for (int i = 0; i < count; ++i)
            {
                ((IFlatBufferDeserializedObject)points[i]).Release();
            }

            return sum;

            //((IFlatBufferDeserializedObject)t).Release();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //Modified.MeshServerModifiedUseCases m = new Modified.MeshServerModifiedUseCases();
            //m.Setup();

            //BenchmarkRunner.Run<MeshServerUseCases>();
            BenchmarkRunner.Run<Modified.MeshServerModifiedUseCases>();

            //BenchmarkRunner.Run<StructVectorClone>();
            //FlatSharpGlobalSettings.CollectPooledObjectStackTraces = true;

            //var table = new SomeTable
            //{
            //    Struct = new Struct
            //    {
            //        Int = 23,
            //        Other = new OtherStruct
            //        {
            //            Long = 45,
            //        },
            //    }
            //};

            //byte[] buffer = new byte[1024];
            //SomeTable.Serializer.Write(buffer, table);

            //var parsed = SomeTable.Serializer.Parse(buffer);
            //(parsed as IFlatBufferDeserializedObject)?.Release();

            //Console.WriteLine(parsed.Struct.Int);
            //Console.WriteLine(parsed.Struct.Other.Long);
            //parsed.Struct.Int--;
            //parsed.Struct.Other = new OtherStruct { Long = 10 };

            //var parsed2 = SomeTable.Serializer.Parse(buffer);
            //Console.WriteLine(parsed2.Struct.Int);
            //Console.WriteLine(parsed2.Struct.Other.Long);

            //parsed2.Struct.Other = null!;

            //var parsed3 = SomeTable.Serializer.Parse(buffer);
            //Console.WriteLine(parsed3.Struct.Int);
            //Console.WriteLine(parsed3.Struct.Other.Long);
        }
    }
}
