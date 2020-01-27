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

table Monster (GenerateSerializer:None) {
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

table Weapon {
  name:string;
  damage:short;
}

root_type Monster;"; 

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema);

            Type weaponType = asm.GetType("MyGame.Weapon");
            Type monsterType = asm.GetTypes().Single(x => x.FullName == "MyGame.Monster");
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
            var parsedMonster = CompilerTestHelpers.CompilerTestSerializer.ReflectionParse(monsterType, data);
        }
    }
}
