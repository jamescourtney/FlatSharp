/*
 * Copyright 2020 James Courtney
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



namespace FlatSharp.Internal;


public struct BoolSpanComparer : ISpanComparer
{
    private readonly bool defaultValue;

    public BoolSpanComparer(bool defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        bool leftValue = leftExists ? left.ReadBool(0) : this.defaultValue;
        bool rightValue = rightExists ? right.ReadBool(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableBoolSpanComparer : ISpanComparer
{
    public NullableBoolSpanComparer(bool? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadBool(0).CompareTo(right.ReadBool(0));
    }
}


public struct UInt8SpanComparer : ISpanComparer
{
    private readonly byte defaultValue;

    public UInt8SpanComparer(byte defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        byte leftValue = leftExists ? left.ReadUInt8(0) : this.defaultValue;
        byte rightValue = rightExists ? right.ReadUInt8(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableUInt8SpanComparer : ISpanComparer
{
    public NullableUInt8SpanComparer(byte? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadUInt8(0).CompareTo(right.ReadUInt8(0));
    }
}


public struct Int8SpanComparer : ISpanComparer
{
    private readonly sbyte defaultValue;

    public Int8SpanComparer(sbyte defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        sbyte leftValue = leftExists ? left.ReadInt8(0) : this.defaultValue;
        sbyte rightValue = rightExists ? right.ReadInt8(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableInt8SpanComparer : ISpanComparer
{
    public NullableInt8SpanComparer(sbyte? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadInt8(0).CompareTo(right.ReadInt8(0));
    }
}


public struct UInt16SpanComparer : ISpanComparer
{
    private readonly ushort defaultValue;

    public UInt16SpanComparer(ushort defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        ushort leftValue = leftExists ? left.ReadUInt16(0) : this.defaultValue;
        ushort rightValue = rightExists ? right.ReadUInt16(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableUInt16SpanComparer : ISpanComparer
{
    public NullableUInt16SpanComparer(ushort? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadUInt16(0).CompareTo(right.ReadUInt16(0));
    }
}


public struct Int16SpanComparer : ISpanComparer
{
    private readonly short defaultValue;

    public Int16SpanComparer(short defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        short leftValue = leftExists ? left.ReadInt16(0) : this.defaultValue;
        short rightValue = rightExists ? right.ReadInt16(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableInt16SpanComparer : ISpanComparer
{
    public NullableInt16SpanComparer(short? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadInt16(0).CompareTo(right.ReadInt16(0));
    }
}


public struct Int32SpanComparer : ISpanComparer
{
    private readonly int defaultValue;

    public Int32SpanComparer(int defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        int leftValue = leftExists ? left.ReadInt32(0) : this.defaultValue;
        int rightValue = rightExists ? right.ReadInt32(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableInt32SpanComparer : ISpanComparer
{
    public NullableInt32SpanComparer(int? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadInt32(0).CompareTo(right.ReadInt32(0));
    }
}


public struct UInt32SpanComparer : ISpanComparer
{
    private readonly uint defaultValue;

    public UInt32SpanComparer(uint defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        uint leftValue = leftExists ? left.ReadUInt32(0) : this.defaultValue;
        uint rightValue = rightExists ? right.ReadUInt32(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableUInt32SpanComparer : ISpanComparer
{
    public NullableUInt32SpanComparer(uint? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadUInt32(0).CompareTo(right.ReadUInt32(0));
    }
}


public struct Int64SpanComparer : ISpanComparer
{
    private readonly long defaultValue;

    public Int64SpanComparer(long defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        long leftValue = leftExists ? left.ReadInt64(0) : this.defaultValue;
        long rightValue = rightExists ? right.ReadInt64(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableInt64SpanComparer : ISpanComparer
{
    public NullableInt64SpanComparer(long? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadInt64(0).CompareTo(right.ReadInt64(0));
    }
}


public struct UInt64SpanComparer : ISpanComparer
{
    private readonly ulong defaultValue;

    public UInt64SpanComparer(ulong defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        ulong leftValue = leftExists ? left.ReadUInt64(0) : this.defaultValue;
        ulong rightValue = rightExists ? right.ReadUInt64(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableUInt64SpanComparer : ISpanComparer
{
    public NullableUInt64SpanComparer(ulong? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadUInt64(0).CompareTo(right.ReadUInt64(0));
    }
}


public struct Float32SpanComparer : ISpanComparer
{
    private readonly float defaultValue;

    public Float32SpanComparer(float defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        float leftValue = leftExists ? left.ReadFloat32(0) : this.defaultValue;
        float rightValue = rightExists ? right.ReadFloat32(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableFloat32SpanComparer : ISpanComparer
{
    public NullableFloat32SpanComparer(float? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadFloat32(0).CompareTo(right.ReadFloat32(0));
    }
}


public struct Float64SpanComparer : ISpanComparer
{
    private readonly double defaultValue;

    public Float64SpanComparer(double defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        double leftValue = leftExists ? left.ReadFloat64(0) : this.defaultValue;
        double rightValue = rightExists ? right.ReadFloat64(0) : this.defaultValue;

        return leftValue.CompareTo(rightValue);
    }
}

[ExcludeFromCodeCoverage] // Not currently used.
public struct NullableFloat64SpanComparer : ISpanComparer
{
    public NullableFloat64SpanComparer(double? notUsed)
    {
    }

    public int Compare<TBuffer>(bool leftExists, TBuffer left, bool rightExists, TBuffer right)
        where TBuffer : IFlatBufferReaderWriter<TBuffer>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (!leftExists || !rightExists)
        {
            return leftExists.CompareTo(rightExists);
        }

        return left.ReadFloat64(0).CompareTo(right.ReadFloat64(0));
    }
}

