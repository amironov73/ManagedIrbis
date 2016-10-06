/* MemoryCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Caching
{
    /// <summary>
    /// Cache base.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MemoryCache<TKey, TValue>
        : AbstractCache<TKey, TValue>
        where TValue: class
    {
        #region Constants

        /// <summary>
        /// Default lifetime, seconds.
        /// </summary>
        public const int DefaultLifetime = 30*60;

        #endregion

        #region Properties

        /// <summary>
        /// Lifetime, seconds.
        /// </summary>
        public int Lifetime { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Dictionary
        /// </summary>
        public MemoryCache()
        {
            Lifetime = DefaultLifetime;
            _dictionary
                = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();
        }

        #endregion

        #region Private members

        private readonly ConcurrentDictionary<TKey, CacheItem<TKey,TValue>>
            _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Add or update item with given key.
        /// </summary>
        public override AbstractCache<TKey, TValue> Add
            (
                TKey key,
                TValue value
            )
        {
            Code.NotNull(value, "value");

            Cleanup();

            CacheItem<TKey, TValue> item = new CacheItem<TKey, TValue>
                (
                    key,
                    value
                );
            _dictionary[key] = item;

            return this;
        }

        /// <summary>
        /// Cleanup.
        /// </summary>
        public void Cleanup()
        {
            RemoveDeadItems();

            DateTime before = DateTime.Now.AddSeconds(-Lifetime);
            RemoveUnusedItems(before);
        }

        /// <summary>
        /// Clear.
        /// </summary>
        public override void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Contains given key?
        /// </summary>
        public override bool ContainsKey
            (
                TKey key
            )
        {
            Cleanup();

            CacheItem<TKey, TValue> item;
            if (!_dictionary.TryGetValue(key, out item))
            {
                return false;
            }

            return item.IsAlive;
        }

        /// <summary>
        /// Get item for given key.
        /// </summary>
        public override TValue Get
            (
                TKey key
            )
        {
            Cleanup();

            CacheItem<TKey, TValue> item;
            if (!_dictionary.TryGetValue(key, out item))
            {
                return default(TValue);
            }

            TValue result = item.Value;
            if (ReferenceEquals(result, null))
            {
                _dictionary.TryRemove(key, out item);
            }

            return result;
        }

        /// <summary>
        /// Remove item for specified key.
        /// </summary>
        public override void Remove
            (
                TKey key
            )
        {
            CacheItem<TKey, TValue> item;
            _dictionary.TryRemove(key, out item);
        }

        /// <summary>
        /// Remove dead items.
        /// </summary>
        public void RemoveDeadItems()
        {
            List<CacheItem<TKey,TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (!item.Value.IsAlive)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }

        /// <summary>
        /// Remove too old items.
        /// </summary>
        public void RemoveOldItems
            (
                DateTime before
            )
        {
            List<CacheItem<TKey, TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (item.Value.Created.CompareTo(before) < 0)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }

        /// <summary>
        /// Remove unused items.
        /// </summary>
        public void RemoveUnusedItems
            (
                DateTime before
            )
        {
            List<CacheItem<TKey, TValue>> list
                = new List<CacheItem<TKey, TValue>>();
            foreach (var item in _dictionary)
            {
                if (item.Value.LastUsed.CompareTo(before) < 0)
                {
                    list.Add(item.Value);
                }
            }

            foreach (var dead in list)
            {
                CacheItem<TKey, TValue> item;
                _dictionary.TryRemove(dead.Key, out item);
            }
        }

        #endregion
    }
}

#endif
