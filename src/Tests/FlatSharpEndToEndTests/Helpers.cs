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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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

    public static string ToCSharpArrayString(this byte[] buffer)
    {
        StringBuilder sb = new();
        sb.Append("new byte[] { ");

        for (int i = 0; i < buffer.Length; ++i)
        {
            if (i % 4 == 0)
            {
                sb.AppendLine();
            }

            sb.Append(buffer[i]);
            sb.Append(", ");
        }

        sb.Append("}");
        return sb.ToString();
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
        Assert.AreEqual(expected.Length, actual.Length);

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
        TProperty newValue,
        Action<TProperty, TProperty>? assertEqual = null)
    {
        Assert.IsTrue(parent is IFlatBufferDeserializedObject);

        {
            var dobj = (IFlatBufferDeserializedObject)parent;

            Assert.AreEqual(option, dobj.DeserializationContext.DeserializationOption);
            Assert.AreEqual(typeof(TSource), dobj.TableOrStructType);

            switch (option)
            {
                case FlatBufferDeserializationOption.Greedy:
                case FlatBufferDeserializationOption.GreedyMutable:
                    Assert.IsFalse(dobj.CanSerializeWithMemoryCopy);
                    Assert.IsNull(dobj.InputBuffer);
                    break;

                default:
                    Assert.IsTrue(dobj.CanSerializeWithMemoryCopy);
                    Assert.IsNotNull(dobj.InputBuffer);
                    break;
            }
        }

        MemberExpression member = propertyLambda.Body as MemberExpression;
        PropertyInfo propInfo = member.Member as PropertyInfo;

        Func<TProperty> get = () => (TProperty)propInfo.GetMethod.Invoke(parent, null);
        Action set = () => propInfo.SetMethod.Invoke(parent, new object[] { newValue });

        // should be equal to itself to start with.
        AssertEquality(option, get(), get(), assertEqual);

        switch (option)
        {
            case FlatBufferDeserializationOption.Lazy when isWriteThrough:
            case FlatBufferDeserializationOption.Progressive when isWriteThrough:
            case FlatBufferDeserializationOption.GreedyMutable when isWriteThrough is false:
                set();
                AssertEquality(option, newValue, get(), assertEqual);
                return;

            default:
                var ex = Assert.ThrowsException<NotMutableException>(new Action(() =>
                {
                    var ex = Assert.ThrowsException<TargetInvocationException>(set).InnerException;
                    throw ex;
                }));

                if (isWriteThrough && option == FlatBufferDeserializationOption.GreedyMutable)
                {
                    Assert.AreEqual("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.", ex.Message);
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
        CheckRangeExceptions(option, isWriteThrough, items);

        if (items is IFlatBufferDeserializedVector vec)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => vec.OffsetOf(-1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => vec.OffsetOf(items.Count));

            for (int i = 0; i < vec.Count; ++i)
            {
                Assert.AreEqual(vec.OffsetOf(i), vec.OffsetBase + (i * vec.ItemSize));
            }

            Assert.ThrowsException<IndexOutOfRangeException>(() => vec.ItemAt(-1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => vec.ItemAt(items.Count));
        }

        {
            T[] target = new T[items.Count];
            items.CopyTo(target, 0);

            Assert.AreEqual(items.Count, target.Length);

            if (option != FlatBufferDeserializationOption.Lazy || typeof(T).IsPrimitive || typeof(T) == typeof(string))
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    if (typeof(T).IsValueType || option == FlatBufferDeserializationOption.Lazy)
                    {
                        Assert.AreEqual(items[i], target[i]);
                    }
                    else
                    {
#pragma warning disable xUnit2005 // Do not use identity check on value type
                        Assert.AreSame(items[i], items[i]);
#pragma warning restore xUnit2005 // Do not use identity check on value type
                    }

                    Assert.AreEqual(i, items.IndexOf(items[i]));
                    Assert.IsTrue(items.Contains(items[i]));
                }
            }
            else if (!typeof(T).IsValueType)
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    Assert.AreNotEqual(i, items.IndexOf(items[i]));
                    Assert.IsFalse(items.Contains(items[i]));
                    Assert.AreNotEqual(items[i], target[i]);
                }
            }
        }

        Assert.AreEqual(
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
                        Assert.AreEqual<T>(newValue, items[i]);
                    }
                    else if (option != FlatBufferDeserializationOption.Lazy)
                    {
                        Assert.IsTrue(object.ReferenceEquals(newValue, items[i]));
                    }

                    break;

                default:
                    var ex = Assert.ThrowsException<NotMutableException>(new Action(() =>
                    {
                        items[i] = newValue;
                    }));

                    if (isWriteThrough && option == FlatBufferDeserializationOption.GreedyMutable)
                    {
                        Assert.AreEqual("WriteThrough fields are implemented as readonly when using 'GreedyMutable' serializers.", ex.Message);
                    }

                    break;
            }
        }

        if (option == FlatBufferDeserializationOption.GreedyMutable && !isWriteThrough)
        {
            items.Clear();
            Assert.AreEqual(0, items.Count);

            items.Add(default);
            Assert.AreEqual(1, items.Count);

            items.Remove(items[0]);
            Assert.AreEqual(0, items.Count);

            items.Insert(0, default);
            Assert.AreEqual(1, items.Count);

            items.RemoveAt(0);
            Assert.AreEqual(0, items.Count);
        }
        else
        {
            Assert.ThrowsException<NotMutableException>(() => items.Clear());
            Assert.ThrowsException<NotMutableException>(() => items.Add(default));
            Assert.ThrowsException<NotMutableException>(() => items.Remove(items[0]));
            Assert.ThrowsException<NotMutableException>(() => items.Insert(0, default));
            Assert.ThrowsException<NotMutableException>(() => items.RemoveAt(0));
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

        Assert.IsFalse(IsFieldLoaded());
        Assert.AreEqual(expected, IsFieldPresent());

        for (int i = 0; i < 10; ++i)
        {
            T item = getter(parent);

            Assert.IsTrue(IsFieldLoaded());
            T next = getter(parent);

            if (typeof(T).IsValueType)
            {
                Assert.AreEqual(item, next);
            }
            else
            {
                Assert.IsTrue(object.ReferenceEquals(item, next));
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

    private static void AssertEquality<T>(FlatBufferDeserializationOption option, T a, T b, Action<T, T>? assertEqual)
    {
        if (assertEqual is null)
        {
            if (typeof(T).IsValueType && (typeof(T).IsPrimitive || typeof(T).IsEnum))
            {
                assertEqual = (a, b) => Assert.AreEqual(a, b);
            }
            else if (typeof(T) == typeof(string))
            {
                assertEqual = (a, b) => Assert.AreEqual(a, b);
            }
        }

        if (!typeof(T).IsValueType && option != FlatBufferDeserializationOption.Lazy)
        {
            Assert.IsTrue(object.ReferenceEquals(a, b));
        }

        assertEqual?.Invoke(a, b);
    }

    private static void CheckRangeExceptions<T>(FlatBufferDeserializationOption option, bool isWriteThrough, IList<T> list)
    {
        if (option == FlatBufferDeserializationOption.Lazy || option == FlatBufferDeserializationOption.Progressive)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => list[-1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => list[list.Count]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => list[-1] = default);
            Assert.ThrowsException<IndexOutOfRangeException>(() => list[list.Count] = default);
        }
        else if (option == FlatBufferDeserializationOption.Greedy
             || (isWriteThrough && option == FlatBufferDeserializationOption.GreedyMutable))
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.ThrowsException<NotMutableException>(() => list[-1] = default);
            Assert.ThrowsException<NotMutableException>(() => list[list.Count] = default);
        }
        else
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[-1] = default);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list[list.Count] = default);
        }
    }
}