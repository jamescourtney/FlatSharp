/*
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

namespace FlatSharpTests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Xunit;

    
    public class RealExamples
    {
        [Theory]
        [InlineData(FlatBufferDeserializationOption.Greedy, "Array", typeof(string[]))]
        [InlineData(FlatBufferDeserializationOption.GreedyMutable, "Array", typeof(string[]))]
        [InlineData(FlatBufferDeserializationOption.Progressive, "IList", typeof(IList<string>))]
        [InlineData(FlatBufferDeserializationOption.Lazy, "IList", typeof(IList<string>))]
        public void MonsterTest(FlatBufferDeserializationOption option, string arrayVectorKind, Type arrayVectorType)
        {
            // https://github.com/google/flatbuffers/blob/master/samples/monster.fbs
            string schema = $@"
{MetadataHelpers.AllAttributes}
namespace MyGame;

enum Color:byte {{ Red = 0, Green, Blue = 2 }}
union Equipment {{ Weapon, Vec3, Vec4, Monster }}

struct Vec3 {{
  x:float;
  y:float;
  z:float;
}}

struct Vec4 {{
  x:float;
  y:float;
  z:float;
  t:float;
}}

table Monster ({MetadataKeys.SerializerKind}:""{option}"") {{
  pos:Vec3;
  mana:short = 150;
  hp:short = 100;
  name:string;
  friendly:bool = false ({MetadataKeys.Deprecated});
  inventory:[ubyte];
  color:Color = Blue;
  weapons:[Weapon];
  equipped:Equipment;
  path:[Vec3];
  vec4:Vec4;
  FakeVector1:[string] ({MetadataKeys.VectorKind}:""IReadOnlyList"");
  FakeVector2:[string] ({MetadataKeys.VectorKind}:""{arrayVectorKind}"");
  FakeVector3:[string] ({MetadataKeys.VectorKind}:""IList"");
  FakeVector4:[string];
  FakeMemoryVector:[ubyte] ({MetadataKeys.VectorKind}:""Memory"");
  FakeMemoryVectorReadOnly:[ubyte] ({MetadataKeys.VectorKind}:""ReadOnlyMemory"");
}}

table Weapon {{
  name:string;
  damage:short;
}}";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type weaponType = asm.GetType("MyGame.Weapon");
            Type monsterType = asm.GetTypes().Single(x => x.FullName == "MyGame.Monster");
            object monster = Activator.CreateInstance(monsterType);
            dynamic dMonster = monster;

            Type vec4Type = asm.GetTypes().Single(x => x.FullName == "MyGame.Vec4");
            Type vec3Type = asm.GetTypes().Single(x => x.FullName == "MyGame.Vec3");
            object vec = Activator.CreateInstance(vec3Type);
            dynamic dVec = vec;

            Assert.Equal((short)150, dMonster.mana);
            Assert.Equal((short)100, dMonster.hp);
            Assert.False(dMonster.friendly);
            Assert.Equal("Blue", dMonster.color.ToString());
            Assert.Null(dMonster.pos);

            Assert.Equal(typeof(IList<byte>), monsterType.GetProperty("inventory").PropertyType);
            Assert.Equal(typeof(IList<>).MakeGenericType(vec3Type), monsterType.GetProperty("path").PropertyType);
            Assert.Equal(typeof(IList<>).MakeGenericType(weaponType), monsterType.GetProperty("weapons").PropertyType);
            Assert.True(typeof(IFlatBufferUnion<,,,>).MakeGenericType(weaponType, vec3Type, vec4Type, monsterType).IsAssignableFrom(Nullable.GetUnderlyingType(monsterType.GetProperty("equipped").PropertyType)));
            Assert.Equal(typeof(string), monsterType.GetProperty("name").PropertyType);
            Assert.True(monsterType.GetProperty("friendly").GetCustomAttribute<FlatBufferItemAttribute>().Deprecated);

            Assert.Equal(arrayVectorType, monsterType.GetProperty("FakeVector2").PropertyType);

            byte[] data = new byte[1024];
            ISerializer monsterSerializer = CompilerTestHelpers.CompilerTestSerializer.Compile(monster);

            monsterSerializer.Write(data, monster);
            var parsedMonster = monsterSerializer.Parse(data);
            Assert.NotEqual(parsedMonster.GetType(), monster.GetType());

            var copiedMonster = Activator.CreateInstance(monsterType, new[] { parsedMonster });
            Assert.Equal(copiedMonster.GetType(), monster.GetType());
        }
    }
}
