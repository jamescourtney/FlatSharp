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

public class StructVectorValidations
{
    [Fact]
    public void OK()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            16, 0, 0, 0,        // uoffset to vector
            12, 0, 8, 0,        // vtable length, table length
            0, 0, 0, 0,         // offset of field 0, padding
            0, 0, 4, 0,         // offset of field 2, 3
            3, 0, 0, 0,         // length of vector
            1, 0, 0, 0,         // item 0
            0, 0, 0, 0,
            2, 0, 0, 0,         // item 1
            0, 0, 0, 0,
            3, 0, 0, 0,         // item 2
            0, 0, 0, 0,
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.True(result.Success);
    }

    [Fact]
    public void Vector_Absurdly_Long()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            16, 0, 0, 0,        // uoffset to vector
            12, 0, 8, 0,        // vtable length, table length
            0, 0, 0, 0,         // offset of field 0, padding
            0, 0, 4, 0,         // offset of field 2, 3
            255, 255, 255, 255, // length of vector
            1, 0, 0, 0,         // item 0
            0, 0, 0, 0,
            2, 0, 0, 0,         // item 1
            0, 0, 0, 0,
            3, 0, 0, 0,         // item 2
            0, 0, 0, 0,
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VectorNumberOfItemsTooLarge, result.Message);
    }

    [Fact]
    public void Vector_Points_Beyond_Buffer()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            16, 0, 0, 0,        // uoffset to vector
            12, 0, 8, 0,        // vtable length, table length
            0, 0, 0, 0,         // offset of field 0, padding
            0, 0, 4, 0,         // offset of field 2, 3
            4, 0, 0, 0,         // length of vector
            1, 0, 0, 0,         // item 0
            0, 0, 0, 0,
            2, 0, 0, 0,         // item 1
            0, 0, 0, 0,
            3, 0, 0, 0,         // item 2
            0, 0, 0, 0,
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VectorOverflows, result.Message);
    }
}

