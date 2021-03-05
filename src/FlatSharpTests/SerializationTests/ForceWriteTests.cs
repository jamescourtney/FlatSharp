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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests that table properties with the force_write attribute are written,
    /// even if they carry the default value.
    /// </summary>
    [TestClass]
    public class ForceWriteTests
    {
        [TestMethod]
        public void ForceWrite_Bool() => this.RunTest<bool>();

        [TestMethod]
        public void ForceWrite_Byte() => this.RunTest<byte>();

        [TestMethod]
        public void ForceWrite_SByte() => this.RunTest<sbyte>();

        [TestMethod]
        public void ForceWrite_UShort() => this.RunTest<ushort>();

        [TestMethod]
        public void ForceWrite_Short() => this.RunTest<short>();

        [TestMethod]
        public void ForceWrite_UInt() => this.RunTest<uint>();

        [TestMethod]
        public void ForceWrite_Int() => this.RunTest<int>();

        [TestMethod]
        public void ForceWrite_ULong() => this.RunTest<ulong>();

        [TestMethod]
        public void ForceWrite_Long() => this.RunTest<long>();

        [TestMethod]
        public void ForceWrite_Float() => this.RunTest<float>();

        [TestMethod]
        public void ForceWrite_Double() => this.RunTest<double>();

        [TestMethod]
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

            Assert.AreEqual(optionalBytesWritten, forceBytesWritten);
            Assert.IsTrue(noForceWriteBytesWritten + 2 < forceBytesWritten);

            Assert.IsTrue(
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
