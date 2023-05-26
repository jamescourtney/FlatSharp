using FlatSharp.Internal;
using FlatSharpEndToEndTests.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatSharpEndToEndTests.Validation;

public class UnionValidations
{
    [Fact]
    public void OK_With_Struct()
    {
    }

    [Fact]
    public void OK_With_String()
    {
        byte[] buffer =
        {
            8, 0, 0, 0,
            65, 66, 67, 68,             // identifier
            246, 255, 255, 255,         // vtable soffset
            24, 0, 0, 0,                // uoffset to string
            2, 0,                       // disriminator, padding

            18, 0, 9, 0,                // vtable length, table length
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 8, 0,                 // empty, offset of discriminator
            4, 0,                       // offset of union uoffset.

            6, 0, 0, 0,                 // string length
            102, 111, 111, 98, 97, 114, // string
            0,                          // null terminator
        };

        var result = ValidationTable.Serializer.Validate(buffer);
        Assert.True(result.Success);
    }

    [Fact]
    public void OK_With_Table()
    {
    }

    [Fact]
    public void Invalid_Discriminator()
    {
        byte[] buffer =
        {
            8, 0, 0, 0,
            65, 66, 67, 68,             // identifier
            246, 255, 255, 255,         // vtable soffset
            24, 0, 0, 0,                // uoffset to string
            240, 0,                       // disriminator, padding

            18, 0, 9, 0,                // vtable length, table length
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 8, 0,                 // empty, offset of discriminator
            4, 0,                       // offset of union uoffset.

            6, 0, 0, 0,                 // string length
            102, 111, 111, 98, 97, 114, // string
            0,                          // null terminator
        };

        var result = ValidationTable.Serializer.Validate(buffer);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.Union_UnknownDiscriminator, result.Message);
    }

    [Fact]
    public void Missing_Union_Offset()
    {
        byte[] buffer =
        {
            8, 0, 0, 0,
            65, 66, 67, 68,             // identifier
            246, 255, 255, 255,         // vtable soffset
            24, 0, 0, 0,                // uoffset to string
            2, 0,                       // disriminator, padding

            18, 0, 9, 0,                // vtable length, table length
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,                 // empty, offset of discriminator
            4, 0,                       // offset of union uoffset.

            6, 0, 0, 0,                 // string length
            102, 111, 111, 98, 97, 114, // string
            0,                          // null terminator
        };

        var result = ValidationTable.Serializer.Validate(buffer);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_MultiPart_NotAllPresent, result.Message);
    }

    [Fact]
    public void Missing_Union_Discriminator()
    {
        byte[] buffer =
        {
            8, 0, 0, 0,
            65, 66, 67, 68,             // identifier
            246, 255, 255, 255,         // vtable soffset
            24, 0, 0, 0,                // uoffset to string
            2, 0,                       // disriminator, padding

            18, 0, 9, 0,                // vtable length, table length
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,                 // empty, offset of discriminator
            4, 0,                       // offset of union uoffset.

            6, 0, 0, 0,                 // string length
            102, 111, 111, 98, 97, 114, // string
            0,                          // null terminator
        };

        var result = ValidationTable.Serializer.Validate(buffer);
        Assert.False(result.Success);
        Assert.Equal(ValidationErrors.VTable_MultiPart_NotAllPresent, result.Message);
    }
}
