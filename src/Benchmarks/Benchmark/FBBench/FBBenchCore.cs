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

namespace Benchmark.FBBench;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using FlatSharp;
using FlatSharp.Attributes;
using ProtoBuf;
using InputBufferKind = FlatSharp.ArrayInputBuffer;
using global::Google.FlatBuffers;

public abstract class FBBenchCore
{
    protected FlatBufferBuilder google_flatBufferBuilder = new FlatBufferBuilder(1024 * 1024);
    protected ByteBuffer google_ByteBuffer;
    public GFB.FooBarContainerT google_defaultContainer;

    public FooBarListContainer defaultContainer;
    public FooBarListContainer_ValueType defaultContainerWithValueStructs;
    public FooBarListContainer_ValueType_NonVirtual defaultContainerWithValueStructsNonVirtual;
    public SortedVectorTable<string> sortedStringContainer;
    public SortedVectorTable<int> sortedIntContainer;
    public UnsortedVectorTable<string> unsortedStringContainer;
    public UnsortedVectorTable<int> unsortedIntContainer;
    public ValueTableVector valueTableVector;

    protected MemoryStream pbdn_writeBuffer = new MemoryStream(1024 * 1024);
    protected MemoryStream pbdn_readBuffer = new MemoryStream(1024 * 1024);

    private byte[] msgPackWriteData;

    protected byte[] fs_readMemory;
    protected readonly byte[] fs_writeMemory = new byte[1024 * 1024];
    public InputBufferKind inputBuffer;

    [Params(30)]
    public virtual int VectorLength { get; set; }

    public virtual int TraversalCount { get; set; }

    public virtual FlatBufferDeserializationOption DeserializeOption { get; set; } = FlatBufferDeserializationOption.Default;

    [GlobalSetup]
    public virtual void GlobalSetup()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Idle;

        FooBar[] fooBars = new FooBar[this.VectorLength];
        FooBar_ValueType[] fooBarsValue = new FooBar_ValueType[this.VectorLength];
        FooBar_ValueType_NonVirtual[] fooBarsValue_NonVirtual = new FooBar_ValueType_NonVirtual[this.VectorLength];

        for (int i = 0; i < fooBars.Length; i++)
        {
            var foo = new Foo
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            var fooValue = new Foo_ValueType
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

            var barValue = new Bar_ValueType
            {
                Parent = fooValue,
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

            var fooBarValue = new FooBar_ValueType
            {
                Name = Guid.NewGuid().ToString(),
                PostFix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = barValue,
            };

            var fooBarValueNV = new FooBar_ValueType_NonVirtual
            {
                Name = Guid.NewGuid().ToString(),
                PostFix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = barValue,
            };

            fooBars[i] = fooBar;
            fooBarsValue[i] = fooBarValue;
            fooBarsValue_NonVirtual[i] = fooBarValueNV;
        }

        this.defaultContainer = new FooBarListContainer
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = fooBars,
        };

