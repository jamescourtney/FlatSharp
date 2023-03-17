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

public class DefaultValuesTests
{
    [Fact]
    public void DefaultValue_Enum_Negative()
    {
        Defaults defaults = new();

        Assert.Equal(long.MaxValue, (long)defaults.EnumLongMax);
        Assert.Equal((long)-1, (long)defaults.EnumLongNegOne);

        Assert.Equal(long.MaxValue, defaults.LongMax);
        Assert.Equal(-1, defaults.LongNegOne);

        Assert.Equal(0ul, defaults.ULongZero);
        Assert.Equal(0ul, defaults.ULongMax);

        Assert.Equal(0ul, (ulong)defaults.EnumULongZero);
        Assert.Equal(0ul, (ulong)defaults.EnumULongMax);
    }
}
