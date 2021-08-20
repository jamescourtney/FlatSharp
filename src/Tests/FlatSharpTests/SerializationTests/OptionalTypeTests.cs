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
    using Xunit;

    /// <summary>
    /// Binary format testing for vector serialization.
    /// </summary>
    
    public class OptionalTypeTests
    {
        [Fact]
        public void OptionalTypeSerialize_Bool() => this.RunTest<bool>();

        [Fact]
        public void OptionalTypeSerialize_Byte() => this.RunTest<byte>();

        [Fact]
        public void OptionalTypeSerialize_SByte() => this.RunTest<sbyte>();

        [Fact]
        public void OptionalTypeSerialize_UShort() => this.RunTest<ushort>();

        [Fact]
        public void OptionalTypeSerialize_Short() => this.RunTest<short>();

        [Fact]
        public void OptionalTypeSerialize_UInt() => this.RunTest<uint>();

        [Fact]
        public void OptionalTypeSerialize_Int() => this.RunTest<int>();

        [Fact]
        public void OptionalTypeSerialize_ULong() => this.RunTest<ulong>();

        [Fact]
        public void OptionalTypeSerialize_Float() => this.RunTest<float>();

        [Fact]
        public void OptionalTypeSerialize_Double() => this.RunTest<double>();

        [Fact]
        public void OptionalTypeSerialize_ByteEnum() => this.RunTest<ByteEnum>();

        [Fact]
        public void OptionalTypeSerialize_LongEnum() => this.RunTest<LongEnum>();

        private void RunTest<T>() where T : struct
        {
            OptionalTypeTable<T> table = new OptionalTypeTable<T>
            {
                Value = null
            };

            byte[] data = new byte[1024];
            int defaultBytesWritten = FlatBufferSerializer.Default.Serialize(table, data);

            Assert.Equal(12, defaultBytesWritten);
            Assert.Null(FlatBufferSerializer.Default.Parse<OptionalTypeTable<T>>(data).Value);

            table.Value = default(T);
            int actualBytesWritten = FlatBufferSerializer.Default.Serialize(table, data);

            Assert.True(actualBytesWritten >= defaultBytesWritten + RuntimeTypeModel.CreateFrom(typeof(T)).PhysicalLayout.Single().InlineSize);
            Assert.Equal(default(T), FlatBufferSerializer.Default.Parse<OptionalTypeTable<T>>(data).Value);
        }

        [FlatBufferEnum(typeof(byte))]
        public enum ByteEnum : byte
        {
            A, B, C
        }

        [FlatBufferEnum(typeof(long))]
        public enum LongEnum : long
        {
            A, B, C
        }

        [FlatBufferTable]
        public class OptionalTypeTable<T> where T : struct
        {
            [FlatBufferItem(0)]
            public virtual T? Value { get; set; }
        }
    }
}
