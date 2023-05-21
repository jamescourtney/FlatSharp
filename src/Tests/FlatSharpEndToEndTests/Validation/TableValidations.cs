using FlatSharp.Internal;
using FlatSharpEndToEndTests.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatSharpEndToEndTests.Validation;

public class TableValidations
{
    [Fact]
    public void Root_UOffset_TooBig()
    {
        byte[] data =
        {
            100, 0, 0, 0,
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
        Assert.Equal(ValidationErrors.InvalidOffset, result.Message);
        Assert.False(result.Success);
    }

    [Fact]
    public void Root_UOffset_TooSmall()
    {
        byte[] data =
        {
            3, 0, 0, 0,
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
        Assert.Equal(ValidationErrors.InvalidUOffset, result.Message);
        Assert.False(result.Success);
    }

    [Fact]
    public void VTable_Bad_Table_Length()
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
    public void VTable_Field_Outside_Of_Table()
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
            0,
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

    [Fact]
    public void Bad_File_Id()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'E',
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
        Assert.Equal(ValidationErrors.WrongFileIdentifier, result.Message);
    }

    [Fact]
    public void Missing_File_Id()
    {
        byte[] data =
        {
            4, 0, 0, 0,
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
        Assert.Equal(ValidationErrors.WrongFileIdentifier, result.Message);
    }

    [Fact]
    public void Invalid_UOffset_Points_Within_Table()
    {
        byte[] data =
        {
            8, 0, 0, 0,
            (byte)'A', (byte)'B', (byte)'C', (byte)'D',
            244, 255, 255, 255, // soffset to vtable
            4, 0, 0, 0, // uoffset to string (invalid -- contained within table)
            12, 0, 0, 0, // uoffset to vector
            8, 0, 12, 0, // vtable length, table length
            4, 0, 8, 0, // offset of field 0, offset of field 1
            0, 0, 0, 0, // vector length
        };

        var result = ValidationTable.Serializer.Validate(data);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.UOffset_ContainedWithinParentObject, result.Message);
    }

    [Fact]
    public void Buffer_Too_Short()
    {
        var result = ValidationTable.Serializer.Validate(new byte[15]);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.BufferLessThanMinLength, result.Message);
    }
}
