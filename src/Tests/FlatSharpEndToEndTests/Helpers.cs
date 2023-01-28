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

using FlatSharp.Internal;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace FlatSharpEndToEndTests;

public static class Helpers
{
    public static void Repeat(Action action, int times = 5)
    {
        for (int i = 0; i < times; ++i)
        {
            action();
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
        //var combined = expected.ToArray().Zip(actual.ToArray()).ToArray();
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
        Assert.True(parent is IFlatBufferDeserializedObject);

        {
            var dobj = (IFlatBufferDeserializedObject)parent;

            Assert.Equal(option, dobj.DeserializationContext.DeserializationOption);
            Assert.Equal(typeof(TSource), dobj.TableOrStructType);

            switch (option)
            {
                case FlatBufferDeserializationOption.Greedy:
                case FlatBufferDeserializationOption.GreedyMutable:
                    Assert.False(dobj.CanSerializeWithMemoryCopy);
                    Assert.Null(dobj.InputBuffer);
                    break;

                default:
                    Assert.True(dobj.CanSerializeWithMemoryCopy);
                    Assert.NotNull(dobj.InputBuffer);
                    break;
            }
        }

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
                var ex = Assert.Throws<NotMutableException>(new Action(() =>
                {
                    var ex = Assert.Throws<TargetInvocationException>(action).InnerException;
                    throw ex;
                }));

                if (isWriteThrough && option == FlatBufferDeserializationOption.GreedyMutable)
                {
                    Assert.Equal("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.", ex.Message);
                }

                return;
        }
    }

    public static void ValidateListVector<T>(
        FlatBufferDeserializationOption option,
        bool isWriteThrough,
        IList<T> items,
        T newValue)
    {
        // This can be lots of things: NotMutable, ArgumentOfRange, IndexOutOfRange, etc.
        Assert.ThrowsAny<Exception>(() => items[-1]);
        Assert.ThrowsAny<Exception>(() => items[items.Count]);
        Assert.ThrowsAny<Exception>(() => items[-1] = default);
        Assert.ThrowsAny<Exception>(() => items[items.Count] = default);

        {
            T[] target = new T[items.Count];
            items.CopyTo(target, 0);

            Assert.Equal(items.Count, target.Length);

            if (option != FlatBufferDeserializationOption.Lazy || typeof(T).IsPrimitive || typeof(T) == typeof(string))
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    Assert.Equal(i, items.IndexOf(items[i]));
                    Assert.True(items.Contains(items[i]));
                    Assert.Equal(items[i], target[i]);
                }
            }
            else if (!typeof(T).IsValueType)
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    Assert.NotEqual(i, items.IndexOf(items[i]));
                    Assert.False(items.Contains(items[i]));
                    Assert.NotEqual(items[i], target[i]);
                }
            }
        }

        Assert.Equal(
            items.IsReadOnly,
            option != FlatBufferDeserializationOption.GreedyMutable);

        for (int i = 0; i < items.Count; ++i)
        {
            switch (option)
            {
                case FlatBufferDeserializationOption.Lazy when isWriteThrough:
                case FlatBufferDeserializationOption.Progressive when isWriteThrough:
                case FlatBufferDeserializationOption.GreedyMutable when isWriteThrough is false:
                    items[i] = newValue;

                    // For value types, validate that they are the same.
                    if (typeof(T).IsValueType)
                    {
                        Assert.Equal<T>(newValue, items[i]);
                    }
                    else if (option != FlatBufferDeserializationOption.Lazy)
                    {
                        Assert.True(object.ReferenceEquals(newValue, items[i]));
                    }

                    break;

                default:
                    var ex = Assert.Throws<NotMutableException>(new Action(() =>
                    {
                        items[i] = newValue;
                    }));

                    if (isWriteThrough && option == FlatBufferDeserializationOption.GreedyMutable)
                    {
                        Assert.Equal("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.", ex.Message);
                    }

                    break;
            }
        }

        if (option == FlatBufferDeserializationOption.GreedyMutable && !isWriteThrough)
        {
            items.Clear();
            Assert.Empty(items);

            items.Add(default);
            Assert.Single(items);

            items.Remove(items[0]);
            Assert.Empty(items);

            items.Insert(0, default);
            Assert.Single(items);

            items.RemoveAt(0);
            Assert.Empty(items);
        }
        else
        {
            Assert.Throws<NotMutableException>(() => items.Clear());
            Assert.Throws<NotMutableException>(() => items.Add(default));
            Assert.Throws<NotMutableException>(() => items.Remove(items[0]));
            Assert.Throws<NotMutableException>(() => items.Insert(0, default));
            Assert.Throws<NotMutableException>(() => items.RemoveAt(0));
        }
    }

    public static T TestProgressiveFieldLoad<P, T>(int index, bool expected, P parent, Func<P, T> getter)
    {
        IFlatBufferDeserializedObject obj = (IFlatBufferDeserializedObject)parent;

        int maskNum = index / 8;
        byte bitMask = (byte)(1 << (index % 8));

        FieldInfo mask = parent.GetType().GetField($"__mask{maskNum}", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo vtable = parent.GetType().GetField("__vtable", BindingFlags.NonPublic | BindingFlags.Instance);

        bool IsFieldLoaded()
        {
            byte value = (byte)mask.GetValue(parent);
            return (value & bitMask) != 0;
        }

        bool IsFieldPresent()
        {
            if (typeof(P).GetCustomAttribute<FlatBufferStructAttribute>() is not null)
            {
                return true;
            }

            IVTable vt = (IVTable)vtable.GetValue(parent);
            return vt.OffsetOf(obj.InputBuffer!, index) != 0;
        }

        Assert.False(IsFieldLoaded());
        Assert.Equal(expected, IsFieldPresent());

        for (int i = 0; i < 10; ++i)
        {
            T item = getter(parent);

            Assert.True(IsFieldLoaded());
            T next = getter(parent);

            if (typeof(T).IsValueType)
            {
                Assert.Equal(item, next);
            }
            else
            {
                Assert.True(object.ReferenceEquals(item, next));
            }
        }

        return getter(parent);
    }

    private static int counter;

    public static IList<T> CreateList<T>(params T[] values)
    {
        return (Interlocked.Increment(ref counter) % 3) switch
        {
            0 => values,
            1 => new List<T>(values),
            _ => new DummyList<T>(values),
        };
    }

    private class DummyList<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly List<T> list = new();

        public DummyList(T[] values)
        {
            this.list.AddRange(values);
        }

        public T this[int index] { get => ((IList<T>)list)[index]; set => ((IList<T>)list)[index] = value; }

        public int Count => ((ICollection<T>)list).Count;

        public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

        public void Add(T item)
        {
            ((ICollection<T>)list).Add(item);
        }

        public void Clear()
        {
            ((ICollection<T>)list).Clear();
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)list).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)list).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)list).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            ((IList<T>)list).Insert(index, item);
        }

        public bool Remove(T item)
        {
            return ((ICollection<T>)list).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<T>)list).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }
}