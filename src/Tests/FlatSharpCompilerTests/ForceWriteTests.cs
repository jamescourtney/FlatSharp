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
    using FlatSharp.TypeModel;
    using Xunit;

    public class ForceWriteTests
    {
        [Fact]
        public void ForceWrite_NoTableOption() => this.RunTest(string.Empty, false);

        [Fact]
        public void ForceWrite_TableOptionDisabled() => this.RunTest($"{MetadataKeys.ForceWrite}:\"false\"", false);

        [Fact]
        public void ForceWrite_TableOptionEnabledImplicit() => this.RunTest(MetadataKeys.ForceWrite, true);

        [Fact]
        public void ForceWrite_TableOptionEnabledExplicit() => this.RunTest($"{MetadataKeys.ForceWrite}:\"true\"", true);

        private void RunTest(string tableMetadata, bool defaultEnabled)
        {
            if (!string.IsNullOrEmpty(tableMetadata))
            {
                tableMetadata = $"({tableMetadata})";
            }

            string schema = $@"
            {MetadataHelpers.AllAttributes}
            namespace ForceWriteTests;
            enum SomeEnum : ubyte {{ First = 0, Second = 1 }}
            struct SomeStruct {{ I:int; }}
            union SomeUnion {{ SomeTable, SomeStruct }}
            table FakeTable {tableMetadata}
            {{ 
                Default:int; 
                DefaultEnum:SomeEnum;
                Disabled:ubyte ({MetadataKeys.ForceWrite}:""false"");
                Enabled:double ({MetadataKeys.ForceWrite});
                EnabledExplicit:long ({MetadataKeys.ForceWrite}:""true"");
                
                StructVector:[SomeStruct];
                TableVector:[SomeTable];
                ScalarVector:[float];
                StringVector:[string];
                
                String:string;
                Struct:SomeStruct;
                Table:SomeTable;
                Union:SomeUnion;
            }}
            
            table SomeTable {{ I:int; }}
            ";

            Assembly asm = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var type = asm.GetType("ForceWriteTests.FakeTable");

            void AssertForceWrite(string propertyName, bool enabled)
            {
                var prop = type.GetProperty(propertyName);
                var attr = prop.GetCustomAttribute<FlatBufferItemAttribute>();
                Assert.NotNull(attr);
                Assert.Equal(enabled, attr.ForceWrite);
            }

            AssertForceWrite("Default", defaultEnabled);
            AssertForceWrite("DefaultEnum", defaultEnabled);
            AssertForceWrite("Enabled", true);
            AssertForceWrite("EnabledExplicit", true);
            AssertForceWrite("Disabled", false);
            AssertForceWrite("StructVector", false);
            AssertForceWrite("TableVector", false);
            AssertForceWrite("ScalarVector", false);
            AssertForceWrite("StringVector", false);
            AssertForceWrite("String", false);
            AssertForceWrite("Struct", false);
            AssertForceWrite("Table", false);
            AssertForceWrite("Union", false);
        }
    }
}
