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
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime;
    using System.Runtime.CompilerServices;
    using System.Text;
    using FlatSharp;
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using Xunit;

    /// <summary>
    /// Verifies expected binary formats for test data.
    /// </summary>
    
    public class AccessModifierTests
    {
        [Fact]
        public void BuildSerializer_AccessMethods_Virtual()
        {
            FlatBufferSerializer.Default.GetMaxSize(new TestClass());
        }

        [Fact]
        public void BuildSerializer_AccessMethods_NonVirtual()
        {
            FlatBufferSerializer.Default.GetMaxSize(new TestClassNonVirtual());
        }

        [FlatBufferTable]
        public class TestClass
        {
            [FlatBufferItem(0)]
            public virtual int BothPublic { get; set; }

            [FlatBufferItem(1)]
            public virtual int PublicGetterProtectedInternalSetter { get; protected internal set; }

            [FlatBufferItem(2)]
            public virtual int PublicGetterProtectedSetter { get; protected set; }

            [FlatBufferItem(3)]
            public virtual TestStruct? Struct { get; protected set; }

#if NET5_0_OR_GREATER
            [FlatBufferItem(4)]
            public virtual int BothPublicInit { get; init; }

            [FlatBufferItem(5)]
            public virtual int PublicGetterProtectedInternalInit { get; protected internal init; }

            [FlatBufferItem(6)]
            public virtual int PublicGetterProtectedInit { get; protected init; }
#endif
        }

        [FlatBufferStruct]
        public class TestStruct
        {
            [FlatBufferItem(0)]
            public virtual int BothPublic { get; set; }

            [FlatBufferItem(1)]
            public virtual int PublicGetterProtectedInternalSetter { get; protected internal set; }

            [FlatBufferItem(2)]
            public virtual int PublicGetterProtectedSetter { get; protected set; }

#if NET5_0_OR_GREATER
            [FlatBufferItem(3)]
            public virtual int BothPublicInit { get; init; }

            [FlatBufferItem(4)]
            public virtual int PublicGetterProtectedInternalInit { get; protected internal init; }

            [FlatBufferItem(5)]
            public virtual int PublicGetterProtectedInit { get; protected init; }
#endif
        }


        [FlatBufferTable]
        public class TestClassNonVirtual
        {
            [FlatBufferItem(0)]
            public int BothPublic { get; set; }

            [FlatBufferItem(1)]
            public int PublicGetterProtectedInternalSetter { get; protected internal set; }

            [FlatBufferItem(2)]
            public int PublicGetterProtectedSetter { get; protected set; }

            [FlatBufferItem(3)]
            public TestStructNonVirtual? Struct { get; protected set; }

#if NET5_0_OR_GREATER
            [FlatBufferItem(4)]
            public int BothPublicInit { get; init; }

            [FlatBufferItem(5)]
            public int PublicGetterProtectedInternalInit { get; protected internal init; }

            [FlatBufferItem(6)]
            public int PublicGetterProtectedInit { get; protected init; }
#endif

        }

        [FlatBufferStruct]
        public class TestStructNonVirtual
        {
            [FlatBufferItem(0)]
            public int BothPublic { get; set; }

            [FlatBufferItem(1)]
            public int PublicGetterProtectedInternalSetter { get; protected internal set; }

            [FlatBufferItem(2)]
            public int PublicGetterProtectedSetter { get; protected set; }

#if NET5_0_OR_GREATER
            [FlatBufferItem(3)]
            public int PublicGetterProtectedInternalInit { get; protected internal init; }

            [FlatBufferItem(4)]
            public int PublicGetterProtectedInit { get; protected init; }
#endif
        }
    }
}
