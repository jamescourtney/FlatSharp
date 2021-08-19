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

namespace FlatSharpTests
{
    using System;
    using System.Linq;
    using FlatSharp;
    using FlatSharp.Attributes;
    using Xunit;

    /// <summary>
    /// Tests that table properties with the force_write attribute are written,
    /// even if they carry the default value.
    /// </summary>
    
    public class ForceWriteTests
    {
        [Fact]
        public void ForceWrite_Bool() => this.RunTest<bool>();

        [Fact]
        public void ForceWrite_Byte() => this.RunTest<byte>();

        [Fact]
        public void ForceWrite_SByte() => this.RunTest<sbyte>();

        [Fact]
        public void ForceWrite_UShort() => this.RunTest<ushort>();

        [Fact]
        public void ForceWrite_Short() => this.RunTest<short>();

        [Fact]
        public void ForceWrite_UInt() => this.RunTest<uint>();

        [Fact]
        public void ForceWrite_Int() => this.RunTest<int>();

        [Fact]
        public void ForceWrite_ULong() => this.RunTest<ulong>();

        [Fact]
        public void ForceWrite_Long() => this.RunTest<long>();

        [Fact]
        public void ForceWrite_Float() => this.RunTest<float>();

        [Fact]
        public void ForceWrite_Double() => this.RunTest<double>();

        [Fact]
        public void ForceWrite_Enum() => this.RunTest<SomeEnum>();

        private void RunTest<T>() where T : struct
        {
            // This table uses non-Nullable<T>, and we are writing the default,
            // but force write is on. This table uses Nullable<T>, and we are 
            // writing the default.
            ForceWriteTable<T> table = new()
            {
                Item = default(T),
            };

            NonForceWriteTable<T> noForceWrite = new()
            {
                Item = default(T),
            };

            OptionalTable<T> optionalTable = new()
            {
                Item = default(T),
            };

            byte[] forceWriteData = new byte[1024];
            byte[] optionalData = new byte[1024];
            byte[] omittedData = new byte[1024];

            int forceBytesWritten = FlatBufferSerializer.Default.Serialize(table, forceWriteData);
            int optionalBytesWritten = FlatBufferSerializer.Default.Serialize(optionalTable, optionalData);
            int noForceWriteBytesWritten = FlatBufferSerializer.Default.Serialize(noForceWrite, omittedData);

            Assert.Equal(optionalBytesWritten, forceBytesWritten);
            Assert.True(noForceWriteBytesWritten + 2 < forceBytesWritten);

            Assert.True(
                forceWriteData
                    .AsSpan()
                    .Slice(0, forceBytesWritten)
                    .SequenceEqual(optionalData.AsSpan().Slice(0, forceBytesWritten)));
        }

        [FlatBufferEnum(typeof(int))]
        public enum SomeEnum
        {
            None = 0,
            One = 1,
            Two = 2,
        }

        [FlatBufferTable]
        public class ForceWriteTable<T> where T : struct
        {
            [FlatBufferItem(0, ForceWrite = true)]
            public virtual T Item { get; set; }
        }

        [FlatBufferTable]
        public class NonForceWriteTable<T> where T : struct
        {
            [FlatBufferItem(0)]
            public virtual T Item { get; set; }
        }

        [FlatBufferTable]
        public class OptionalTable<T> where T : struct
        {
            [FlatBufferItem(0)]
            public virtual T? Item { get; set; }
        }
    }
}
