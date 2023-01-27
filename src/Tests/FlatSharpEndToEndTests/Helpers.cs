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

using System.Linq.Expressions;
using System.Threading;

namespace FlatSharpEndToEndTests;

public static class Helpers
{
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

    public static T SerializeAndParse<T>(
        this T item,
        FlatBufferDeserializationOption option,
        out byte[] buffer) where T : class, IFlatBufferSerializable<T>
    {
        buffer = item.AllocateAndSerialize();
        return item.Serializer.Parse(buffer, option);
    }

    public static void AssertSequenceEqual(
        Span<byte> expected,
        Span<byte> actual)
    {
        Assert.Equal(expected.Length, actual.Length);

        for (int i = 0; i < expected.Length; ++i)
        {
            if (expected[i] != actual[i])
            {
                throw new Exception($"Buffers differed at position {i}. Expected = {expected[i]}. Actual = {actual[i]}");
            }
        }
    }

    public static void AssertMutationWorks<TSource, TProperty>(
        FlatBufferDeserializationOption option,
        TSource parent,
        bool isWriteThrough,
        Expression<Func<TSource, TProperty>> propertyLambda,
        TProperty newValue)
    {
        MemberExpression member = propertyLambda.Body as MemberExpression;
        PropertyInfo propInfo = member.Member as PropertyInfo;
        Action action = () => propInfo.SetMethod.Invoke(parent, new object[] { newValue });

        switch (option)
        {
            case FlatBufferDeserializationOption.Lazy when isWriteThrough:
            case FlatBufferDeserializationOption.Progressive when isWriteThrough:
            case FlatBufferDeserializationOption.GreedyMutable when isWriteThrough is false:
                action();

                // For value types, validate that they are the same.
                if (typeof(TProperty).IsValueType)
                {
                    TProperty readValue = (TProperty)propInfo.GetMethod.Invoke(parent, null);
                    Assert.Equal<TProperty>(newValue, readValue);
                }
                else if (option != FlatBufferDeserializationOption.Lazy)
                {
                    TProperty readValue = (TProperty)propInfo.GetMethod.Invoke(parent, null);
                    Assert.True(object.ReferenceEquals(newValue, readValue));
                }

                return;

            default:
                Assert.Throws<NotMutableException>(new Action(() =>
                {
                    var ex = Assert.Throws<TargetInvocationException>(action).InnerException;
                    throw ex;
                }));

                return;
        }
    }


    public static void AssertMutationWorks<TSource, TProperty>(
        FlatBufferDeserializationOption option,
        TSource parent,
        TProperty newValue,
        bool isWriteThrough,
        Func<TSource, TProperty> getValue,
        Action<TSource, TProperty> setValue)
    {
        switch (option)
        {
            case FlatBufferDeserializationOption.Lazy when isWriteThrough:
            case FlatBufferDeserializationOption.Progressive when isWriteThrough:
            case FlatBufferDeserializationOption.GreedyMutable when isWriteThrough is false:
                setValue(parent, newValue);
                TProperty readValue = getValue(parent);

                // For value types, validate that they are the same.
                if (typeof(TProperty).IsValueType)
                {
                    Assert.Equal<TProperty>(newValue, readValue);
                }
                else if (option != FlatBufferDeserializationOption.Lazy)
                {
                    Assert.True(object.ReferenceEquals(newValue, readValue));
                }

                return;

            default:
                Assert.Throws<NotMutableException>(() => setValue(parent, newValue));
                return;
        }
    }
}