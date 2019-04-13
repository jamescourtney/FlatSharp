/*
 * Copyright 2018 James Courtney
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

 namespace FlatSharpTests
{
    using FlatSharp;
    using FlatSharp.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    
    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    [TestClass]
    public class DefaultValueTests
    {
        [TestMethod]
        public void Read_DefaultValues()
        {
            // hand-craft a table here:
            byte[] data =
            {
                4, 0, 0, 0,           // uoffset to the start of the table.
                252, 255, 255, 255,   // soffset_t to the vtable
                4, 0,                 // vtable size
                4, 0,                 // table length
            };

            var parsed = FlatBufferSerializer.Default.Parse<DefaultValueTable>(data);

            Assert.AreEqual(5, parsed.Int);
            Assert.AreEqual((short)6, parsed.Short);
            Assert.AreEqual(3.14159f, parsed.Float);

            byte[] serialized = new byte[128];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(parsed, serialized.AsSpan());

            Assert.AreEqual(Convert.ToBase64String(data), Convert.ToBase64String(serialized, 0, bytesWritten));
        }

        [TestMethod]
        public void Write_DefaultValues()
        {
            // hand-craft a table here:
            byte[] data =
            {
                4, 0, 0, 0,           // uoffset to the start of the table.
                252, 255, 255, 255,   // soffset_t to the vtable
                4, 0,                 // vtable size
                4, 0,                 // table length
            };

            var table = new DefaultValueTable
            {
                Int = 5,
                Short = 6,
                Float = 3.14159f
            };

            Span<byte> span = new byte[100];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(table, span);

            Assert.AreEqual(Convert.ToBase64String(data), Convert.ToBase64String(span.Slice(0, bytesWritten).ToArray()));
        }

        [FlatBufferTable]
        public class DefaultValueTable
        {
            [FlatBufferItem(0)]
            [DefaultValue(5)]
            public virtual int Int { get; set; }

            [FlatBufferItem(1)]
            [DefaultValue((short)6)]
            public virtual short Short { get; set; }

            [FlatBufferItem(2)]
            [DefaultValue((float)3.14159)]
            public virtual float Float { get; set; }
        }
    }
}
