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

using System.IO;

namespace FlatSharp.Internal;

[ExcludeFromCodeCoverage]
public static class FSThrow
{
    #region InvalidOperation

    [DoesNotReturn]
    public static void InvalidOperation(string message)
        => throw new InvalidOperationException(message);

    [DoesNotReturn]
    public static void InvalidOperation_SizeNotMultipleOfAlignment(Type elementType, int size, int alignment)
        => throw new InvalidOperationException($"Type '{elementType.FullName}' does not support Unsafe Span operations because the size ({size}) is not a multiple of the alignment ({alignment}).");

    [DoesNotReturn]
    public static void InvalidOperation_RequiredPropertyNotSet(string propertyName)
        => throw new InvalidOperationException($"Table property '{propertyName}' is marked as required, but was not set.");

    [DoesNotReturn]
    public static T InvalidOperation_InvalidUnionDiscriminator<T>(byte discriminator)
        => throw new InvalidOperationException($"Unexpected union discriminator value '{discriminator}' for Union {typeof(T).FullName}");

    [DoesNotReturn]
    public static void InvalidOperation_UnionIsNotOfType()
        => throw new InvalidOperationException("The union is not of the requested type.");

    [DoesNotReturn]
    public static void InvalidOperation_AotHelper()
        => throw new InvalidOperationException("AotHelper is not intended to be invoked");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T InvalidOperation<T>(string message)
    {
        InvalidOperation(message);
        return default;
    }

    #endregion

    #region InvalidData

    [DoesNotReturn]
    public static void InvalidData_UOffsetTooSmall(uint uoffset)
        => throw new InvalidDataException($"FlatBuffer was in an invalid format: Decoded uoffset_t had value less than {sizeof(uint)}. Value = {uoffset}");

    [DoesNotReturn]
    public static void InvalidData_VTableTooShort()
        => throw new InvalidDataException("FlatBuffer was in an invalid format: VTable was not long enough to be valid.");

    [DoesNotReturn]
    public static void InvalidData_InvalidNull()
        => throw new InvalidDataException("FlatSharp encountered a null reference in an invalid context, such as a vector. Vectors are not permitted to have null objects.");

    [DoesNotReturn]
    public static void InvalidData_DepthLimit()
        => throw new InvalidDataException("FlatSharp passed the configured depth limit when deserializing. This can be configured with 'IGeneratedSerializer.WithSettings'.");

    [DoesNotReturn]
    public static void InvalidData_RequiredPropertyNotSet(string propertyName)
        => throw new InvalidDataException($"Table property '{propertyName}' is marked as required, but was missing from the buffer.");

    [DoesNotReturn]
    public static void InvalidData_UnionVectorMismatchedLength()
        => throw new InvalidDataException("Union vector had mismatched number of discriminators and offsets.");

    [DoesNotReturn]
    public static void InvalidData_UnionOnlyPartiallyPresent(string fieldName)
        => throw new InvalidDataException($"FlatBuffer table property '{fieldName}' was only partially included in the buffer.");

    [DoesNotReturn]
    public static void InvalidData(string message) => throw new InvalidDataException(message);

    #endregion

    #region Argument

    [DoesNotReturn]
    public static void ArgumentOutOfRange(string paramName) => throw new ArgumentOutOfRangeException(paramName);

    [DoesNotReturn]
    public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ArgumentNull<T>(string paramName)
    {
        ArgumentNull(paramName);
        return default;
    }

    [DoesNotReturn]
    public static void ArgumentNull(string paramName, string message) => throw new ArgumentNullException(paramName, message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ArgumentNull<T>(string paramName, string message)
    {
        ArgumentNull(paramName, message);
        return default;
    }

    [DoesNotReturn]
    public static void Argument(string message) => throw new ArgumentException(message);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Argument<T>(string message)
    {
        Argument(message);
        return default;
    }

    #endregion

    #region BufferTooSmall

    [DoesNotReturn]
    public static void BufferTooSmall(int sizeNeeded)
    {
        throw new BufferTooSmallException
        {
            SizeNeeded = sizeNeeded
        };
    }

    #endregion

    #region NotMutable

    [DoesNotReturn]
    public static void NotMutable(string message) => throw new NotMutableException(message);

    [DoesNotReturn]
    public static void NotMutable_GreedyMutableWriteThrough()
        => throw new NotMutableException("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotMutable<T>(string message)
    {
        NotMutable(message);
        return default;
    }

    [DoesNotReturn]
    public static void NotMutable() => throw new NotMutableException();

    [DoesNotReturn]
    public static bool NotMutable_DeserializedVector()
    {
        FSThrow.NotMutable("FlatBufferVector does not support this operation.");
        return false;
    }

    [DoesNotReturn]
    public static T NotMutable<T>() => throw new NotMutableException();

    #endregion

    #region KeyNotFound

    [DoesNotReturn]
    public static void KeyNotFound() => throw new KeyNotFoundException();

    #endregion

    #region IndexOutOfRange

    [DoesNotReturn]
    public static void IndexOutOfRange() => throw new IndexOutOfRangeException();

    [DoesNotReturn]
    public static T IndexOutOfRange<T>() => throw new IndexOutOfRangeException();

    #endregion

    #region NotSupported

    [DoesNotReturn]
    public static void NotSupported_NativeArray_NonPinned() 
        => throw new NotSupportedException("Non-greedy parsing of a NativeArray requires a pinned buffer.");

    #endregion
}
