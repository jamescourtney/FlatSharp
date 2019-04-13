
namespace ExperimentalBenchmark.Generated
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using FlatSharp;

    public sealed class Serializer : IGeneratedSerializer<Benchmark.FBBench.FooBarListContainer>
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(SpanWriter writer, Span<byte> target, Benchmark.FBBench.FooBarListContainer root, int offset, SerializationContext context)
        {
            WriteInlineValueOf_1e52fc1b6967478dab2d648f7bd2b8ab(writer, target, root, offset, context);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetMaxSize(Benchmark.FBBench.FooBarListContainer root)
        {
            return GetMaxSizeOf_1e52fc1b6967478dab2d648f7bd2b8ab(root);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Benchmark.FBBench.FooBarListContainer Parse(InputBuffer buffer, int offset)
        {
            return Read_1e52fc1b6967478dab2d648f7bd2b8ab(buffer, offset);
        }


        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlineValueOf_1e52fc1b6967478dab2d648f7bd2b8ab(SpanWriter writer, Span<byte> span, Benchmark.FBBench.FooBarListContainer item, int originalOffset, SerializationContext context)
        {

            int tableStart = context.AllocateSpace(22, sizeof(int));
            writer.WriteUOffset(span, originalOffset, tableStart, context);
            int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

            var vtable = context.VTableBuilder;
            vtable.StartObject(3);


            var index0Value = item.List;
            int index0Offset = 0;
            if (index0Value != default(System.Collections.Generic.IList<Benchmark.FBBench.FooBar>))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 4);
                index0Offset = currentOffset;
                vtable.SetOffset(0, currentOffset - tableStart);
                currentOffset += 4;
            }

            var index1Value = item.Initialized;
            int index1Offset = 0;
            if (index1Value != default(System.Boolean))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 1);
                index1Offset = currentOffset;
                vtable.SetOffset(1, currentOffset - tableStart);
                currentOffset += 1;
            }

            var index2Value = item.Fruit;
            int index2Offset = 0;
            if (index2Value != default(System.Int16))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 2);
                index2Offset = currentOffset;
                vtable.SetOffset(2, currentOffset - tableStart);
                currentOffset += 2;
            }

            var index3Value = item.Location;
            int index3Offset = 0;
            if (index3Value != default(System.String))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 4);
                index3Offset = currentOffset;
                vtable.SetOffset(3, currentOffset - tableStart);
                currentOffset += 4;
            }
            int tableLength = currentOffset - tableStart;
            context.Offset -= 22 - tableLength;
            int vtablePosition = vtable.EndObject(span, writer, tableLength);
            writer.WriteInt(span, tableStart - vtablePosition, tableStart, context);

            if (index0Offset != 0)
            {
                WriteInlineValueOf_ListVector_4102aef559a9461d855ddf7a13dfee76(writer, span, index0Value, index0Offset, context);
            }

            if (index1Offset != 0)
            {
                writer.WriteBool(span, index1Value, index1Offset, context);
            }

            if (index2Offset != 0)
            {
                writer.WriteShort(span, index2Value, index2Offset, context);
            }

            if (index3Offset != 0)
            {
                writer.WriteString(span, index3Value, index3Offset, context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlineValueOf_ListVector_4102aef559a9461d855ddf7a13dfee76(SpanWriter writer, Span<byte> span, System.Collections.Generic.IList<Benchmark.FBBench.FooBar> item, int originalOffset, SerializationContext context)
        {

            int count = item.Count;
            int vectorOffset = context.AllocateVector(4, count, 4);
            writer.WriteUOffset(span, originalOffset, vectorOffset, context);
            writer.WriteInt(span, count, vectorOffset, context);
            vectorOffset += sizeof(int);
            for (int i = 0; i < count; ++i)
            {
                var current = item[i];
                FlatSharp.SerializationHelpers.EnsureNonNull(current);
                WriteInlineValueOf_091fe8bc32ff48f196948bc932da000f(writer, span, current, vectorOffset, context);
                vectorOffset += 4;
            }
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlineValueOf_091fe8bc32ff48f196948bc932da000f(SpanWriter writer, Span<byte> span, Benchmark.FBBench.FooBar item, int originalOffset, SerializationContext context)
        {

            int tableStart = context.AllocateSpace(60, sizeof(int));
            writer.WriteUOffset(span, originalOffset, tableStart, context);
            int currentOffset = tableStart + sizeof(int); // skip past vtable soffset_t.

            var vtable = context.VTableBuilder;
            vtable.StartObject(3);


            var index0Value = item.Sibling;
            int index0Offset = 0;
            if (index0Value != default(Benchmark.FBBench.Bar))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 8);
                index0Offset = currentOffset;
                vtable.SetOffset(0, currentOffset - tableStart);
                currentOffset += 26;
            }

            var index1Value = item.Name;
            int index1Offset = 0;
            if (index1Value != default(System.String))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 4);
                index1Offset = currentOffset;
                vtable.SetOffset(1, currentOffset - tableStart);
                currentOffset += 4;
            }

            var index2Value = item.Rating;
            int index2Offset = 0;
            if (index2Value != default(System.Double))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 8);
                index2Offset = currentOffset;
                vtable.SetOffset(2, currentOffset - tableStart);
                currentOffset += 8;
            }

            var index3Value = item.PostFix;
            int index3Offset = 0;
            if (index3Value != default(System.Byte))
            {
                currentOffset += FlatSharp.SerializationHelpers.GetAlignmentError(currentOffset, 1);
                index3Offset = currentOffset;
                vtable.SetOffset(3, currentOffset - tableStart);
                currentOffset += 1;
            }
            int tableLength = currentOffset - tableStart;
            context.Offset -= 60 - tableLength;
            int vtablePosition = vtable.EndObject(span, writer, tableLength);
            writer.WriteInt(span, tableStart - vtablePosition, tableStart, context);

            if (index0Offset != 0)
            {
                WriteInlineValueOf_5a57adb797a449229eebcc14f5f6a5d2(writer, span, index0Value, index0Offset, context);
            }

            if (index1Offset != 0)
            {
                writer.WriteString(span, index1Value, index1Offset, context);
            }

            if (index2Offset != 0)
            {
                writer.WriteDouble(span, index2Value, index2Offset, context);
            }

            if (index3Offset != 0)
            {
                writer.WriteByte(span, index3Value, index3Offset, context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlineValueOf_5a57adb797a449229eebcc14f5f6a5d2(SpanWriter writer, Span<byte> span, Benchmark.FBBench.Bar item, int originalOffset, SerializationContext context)
        {
            WriteInlineValueOf_1cbcc23cbffb4e4da9e6e76fc4bc798b(writer, span, item.Parent, 0 + originalOffset, context);
            writer.WriteInt(span, item.Time, 16 + originalOffset, context);
            writer.WriteFloat(span, item.Ratio, 20 + originalOffset, context);
            writer.WriteUShort(span, item.Size, 24 + originalOffset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlineValueOf_1cbcc23cbffb4e4da9e6e76fc4bc798b(SpanWriter writer, Span<byte> span, Benchmark.FBBench.Foo item, int originalOffset, SerializationContext context)
        {
            writer.WriteULong(span, item.Id, 0 + originalOffset, context);
            writer.WriteShort(span, item.Count, 8 + originalOffset, context);
            writer.WriteSByte(span, item.Prefix, 10 + originalOffset, context);
            writer.WriteUInt(span, item.Length, 12 + originalOffset, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Benchmark.FBBench.FooBarListContainer Read_1e52fc1b6967478dab2d648f7bd2b8ab(InputBuffer memory, int offset)
        {
            return new tableReader_890dcdb8990345dd81368e2e6491950f(memory, offset + memory.ReadUOffset(offset));
        }


        private sealed class tableReader_890dcdb8990345dd81368e2e6491950f : Benchmark.FBBench.FooBarListContainer
        {
            private readonly InputBuffer buffer;
            private readonly int offset;

            private bool hasIndex0;
            private System.Collections.Generic.IList<Benchmark.FBBench.FooBar> index0Value;
            private bool hasIndex1;
            private System.Boolean index1Value;
            private bool hasIndex2;
            private System.Int16 index2Value;
            private bool hasIndex3;
            private System.String index3Value;

            public tableReader_890dcdb8990345dd81368e2e6491950f(InputBuffer buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;
            }


            public override System.Collections.Generic.IList<Benchmark.FBBench.FooBar> List
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex0)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 0);
                        if (absoluteLocation == 0)
                        {
                            this.index0Value = default(System.Collections.Generic.IList<Benchmark.FBBench.FooBar>);
                        }
                        else
                        {
                            this.index0Value = Read_ListVector_4102aef559a9461d855ddf7a13dfee76(buffer, absoluteLocation);
                        }
                        this.hasIndex0 = true;
                    }

                    return this.index0Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Boolean Initialized
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex1)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 1);
                        if (absoluteLocation == 0)
                        {
                            this.index1Value = default(System.Boolean);
                        }
                        else
                        {
                            this.index1Value = buffer.ReadBool(absoluteLocation);
                        }
                        this.hasIndex1 = true;
                    }

                    return this.index1Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Int16 Fruit
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex2)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 2);
                        if (absoluteLocation == 0)
                        {
                            this.index2Value = default(System.Int16);
                        }
                        else
                        {
                            this.index2Value = buffer.ReadShort(absoluteLocation);
                        }
                        this.hasIndex2 = true;
                    }

                    return this.index2Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.String Location
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex3)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 3);
                        if (absoluteLocation == 0)
                        {
                            this.index3Value = default(System.String);
                        }
                        else
                        {
                            this.index3Value = buffer.ReadString(absoluteLocation);
                        }
                        this.hasIndex3 = true;
                    }

                    return this.index3Value;
                }

                set { throw new NotMutableException(); }
            }

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static System.Collections.Generic.IList<Benchmark.FBBench.FooBar> Read_ListVector_4102aef559a9461d855ddf7a13dfee76(InputBuffer memory, int offset)
        {
            return new FlatBufferVector<Benchmark.FBBench.FooBar>(
                memory,
                offset + memory.ReadUOffset(offset),
                4,
                (b, o) => Read_091fe8bc32ff48f196948bc932da000f(b, o));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Benchmark.FBBench.FooBar Read_091fe8bc32ff48f196948bc932da000f(InputBuffer memory, int offset)
        {
            return new tableReader_a2162e0913d744c5b1bcf8807a941631(memory, offset + memory.ReadUOffset(offset));
        }


        private sealed class tableReader_a2162e0913d744c5b1bcf8807a941631 : Benchmark.FBBench.FooBar
        {
            private readonly InputBuffer buffer;
            private readonly int offset;

            private bool hasIndex0;
            private Benchmark.FBBench.Bar index0Value;
            private bool hasIndex1;
            private System.String index1Value;
            private bool hasIndex2;
            private System.Double index2Value;
            private bool hasIndex3;
            private System.Byte index3Value;

            public tableReader_a2162e0913d744c5b1bcf8807a941631(InputBuffer buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;
            }


            public override Benchmark.FBBench.Bar Sibling
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex0)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 0);
                        if (absoluteLocation == 0)
                        {
                            this.index0Value = default(Benchmark.FBBench.Bar);
                        }
                        else
                        {
                            this.index0Value = Read_5a57adb797a449229eebcc14f5f6a5d2(buffer, absoluteLocation);
                        }
                        this.hasIndex0 = true;
                    }

                    return this.index0Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.String Name
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex1)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 1);
                        if (absoluteLocation == 0)
                        {
                            this.index1Value = default(System.String);
                        }
                        else
                        {
                            this.index1Value = buffer.ReadString(absoluteLocation);
                        }
                        this.hasIndex1 = true;
                    }

                    return this.index1Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Double Rating
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex2)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 2);
                        if (absoluteLocation == 0)
                        {
                            this.index2Value = default(System.Double);
                        }
                        else
                        {
                            this.index2Value = buffer.ReadDouble(absoluteLocation);
                        }
                        this.hasIndex2 = true;
                    }

                    return this.index2Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Byte PostFix
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex3)
                    {
                        var buffer = this.buffer;
                        int absoluteLocation = buffer.GetAbsoluteTableFieldLocation(this.offset, 3);
                        if (absoluteLocation == 0)
                        {
                            this.index3Value = default(System.Byte);
                        }
                        else
                        {
                            this.index3Value = buffer.ReadByte(absoluteLocation);
                        }
                        this.hasIndex3 = true;
                    }

                    return this.index3Value;
                }

                set { throw new NotMutableException(); }
            }

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Benchmark.FBBench.Bar Read_5a57adb797a449229eebcc14f5f6a5d2(InputBuffer memory, int offset)
        {
            return new structReader_f4f073a4e96b46baaff947dfc5e2ed94(memory, offset);
        }


        private sealed class structReader_f4f073a4e96b46baaff947dfc5e2ed94 : Benchmark.FBBench.Bar
        {
            private readonly InputBuffer buffer;
            private readonly int offset;

            private bool hasIndex0;
            private Benchmark.FBBench.Foo index0Value;
            private bool hasIndex1;
            private System.Int32 index1Value;
            private bool hasIndex2;
            private System.Single index2Value;
            private bool hasIndex3;
            private System.UInt16 index3Value;

            public structReader_f4f073a4e96b46baaff947dfc5e2ed94(InputBuffer buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;
            }


            public override Benchmark.FBBench.Foo Parent
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex0)
                    {
                        this.index0Value = Read_1cbcc23cbffb4e4da9e6e76fc4bc798b(this.buffer, this.offset + 0);
                        this.hasIndex0 = true;
                    }

                    return this.index0Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Int32 Time
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex1)
                    {
                        this.index1Value = this.buffer.ReadInt(this.offset + 16);
                        this.hasIndex1 = true;
                    }

                    return this.index1Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Single Ratio
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex2)
                    {
                        this.index2Value = this.buffer.ReadFloat(this.offset + 20);
                        this.hasIndex2 = true;
                    }

                    return this.index2Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.UInt16 Size
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex3)
                    {
                        this.index3Value = this.buffer.ReadUShort(this.offset + 24);
                        this.hasIndex3 = true;
                    }

                    return this.index3Value;
                }

                set { throw new NotMutableException(); }
            }

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Benchmark.FBBench.Foo Read_1cbcc23cbffb4e4da9e6e76fc4bc798b(InputBuffer memory, int offset)
        {
            return new structReader_0f04d8b6b26949fab6fd48dd892376ac(memory, offset);
        }


        private sealed class structReader_0f04d8b6b26949fab6fd48dd892376ac : Benchmark.FBBench.Foo
        {
            private readonly InputBuffer buffer;
            private readonly int offset;

            private bool hasIndex0;
            private System.UInt64 index0Value;
            private bool hasIndex1;
            private System.Int16 index1Value;
            private bool hasIndex2;
            private System.SByte index2Value;
            private bool hasIndex3;
            private System.UInt32 index3Value;

            public structReader_0f04d8b6b26949fab6fd48dd892376ac(InputBuffer buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;
            }


            public override System.UInt64 Id
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex0)
                    {
                        this.index0Value = this.buffer.ReadULong(this.offset + 0);
                        this.hasIndex0 = true;
                    }

                    return this.index0Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.Int16 Count
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex1)
                    {
                        this.index1Value = this.buffer.ReadShort(this.offset + 8);
                        this.hasIndex1 = true;
                    }

                    return this.index1Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.SByte Prefix
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex2)
                    {
                        this.index2Value = this.buffer.ReadSByte(this.offset + 10);
                        this.hasIndex2 = true;
                    }

                    return this.index2Value;
                }

                set { throw new NotMutableException(); }
            }


            public override System.UInt32 Length
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if (!this.hasIndex3)
                    {
                        this.index3Value = this.buffer.ReadUInt(this.offset + 12);
                        this.hasIndex3 = true;
                    }

                    return this.index3Value;
                }

                set { throw new NotMutableException(); }
            }

        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMaxSizeOf_1e52fc1b6967478dab2d648f7bd2b8ab(Benchmark.FBBench.FooBarListContainer item)
        {
            int runningSum = 0;
            var indexValue_0 = item.List;
            if (indexValue_0 != null)
            {
                runningSum += GetMaxSizeOf_ListVector_4102aef559a9461d855ddf7a13dfee76(indexValue_0);
            }
            var indexValue_3 = item.Location;
            if (indexValue_3 != null)
            {
                runningSum += FlatSharp.SerializationHelpers.GetMaxSize(indexValue_3);
            }
            return runningSum + 31;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMaxSizeOf_ListVector_4102aef559a9461d855ddf7a13dfee76(System.Collections.Generic.IList<Benchmark.FBBench.FooBar> item)
        {
            int count = item.Count;
            int maxSize = 7;
            for (int i = 0; i < count; ++i)
            {
                var current = item[i];
                FlatSharp.SerializationHelpers.EnsureNonNull(current);
                maxSize += GetMaxSizeOf_091fe8bc32ff48f196948bc932da000f(current);
            }
            return maxSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMaxSizeOf_091fe8bc32ff48f196948bc932da000f(Benchmark.FBBench.FooBar item)
        {
            int runningSum = 0;
            var indexValue_1 = item.Name;
            if (indexValue_1 != null)
            {
                runningSum += FlatSharp.SerializationHelpers.GetMaxSize(indexValue_1);
            }
            return runningSum + 69;
        }
    }

}
