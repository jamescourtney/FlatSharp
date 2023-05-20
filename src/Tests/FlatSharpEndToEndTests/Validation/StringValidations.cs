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

public class StringValidations
{
    [Fact]
    public void OK()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            4, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
            0, // null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.True(result.Success);
    }

    [Fact]
    public void No_Null_Terminator()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            4, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
            5, 5, 5,
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.NoNullTerminator, result.Message);
    }

    [Fact]
    public void Length_Too_Long()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            255, 255, 255, 255, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
            5, 5, 5,
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.String_TooLong, result.Message);
    }

    [Fact]
    public void Overflows()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            16, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.String_Overflows, result.Message);
    }


    [Fact]
    public void Bad_Table_Length()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 3, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            16, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_TableLengthTooShort, result.Message);
    }

    [Fact]
    public void Field_Outside_Of_Table()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            6, 0, 6, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            16, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_FieldBeyondTableBoundary, result.Message);
    }

    [Fact]
    public void VTable_OddLength()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            7, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            4, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
            0, // null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_OddLength, result.Message);
    }

    [Fact]
    public void VTable_TooShort()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            248, 255, 255, 255, // soffset to vtable
            12, 0, 0, 0, // uoffset to string
            2, 0, 8, 0, // vtable length, table length
            4, 0, 0, 0, // offset of field 0, padding
            4, 0, 0, 0, // string length
            (byte)'e', (byte)'f', (byte)'g', (byte)'h', // content
            0, // null terminator
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_TooShort, result.Message);
    }
}

