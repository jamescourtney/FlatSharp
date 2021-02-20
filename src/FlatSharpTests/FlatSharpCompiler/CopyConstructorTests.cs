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
    using System.Linq;
    using System.Reflection;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.Compiler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CopyConstructorTests
    {
        [TestMethod]
        public void CopyConstructorsTest()
        {
            string schema = @"
namespace CopyConstructorTest;
enum Color:byte { Red = 0, Green, Blue = 2 }

union Union { OuterTable, InnerTable, OuterStruct, InnerStruct } // Optionally add more tables.

table OuterTable {
  A:string;

  B:byte;
  C:ubyte;
  D:int16;
  E:uint16;
  F:int32;
  G:uint32;
  H:int64;
  I:uint64;
  
  IntVector_List:[int] (VectorType:""IList"");
  IntVector_RoList:[int] (VectorType:""IReadOnlyList"");
  IntVector_Array:[int] (VectorType:""Array"");
  
  TableVector_List:[InnerTable] (VectorType:""IList"");
  TableVector_RoList:[InnerTable] (VectorType:""IReadOnlyList"");
  TableVector_Indexed:[InnerTable] (VectorType:""IIndexedVector"");

  ByteVector:[ubyte] (VectorType:""Memory"");
  ByteVector_RO:[ubyte] (VectorType:""ReadOnlyMemory"");
}

struct OuterStruct {
    Foo:int;
    Bar:int64;
    Inner:InnerStruct;
}

struct InnerStruct {
    X:float;
    Y:double;
    Z:float;
}

table InnerTable {
  Name:string (key);
}

";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());

            Type outerTableType = asm.GetType("CopyConstructorTest.OuterTable");
            Type innerTableType = asm.GetType("CopyConstructorTest.InnerTable");
            Type outerStructType = asm.GetType("CopyConstructorTest.OuterStruct");
            Type innerStructType = asm.GetType("CopyConstructorTest.InnerStruct");

            dynamic outerTable = Activator.CreateInstance(outerTableType);
            outerTable.A = "string";
            outerTable.B = (sbyte)1;
            outerTable.C = (byte)2;
            outerTable.D = (short)3;
            outerTable.E = (ushort)4;
            outerTable.F = (int)5;
            outerTable.G = (uint)6;
            outerTable.H = (long)7;
            outerTable.I = (ulong)8;

            int[] intVector = { 1, 2, 3, 4, 5, 6 };
            outerTable.IntVector_List = (IList<int>)intVector;
            outerTable.IntVector_RoList = (IReadOnlyList<int>)intVector;
            outerTable.IntVector_Array = intVector;

            byte[] byteVector = { 1, 2, 3, };
            outerTable.ByteVector = byteVector.AsMemory();
            outerTable.ByteVector_RO = (ReadOnlyMemory<byte>)byteVector.AsMemory();

            var innerItems = this.CreateInner(innerTableType, "Rocket", "Molly", "Jingle");


        }

        private List<object> CreateInner(Type type, params string[] names)
        {
            var items = new List<object>();
            foreach (var name in names)
            {
                object item = Activator.CreateInstance(type);
                dynamic d = item;
                d.Name = name;

                items.Add(item);
            }

            return items;
        }
    }
}

