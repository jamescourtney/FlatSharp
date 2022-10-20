/*
 * Copyright 2021 James Courtney
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

namespace FlatSharp.Internal;

/// <summary>
/// A base flat buffer vector, common to standard vectors and unions.
/// </summary>
public sealed class FlatBufferVectorBase<T, TInputBuffer, TItemAccessor, TActualType>
    : IList<T>
    , IReadOnlyList<T>
    , IFlatBufferDeserializedVector
    , IFlatBufferVector<T>

    where TInputBuffer : IInputBuffer
    where TItemAccessor : IVectorItemAccessor<T, TInputBuffer, TActualType>
    where TActualType : T
{
    private readonly TInputBuffer memory;
    private readonly TableFieldContext fieldContext;
    private readonly short remainingDepth;
    private readonly TItemAccessor itemAccessor;

    public FlatBufferVectorBase(
        TInputBuffer memory,
        TItemAccessor itemAccessor,
        short remainingDepth,
        TableFieldContext fieldContext)
    {
        this.memory = memory;
        this.remainingDepth = remainingDepth;
        this.fieldContext = fieldContext;
        this.itemAccessor = itemAccessor;
    }

    /// <summary>
    /// Gets the item at the given index.
    /// </summary>
    public T this[int index]
    {
        get
        {
            this.CheckIndex(index);
            this.itemAccessor.ParseItem(index, this.memory, this.remainingDepth, this.fieldContext, out TActualType item);
            return item;
        }
        set
        {
            this.CheckIndex(index);
            this.itemAccessor.WriteThrough(index, value, this.memory, this.fieldContext);
        }
    }

    public int Count => this.itemAccessor.Count;

    public bool IsReadOnly => true;

    IInputBuffer IFlatBufferDeserializedVector.InputBuffer => this.memory;

    int IFlatBufferDeserializedVector.ItemSize => this.itemAccessor.ItemSize;

    int IFlatBufferDeserializedVector.Count => this.Count;

    public bool Contains(T? item)
    {
        return this.IndexOf(item) >= 0;
    }

    public void CopyTo(T[]? array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var count = this.Count;
        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out TActualType item);
            array[arrayIndex + i] = item;
        }
    }

    internal void CopyRangeTo(int startIndex, T?[] array, uint count)
    {
        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        // might have more elements than we do.
        count = Math.Min(
            checked((uint)(this.Count - startIndex)),
            count);

        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i + startIndex, buffer, remainingDepth, context, out TActualType item);
            array[i] = item;
        }
    }

    public T[] ToArray()
    {
        T[] array = new T[this.Count];
        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < array.Length; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out TActualType item);
            array[i] = item;
        }

        return array;
    }

    public IEnumerator<T> GetEnumerator()
    {
        int count = this.Count;
        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out TActualType parsed);
            yield return parsed;
        }
    }

    public int IndexOf(T? item)
    {
        // FlatBuffer vectors are not allowed to have null by definition.
        if (item is null)
        {
            return -1;
        }

        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        int count = this.Count;
        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out TActualType parsed);
            if (item.Equals(parsed))
            {
                return i;
            }
        }

        return -1;
    }

    public List<T> FlatBufferVectorToList()
    {
        int count = this.Count;
        var list = new List<T>(count);

        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out TActualType item);
            list.Add(item);
        }

        return list;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckIndex(int index)
    {
        if ((uint)index >= this.Count)
        {
            throw new IndexOutOfRangeException();
        }
    }

    public void Add(T item)
    {
        throw new NotMutableException("FlatBufferVector does not allow adding items.");
    }

    public void Clear()
    {
        throw new NotMutableException("FlatBufferVector does not support clearing.");
    }

    public void Insert(int index, T item)
    {
        throw new NotMutableException("FlatBufferVector does not support inserting.");
    }

    public bool Remove(T item)
    {
        throw new NotMutableException("FlatBufferVector does not support removing.");
    }

    public void RemoveAt(int index)
    {
        throw new NotMutableException("FlatBufferVector does not support removing.");
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    int IFlatBufferDeserializedVector.OffsetOf(int index) => this.itemAccessor.OffsetOf(index);

    object IFlatBufferDeserializedVector.ItemAt(int index) => this[index]!;

    internal TActualType GetValue(int index)
    {
        this.CheckIndex(index);
        this.itemAccessor.ParseItem(index, this.memory, this.remainingDepth, this.fieldContext, out TActualType item);
        return item;
    }

    public TReturn Iterate<TIterator, TReturn>(TIterator iterator)
#pragma warning disable CS0618 // Type or member is obsolete
        where TIterator : IFlatBufferVectorIterator<TReturn, T>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        if (typeof(T).IsValueType)
        {
            if (iterator is IFlatBufferValueVectorIterator<TReturn, T>)
            {
                return ((IFlatBufferValueVectorIterator<TReturn, T>)iterator).Iterate(new ValueVector(this));
            }

            throw new Exception("something");
        }
        else
        {
            if (iterator is IFlatBufferReferenceVectorIterator<TReturn, T>)
            {
                return ((IFlatBufferReferenceVectorIterator<TReturn, T>)iterator).Iterate<ReferenceVector, TActualType>(new ReferenceVector(this));
            }

            throw new Exception("something");
        }
    }

    private struct ValueVector : IFlatBufferIterableVector<T>
    {
        private readonly FlatBufferVectorBase<T, TInputBuffer, TItemAccessor, TActualType> inner;

        public ValueVector(FlatBufferVectorBase<T, TInputBuffer, TItemAccessor, TActualType> inner)
        {
            this.inner = inner;
        }

        public T this[int index]
        {
            get => this.inner.GetValue(index);
            set => this.inner[index] = value;
        }

        public int Count => this.inner.Count;
    }

    private struct ReferenceVector : IFlatBufferIterableVector<TActualType>
    {
        private readonly FlatBufferVectorBase<T, TInputBuffer, TItemAccessor, TActualType> inner;

        public ReferenceVector(FlatBufferVectorBase<T, TInputBuffer, TItemAccessor, TActualType> inner)
        {
            this.inner = inner;
        }

        public TActualType this[int index]
        {
            get => this.inner.GetValue(index);
            set => this.inner[index] = value;
        }

        public int Count => this.inner.Count;
    }
}
