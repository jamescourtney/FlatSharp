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

#define FLATSHARP  // enable flat sharp benchmarks
#define ZERO       // enable zero formatter
#define PBDN       // enable protobuf.net
#define GOOG       // enable google flatbuffers.

namespace Benchmark.FBBench
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using BenchmarkDotNet.Attributes;
    using FlatBuffers;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Unsafe;
    using ProtoBuf;

    [CoreJob]
    [RPlotExporter]
    //[ShortRunJob]
    //[InProcess]
    //[MemoryDiagnoser]
    public class Google_FBBench
    {
        [Params(3, 30)]
        public int VectorLength;

        [Params(1, 5)]
        public int TraversalCount;

#if FLATSHARP
        [Params(false, true)]
        public bool FlatSharpVectorCache;
#endif

        private FlatBufferBuilder google_flatBufferBuilder = new FlatBufferBuilder(64 * 1024);
        private ByteBuffer google_ByteBuffer;

        public FooBarListContainer defaultContainer;

        private MemoryStream pbdn_writeBuffer = new MemoryStream(64 * 1024);
        private MemoryStream pbdn_readBuffer = new MemoryStream(64 * 1024);

        private byte[] zf_writeBuffer = new byte[64 * 1024];
        private byte[] zf_readBuffer;

        private FlatBufferSerializer fs_serializer;
        private byte[] fs_readMemory;
        private byte[] fs_writeMemory = new byte[64 * 1024];
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            FooBar[] fooBars = new FooBar[this.VectorLength];
            for (int i = 0; i < fooBars.Length; i++)
            {
                var foo = new Foo
                {
                    Id = 0xABADCAFEABADCAFE + (ulong)i,
                    Count = (short)(10000 + i),
                    Prefix = (sbyte)('@' + i),
                    Length = (uint)(1000000 + i)
                };

                var bar = new Bar
                {
                    Parent = foo,
                    Ratio = 3.14159f + i,
                    Size = (ushort)(10000 + i),
                    Time = 123456 + i
                };

                var fooBar = new FooBar
                {
                    Name = Guid.NewGuid().ToString(),
                    PostFix = (byte)('!' + i),
                    Rating = 3.1415432432445543543 + i,
                    Sibling = bar,
                };

                fooBars[i] = fooBar;
            }

            this.defaultContainer = new FooBarListContainer
            {
                Fruit = 123,
                Initialized = true,
                Location = "http://google.com/flatbuffers/",
                List = fooBars,
            };

#if GOOG
            {
                this.Google_FlatBuffers_Serialize();
                this.google_ByteBuffer = new FlatBuffers.ByteBuffer(this.google_flatBufferBuilder.SizedByteArray());
            }
#endif

#if FLATSHARP
            {
                this.fs_serializer = new FlatBufferSerializer(new FlatBufferSerializerOptions(cacheListVectorData: this.FlatSharpVectorCache));
                int offset = this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
                this.fs_readMemory = this.fs_writeMemory.AsSpan(0, offset).ToArray();
                this.FlatSharp_ParseAndTraverse_SafeMem();
            }
#endif

#if PBDN
            {
                this.PBDN_Serialize();
                this.pbdn_writeBuffer.Position = 0;
                this.pbdn_writeBuffer.CopyTo(this.pbdn_readBuffer);
                this.PBDN_ParseAndTraverse();
            }
#endif

#if ZERO
            {
                this.ZF_Serialize();
                this.zf_readBuffer = this.zf_writeBuffer.AsMemory().ToArray();
                this.ZF_ParseAndTraverse();
            }
#endif
        }

        #region Google.FlatBuffers

