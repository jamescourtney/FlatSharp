using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

public class StringVectorTests
{
    [Theory]
    [ClassData(typeof(DeserializationOptionClassData))]
    public void StringTableField(FlatBufferDeserializationOption option) => Helpers.Repeat(() =>
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.True(vectors.IsInitialized);
        Assert.True(Vectors.IsStaticInitialized);

        IList<string> theBoys = vectors.Str;
        Assert.Equal(3, theBoys.Count);
        Assert.Equal("billy", theBoys[0]);
        Assert.Equal("mm", theBoys[1]);
        Assert.Equal("frenchie", theBoys[2]);

        Helpers.AssertMutationWorks(option, vectors, false, s => s.Str, new string[0]);
        Helpers.ValidateListVector(option, false, theBoys, string.Empty);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    });

    private Root CreateRoot(out byte[] expectedData)
    {
        static byte B(char c) => (byte)c;

        Root root = new()
        {
            Vectors = new()
            {
                Str = Helpers.CreateList("billy", "mm", "frenchie")
            }
        };

        expectedData = new byte[]
        {
            4, 0, 0, 0,
            248, 255, 255, 255,       // soffset to vtable
            12, 0, 0, 0,              // uoffset to Vectors subtable.

            // Vtable for Root
            8, 0, 8, 0,
            0, 0, 4, 0,

            248, 255, 255, 255, // soffset to vtable
            16, 0, 0, 0,        // uoffset to vector

            12, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            0, 0,               // field 2 (scalar)
            4, 0,               // field 3 (str)

            3, 0, 0, 0,         // vector length
            12, 0, 0, 0,
            20, 0, 0, 0,
            24, 0, 0, 0,

            5, 0, 0, 0,
            B('b'), B('i'), B('l'), B('l'), B('y'), 0,
            0, 0,

            2, 0, 0, 0,
            B('m'), B('m'), 0, 0,

            8, 0, 0, 0,
            B('f'), B('r'), B('e'), B('n'), 
            B('c'), B('h'), B('i'), B('e'),
            0,
        };

        return root;
    }
}
