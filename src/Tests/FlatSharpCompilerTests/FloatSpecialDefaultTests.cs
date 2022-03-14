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

namespace FlatSharpTests.Compiler;

public class FloatSpecialDefaultTests
{
    /// <summary>
    /// Tests that we can compile a schema using special float constants.
    /// </summary>
    [Fact]
    public void TestFloatSpecialDefaultValues()
    {
        string schema = $@"
        namespace FloatSpecialDefaultTests;

        table FloatSpecialDefaultTable
        {{
            FloatNan : float = nan;
            FloatNInf : float = -inf;
            FloatPInf : float = +inf;
            FloatInf : float = inf;
            FloatNInfinity : float = -infinity;
            FloatPInfinity : float = +infinity;
            FloatInfinity : float = infinity;
        }}

        table DoubleSpecialDefaultTable
        {{
            DoubleNan : double = nan;
            DoubleNInf : double = -inf;
            DoublePInf : double = +inf;
            DoubleInf : double = inf;
            DoubleNInfinity : double = -infinity;
            DoublePInfinity : double = +infinity;
            DoubleInfinity : double = infinity;
        }}
        ";

        FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
    }
}