#if GOOG

        [Benchmark]
        public void Google_FlatBuffers_Serialize()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();

            var vectorLength = this.VectorLength;

            Offset<Google.FooBar>[] offsets = new Offset<Google.FooBar>[vectorLength];
            for (int i = 0; i < vectorLength; i++)
            {
                var str = builder.CreateString("Hello, World!");
                Google.FooBar.StartFooBar(builder);
                Google.FooBar.AddSibling(builder, Google.Bar.CreateBar(
                    builder,
                    0xABADCAFEABADCAFE + (ulong)i,
                    (short)(10000 + i),
                    (sbyte)('@' + i),
                    (uint)(1000000 + i),
                    123456 + i,
                    3.14159f + i,
                    (ushort)(10000 + i)));

                Google.FooBar.AddName(builder, str);
                Google.FooBar.AddRating(builder, 3.1415432432445543543 + i);
                Google.FooBar.AddPostfix(builder, (byte)('!' + i));
                var offset = Google.FooBar.EndFooBar(builder);
                offsets[i] = offset;
            }

            var foobarOffset = Google.FooBarContainer.CreateFooBarContainer(
                builder,
                builder.CreateVectorOfTables(offsets),
                true,
                123,
                builder.CreateString("http://google.com/flatbuffers/"));

            builder.Finish(foobarOffset.Value);
        }

        [Benchmark]
        public int Google_Flatbuffers_ParseAndTraverse()
        {
            var iterations = this.TraversalCount;
            int sum = 0;

            for (int loop = 0; loop < iterations; ++loop)
            {
                sum = 0;
                var foobar = Google.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);

                sum += foobar.Initialized ? 1 : 0;
                sum += foobar.Location.Length;
                sum += foobar.Fruit;

                int listLength = foobar.ListLength;
                for (int i = 0; i < listLength; ++i)
                {
                    var item = foobar.List(i).Value;
                    sum += item.Name.Length;
                    sum += item.Postfix;
                    sum += (int)item.Rating;

                    var bar = item.Sibling.Value;
                    sum += (int)bar.Ratio;
                    sum += bar.Size;
                    sum += bar.Time;

                    var parent = bar.Parent;
                    sum += parent.Count;
                    sum += (int)parent.Id;
                    sum += (int)parent.Length;
                    sum += parent.Prefix;
                }
            }

            return sum;
        }

#endif

        #endregion

        #region FlatSharp

#if FLATSHARP
        [Benchmark]
        public void FlatSharp_GetMaxSize()
        {
            this.fs_serializer.GetMaxSize(this.defaultContainer);
        }

        [Benchmark]
        public void FlatSharp_Serialize()
        {
            this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
        }

        [Benchmark]
        public void FlatSharp_ParseAndTraverse_SafeMem()
        {
            var item = this.fs_serializer.Parse<FooBarListContainer>(new Memory<byte>(this.fs_readMemory));
            this.TraverseFooBarContainer(item);
        }

        [Benchmark]
        public void FlatSharp_ParseAndTraverse_SafeArray()
        {
            var item = this.fs_serializer.Parse<FooBarListContainer>(new ArrayInputBuffer(this.fs_readMemory));
            this.TraverseFooBarContainer(item);
        }

        [Benchmark]
        public void FlatSharp_ParseAndTraverse_UnsafeMem()
        {
            using (var unsafeRef = new UnsafeMemoryInputBuffer(this.fs_readMemory))
            {
                var item = this.fs_serializer.Parse<FooBarListContainer>(unsafeRef);
                this.TraverseFooBarContainer(item);
            }
        }

        //[Benchmark]
        //public void FlatSharp_ParseAndTraverse_UnsafeMem_NoDispose()
        //{
        //    var unsafeRef = new UnsafeMemoryInputBuffer(this.fs_readMemory);
        //    {
        //        var item = this.fs_serializer.Parse<FooBarListContainer>(unsafeRef);
        //        this.TraverseFooBarContainer(item);
        //    }
        //}

        [Benchmark]
        public void FlatSharp_ParseAndTraverse_UnsafeArray()
        {
            var item = this.fs_serializer.Parse<FooBarListContainer>(new UnsafeArrayInputBuffer(this.fs_readMemory));
            this.TraverseFooBarContainer(item);
        }

        [Benchmark]
        public void FlatSharp_ParseAndTraverse_UnsafeArray_Array()
        {
            var item = this.fs_serializer.Parse<FooBarArrayContainer>(new UnsafeArrayInputBuffer(this.fs_readMemory));
            this.TraverseFooBarContainer(item);
        }

#endif

        #endregion


        #region PBDN
#if PBDN
        [Benchmark]
        public void PBDN_Serialize()
        {
            this.pbdn_writeBuffer.Position = 0;
            ProtoBuf.Serializer.Serialize(this.pbdn_writeBuffer, this.defaultContainer);
        }

        [Benchmark]
        public void PBDN_ParseAndTraverse()
        {
            this.pbdn_readBuffer.Position = 0;
            var item = ProtoBuf.Serializer.Deserialize<FooBarListContainer>(this.pbdn_readBuffer);
            this.TraverseFooBarContainer(item);
        }
#endif

        #endregion

        #region ZeroFormatter

#if ZERO
        [Benchmark]
        public void ZF_Serialize()
        {
            ZeroFormatter.ZeroFormatterSerializer.Serialize(ref this.zf_writeBuffer, 0, this.defaultContainer);
        }

        [Benchmark]
        public void ZF_ParseAndTraverse()
        {
            var item = ZeroFormatter.ZeroFormatterSerializer.Deserialize<FooBarListContainer>(this.zf_writeBuffer);
            this.TraverseFooBarContainer(item);
        }
