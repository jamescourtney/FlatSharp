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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrecompiledSerializerTests
    {
        [TestMethod]
        public void MonsterTest()
        {
            // https://github.com/google/flatbuffers/blob/master/samples/monster.fbs
            string schema = @"
namespace MyGame;
enum Color:byte { Red = 0, Green, Blue = 2 }

union Equipment { Weapon } // Optionally add more tables.

struct Vec3 {
  x:float;
  y:float;
  z:float;
}

table Monster (PrecompiledSerializer:""greedy|mutable"") {
  pos:Vec3;
  mana:short = 150;
  hp:short = 100;
  name:string;
  friendly:bool = false (deprecated);
  inventory:[ubyte];
  color:Color = Blue;
  weapons:[Weapon];
  equipped:Equipment;
  path:[Vec3];
}

table Weapon (PrecompiledSerializer:none) {
  name:string;
  damage:short;
}

root_type Monster;"; 

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            Type weaponType = asm.GetType("MyGame.Weapon");
            Type monsterType = asm.GetTypes().Single(x => x.FullName == "MyGame.Monster");
            dynamic serializer = monsterType.GetProperty("Serializer", BindingFlags.Static | BindingFlags.Public).GetValue(null);

            object monster = Activator.CreateInstance(monsterType);
            dynamic dMonster = monster;

            Type vecType = asm.GetTypes().Single(x => x.FullName == "MyGame.Vec3");
            object vec = Activator.CreateInstance(vecType);
            dynamic dVec = vec;

            Assert.AreEqual((short)150, dMonster.mana);
            Assert.AreEqual((short)100, dMonster.hp);
            Assert.IsFalse(dMonster.friendly);
            Assert.AreEqual("Blue", dMonster.color.ToString());
            Assert.IsNull(dMonster.pos);

            Assert.AreEqual(typeof(IList<byte>), monsterType.GetProperty("inventory").PropertyType);
            Assert.AreEqual(typeof(IList<>).MakeGenericType(vecType), monsterType.GetProperty("path").PropertyType);
            Assert.AreEqual(typeof(IList<>).MakeGenericType(weaponType), monsterType.GetProperty("weapons").PropertyType);
            Assert.AreEqual(typeof(FlatBufferUnion<>).MakeGenericType(weaponType), monsterType.GetProperty("equipped").PropertyType);
            Assert.AreEqual(typeof(string), monsterType.GetProperty("name").PropertyType);
            Assert.IsTrue(monsterType.GetProperty("friendly").GetCustomAttribute<FlatBufferItemAttribute>().Deprecated);

            byte[] data = new byte[1024];

            CompilerTestHelpers.CompilerTestSerializer.ReflectionSerialize(monster, data);
            var parsedMonster = serializer.Parse(new ArrayInputBuffer(data));

            Assert.AreEqual("Blue", parsedMonster.color.ToString());
        }

        [TestMethod]
        public void FlagsOptions_Greedy()
        {
            this.TestFlags(FlatBufferSerializerFlags.GreedyDeserialize, $"PrecompiledSerializer:{nameof(FlatBufferSerializerFlags.GreedyDeserialize)}");
        }

        [TestMethod]
        public void FlagsOptions_Greedy_Shorthand()
        {
            this.TestFlags(FlatBufferSerializerFlags.GreedyDeserialize, $"PrecompiledSerializer:greedy");
        }

        [TestMethod]
        public void FlagsOptions_Default()
        {
            this.TestFlags(FlatBufferSerializerFlags.Default, $"PrecompiledSerializer:{nameof(FlatBufferSerializerFlags.Default)}");
        }

        [TestMethod]
        public void FlagsOptions_Default_Implicit()
        {
            this.TestFlags(FlatBufferSerializerFlags.Default, $"PrecompiledSerializer");
        }

        [TestMethod]
        public void FlagsOptions_None()
        {
            this.TestFlags(FlatBufferSerializerFlags.None, $"PrecompiledSerializer:{nameof(FlatBufferSerializerFlags.None)}");
        }

        [TestMethod]
        public void FlagsOptions_Mutable()
        {
            this.TestFlags(FlatBufferSerializerFlags.GenerateMutableObjects, $"PrecompiledSerializer:{nameof(FlatBufferSerializerFlags.GenerateMutableObjects)}");
        }

        [TestMethod]
        public void FlagsOptions_Mutable_Shorthand()
        {
            this.TestFlags(FlatBufferSerializerFlags.GenerateMutableObjects, $"PrecompiledSerializer:mutable");
        }

        [TestMethod]
        public void FlagsOptions_CacheListVectorData()
        {
            this.TestFlags(FlatBufferSerializerFlags.CacheListVectorData, $"PrecompiledSerializer:{nameof(FlatBufferSerializerFlags.CacheListVectorData)}");
        }

        [TestMethod]
        public void FlagsOptions_CacheListVectorData_ShortHand()
        {
            this.TestFlags(FlatBufferSerializerFlags.CacheListVectorData, $"PrecompiledSerializer:vectorcache");
        }

        [TestMethod]
        public void FlagsOptions_Compound_ShortHand()
        {
            var expectedFlags = FlatBufferSerializerFlags.CacheListVectorData | FlatBufferSerializerFlags.GenerateMutableObjects | FlatBufferSerializerFlags.GreedyDeserialize;
            this.TestFlags(expectedFlags, $"PrecompiledSerializer:\"vectorcache|mutable|greedy\"");
        }

        [TestMethod]
        public void FlagsOptions_Invalid()
        {
            Assert.ThrowsException<InvalidFbsFileException>(() => this.TestFlags(default, $"PrecompiledSerializer:banana"));
            Assert.ThrowsException<InvalidFbsFileException>(() => this.TestFlags(default, $"PrecompiledSerializer:\"banana\""));
            Assert.ThrowsException<InvalidFbsFileException>(() => this.TestFlags(default, $"PrecompiledSerializer:\"greedy|banana\""));
            Assert.ThrowsException<InvalidFbsFileException>(() => this.TestFlags(default, $"PrecompiledSerializer:\"greedy|banana"));
        }

        private void TestFlags(FlatBufferSerializerFlags expectedFlags, string metadata)
        {
            string schema = $"namespace Test; table FooTable ({metadata}) {{ foo:string; }}";
            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            Type type = asm.GetType("Test.FooTable");
            Assert.IsNotNull(type);

            Type serializerType = type.GetNestedType(RoslynSerializerGenerator.GeneratedSerializerClassName, BindingFlags.NonPublic | BindingFlags.Public);
            
            Assert.IsNotNull(serializerType);
            Assert.IsTrue(serializerType.IsNested);
            Assert.IsTrue(serializerType.IsNestedPrivate);

            var attribute = serializerType.GetCustomAttribute<FlatSharpGeneratedSerializerAttribute>();
            Assert.IsNotNull(attribute);
            Assert.AreEqual(expectedFlags, attribute.Flags);
        }
    }
}

