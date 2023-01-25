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

using System.Threading;

namespace FlatSharp.Internal;

public sealed class GreedyIndexedVector<TKey, TValue> : IIndexedVector<TKey, TValue>
    where TValue : class, ISortableTable<TKey>
    where TKey : notnull
{
    private int alive;
    private readonly Dictionary<TKey, TValue> backingDictionary;
    private IIndexedVectorSource<TValue> backingVector; // hang on until we return to pool.
    private bool mutable;

    private GreedyIndexedVector()
    {
        this.backingDictionary = new Dictionary<TKey, TValue>();
        this.backingVector = null!;
        this.mutable = true;
    }

    public static GreedyIndexedVector<TKey, TValue> GetOrCreate(IIndexedVectorSource<TValue> backing, bool mutable)
    {
        if (!ObjectPool.TryGet(out GreedyIndexedVector<TKey, TValue>? vector))
        {
            vector = new();
        }

        vector.backingVector = backing;
        vector.mutable = mutable;
        vector.alive = 1;

        var dict = vector.backingDictionary;

#if !NETSTANDARD2_0
        dict.EnsureCapacity(backing.Count);
#endif

        backing.Accept<InitializerVistor, bool>(new InitializerVistor(dict));

        return vector;
    }

    private struct InitializerVistor : IReferenceVectorVisitor<TValue, bool>
    {
        private readonly Dictionary<TKey, TValue> dictionary;

        public InitializerVistor(Dictionary<TKey, TValue> dictionary) => this.dictionary = dictionary;

        public bool Visit<TDerived, TVector>(TVector vector)
            where TDerived : TValue
            where TVector : struct, ISimpleVector<TDerived>
        {
            int count = vector.Count;
            var dictionary = this.dictionary;

            for (int i = 0; i < count; ++i)
            {
                TDerived value = vector[i];
                TKey key = SortedVectorHelpers.KeyLookup<TValue, TKey>.KeyGetter(value);

                if (dictionary.TryGetValue(key, out var existingValue))
                {
                    (existingValue as IPoolableObject)?.ReturnToPool();
                }

                dictionary[key] = value;
            }

            return true;
        }
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
            throw new NotMutableException();
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
            throw new NotMutableException();
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
            throw new NotMutableException();
        }

        this.backingDictionary.Clear();
    }

    public bool Remove(TKey key)
    {
        if (!this.mutable)
        {
            throw new NotMutableException();
        }

        return this.backingDictionary.Remove(key);
    }

    public void ReturnToPool(bool unsafeForce = false)
    {
        if (FlatBufferDeserializationOption.Greedy.ShouldReturnToPool(unsafeForce))
        {
            if (Interlocked.Exchange(ref this.alive, 0) != 0)
            {
                var dict = this.backingDictionary;

                foreach (var item in dict)
                {
                    if (item.Value is IPoolableObject obj)
                    {
                        obj.ReturnToPool(true);
                    }
                }

                dict.Clear();
                this.mutable = false;
                this.backingVector.ReturnToPool(true);
                this.backingVector = null!;

                ObjectPool.Return(this);
            }
        }
    }
}
