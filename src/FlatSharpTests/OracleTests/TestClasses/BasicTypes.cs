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
    using FlatSharp.Attributes;

    public interface IBasicTypes
    {
        bool Bool { get; set; }
        byte Byte { get; set; }
        double Double { get; set; }
        float Float { get; set; }
        int Int { get; set; }
        long Long { get; set; }
        sbyte SByte { get; set; }
        short Short { get; set; }
        string? String { get; set; }
        uint UInt { get; set; }
        ulong ULong { get; set; }
        ushort UShort { get; set; }
    }

    [FlatBufferTable]
    public class BasicTypes : IBasicTypes
    {
        // Declared out of order to make sure we honor the 'index' property.
        [FlatBufferItem(0)] public virtual byte Byte { get; set; }
        [FlatBufferItem(3)] public virtual bool Bool { get; set; }
        [FlatBufferItem(6)] public virtual sbyte SByte { get; set; }

        [FlatBufferItem(1)] public virtual ushort UShort { get; set; }
        [FlatBufferItem(2)] public virtual short Short { get; set; }
        [FlatBufferItem(4)] public virtual uint UInt { get; set; }
        [FlatBufferItem(5)] public virtual int Int { get; set; }
        [FlatBufferItem(7)] public virtual ulong ULong { get; set; }
        [FlatBufferItem(8)] public virtual long Long { get; set; }

        [FlatBufferItem(9)] public virtual float Float { get; set; }
        [FlatBufferItem(10)] public virtual double Double { get; set; }

        [FlatBufferItem(11)] public virtual string? String { get; set; }
    }

    [FlatBufferTable]
    public class BasicTypesForceWrite : IBasicTypes
    {
        // Declared out of order to make sure we honor the 'index' property.
        [FlatBufferItem(0)] public virtual byte Byte { get; set; }
        [FlatBufferItem(3)] public virtual bool Bool { get; set; }
        [FlatBufferItem(6)] public virtual sbyte SByte { get; set; }

        [FlatBufferItem(1)] public virtual ushort UShort { get; set; }
        [FlatBufferItem(2)] public virtual short Short { get; set; }
        [FlatBufferItem(4)] public virtual uint UInt { get; set; }
        [FlatBufferItem(5)] public virtual int Int { get; set; }
        [FlatBufferItem(7)] public virtual ulong ULong { get; set; }
        [FlatBufferItem(8, ForceWrite = true)] public virtual long Long { get; set; }

        [FlatBufferItem(9)] public virtual float Float { get; set; }
        [FlatBufferItem(10)] public virtual double Double { get; set; }

        [FlatBufferItem(11)] public virtual string? String { get; set; }
    }
}
