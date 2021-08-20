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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FlatSharp.Compiler;
    using Xunit;

    
    public class IncludeTests
    {
        [Fact]
        public void SimpleInclude()
        {
            string baseFbs = "include \"OtherTable.fbs\"; namespace Foo; table BaseTable { OtherTable:OtherTable; Int:int; }";
            var includes = new Dictionary<string, string>
            {
                { "OtherTable.fbs", "namespace Foo; table OtherTable { Foo:string; }" }
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.True(!string.IsNullOrEmpty(cSharp));

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.True(member is RootNodeDefinition);
            Assert.Equal(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.Equal("Foo", nsDef.Name);

            Assert.Equal(2, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTable = nsDef.Children["OtherTable"];

            Assert.NotNull(basetable);
            Assert.NotNull(otherTable);

            Assert.Equal("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.Equal("OtherTable.fbs", Path.GetFileName(otherTable.DeclaringFile));
        }

        /// <summary>
        /// Tests that a single file is only included once.
        /// </summary>
        [Fact]
        public void DeduplicationInclude()
        {
            string baseFbs = "include \"A.fbs\"; include \"B.fbs\"; namespace Foo; table BaseTable { OtherTable:OtherTableA; Int:int; }";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", "include \"B.fbs\"; namespace Foo; table OtherTableA { Foo:OtherTableB; }" },
                { "B.fbs", "namespace Foo; table OtherTableB { Foo:string; }" }
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.True(!string.IsNullOrEmpty(cSharp));

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.True(member is RootNodeDefinition);
            Assert.Equal(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.Equal("Foo", nsDef.Name);

            Assert.Equal(3, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTableA = nsDef.Children["OtherTableA"];
            var otherTableB = nsDef.Children["OtherTableB"];

            Assert.NotNull(basetable);
            Assert.NotNull(otherTableA);
            Assert.NotNull(otherTableB);

            Assert.Equal("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.Equal("A.fbs", Path.GetFileName(otherTableA.DeclaringFile));
            Assert.Equal("B.fbs", Path.GetFileName(otherTableB.DeclaringFile));
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [Fact]
        public void CircularReferenceInclude()
        {
            string baseFbs = $"include \"A.fbs\"; namespace Foo; table BaseTable ({MetadataKeys.SerializerKind}) {{ OtherTable:OtherTable; }}";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", $"include \"root.fbs\"; namespace Foo; table OtherTable ({MetadataKeys.SerializerKind}) {{ Foo:BaseTable; }}" },
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.True(!string.IsNullOrEmpty(cSharp));
            Assert.Contains("public static ISerializer<Foo.BaseTable> Serializer", cSharp);
            Assert.DoesNotContain("public static ISerializer<Foo.OtherTable> Serializer", cSharp);

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.True(member is RootNodeDefinition);
            Assert.Equal(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.Equal("Foo", nsDef.Name);

            Assert.Equal(2, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTable = nsDef.Children["OtherTable"];

            Assert.NotNull(basetable);
            Assert.NotNull(otherTable);

            Assert.Equal("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.Equal("A.fbs", Path.GetFileName(otherTable.DeclaringFile));
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [Fact]
        public void DuplicateTypesDifferentFiles()
        {
            string baseFbs = $"include \"A.fbs\"; namespace Foo; table BaseTable ({MetadataKeys.SerializerKind}) {{ BaseTable:BaseTable; }}";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", "namespace Foo; table BaseTable { BaseTable:BaseTable; }" },
            };

            var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes));
            Assert.Contains("Duplicate member name", ex.Message);
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [Fact]
        public void ChangeInIncludedFileModifiesIncludingFileHash()
        {
            string baseFbs = "include \"A.fbs\"; namespace Foo; table RootTable { Foo:int; }";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", "namespace Foo; table TableA { Bar:string; }" },
            };

            RootNodeDefinition rootDef1 = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            
            includes["A.fbs"] += " ";
            RootNodeDefinition rootDef2 = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);

            Assert.NotEqual(rootDef1.InputHash, rootDef2.InputHash);
        }
    }
}
