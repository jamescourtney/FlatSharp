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

namespace FlatSharp;

/// <summary>
/// An <see cref="IIndexedVector{TKey, TValue}"/> implementation based on a <see cref="Dictionary{TKey, TValue}"/>.
/// </summary>
public sealed class IndexedVector<TKey, TValue> : IIndexedVector<TKey, TValue>
    where TValue : class, ISortableTable<TKey>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> backingDictionary;
    private bool mutable;

    public IndexedVector()
    {
        this.backingDictionary = new Dictionary<TKey, TValue>();
        this.mutable = true;
    }

    public IndexedVector(int capacity)
    {
        this.backingDictionary = new Dictionary<TKey, TValue>(capacity);
        this.mutable = true;
    }

    public IndexedVector(IEnumerable<TValue> items, int capacity, bool mutable)
    {
        var dictionary = new Dictionary<TKey, TValue>(capacity);
        foreach (var item in items)
        {
            dictionary[GetKey(item)] = item;
        }

        this.backingDictionary = dictionary;
        this.mutable = mutable;
    }

    public IndexedVector(IReadOnlyList<TValue> items, bool mutable) : this(items, items.Count, mutable)
    {
    }

    /// <summary>
    /// Gets the key from the given value.
    /// </summary>
    public static TKey GetKey(TValue value) => SortedVectorHelpers.KeyLookup<TValue, TKey>.KeyGetter(value);

    /// <summary>
    /// An indexer for getting values by their keys.
    /// </summary>
    public TValue this[TKey key]
    {
        get => this.backingDictionary[key];
    }

    /// <summary>
    /// Indicates if this IndexedVector is read only.
    /// </summary>
    public bool IsReadOnly => !this.mutable;

    /// <summary>
    /// Gets the count of items.
    /// </summary>
    public int Count => this.backingDictionary.Count;

    /// <summary>
    /// Freezes an Indexed vector, preventing further modifications.
    /// </summary>
    public void Freeze()
    {
        this.mutable = false;
    }

    /// <summary>
    /// Returns true if the vector contains the given key.
    /// </summary>
    public bool ContainsKey(TKey key) => this.backingDictionary.ContainsKey(key);

    /// <summary>
    /// Tries to get the given value from the backing dictionary.
    /// </summary>
    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
    {
        return this.backingDictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets the dictionary's enumerator.
    /// </summary>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.backingDictionary.GetEnumerator();

    /// <summary>
    /// Gets a non-generic enumerator.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <summary>
    /// Adds or replaces the item with the given key to the indexed vector.
    /// </summary>
    public void AddOrReplace(TValue value)
    {
        if (!this.mutable)
        {
            FSThrow.NotMutable();
        }

        this.backingDictionary[GetKey(value)] = value;
    }

    /// <summary>
    /// Attempts to add the value to the indexed vector, if a key does not already exist.
    /// </summary>
    public bool Add(TValue value)
    {
        if (!this.mutable)
        {
            return FSThrow.NotMutable<bool>();
        }

        TKey key = GetKey(value);

#if NETSTANDARD2_0
        var dictionary = this.backingDictionary;
        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary[key] = value;
        return true;
#else
        return this.backingDictionary.TryAdd(key, value);
#endif
    }

    public void Clear()
    {
        if (!this.mutable)
        {
            FSThrow.NotMutable();
        }

        this.backingDictionary.Clear();
    }

    public bool Remove(TKey key)
    {
        if (!this.mutable)
        {
            return FSThrow.NotMutable<bool>();
        }

        return this.backingDictionary.Remove(key);
    }

    [ExcludeFromCodeCoverage]
    public void ReturnToPool(bool unsafeForce = false)
    {
    }
}
