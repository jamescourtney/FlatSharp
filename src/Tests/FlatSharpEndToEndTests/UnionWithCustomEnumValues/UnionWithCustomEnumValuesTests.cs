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

namespace FlatSharpEndToEndTests.UnionWithCustomEnumValues;

[TestClass]
public class UnionWithCustomEnumValuesTests
{
    [TestMethod]
    public void UnionWithCustomEnumValues_ItemKind()
    {
        Assert.AreEqual((byte)C.ItemKind.A, 2);
        Assert.AreEqual((byte)C.ItemKind.B, 4);
    }

    [TestMethod]
    public void UnionWithCustomEnumValues_Discriminator()
    {
        var a = new A();
        var c = new C(a);
        Assert.AreEqual(c.Discriminator, 2);

        var b = new B();
        c = new C(b);
        Assert.AreEqual(c.Discriminator, 4);
    }

    [TestMethod]
    public void UnionWithCustomEnumValues_Accept()
    {
        C c = new C(new A());

        Assert.AreEqual(2, c.Discriminator);
        Assert.AreEqual(C.ItemKind.A, c.Kind);

        Assert.IsNotNull(c.Item1);
        Assert.ThrowsException<InvalidOperationException>(() => c.Item2);

        Assert.AreEqual("A", c.Accept<Visitor, string>(new()));

        Assert.AreEqual(
            "A",
            c.Match(
                (A a) => "A",
                (B b) => "B"));

        c = new C(new B());

        Assert.AreEqual(4, c.Discriminator);
        Assert.AreEqual(C.ItemKind.B, c.Kind);

        Assert.IsNotNull(c.Item2);
        Assert.ThrowsException<InvalidOperationException>(() => c.Item1);

        Assert.AreEqual("B", c.Accept<Visitor, string>(new()));

        Assert.AreEqual(
            "B",
            c.Match(
                (A a) => "A",
                (B b) => "B"));
    }

    [TestMethod]
    public void UnionWithCustomEnumValues_Clone()
    {
        Outer outer = new()
        {
            Union = new(new B())
        };

        Outer parsed = outer.SerializeAndParse();
        Assert.AreEqual(4, parsed.Union.Value.Discriminator);

        Outer cloned = new Outer(parsed);
        Assert.AreEqual(4, cloned.Union.Value.Discriminator);
    }

    private struct Visitor : C.Visitor<string>
    {
        public string Visit(A item) => typeof(A).Name;

        public string Visit(B item) => typeof(B).Name;
    }
}