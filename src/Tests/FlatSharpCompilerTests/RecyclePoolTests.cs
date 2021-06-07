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
    using System.Reflection;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RecyclePoolTests
    {
        [TestMethod]
        public void Attributes_Correct()
        {
            static void AssertRecyclePoolSize(Assembly asm, string typeName, int expectedPoolSize)
            {
                Type? table = asm.GetType(typeName);
                Assert.IsNotNull(table);
                var tableAttribute = table.GetCustomAttribute<FlatBufferTableAttribute>();
                var structAttribute = table.GetCustomAttribute<FlatBufferStructAttribute>();

                if (tableAttribute is not null)
                {
                    Assert.AreEqual(expectedPoolSize, tableAttribute.RecyclePoolSize);
                }
                else if (structAttribute is not null)
                {
                    Assert.AreEqual(expectedPoolSize, structAttribute.RecyclePoolSize);
                }
                else
                {
                    Assert.Fail();
                }
            }

            string schema = $@"
            namespace RecyclePoolTests;
            table NoPool {{ value : int; }}
            struct DefaultRecyclePoolSize ({MetadataKeys.RecyclePoolSize}) {{ value : int; }}
            table UnboundedRecyclePoolSize 
                ({MetadataKeys.RecyclePoolSize}:""-1"", {MetadataKeys.FileIdentifier}:""abcd"") 
            {{ 
                value : int; 
            }}
            table SpecificRecyclePoolSize ({MetadataKeys.RecyclePoolSize}:""2048"") {{ value : int; }}
            ";
            
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            AssertRecyclePoolSize(asm, "RecyclePoolTests.NoPool", 0);
            AssertRecyclePoolSize(asm, "RecyclePoolTests.DefaultRecyclePoolSize", 1024);
            AssertRecyclePoolSize(asm, "RecyclePoolTests.UnboundedRecyclePoolSize", -1);
            AssertRecyclePoolSize(asm, "RecyclePoolTests.SpecificRecyclePoolSize", 2048);
        }
    }
}
