/*
 * Copyright 2022 James Courtney
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

using System.Runtime.InteropServices;

namespace FlatSharp.Internal;

internal static class VTableHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Parse<TInputBuffer, TVTable>(TInputBuffer inputBuffer, int offset, out TVTable item)
        where TInputBuffer : IInputBuffer
        where TVTable : struct, IVTable
    {
        inputBuffer.InitializeVTable(
            offset,
            out _,
            out _,
            out ReadOnlySpan<byte> fieldData);

        if (fieldData.Length >= Unsafe.SizeOf<TVTable>())
        {
            item = Unsafe.ReadUnaligned<TVTable>(ref MemoryMarshal.GetReference(fieldData));
        }
        else
        {
#if NETSTANDARD2_0
            Span<byte> data = stackalloc byte[Unsafe.SizeOf<TVTable>()];
            fieldData.CopyTo(data);
            item = Unsafe.ReadUnaligned<TVTable>(ref MemoryMarshal.GetReference(data));
#else
            item = default;
            Span<byte> itemSpan = MemoryMarshal.Cast<TVTable, byte>(MemoryMarshal.CreateSpan(ref item, 1));
            fieldData.CopyTo(itemSpan);
#endif
        }
    }
}
