using FlatSharp.Internal;
using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpStrykerTests;

[TestClass]
public class MemoryVectorTests
{
    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void MemoryVector(FlatBufferDeserializationOption option)
    {
        Root root = CreateRoot(out byte[] expectedData);
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Vectors vectors = parsed.Vectors;
        Assert.IsTrue(vectors.IsInitialized);
        Assert.IsTrue(Vectors.IsStaticInitialized);

        Memory<byte> vector = vectors.Memory.Value;
        Assert.AreEqual(5, vector.Length);
        for (int i = 0; i < vector.Length; ++i)
        {
            Assert.AreEqual(i + 1, vector.Span[i]);
        }

        Helpers.AssertMutationWorks(option, vectors, false, s => s.Memory, new byte[0]);
        Helpers.AssertSequenceEqual(expectedData, buffer);
    }

    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void MemoryVector_NotPresent(FlatBufferDeserializationOption option)
    {
        Root root = new Root() { Vectors = new() };
        Root parsed = root.SerializeAndParse(option, out byte[] buffer);

        Assert.IsNull(parsed.Vectors.Memory);
    }

    private Root CreateRoot(out byte[] expectedData)
    {
        Root root = new()
        {
            Vectors = new()
            {
                Memory = new byte[] { 1, 2, 3, 4, 5, }
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

            10, 0,              // vtable length
            8, 0,               // table length
            0, 0,               // field 0
            0, 0,               // field 1
            4, 0,               // field 2 (memory)
            0, 0,               // padding

            5, 0, 0, 0,         // vector length
            1, 2, 3, 4, 5,
        };

        return root;
    }
}