#endif

        #endregion

        private int TraverseFooBarContainer(IFooBarContainer foobar)
        {
            var iterations = this.TraversalCount;
            int sum = 0;

            for (int loop = 0; loop < iterations; ++loop)
            {
                sum = 0;
                sum += foobar.Initialized ? 1 : 0;
                sum += foobar.Location.Length;
                sum += foobar.Fruit;

                var list = foobar.List;
                int count = list.Count;

                for (int i = 0; i < count; ++i)
                {
                    var item = list[i];
                    sum += item.Name.Length;
                    sum += item.PostFix;
                    sum += (int)item.Rating;

                    var bar = item.Sibling;
                    sum += (int)bar.Ratio;
                    sum += bar.Size;
                    sum += bar.Time;

                    var parent = bar.Parent;
                    sum += parent.Count;
                    sum += (int)parent.Id;
                    sum += (int)parent.Length;
                    sum += parent.Prefix;
                }
            }

            return sum;
        }
    }

    #region Shared Contracts

    [ProtoContract]
    [FlatBufferStruct]
    [ZeroFormatter.ZeroFormattable]
    public class Foo
    {
        [ProtoMember(1), FlatBufferItem(0), ZeroFormatter.Index(0)]
        public virtual ulong Id { get; set; }

        [ProtoMember(2), FlatBufferItem(1), ZeroFormatter.Index(1)]
        public virtual short Count { get; set; }

        [ProtoMember(3), FlatBufferItem(2), ZeroFormatter.Index(2)]
        public virtual sbyte Prefix { get; set; }

        [ProtoMember(4), FlatBufferItem(3), ZeroFormatter.Index(3)]
        public virtual uint Length { get; set; }
    }

    [ProtoContract]
    [FlatBufferStruct]
    [ZeroFormatter.ZeroFormattable]
    public class Bar
    {
        [ProtoMember(1), FlatBufferItem(0), ZeroFormatter.Index(0)]
        public virtual Foo Parent { get; set; }

        [ProtoMember(2), FlatBufferItem(1), ZeroFormatter.Index(1)]
        public virtual int Time { get; set; }

        [ProtoMember(3), FlatBufferItem(2), ZeroFormatter.Index(2)]
        public virtual float Ratio { get; set; }

        [ProtoMember(4), FlatBufferItem(3), ZeroFormatter.Index(3)]
        public virtual ushort Size { get; set; }
    }

    [ProtoContract]
    [FlatBufferTable]
    [ZeroFormatter.ZeroFormattable]
    public class FooBar
    {
        [ProtoMember(1), FlatBufferItem(0), ZeroFormatter.Index(0)]
        public virtual Bar Sibling { get; set; }

        [ProtoMember(2), FlatBufferItem(1), ZeroFormatter.Index(1)]
        public virtual string Name { get; set; }

        [ProtoMember(3), FlatBufferItem(2), ZeroFormatter.Index(2)]
        public virtual double Rating { get; set; }

        [ProtoMember(4), FlatBufferItem(3), ZeroFormatter.Index(3)]
        public virtual byte PostFix { get; set; }
    }

    public interface IFooBarContainer
    {
        IList<FooBar> List { get; }
        bool Initialized { get; set; }
        short Fruit { get; set; }
        string Location { get; set; }
    }

    [ProtoContract]
    [FlatBufferTable]
    [ZeroFormatter.ZeroFormattable]
    public class FooBarArrayContainer : IFooBarContainer
    {
        [ProtoMember(1), FlatBufferItem(0), ZeroFormatter.Index(0)]
        public virtual FooBar[] List { get; set; }

        [ProtoMember(2), FlatBufferItem(1), ZeroFormatter.Index(1)]
        public virtual bool Initialized { get; set; }

        [ProtoMember(3), FlatBufferItem(2), ZeroFormatter.Index(2)]
        public virtual short Fruit { get; set; }

        [ProtoMember(4), FlatBufferItem(3), ZeroFormatter.Index(3)]
        public virtual string Location { get; set; }

        IList<FooBar> IFooBarContainer.List
        {
            get => this.List;
        }
    }

    [ProtoContract]
    [FlatBufferTable]
    [ZeroFormatter.ZeroFormattable]
    public class FooBarListContainer : IFooBarContainer
    {
        [ProtoMember(1), FlatBufferItem(0), ZeroFormatter.Index(0)]
        public virtual IList<FooBar> List { get; set; }

        [ProtoMember(2), FlatBufferItem(1), ZeroFormatter.Index(1)]
        public virtual bool Initialized { get; set; }

        [ProtoMember(3), FlatBufferItem(2), ZeroFormatter.Index(2)]
        public virtual short Fruit { get; set; }

        [ProtoMember(4), FlatBufferItem(3), ZeroFormatter.Index(3)]
        public virtual string Location { get; set; }
    }

    #endregion
}
