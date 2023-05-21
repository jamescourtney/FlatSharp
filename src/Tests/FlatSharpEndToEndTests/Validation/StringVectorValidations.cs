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

public class StringVectorValidations
{
    [Fact]
    public void OK()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to vector

            8, 0, 8, 0, // vtable length, table length
            0, 0, 4, 0, // offset of field 0, offset of field 1

            3, 0, 0, 0, // vector length
            12, 0, 0, 0, // item 1 uoffset
            16, 0, 0, 0, // item 2 uoffset
            20, 0, 0, 0, // item 3 uoffset

            1, 0, 0, 0, // string length
            (byte)'a', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'b', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'c', 0 // content, null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.True(result.Success);
    }

    [Fact]
    public void Vector_Overflows_Buffer()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to vector

            8, 0, 8, 0, // vtable length, table length
            0, 0, 4, 0, // offset of field 0, offset of field 1

            100, 0, 0, 0, // vector length
            12, 0, 0, 0, // item 1 uoffset
            16, 0, 0, 0, // item 2 uoffset
            20, 0, 0, 0, // item 3 uoffset

            1, 0, 0, 0, // string length
            (byte)'a', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'b', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'c', 0 // content, null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VectorOverflows, result.Message);
    }

    [Fact]
    public void Too_Many_Items()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to vector

            8, 0, 8, 0, // vtable length, table length
            0, 0, 4, 0, // offset of field 0, offset of field 1

            255, 255, 255, 255, // vector length
            12, 0, 0, 0, // item 1 uoffset
            16, 0, 0, 0, // item 2 uoffset
            20, 0, 0, 0, // item 3 uoffset

            1, 0, 0, 0, // string length
            (byte)'a', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'b', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'c', 0 // content, null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VectorNumberOfItemsTooLarge, result.Message);
    }

    [Fact]
    public void Null_Item()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to vector

            8, 0, 8, 0, // vtable length, table length
            0, 0, 4, 0, // offset of field 0, offset of field 1

            3, 0, 0, 0, // vector length
            0, 0, 0, 0, // item 1 uoffset
            16, 0, 0, 0, // item 2 uoffset
            20, 0, 0, 0, // item 3 uoffset

            1, 0, 0, 0, // string length
            (byte)'a', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'b', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'c', 0 // content, null terminator
        };
        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.InvalidUOffset, result.Message);
    }

    [Fact]
    public void Invalid_UOffset_Points_Within_Vector()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to vector

            8, 0, 8, 0, // vtable length, table length
            0, 0, 4, 0, // offset of field 0, offset of field 1

            3, 0, 0, 0, // vector length
            4, 0, 0, 0, // item 1 uoffset; points to the next item (contained within the parent vector).
            16, 0, 0, 0, // item 2 uoffset
            20, 0, 0, 0, // item 3 uoffset

            1, 0, 0, 0, // string length
            (byte)'a', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'b', 0, 1, 1, // content, null terminator, padding

            1, 0, 0, 0, // string length
            (byte)'c', 0 // content, null terminator
        };
        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.UOffset_ContainedWithinParentObject, result.Message);
    }
}

