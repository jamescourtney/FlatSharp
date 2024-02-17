﻿/*
 * Copyright 2020 James Courtney
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
using System.Text;

namespace FlatSharpEndToEndTests.ClassLib;

public class SerializationHelpersTests
{
    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 100)]
    public void VTableMax(int a, int b)
    {
        Assert.Equal(Math.Max(a, b), SerializationHelpers.VTableMax(a, b));
    }
}
