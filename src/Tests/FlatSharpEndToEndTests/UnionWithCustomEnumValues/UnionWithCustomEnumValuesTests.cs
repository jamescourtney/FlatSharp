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
}