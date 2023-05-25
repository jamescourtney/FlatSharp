/*
 * Copyright 2021 James Courtney
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

/// <summary>
/// Error messages for buffer validation.
/// </summary>
public static class ValidationHelpers
{
    public static bool TryValidateVTable<TInputBuffer>(
        TInputBuffer buffer,
        int tableOffset,
        out VTableGeneric vtable,
        out ushort tableLength,
        out ValidationResult error)
        where TInputBuffer : IInputBuffer
    {
        vtable = default;
        tableLength = 0;

        error = ValidateFixedSizeItemAtOffset(buffer, tableOffset, sizeof(int)); // soffset to vtable
        if (!error.Success)
        {
            return false;
        }

        int vtableOffset = tableOffset - buffer.ReadInt(tableOffset);
        const int MinVTableLength = 4;

        error = ValidateFixedSizeItemAtOffset(buffer, vtableOffset, MinVTableLength);
        if (!error.Success)
        {
            return false;
        }

        ushort vtableLength = buffer.ReadUShort(vtableOffset);
        if (vtableLength % 2 != 0)
        {
            error = new() { Message = ValidationErrors.VTable_OddLength };
            return false;
        }

        error = ValidateFixedSizeItemAtOffset(buffer, vtableOffset, vtableLength);
        if (!error.Success)
        {
            return false;
        }

        if (vtableLength < 4)
        {
            error = new() { Message = ValidationErrors.VTable_TooShort };
            return false;
        }

        tableLength = buffer.ReadUShort(vtableOffset + sizeof(ushort));
        if (tableLength < sizeof(int))
        {
            error = new() { Message = ValidationErrors.VTable_TableLengthTooShort };
            return false;
        }

        VTableGeneric.Create(buffer, tableOffset, out vtable);

        error = ValidateFixedSizeItemAtOffset(buffer, tableOffset, tableLength);
        if (!error.Success)
        {
            return false;
        }

        error = ValidationResult.OK;
        return true;
    }

    public static bool TryFollowUOffset<TBuffer>(this TBuffer buffer, ref int offset, out ValidationResult error)
        where TBuffer : IInputBuffer
    {
        error = ValidateFixedSizeItemAtOffset(buffer, offset, sizeof(uint));
        if (!error.Success)
        {
            return false;
        }

        uint uoffset = buffer.ReadUInt(offset);

        // Catch offsets too big and too small.
        if ((int)uoffset < sizeof(uint))
        {
            error = new ValidationResult()
            {
                Message = ValidationErrors.InvalidUOffset,
                Success = false,
            };
        }

        offset += (int)uoffset;

        if ((uint)offset >= buffer.Length)
        {
            error = new ValidationResult()
            {
                Message = ValidationErrors.InvalidOffset,
                Success = false,
            };
        }

        return error.Success;
    }

    public static ValidationResult ValidateFixedSizeItemAtOffset<TBuffer>(this TBuffer buffer, int offset, ushort size)
        where TBuffer : IInputBuffer
    {
        uint uoffset = (uint)offset;
        if (uoffset >= buffer.Length)
        {
            return new ValidationResult()
            {
                Message = ValidationErrors.InvalidOffset,
                Success = false,
            };
        }

        if (uoffset + size > buffer.Length)
        {
            return new ValidationResult()
            {
                Message = ValidationErrors.FixedSizeElementOverflows,
                Success = false,
            };
        }

        return ValidationResult.OK;
    }
}