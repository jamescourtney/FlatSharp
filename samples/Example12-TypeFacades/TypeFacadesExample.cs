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

using FlatSharp.Runtime;
using FlatSharp.TypeModel;

namespace Samples.TypeFacades;

/// <summary>
/// FlatSharp supports a feature called Type Facades. Type Facades are an easy way to extend FlatSharp
/// to expose custom types that are not defined in FlatBuffers proper. Type Facades need 3 things:
/// 
/// 1) A type you want FlatSharp to understand (the Facade)
/// 2) A type FlatSharp already understands (long/int/string/array/etc)
/// 3) A converter that can convert back and forth between them.
/// 
/// Some examples of Facades are:
/// 1) DateTimeOffset stored as UtcTicks
/// 2) Guid stored as byte array
/// 3) JSON object stored as string
/// 
/// Type facades are *ONLY* supported in FlatSharp's runtime mode with attributes. It is still possible
/// to use type facades with features from other languages, but you'll need a separate FBS file and the
/// underlying type will need to be exposed.
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
            new FlatBufferSerializerOptions(FlatBufferDeserializationOption.Greedy),
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
    /// Convert between Guid and Byte Array.
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
