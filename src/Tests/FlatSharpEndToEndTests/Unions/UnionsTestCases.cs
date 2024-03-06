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

using FlatSharp.Internal;
using System.Runtime.InteropServices;

namespace FlatSharpEndToEndTests.Unions;

[TestClass]
public class UnionsTestCases
{
    [TestMethod]
    public void Custom_Union_Accept_Works()
    {
        var c = this.Setup();

        UnionVisitor visitor = new();

        Assert.AreEqual(typeof(A), c.Value[0].Accept<UnionVisitor, Type>(visitor));
        Assert.AreEqual(typeof(B), c.Value[1].Accept<UnionVisitor, Type>(visitor));
        Assert.AreEqual(typeof(C), c.Value[2].Accept<UnionVisitor, Type>(visitor));
        Assert.AreEqual(typeof(D), c.Value[3].Accept<UnionVisitor, Type>(visitor));
    }

    [TestMethod]
    public void Custom_Union_Match_Works()
    {
        var c = this.Setup();

        Type[] expected = new[] { typeof(A), typeof(B), typeof(C), typeof(D) };

        for (int i = 0; i < c.Value.Count; ++i)
        {
            Type result = c.Value[i].Match(
                a => typeof(A),
                b => typeof(B),
                c => typeof(C),
                d => typeof(D));

            Assert.AreEqual(expected[i], result);
        }
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// In this test, the FBS file lies about the size of <see cref="System.Numerics.Vector{T}"/>. Depending on the machine,
    /// the size should be 16 (SSE), 32 (AVX2), or 64 (AVX512). The FBS defines it as 4 bytes.
    /// </summary>
    [TestMethod]
    public void Unsafe_Unions_ExternalStruct_WrongSize()
    {
        long[] headerTrailer = new[] { 0L, -1L };

        foreach (var guard in headerTrailer)
        {
            Wrapper<UnsafeUnion> wrapper = new();
            Span<byte> wrapperBytes = MemoryMarshal.CreateSpan(ref Unsafe.As<Wrapper<UnsafeUnion>, byte>(ref wrapper), Unsafe.SizeOf<Wrapper<UnsafeUnion>>());
            var ex = Assert.ThrowsException<FlatSharpInternalException>(() => wrapper.Union = new(new System.Numerics.Vector<byte>(1)));
            Assert.IsTrue(ex.Message.Contains("to have size 4. Unsafe.SizeOf reported size "));
        }
    }
#endif

    [TestMethod]
    public void Unsafe_Unions_ExternalStruct_CorrectSize()
    {
        long[] headerTrailer = new[] { 0L, -1L };

        foreach (var guard in headerTrailer)
        {
            Wrapper<UnsafeUnion> wrapper = new();

            wrapper.Header = guard;
            wrapper.Trailer = guard;
            wrapper.Union = new(new System.Numerics.Vector3(1, 2, 3));

            Assert.AreEqual(UnsafeUnion.ItemKind.ExtCorrect, wrapper.Union.Kind);
            Assert.AreEqual(1.0f, wrapper.Union.ExtCorrect.X);
            Assert.AreEqual(2.0f, wrapper.Union.ExtCorrect.Y);
            Assert.AreEqual(3.0f, wrapper.Union.ExtCorrect.Z);
            Assert.AreEqual(guard, wrapper.Header);
            Assert.AreEqual(guard, wrapper.Trailer);
        }
    }

    [TestMethod]
    public void Unsafe_Unions_DefinedStruct()
    {
        long[] headerTrailer = new[] { 0L, -1L };

        foreach (var guard in headerTrailer)
        {
            Wrapper<UnsafeUnion> wrapper = new();

            wrapper.Header = guard;
            wrapper.Trailer = guard;
            wrapper.Union = new(new ValueStructVec3 { X = 1, Y = 2, Z = 3 });

            Assert.AreEqual(UnsafeUnion.ItemKind.ValueStructVec3, wrapper.Union.Kind);

            Assert.AreEqual(1.0f, wrapper.Union.ValueStructVec3.X);
            Assert.AreEqual(2.0f, wrapper.Union.ValueStructVec3.Y);
            Assert.AreEqual(3.0f, wrapper.Union.ValueStructVec3.Z);
            Assert.AreEqual(guard, wrapper.Header);
            Assert.AreEqual(guard, wrapper.Trailer);
        }
    }

    private Container Setup()
    {
        Container c = new Container
        {
            Value = new MyUnion[]
            {
                new MyUnion(new A()),
                new MyUnion(new B()),
                new MyUnion(new C()),
                new MyUnion(new D()),
            }
        };

        byte[] buffer = new byte[Container.Serializer.GetMaxSize(c)];
        Container.Serializer.Write(buffer, c);

        return Container.Serializer.Parse(buffer);
    }

    private struct UnionVisitor : MyUnion.Visitor<Type>
    {
        public Type Visit(A item) => typeof(A);

        public Type Visit(B item) => typeof(B);

        public Type Visit(C item) => typeof(C);

        public Type Visit(D item) => typeof(D);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct Wrapper<T> where T : struct
    {
        // Guard bytes to ensure we aren't overwriting anything outside our scope.
        public long Header;

        public T Union;

        // Guard bytes.
        public long Trailer;
    }
}