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

namespace Benchmark.FBBench
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using BenchmarkDotNet.Attributes;
    using FlatBuffers;
    using FlatSharp;
    using FlatSharp.Attributes;
    using ProtoBuf;

    using JobKind = BenchmarkDotNet.Attributes.MediumRunJobAttribute;

    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.Net47)]
    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp31)]
    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp21)]
    [JobKind(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50)]
    [CsvExporter(BenchmarkDotNet.Exporters.Csv.CsvSeparator.Comma)]
    public abstract class FBBenchCore
    {
        protected FlatBufferBuilder google_flatBufferBuilder = new FlatBufferBuilder(64 * 1024);
        protected ByteBuffer google_ByteBuffer;
        public Google.FooBarContainerT google_defaultContainer;

        public FooBarListContainer defaultContainer;
        public SortedVectorTable<string> sortedStringContainer;
        public SortedVectorTable<int> sortedIntContainer;
        public UnsortedVectorTable<string> unsortedStringContainer;
        public UnsortedVectorTable<int> unsortedIntContainer;

        protected MemoryStream pbdn_writeBuffer = new MemoryStream(64 * 1024);
        protected MemoryStream pbdn_readBuffer = new MemoryStream(64 * 1024);

        private FlatBufferSerializer fs_serializer;

        protected byte[] fs_readMemory;
        protected readonly byte[] fs_writeMemory = new byte[64 * 1024];
        public ArrayInputBuffer inputBuffer;

        public FBBenchCore()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        }

        [Params(3, 30)]
        public virtual int VectorLength { get; set; }

        public virtual int TraversalCount { get; set; }

        public virtual FlatBufferDeserializationOption DeserializeOption { get; set; }

        protected virtual ArrayInputBuffer GetInputBuffer(byte[] data) => new ArrayInputBuffer(data);

        [GlobalSetup]
        public virtual void GlobalSetup()
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

            Random rng = new Random();
            this.sortedIntContainer = new SortedVectorTable<int> { Vector = new List<SortedVectorTableItem<int>>() };
            this.sortedStringContainer = new SortedVectorTable<string> { Vector = new List<SortedVectorTableItem<string>>() };
            this.unsortedIntContainer = new UnsortedVectorTable<int> { Vector = new List<UnsortedVectorTableItem<int>>() };
            this.unsortedStringContainer = new UnsortedVectorTable<string> { Vector = new List<UnsortedVectorTableItem<string>>() };

            for (int i = 0; i < this.VectorLength; ++i)
            {
                this.sortedIntContainer.Vector.Add(new SortedVectorTableItem<int> { Key = rng.Next() });
                this.sortedStringContainer.Vector.Add(new SortedVectorTableItem<string> { Key = Guid.NewGuid().ToString() });
                this.unsortedIntContainer.Vector.Add(new UnsortedVectorTableItem<int> { Key = rng.Next() });
                this.unsortedStringContainer.Vector.Add(new UnsortedVectorTableItem<string> { Key = Guid.NewGuid().ToString() });
            }

            {
                var options = new FlatBufferSerializerOptions(this.DeserializeOption);

                this.fs_serializer = new FlatBufferSerializer(options);

                int offset = this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
                this.fs_readMemory = this.fs_writeMemory.AsSpan(0, offset).ToArray();
                this.inputBuffer = new ArrayInputBuffer(this.fs_readMemory);
                this.FlatSharp_ParseAndTraverse();
            }

            {
                this.google_ByteBuffer = new FlatBuffers.ByteBuffer(this.fs_readMemory.ToArray());
                this.google_defaultContainer = Google.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer).UnPack();
            }

            {
                this.PBDN_Serialize();
                this.pbdn_writeBuffer.Position = 0;
                this.pbdn_writeBuffer.CopyTo(this.pbdn_readBuffer);
                this.PBDN_ParseAndTraverse();
            }
        }

        #region Google.FlatBuffers

        public virtual void Google_FlatBuffers_Serialize()
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

        public virtual void Google_FlatBuffers_Serialize_ObjectApi()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();
            var offset = Google.FooBarContainer.Pack(builder, this.google_defaultContainer);
            builder.Finish(offset.Value);
        }

        public virtual int Google_Flatbuffers_ParseAndTraverse()
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

        public virtual int Google_Flatbuffers_ParseAndTraverse_ObjectApi()
        {
            var @struct = Google.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);
            var foobar = @struct.UnPack();

            var iterations = this.TraversalCount;
            int sum = 0;

            for (int loop = 0; loop < iterations; ++loop)
            {
                sum = 0;

                sum += foobar.Initialized ? 1 : 0;
                sum += foobar.Location.Length;
                sum += foobar.Fruit;

                int listLength = foobar.List.Count;
                for (int i = 0; i < listLength; ++i)
                {
                    var item = foobar.List[i];
                    sum += item.Name.Length;
                    sum += item.Postfix;
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

        public virtual int Google_Flatbuffers_ParseAndTraversePartial()
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

                    var bar = item.Sibling.Value;
                    sum += (int)bar.Ratio;

                    var parent = bar.Parent;
                    sum += parent.Count;
                }
            }

            return sum;
        }

        public virtual int Google_Flatbuffers_ParseAndTraversePartial_ObjectApi()
        {
            var @struct = Google.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);
            var foobar = @struct.UnPack();

            var iterations = this.TraversalCount;
            int sum = 0;

            for (int loop = 0; loop < iterations; ++loop)
            {
                sum = 0;

                sum += foobar.Initialized ? 1 : 0;
                sum += foobar.Location.Length;
                sum += foobar.Fruit;

                int listLength = foobar.List.Count;
                for (int i = 0; i < listLength; ++i)
                {
                    var item = foobar.List[i];
                    sum += item.Name.Length;

                    var bar = item.Sibling;
                    sum += (int)bar.Ratio;

                    var parent = bar.Parent;
                    sum += parent.Count;
                }
            }

            return sum;
        }

        public virtual void Google_Flatbuffers_StringVector_Sorted()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();

            var offsets = this.CreateSortedStringVectorOffsets(builder);
            var vectorOffset = Google.SortedVectorStringKey.CreateSortedVectorOfSortedVectorStringKey(builder, offsets);
            var tableOffset = Google.SortedVectorContainer.CreateSortedVectorContainer(builder, StringVectorOffset: vectorOffset);
            builder.Finish(tableOffset.Value);
        }

        public virtual void Google_Flatbuffers_IntVector_Sorted()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();

            var offsets = this.CreateSortedIntVectorOffsets(builder);
            var vectorOffset = Google.SortedVectorIntKey.CreateSortedVectorOfSortedVectorIntKey(builder, offsets);
            var tableOffset = Google.SortedVectorContainer.CreateSortedVectorContainer(builder, IntVectorOffset: vectorOffset);
            builder.Finish(tableOffset.Value);
        }

        public virtual void Google_Flatbuffers_StringVector_Unsorted()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();

            var offsets = this.CreateSortedStringVectorOffsets(builder);
            var vectorOffset = Google.SortedVectorContainer.CreateStringVectorVector(builder, offsets);
            var tableOffset = Google.SortedVectorContainer.CreateSortedVectorContainer(builder, StringVectorOffset: vectorOffset);
            builder.Finish(tableOffset.Value);
        }

        public virtual void Google_Flatbuffers_IntVector_Unsorted()
        {
            var builder = this.google_flatBufferBuilder;
            builder.Clear();

            var offsets = this.CreateSortedIntVectorOffsets(builder);
            var vectorOffset = Google.SortedVectorContainer.CreateIntVectorVector(builder, offsets);
            var tableOffset = Google.SortedVectorContainer.CreateSortedVectorContainer(builder, IntVectorOffset: vectorOffset);
            builder.Finish(tableOffset.Value);
        }

        private Offset<Google.SortedVectorStringKey>[] CreateSortedStringVectorOffsets(FlatBufferBuilder builder)
        {
            var stringVector = this.sortedStringContainer.Vector;
            int stringVectorLength = stringVector.Count;

            var offsets = new Offset<Google.SortedVectorStringKey>[stringVectorLength];
            for (int i = 0; i < stringVectorLength; ++i)
            {
                offsets[i] = Google.SortedVectorStringKey.CreateSortedVectorStringKey(
                    builder,
                    builder.CreateString(stringVector[i].Key));
            }

            return offsets;
        }

        private Offset<Google.SortedVectorIntKey>[] CreateSortedIntVectorOffsets(FlatBufferBuilder builder)
        {
            var intVector = this.sortedIntContainer.Vector;
            int intVectorLength = intVector.Count;

            var offsets = new Offset<Google.SortedVectorIntKey>[intVectorLength];
            for (int i = 0; i < intVectorLength; ++i)
            {
                offsets[i] = Google.SortedVectorIntKey.CreateSortedVectorIntKey(
                    builder,
                    intVector[i].Key);
            }

            return offsets;
        }

        #endregion

        #region FlatSharp

        public virtual void FlatSharp_GetMaxSize()
        {
            this.fs_serializer.GetMaxSize(this.defaultContainer);
        }

        public virtual void FlatSharp_Serialize()
        {
            this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
        }

        public virtual void FlatSharp_ParseAndTraverse()
        {
            var item = this.fs_serializer.Parse<FooBarListContainer>(this.inputBuffer);

            this.TraverseFooBarContainer(item);
        }

        public virtual void FlatSharp_ParseAndTraversePartial()
        {
            var item = this.fs_serializer.Parse<FooBarListContainer>(this.inputBuffer);

            this.TraverseFooBarContainerPartial(item);
        }
        
        public virtual void FlatSharp_Serialize_StringVector_Sorted()
        {
            this.fs_serializer.Serialize(this.sortedStringContainer, this.fs_writeMemory);
        }

        public virtual void FlatSharp_Serialize_IntVector_Sorted()
        {
            this.fs_serializer.Serialize(this.sortedIntContainer, this.fs_writeMemory);
        }

        public virtual void FlatSharp_Serialize_StringVector_Unsorted()
        {
            this.fs_serializer.Serialize(this.unsortedStringContainer, this.fs_writeMemory);
        }

        public virtual void FlatSharp_Serialize_IntVector_Unsorted()
        {
            this.fs_serializer.Serialize(this.unsortedIntContainer, this.fs_writeMemory);
        }

