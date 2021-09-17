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
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    public class WriteThroughTests
    {
        [Fact]
        public void WriteThrough_OnStructField()
        {
            static void Test(FlatBufferDeserializationOption option)
            {
                string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace ForceWriteTests;
                table Table ({MetadataKeys.SerializerKind}:""{option}"") {{ Struct:Struct; }}
                struct Struct ({MetadataKeys.WriteThrough}) {{ foo:int; bar:int ({MetadataKeys.WriteThrough}:""false""); }} 
                ";

                Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
                Type tableType = asm.GetType("ForceWriteTests.Table");
                Type structType = asm.GetType("ForceWriteTests.Struct");

                ISerializer serializer = (ISerializer)tableType.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                dynamic table = Activator.CreateInstance(tableType);
                table.Struct = (dynamic)Activator.CreateInstance(structType);
                table.Struct.foo = 42;
                table.Struct.bar = 65;

                byte[] data = new byte[100];
                serializer.Write(data, (object)table);

                dynamic parsed = serializer.Parse(data);
                parsed.Struct.foo = 100;

                Assert.Throws<NotMutableException>(() => parsed.Struct.bar = 22);

                dynamic parsed2 = serializer.Parse(data);
                Assert.Equal(100, (int)parsed2.Struct.foo);
                Assert.Equal(65, (int)parsed2.Struct.bar);
            }

            Test(FlatBufferDeserializationOption.Lazy);
            Test(FlatBufferDeserializationOption.Progressive);
        }
    }
}
