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

namespace FlatSharpTests.Compiler;

public class DeserializationModesTests
{
    [Theory]
    [InlineData(FlatBufferDeserializationOption.Lazy)]
    [InlineData(FlatBufferDeserializationOption.Progressive)]
    [InlineData(FlatBufferDeserializationOption.Greedy)]
    [InlineData(FlatBufferDeserializationOption.GreedyMutable)]
    public void Only_Single_Mode(FlatBufferDeserializationOption singleOption)
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table Blah ({MetadataKeys.SerializerKind}) {{ foo : string; }}
        ";

        Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(fbs, new() { Deserializers = new[] { singleOption } });
        Type t = asm.GetTypes().Single(x => x.FullName == "Foo.Bar.Blah");
        Assert.NotNull(t);

        var item = Activator.CreateInstance(t);
        ISerializer serializer = (ISerializer)t.GetProperty("Serializer", BindingFlags.Public | BindingFlags.Static).GetValue(null);
        byte[] buffer = new byte[100];

        serializer.Write(buffer, item);

        foreach (FlatBufferDeserializationOption option in Enum.GetValues<FlatBufferDeserializationOption>())
        {
            if (option == singleOption)
            {
                serializer.Parse(buffer, option);
            }
            else
            {
                var ex = Assert.Throws<NotImplementedException>(() => serializer.Parse(buffer, option));
                Assert.Equal($"Deserializer type '{option}' was excluded from generation at compile time.", ex.Message);
            }
        }
    }

    [Fact]
    public void Code_Is_Smaller()
    {
        string fbs = $@"
            {MetadataHelpers.AllAttributes}
            namespace Foo.Bar;
            table Blah ({MetadataKeys.SerializerKind}) {{ foo : string; }}
        ";

        (_, string singleOption) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(fbs, new() { Deserializers = new[] { FlatBufferDeserializationOption.Lazy } });
        (_, string allOptions) = FlatSharpCompiler.CompileAndLoadAssemblyWithCode(fbs, new() { Deserializers = Enum.GetValues<FlatBufferDeserializationOption>().Distinct().ToList() });

        Assert.True(singleOption.Length < allOptions.Length);
    }
}
