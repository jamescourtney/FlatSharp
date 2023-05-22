/*
 * Copyright 2023 James Courtney
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

using FlatSharp.Internal;
using System.IO;

namespace FlatSharpEndToEndTests.Validation;

public class StructValidations
{
    [Fact]
    public void OK()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            240, 255, 255, 255, // soffset to vtable
            0, 0, 0, 0,    // padding
            12, 0, 0, 0,   // value (ulong)
            0, 0, 0, 0,

            10, 0, 16, 0, // vtable length, table length
            0, 0, 0, 0,   // offset of field 0, padding
            8, 0,         // offset of field 2
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.True(result.Success);
    }

    [Fact]
    public void Table_Too_Short()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            240, 255, 255, 255, // soffset to vtable
            0, 0, 0, 0,    // padding
            12, 0, 0, 0,   // value (ulong)
            0, 0, 0, 0,

            10, 0, 15, 0, // vtable length, table length
            0, 0, 0, 0,   // offset of field 0, padding
            8, 0,         // offset of field 2
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_FieldBeyondTableBoundary, result.Message);
    }
}

