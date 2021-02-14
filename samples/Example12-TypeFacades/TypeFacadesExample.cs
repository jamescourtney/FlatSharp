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

using FlatSharp;
using FlatSharp.Attributes;
using FlatSharp.Runtime;
using FlatSharp.TypeModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Samples.TypeFacades
{
    /// <summary>
    /// This example shows how to use FlatSharp with unions. FlatBuffer unions are discriminated unions, which means exactly one of the fields may be set.
    /// FlatSharp can 
    /// </summary>
    public class TypeFacadesExample
    {
        public static void Run()
        {
            TypeModelContainer container = TypeModelContainer.CreateDefault();

            container.RegisterTypeFacade<long, DateTimeOffset, DateTimeOffsetTypeFacadeConverter>();
            container.RegisterTypeFacade<byte[], Guid, GuidByteArrayConverter>();

            var example = new FacadeExampleTable
            {
                Guid = Guid.NewGuid(),
                Timestamp = DateTimeOffset.UtcNow,
            };

            FlatBufferSerializer serializer = new FlatBufferSerializer(
                new FlatBufferSerializerOptions(FlatBufferDeserializationOption.PropertyCache),
                container);

            byte[] destination = new byte[1024];
            serializer.Serialize(example, destination);

            var parsed = serializer.Parse<FacadeExampleTable>(destination);

            Debug.Assert(parsed.Guid == example.Guid);
            Debug.Assert(parsed.Timestamp == example.Timestamp);
        }

        [FlatBufferTable]
        public class FacadeExampleTable
        {
            [FlatBufferItem(0)]
            public virtual Guid Guid { get; set; }

            [FlatBufferItem(1)]
            public virtual DateTimeOffset Timestamp { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public struct GuidByteArrayConverter : ITypeFacadeConverter<byte[], Guid>
        {
            public Guid ConvertFromUnderlyingType(byte[] item) => new Guid(item);
            
            public byte[] ConvertToUnderlyingType(Guid item) => item.ToByteArray();
        }

        /// <summary>
        /// Convert between long and DateTimeOffset
        /// </summary>
        public struct DateTimeOffsetTypeFacadeConverter : ITypeFacadeConverter<long, DateTimeOffset>
        {
            public DateTimeOffset ConvertFromUnderlyingType(long item) => new DateTimeOffset(item, TimeSpan.Zero);

            public long ConvertToUnderlyingType(DateTimeOffset item) => item.UtcTicks;
        }
    }
}
