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

namespace FlatSharpEndToEndTests;

public static class Helpers
{
    public static IEnumerable<FlatBufferDeserializationOption> AllOptions
    {
        get
        {
            yield return FlatBufferDeserializationOption.Lazy;
            yield return FlatBufferDeserializationOption.Progressive;
            yield return FlatBufferDeserializationOption.Greedy;
            yield return FlatBufferDeserializationOption.GreedyMutable;
        }
    }

    public static byte[] AllocateAndSerialize<T>(this T item) where T : class, IFlatBufferSerializable<T>
    {
        return item.AllocateAndSerialize(item.Serializer);
    }

    public static byte[] AllocateAndSerialize<T>(this T item, ISerializer<T> serializer) where T : class, IFlatBufferSerializable<T>
    {
        byte[] data = new byte[serializer.GetMaxSize(item)];
        int bytesWritten = serializer.Write(data, item);
        return data.AsMemory().Slice(0, bytesWritten).ToArray();
    }

    public static T SerializeAndParse<T>(this T item) where T : class, IFlatBufferSerializable<T>
    {
        return SerializeAndParse<T>(item, item.Serializer);
    }

    public static T SerializeAndParse<T>(
        this T item,
        FlatBufferDeserializationOption? option) where T : class, IFlatBufferSerializable<T>
    {
        byte[] buffer = item.AllocateAndSerialize();
        return item.Serializer.Parse(buffer, option);
    }

    public static T SerializeAndParse<T>(
        this T item,
        ISerializer<T>? serializer) where T : class, IFlatBufferSerializable<T>
    {
        serializer ??= item.Serializer;

        byte[] buffer = item.AllocateAndSerialize();
        return serializer.Parse(buffer);
    }
}

