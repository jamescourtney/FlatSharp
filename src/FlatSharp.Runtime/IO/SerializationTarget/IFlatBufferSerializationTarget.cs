/*
 * Copyright 2024 James Courtney
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

using System.Buffers.Binary;

namespace FlatSharp;

/// <summary>
/// Represents a target for a FlatBuffer serialization operation.
/// </summary>
public interface IFlatBufferSerializationTarget<out T>
    where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{
    /// <summary>
    /// Gets or sets the value at the given index.
    /// </summary>
    byte this[long index] { get; set; }
    
    /// <summary>
    /// Gets the length.
    /// </summary>
    long Length { get; }

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start, long length);

    /// <summary>
    /// Slices this object.
    /// </summary>
    T Slice(long start);

    /// <summary>
    /// Returns a Span{byte} over the given range.
    /// </summary>
    Span<byte> AsSpan(long start, int length);
}

/// <summary>
/// Extensions for IFlatBufferSerializationTarget
/// </summary>
public static class FlatBufferSerializationTargetExtensions
{
    public static void WriteBool<T>(this T target, long offset, bool value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target.WriteUInt8(offset, value ? SerializationHelpers.True : SerializationHelpers.False);
    }
    
    public static void WriteUInt8<T>(this T target, long offset, byte value)
        where T : IFlatBufferSerializationTarget<T>
    #if NET9_0_OR_GREATER
        , allows ref struct
    #endif
    {
        target[offset] = value;
    }
    
    public static void WriteInt8<T>(this T target, long offset, sbyte value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        target[offset] = unchecked((byte)value);
    }
    
    public static void WriteUInt16<T>(this T target, long offset, ushort value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteUInt16LittleEndian(
            target.AsSpan(offset, sizeof(ushort)),
            value);
    }
    
    public static void WriteInt16<T>(this T target, long offset, short value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteInt16LittleEndian(
            target.AsSpan(offset, sizeof(short)),
            value);
    }
    
    public static void WriteUInt32<T>(this T target, long offset, uint value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteUInt32LittleEndian(
            target.AsSpan(offset, sizeof(uint)),
            value);
    }
    
    public static void WriteInt32<T>(this T target, long offset, int value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteInt32LittleEndian(
            target.AsSpan(offset, sizeof(int)),
            value);
    }
    
    public static void WriteUInt64<T>(this T target, long offset, ulong value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteUInt64LittleEndian(
            target.AsSpan(offset, sizeof(ulong)),
            value);
    }
    
    public static void WriteInt64<T>(this T target, long offset, long value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        BinaryPrimitives.WriteInt64LittleEndian(
            target.AsSpan(offset, sizeof(long)),
            value);
    }
    
    
    public static void WriteFloat32<T>(this T target, long offset, float value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        
#if NETSTANDARD
        ScalarSpanReader.FloatLayout floatLayout = new ScalarSpanReader.FloatLayout
        {
            value = value
        };
        
        target.WriteUInt32(offset, floatLayout.bytes);
#else
        BinaryPrimitives.WriteSingleLittleEndian(
            target.AsSpan(offset, sizeof(float)),
            value);
#endif
    }
    
    public static void WriteFloat64<T>(this T target, long offset, double value)
        where T : IFlatBufferSerializationTarget<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
#if NETSTANDARD
        target.WriteInt64(offset, BitConverter.DoubleToInt64Bits(value));
#else
        BinaryPrimitives.WriteDoubleLittleEndian(
            target.AsSpan(offset, sizeof(double)),
            value);  
#endif   
    }
}