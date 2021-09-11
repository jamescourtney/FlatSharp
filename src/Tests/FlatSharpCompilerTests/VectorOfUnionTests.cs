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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using FlatSharp.TypeModel;
    using Xunit;

    
    public class VectorOfUnionTests
    {
        [Fact]
        public void VectorOfUnion_CompilerTests()
        {
            foreach (var vectorKind in new[] { "IList", "IReadOnlyList", "Array" })
            {
                foreach (FlatBufferDeserializationOption option in Enum.GetValues(typeof(FlatBufferDeserializationOption)))
                {
                    if (vectorKind == "Array" && option != FlatBufferDeserializationOption.Greedy && option != FlatBufferDeserializationOption.GreedyMutable)
                    {
                        continue;
                    }

                    this.RunTest(vectorKind, option);
                }
            }
        }

        private void RunTest(string vectorKind, FlatBufferDeserializationOption option)
        {
            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace VectorOfUnionTests;
            
            union Union {{ Foo, Table2 }}

            table Table ({MetadataKeys.SerializerKind}:""{option}"") {{
                Vector:[Union] (fs_vector:""{vectorKind}"");
            }}

            struct Foo {{
              A:int;
              B:uint64;
            }}
            table Table2 {{
                A:int;
                B:[string];
            }}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(
                schema, 
                new());
        }
    }
}
