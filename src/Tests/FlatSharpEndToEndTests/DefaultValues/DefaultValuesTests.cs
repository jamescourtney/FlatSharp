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

namespace FlatSharpEndToEndTests.DefaultValues;

[TestClass]
public class DefaultValuesTests
{
    [TestMethod]
    public void DefaultValue_Enum_Negative()
    {
        Defaults defaults = new();

        Assert.AreEqual(long.MaxValue, (long)defaults.EnumLongMax);
        Assert.AreEqual((long)-1, (long)defaults.EnumLongNegOne);

        Assert.AreEqual(long.MaxValue, defaults.LongMax);
        Assert.AreEqual(-1, defaults.LongNegOne);

        Assert.AreEqual(0ul, defaults.ULongZero);

        // FlatC bug: ulong values over 2^63 are not supported.
        Assert.AreEqual(0ul, defaults.ULongMax);

        Assert.AreEqual(0ul, (ulong)defaults.EnumULongZero);

        // FlatC bug: ulong values over 2^63 are not supported.
        Assert.AreEqual(0ul, (ulong)defaults.EnumULongMax);
    }

    [TestMethod]
    public void Enum_Values()
    {
        Assert.AreEqual(-1L, (long)Int64Enum.NegOne);
        Assert.AreEqual(long.MaxValue, (long)Int64Enum.LongMax);

        Assert.AreEqual(0UL, (ulong)UInt64Enum.Zero);
        Assert.AreEqual(ulong.MaxValue, (ulong)UInt64Enum.Max);
    }
}
