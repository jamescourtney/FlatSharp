﻿/*
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

using System.Threading;

namespace FlatSharp.Internal;

/// <summary>
/// A base flat buffer vector, common to standard vectors and unions.
/// </summary>
public sealed class FlatBufferVectorBase<T, TInputBuffer, TItemAccessor> 
    : IList<T>
    , IReadOnlyList<T>
    , IFlatBufferDeserializedVector
    , IPoolableObject

    where TInputBuffer : IInputBuffer
    where TItemAccessor : IVectorItemAccessor<T, TInputBuffer>
{
    private TInputBuffer memory;
    private TableFieldContext fieldContext;
    private short remainingDepth;
    private TItemAccessor itemAccessor;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private FlatBufferVectorBase()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private void Initialize(
        TInputBuffer memory,
        TItemAccessor itemAccessor,
        short remainingDepth,
        TableFieldContext fieldContext,
        FlatBufferDeserializationOption option)
    {
        this.memory = memory;
        this.remainingDepth = remainingDepth;
        this.itemAccessor = itemAccessor;
        this.DeserializationOption = option;
        this.fieldContext = fieldContext;
    }

    public static FlatBufferVectorBase<T, TInputBuffer, TItemAccessor> GetOrCreate(
        TInputBuffer memory,
        TItemAccessor itemAccessor,
        short remainingDepth,
        TableFieldContext fieldContext,
        FlatBufferDeserializationOption option)
    {
        if (!ObjectPool.TryGet<FlatBufferVectorBase<T, TInputBuffer, TItemAccessor>>(out var item))
        {
            item = new FlatBufferVectorBase<T, TInputBuffer, TItemAccessor>();
        }

        item.Initialize(memory, itemAccessor, remainingDepth, fieldContext, option);

        return item;
    }

    /// <summary>
    /// Gets the item at the given index.
    /// </summary>
    public T this[int index]
    {
        get
        {
            this.CheckIndex(index);
            this.itemAccessor.ParseItem(index, this.memory, this.remainingDepth, this.fieldContext, out T item);
            return item;
        }
        set
        {
            this.CheckIndex(index);
            this.itemAccessor.WriteThrough(index, value, this.memory, this.fieldContext);
        }
    }

    public FlatBufferDeserializationOption DeserializationOption { get; private set; }

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
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out array[arrayIndex + i]);
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
            this.itemAccessor.ParseItem(i + startIndex, buffer, remainingDepth, context, out array[i]);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        int count = this.Count;
        var context = this.fieldContext;
        var remainingDepth = this.remainingDepth;
        var buffer = this.memory;

        for (int i = 0; i < count; ++i)
        {
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out T parsed);
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
            this.itemAccessor.ParseItem(i, buffer, remainingDepth, context, out T parsed);
            if (item.Equals(parsed))
            {
                return i;
            }
        }

        return -1;
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

    public void ReturnToPool(bool force = false)
    {
        if (this.DeserializationOption.ShouldReturnToPool(force))
        {
            var context = Interlocked.Exchange(ref this.fieldContext!, null);
            if (context is not null)
            {
                this.memory = default!;
                this.remainingDepth = default;
                this.itemAccessor = default!;
                ObjectPool.Return(this);
            }
        }
    }
}
