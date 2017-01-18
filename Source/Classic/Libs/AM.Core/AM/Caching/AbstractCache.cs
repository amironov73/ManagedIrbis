// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AbstractCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Caching
{
    /// <summary>
    /// Abstract cache.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractCache<TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// Requester.
        /// </summary>
        [CanBeNull]
        public Func<TKey, TValue> Requester { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        /// <summary>
        /// Add or update item with given key.
        /// </summary>
        public abstract AbstractCache<TKey, TValue> Add
            (
                TKey key,
                [NotNull] TValue value
            );

        /// <summary>
        /// Clear.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Contains given key?
        /// </summary>
        public abstract bool ContainsKey
            (
                TKey key
            );

        /// <summary>
        /// Get item for given key.
        /// </summary>
        public abstract TValue Get
            (
                TKey key
            );

        /// <summary>
        /// Get or request item for given key.
        /// </summary>
        public virtual TValue GetOrRequest
            (
                TKey key
            )
        {
            TValue result = Get(key);

            if (ReferenceEquals(result, null)
                && !ReferenceEquals(Requester, null)
                )
            {
                result = Requester(key);

                if (!ReferenceEquals(result, null))
                {
                    Add(key, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Remove item for specified key.
        /// </summary>
        public abstract void Remove
            (
                TKey key
            );
    }
}
