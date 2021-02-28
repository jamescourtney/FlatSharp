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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IncludeTests
    {
        [TestMethod]
        public void SimpleInclude()
        {
            string baseFbs = "include \"OtherTable.fbs\"; namespace Foo; table BaseTable { OtherTable:OtherTable; Int:int; }";
            var includes = new Dictionary<string, string>
            {
                { "OtherTable.fbs", "namespace Foo; table OtherTable { Foo:string; }" }
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.IsTrue(!string.IsNullOrEmpty(cSharp));

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.IsTrue(member is RootNodeDefinition);
            Assert.AreEqual(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.AreEqual("Foo", nsDef.Name);

            Assert.AreEqual(2, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTable = nsDef.Children["OtherTable"];

            Assert.IsNotNull(basetable);
            Assert.IsNotNull(otherTable);

            Assert.AreEqual("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.AreEqual("OtherTable.fbs", Path.GetFileName(otherTable.DeclaringFile));
        }

        /// <summary>
        /// Tests that a single file is only included once.
        /// </summary>
        [TestMethod]
        public void DeduplicationInclude()
        {
            string baseFbs = "include \"A.fbs\"; include \"B.fbs\"; namespace Foo; table BaseTable { OtherTable:OtherTableA; Int:int; }";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", "include \"B.fbs\"; namespace Foo; table OtherTableA { Foo:OtherTableB; }" },
                { "B.fbs", "namespace Foo; table OtherTableB { Foo:string; }" }
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.IsTrue(!string.IsNullOrEmpty(cSharp));

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.IsTrue(member is RootNodeDefinition);
            Assert.AreEqual(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.AreEqual("Foo", nsDef.Name);

            Assert.AreEqual(3, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTableA = nsDef.Children["OtherTableA"];
            var otherTableB = nsDef.Children["OtherTableB"];

            Assert.IsNotNull(basetable);
            Assert.IsNotNull(otherTableA);
            Assert.IsNotNull(otherTableB);

            Assert.AreEqual("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.AreEqual("A.fbs", Path.GetFileName(otherTableA.DeclaringFile));
            Assert.AreEqual("B.fbs", Path.GetFileName(otherTableB.DeclaringFile));
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [TestMethod]
        public void CircularReferenceInclude()
        {
            string baseFbs = $"include \"A.fbs\"; namespace Foo; table BaseTable ({MetadataKeys.SerializerKind}) {{ OtherTable:OtherTable; }}";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", $"include \"root.fbs\"; namespace Foo; table OtherTable ({MetadataKeys.SerializerKind}) {{ Foo:BaseTable; }}" },
            };

            string cSharp = FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes);
            Assert.IsTrue(!string.IsNullOrEmpty(cSharp));
            Assert.IsTrue(cSharp.Contains("public static ISerializer<Foo.BaseTable> Serializer"));
            Assert.IsFalse(cSharp.Contains("public static ISerializer<Foo.OtherTable> Serializer"));

            BaseSchemaMember member = FlatSharpCompiler.TestHookParseSyntax(baseFbs, includes);
            Assert.IsTrue(member is RootNodeDefinition);
            Assert.AreEqual(1, member.Children.Count);

            NamespaceDefinition nsDef = (NamespaceDefinition)member.Children.Single().Value;
            Assert.AreEqual("Foo", nsDef.Name);

            Assert.AreEqual(2, nsDef.Children.Count);

            var basetable = nsDef.Children["BaseTable"];
            var otherTable = nsDef.Children["OtherTable"];

            Assert.IsNotNull(basetable);
            Assert.IsNotNull(otherTable);

            Assert.AreEqual("root.fbs", Path.GetFileName(basetable.DeclaringFile));
            Assert.AreEqual("A.fbs", Path.GetFileName(otherTable.DeclaringFile));
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [TestMethod]
        public void DuplicateTypesDifferentFiles()
        {
            string baseFbs = $"include \"A.fbs\"; namespace Foo; table BaseTable ({MetadataKeys.SerializerKind}) {{ BaseTable:BaseTable; }}";
            var includes = new Dictionary<string, string>
            {
                { "A.fbs", "namespace Foo; table BaseTable { BaseTable:BaseTable; }" },
            };

            var ex = Assert.ThrowsException<InvalidFbsFileException>(() => FlatSharpCompiler.TestHookCreateCSharp(baseFbs, new(), includes));
            Assert.IsTrue(ex.Message.Contains("Duplicate member name"));
        }

        /// <summary>
        /// Tests that two files can include each other. Also tests that serializers are only generated for the root file.
        /// </summary>
        [TestMethod]
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

            Assert.AreNotEqual(rootDef1.InputHash, rootDef2.InputHash);
        }
    }
}
