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
    using Xunit;
    using System;
    using System.Diagnostics;
    
    /// <summary>
    /// Tests default values on a table.
    /// </summary>
    
    public class DeprecatedValueTests
    {
        [Fact]
        public void Ignore_DeprecatedValueOnRead()
        {
            // hand-craft a table here:
            byte[] data =
            {
                4, 0, 0, 0,               // uoffset to the start of the table.
                236, 255, 255, 255,       // soffset_t to the vtable
                123, 0, 0, 0,             // index 0 value
                0, 0, 0, 0,               // padding to 8 byte alignment
                255, 0, 0, 0, 0, 0, 0, 0, // index 1 value

                8, 0,                     // vtable size
                20, 0,                    // table length
                4, 0,                     // index 0 offset (deprecated)
                12, 0,                    // index 1 offset
            };

            var parsed = FlatBufferSerializer.Default.Parse<DeprecatedTable>(data);

            Assert.Equal(0, parsed.Value);
            Assert.Equal(255L, parsed.Long);

            var nonDeprecatedParsed = FlatBufferSerializer.Default.Parse<NonDeprecatedTable>(data);

            Assert.Equal(123, nonDeprecatedParsed.Value);
            Assert.Equal(255L, nonDeprecatedParsed.Long);
        }

        [Fact]
        public void Ignore_DeprecatedValueOnWrite()
        {
            if (Debugger.IsAttached)
            {
                // This test has a wonderful way of crashing Visual Studio when the debugger is attached. It runs fine without the debugger attached.
                return;
            }

            var deprecatedTable = new DeprecatedTable
            {
                Value = 123,
                Long = 255,
            };

            // hand-craft a table here: 
            byte[] data =
            {
                4, 0, 0, 0,               // uoffset to the start of the table.
                244, 255, 255, 255,       // soffset_t to the vtable
                255, 0, 0, 0, 0, 0, 0, 0, // index 1 value

                8, 0,                     // vtable size
                12, 0,                    // table length
                0, 0,                     // index 0 offset (deprecated)
                4, 0,                     // index 1 offset
            };

            Span<byte> buffer = new byte[100];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(deprecatedTable, buffer);
            byte[] actualBytes = buffer.Slice(0, bytesWritten).ToArray();

            Assert.True(data.AsSpan().SequenceEqual(actualBytes));

            var nonDeprecatedParsed = FlatBufferSerializer.Default.Parse<NonDeprecatedTable>(actualBytes);

            Assert.Equal(0, nonDeprecatedParsed.Value);
            Assert.Equal(255L, nonDeprecatedParsed.Long);
        }

        [FlatBufferTable]
        public class DeprecatedTable
        {
            [FlatBufferItem(0, Deprecated = true)]
            public virtual int Value { get; set; }

            [FlatBufferItem(1)]
            public virtual long Long { get; set; }
        }

        [FlatBufferTable]
        public class NonDeprecatedTable
        {
            [FlatBufferItem(0)]
            public virtual int Value { get; set; }

            [FlatBufferItem(1)]
            public virtual long Long { get; set; }
        }
    }
}
