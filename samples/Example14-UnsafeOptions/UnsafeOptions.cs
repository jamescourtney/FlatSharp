/*
 * Copyright 2021 James Courtney
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

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Samples.UnsafeOptions;

/// <summary>
/// This sample shows various unsafe optimizaitons that FlatSharp can make.
/// These unsafe operations are generally more performant than their safe alternatives,
/// but carry the general risks that Unsafe code does. These are "at your own risk"
/// options. While they are tested and validated in FlatSharp, you own ultimate
/// validation and testing if you choose to use these features.
/// </summary>
public class UnsafeOptionsExample : IFlatSharpSample
{
    public bool HasConsoleOutput => false;

    public void Run()
    {
        FourBytes four = new FourBytes { A = 4 };
        SixteenBytes sixteen = new SixteenBytes();

        for (int i = 0; i < sixteen.Data_Length; ++i)
        {
            sixteen.Data(i) = (byte)i;
        }

        Vector3 vec3 = new Vector3(1, 2, 3);
        TwentyBytes twenty = new TwentyBytes { Four = four, Sixteen = sixteen };

        RootTable table = new()
        {
            Union = new UnsafeUnion[]
            {
                new UnsafeUnion(four),
                new UnsafeUnion(sixteen),
                new UnsafeUnion(vec3),
                new UnsafeUnion(twenty),
            }
        };

        Assert.True(
            Unsafe.SizeOf<UnsafeUnion>() == 21,
            "The size of unsafe unsions is that of their largest member plus 1.");

        Assert.True(
            Unsafe.SizeOf<TwentyBytes>() == 20,
            "Size is what we expected!");
    }
}