#endregion

#region PBDN

        public virtual void PBDN_Serialize()
        {
            this.pbdn_writeBuffer.Position = 0;
            ProtoBuf.Serializer.Serialize(this.pbdn_writeBuffer, this.defaultContainer);
        }

        public virtual void PBDN_ParseAndTraverse()
        {
            this.pbdn_readBuffer.Position = 0;
            var item = ProtoBuf.Serializer.Deserialize<FooBarListContainer>(this.pbdn_readBuffer);
            this.TraverseFooBarContainer(item);
        }

        public virtual void PBDN_ParseAndTraversePartial()
        {
            this.pbdn_readBuffer.Position = 0;
            var item = ProtoBuf.Serializer.Deserialize<FooBarListContainer>(this.pbdn_readBuffer);
            this.TraverseFooBarContainerPartial(item);
        }

#endregion

        public int TraverseFooBarContainer(FooBarListContainer foobar)
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

        private int TraverseFooBarContainerPartial(FooBarListContainer foobar)
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

                    var bar = item.Sibling;
                    sum += (int)bar.Ratio;

                    var parent = bar.Parent;
                    sum += parent.Count;
                }
            }

            return sum;
        }
    }


#region Shared Contracts

    [ProtoContract]
    [FlatBufferStruct]
    public class Foo
    {
        [ProtoMember(1), FlatBufferItem(0)]
        public virtual ulong Id { get; set; }

        [ProtoMember(2), FlatBufferItem(1)]
        public virtual short Count { get; set; }

        [ProtoMember(3), FlatBufferItem(2)]
        public virtual sbyte Prefix { get; set; }

        [ProtoMember(4), FlatBufferItem(3)]
        public virtual uint Length { get; set; }
    }

    [ProtoContract]
    [FlatBufferStruct]
    public class Bar
    {
        [ProtoMember(1), FlatBufferItem(0)]
        public virtual Foo Parent { get; set; }

        [ProtoMember(2), FlatBufferItem(1)]
        public virtual int Time { get; set; }

        [ProtoMember(3), FlatBufferItem(2)]
        public virtual float Ratio { get; set; }

        [ProtoMember(4), FlatBufferItem(3)]
        public virtual ushort Size { get; set; }
    }

    [ProtoContract]
    [FlatBufferTable]
    public class FooBar
    {
        [ProtoMember(1), FlatBufferItem(0)]
        public virtual Bar Sibling { get; set; }

        [ProtoMember(2), FlatBufferItem(1)]
        public virtual string Name { get; set; }

        [ProtoMember(3), FlatBufferItem(2)]
        public virtual double Rating { get; set; }

        [ProtoMember(4), FlatBufferItem(3)]
        public virtual byte PostFix { get; set; }
    }

    [ProtoContract]
    [FlatBufferTable]
    public class FooBarListContainer
    {
        [ProtoMember(1), FlatBufferItem(0)]
        public virtual IList<FooBar> List { get; set; }

        [ProtoMember(2), FlatBufferItem(1)]
        public virtual bool Initialized { get; set; }

        [ProtoMember(3), FlatBufferItem(2)]
        public virtual short Fruit { get; set; }

        [ProtoMember(4), FlatBufferItem(3)]
        public virtual string Location { get; set; }
    }

#endregion

#region Sorted Vector Contracts

    [FlatBufferTable]
    public class SortedVectorTable<T>
    {
        [FlatBufferItem(0, SortedVector = true)]
        public virtual IList<SortedVectorTableItem<T>> Vector { get; set; }
    }

    [FlatBufferTable]
    public class UnsortedVectorTable<T>
    {
        [FlatBufferItem(0)]
        public virtual IList<UnsortedVectorTableItem<T>> Vector { get; set; }
    }

    [FlatBufferTable]
    public class SortedVectorTableItem<T>
    {
        [FlatBufferItem(0, Key = true)]
        public virtual T Key { get; set; }
    }

    [FlatBufferTable]
    public class UnsortedVectorTableItem<T>
    {
        [FlatBufferItem(0)]
        public virtual T Key { get; set; }
    }

#endregion
}