        this.defaultContainerWithValueStructs = new FooBarListContainer_ValueType
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = fooBarsValue,
        };

        this.defaultContainerWithValueStructsNonVirtual = new FooBarListContainer_ValueType_NonVirtual
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = fooBarsValue_NonVirtual,
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

        this.valueTableVector = new ValueTableVector
        {
            ValueTables = Enumerable.Range(0, this.VectorLength).Select(x => new ValueTable
            {
                A = 1,
                B = 2,
                C = 3,
                D = 4,
                E = 5,
                F = 6,
                G = 7,
                H = 8,
                I = 9,
                J = 10,
                K = true,
            }).ToArray()
        };

        {
            var options = new FlatBufferSerializerOptions(this.DeserializeOption);

            this.fs_serializer = new FlatBufferSerializer(options);

            int offset = this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
            this.fs_readMemory = this.fs_writeMemory.AsSpan(0, offset).ToArray();
            this.inputBuffer = new InputBufferKind(this.fs_readMemory);
            this.FlatSharp_ParseAndTraverse();
        }

        int googleLength = 0;
        {
            this.google_ByteBuffer = new ByteBuffer(this.fs_readMemory.ToArray());
            this.google_defaultContainer = GFB.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer).UnPack();
            googleLength = this.Google_FlatBuffers_Serialize_ObjectApi();
        }

        {
            this.PBDN_Serialize();
            this.pbdn_writeBuffer.Position = 0;
            this.pbdn_writeBuffer.CopyTo(this.pbdn_readBuffer);
            this.PBDN_ParseAndTraverse();
        }

        {
            this.MsgPack_Serialize_NonVirtual();
        }

        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

        Console.WriteLine($"Sizes: MsgPack: {this.msgPackWriteData.Length}");
        Console.WriteLine($"Sizes: FlatSharp: {this.fs_readMemory.Length}");
        Console.WriteLine($"Sizes: Google: {googleLength}");
        Console.WriteLine($"Sizes: Pbdn: {this.pbdn_writeBuffer.Length}");
    }

    public virtual void Google_FlatBuffers_Serialize()
    {
        var builder = this.google_flatBufferBuilder;
        builder.Clear();
        var vectorLength = this.VectorLength;

        Offset<GFB.FooBar>[] offsets = new Offset<GFB.FooBar>[vectorLength];
        for (int i = 0; i < vectorLength; i++)
        {
            var str = builder.CreateString("Hello, World!");
            GFB.FooBar.StartFooBar(builder);
            GFB.FooBar.AddSibling(builder, GFB.Bar.CreateBar(
                builder,
                0xABADCAFEABADCAFE + (ulong)i,
                (short)(10000 + i),
                (sbyte)('@' + i),
                (uint)(1000000 + i),
                123456 + i,
                3.14159f + i,
                (ushort)(10000 + i)));

            GFB.FooBar.AddName(builder, str);
            GFB.FooBar.AddRating(builder, 3.1415432432445543543 + i);
            GFB.FooBar.AddPostfix(builder, (byte)('!' + i));
            var offset = GFB.FooBar.EndFooBar(builder);
            offsets[i] = offset;
        }

        var foobarOffset = GFB.FooBarContainer.CreateFooBarContainer(
            builder,
            builder.CreateVectorOfTables(offsets),
            true,
            123,
            builder.CreateString("http://google.com/flatbuffers/"));

        builder.Finish(foobarOffset.Value);
    }

    public virtual int Google_FlatBuffers_Serialize_ObjectApi()
    {
        var builder = this.google_flatBufferBuilder;
        builder.Clear();
        var offset = GFB.FooBarContainer.Pack(builder, this.google_defaultContainer);
        builder.Finish(offset.Value);
        return offset.Value;
    }

    public virtual int Google_Flatbuffers_ParseAndTraverse()
    {
        var iterations = this.TraversalCount;
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            var foobar = GFB.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);

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
        var @struct = GFB.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);
        var foobar = @struct.UnPack();

        var iterations = this.TraversalCount;
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
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
            var foobar = GFB.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);

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
        var @struct = GFB.FooBarContainer.GetRootAsFooBarContainer(this.google_ByteBuffer);
        var foobar = @struct.UnPack();

        var iterations = this.TraversalCount;
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
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
        var vectorOffset = GFB.SortedVectorStringKey.CreateSortedVectorOfSortedVectorStringKey(builder, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(builder, StringVectorOffset: vectorOffset);
        builder.Finish(tableOffset.Value);
    }

    public virtual void Google_Flatbuffers_IntVector_Sorted()
    {
        var builder = this.google_flatBufferBuilder;
        builder.Clear();

        var offsets = this.CreateSortedIntVectorOffsets(builder);
        var vectorOffset = GFB.SortedVectorIntKey.CreateSortedVectorOfSortedVectorIntKey(builder, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(builder, IntVectorOffset: vectorOffset);
        builder.Finish(tableOffset.Value);
    }

    public virtual void Google_Flatbuffers_StringVector_Unsorted()
    {
        var builder = this.google_flatBufferBuilder;
        builder.Clear();

        var offsets = this.CreateSortedStringVectorOffsets(builder);
        var vectorOffset = GFB.SortedVectorContainer.CreateStringVectorVector(builder, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(builder, StringVectorOffset: vectorOffset);
        builder.Finish(tableOffset.Value);
    }

    public virtual void Google_Flatbuffers_IntVector_Unsorted()
    {
        var builder = this.google_flatBufferBuilder;
        builder.Clear();

        var offsets = this.CreateSortedIntVectorOffsets(builder);
        var vectorOffset = GFB.SortedVectorContainer.CreateIntVectorVector(builder, offsets);
        var tableOffset = GFB.SortedVectorContainer.CreateSortedVectorContainer(builder, IntVectorOffset: vectorOffset);
        builder.Finish(tableOffset.Value);
    }

    private Offset<GFB.SortedVectorStringKey>[] CreateSortedStringVectorOffsets(FlatBufferBuilder builder)
    {
        var stringVector = this.sortedStringContainer.Vector;
        int stringVectorLength = stringVector.Count;

        var offsets = new Offset<GFB.SortedVectorStringKey>[stringVectorLength];
        for (int i = 0; i < stringVectorLength; ++i)
        {
            offsets[i] = GFB.SortedVectorStringKey.CreateSortedVectorStringKey(
                builder,
                builder.CreateString(stringVector[i].Key));
        }

        return offsets;
    }

    private Offset<GFB.SortedVectorIntKey>[] CreateSortedIntVectorOffsets(FlatBufferBuilder builder)
    {
        var intVector = this.sortedIntContainer.Vector;
        int intVectorLength = intVector.Count;

        var offsets = new Offset<GFB.SortedVectorIntKey>[intVectorLength];
        for (int i = 0; i < intVectorLength; ++i)
        {
            offsets[i] = GFB.SortedVectorIntKey.CreateSortedVectorIntKey(
                builder,
                intVector[i].Key);
        }

        return offsets;
    }

#endregion

    public virtual void FlatSharp_GetMaxSize()
    {
        this.fs_serializer.GetMaxSize(this.defaultContainer);
    }

    public virtual void FlatSharp_Serialize()
    {
        this.fs_serializer.Serialize(this.defaultContainer, this.fs_writeMemory);
    }

    public virtual void FlatSharp_Serialize_ValueStructs()
    {
        this.fs_serializer.Serialize(this.defaultContainerWithValueStructs, this.fs_writeMemory);
    }

    public virtual void FlatSharp_ParseAndTraverse()
    {
        var item = this.Parse<FooBarListContainer, InputBufferKind>(this.inputBuffer);
        this.TraverseFooBarContainer(item);
    }

    public virtual void FlatSharp_ParseAndTraversePartial()
    {
        var item = this.Parse<FooBarListContainer, InputBufferKind>(this.inputBuffer);
        this.TraverseFooBarContainerPartial(item);
    }

    public virtual void FlatSharp_ParseAndTraverse_ValueStructs()
    {
        var item = this.Parse<FooBarListContainer_ValueType, InputBufferKind>(this.inputBuffer);
        this.TraverseFooBarContainer(item);
    }

    public virtual void FlatSharp_ParseAndTraversePartial_ValueStructs()
    {
        var item = this.Parse<FooBarListContainer_ValueType, InputBufferKind>(this.inputBuffer);
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

    public virtual void FlatSharp_Serialize_ValueTableVector()
    {
        this.fs_serializer.Serialize(this.valueTableVector, this.fs_writeMemory);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T Parse<T, TInputBuffer>(TInputBuffer buffer)
        where TInputBuffer : IInputBuffer
        where T : class
    {
        return this.fs_serializer.Parse<T>(buffer);
    }

    public virtual void PBDN_Serialize()
    {
        this.pbdn_writeBuffer.Position = 0;
        ProtoBuf.Serializer.Serialize(this.pbdn_writeBuffer, this.defaultContainer);
    }

    public virtual void PBDN_Serialize_NonVirtual()
    {
        this.pbdn_writeBuffer.Position = 0;
        ProtoBuf.Serializer.Serialize(this.pbdn_writeBuffer, this.defaultContainerNonVirtual);
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

    public virtual void PBDN_ParseAndTraverse_NonVirtual()
    {
        this.pbdn_readBuffer.Position = 0;
        var item = ProtoBuf.Serializer.Deserialize<FooBarListContainerNonVirtual>(this.pbdn_readBuffer);
        this.TraverseFooBarContainer(item);
    }

    public virtual void PBDN_ParseAndTraversePartial_NonVirtual()
    {
        this.pbdn_readBuffer.Position = 0;
        var item = ProtoBuf.Serializer.Deserialize<FooBarListContainerNonVirtual>(this.pbdn_readBuffer);
        this.TraverseFooBarContainerPartial(item);
    }

    public virtual void MsgPack_Serialize_NonVirtual()
    {
        this.msgPackWriteData = MessagePack.MessagePackSerializer.Serialize(this.defaultContainerNonVirtual);
    }

    public virtual void MsgPack_ParseAndTraverse()
    {
        var item = MessagePack.MessagePackSerializer.Deserialize<FooBarListContainerNonVirtual>(this.msgPackWriteData, out int len);
        this.TraverseFooBarContainer(item);
    }

    public virtual void MsgPack_ParseAndTraversePartial()
    {
        var item = MessagePack.MessagePackSerializer.Deserialize<FooBarListContainerNonVirtual>(this.msgPackWriteData, out int len);
        this.TraverseFooBarContainerPartial(item);
    }
}
