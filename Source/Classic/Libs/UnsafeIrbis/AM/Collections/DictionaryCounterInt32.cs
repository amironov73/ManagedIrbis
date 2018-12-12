// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryCounterInt32.cs -- simple dictionary to count values
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace UnsafeAM.Collections
{
    /// <summary>
    /// Simple dictionary to count values.
    /// </summary>
    [PublicAPI]
    public sealed class DictionaryCounterInt32<TKey>
        : Dictionary<TKey, int>
    {
        #region Properties

        /// <summary>
        /// Gets the total.
        /// </summary>
        [JsonIgnore]
        public int Total
        {
            get
            {
                lock (_syncRoot)
                {
                    int result = 0;
                    foreach (int value in Values)
                    {
                        result += value;
                    }

                    return result;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryCounterInt32()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DictionaryCounterInt32
            (
                [NotNull] IEqualityComparer<TKey> comparer
            )
            : base(comparer)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public DictionaryCounterInt32
            (
                int capacity
            )
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterInt32{TKey}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DictionaryCounterInt32
            (
                [NotNull] DictionaryCounterInt32<TKey> dictionary
            )
            : base(dictionary)
        {
        }

        #endregion

        #region Private members

        private object _syncRoot
        {
            [DebuggerStepThrough]
            get
            {
                return ((ICollection)this).SyncRoot;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Augments the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="increment">The value.</param>
        /// <returns>New value for given key.</returns>
        public int Augment
            (
                [NotNull] TKey key,
                int increment
            )
        {
            lock (_syncRoot)
            {
                int value;
                TryGetValue(key, out value);
                value += increment;
                this[key] = value;
                return value;
            }
        }

        /// <summary>
        /// Get accumulated value for the specified key.
        /// </summary>
        public int GetValue
            (
                [NotNull] TKey key
            )
        {
            int result;

            TryGetValue(key, out result);

            return result;
        }

        /// <summary>
        /// Increment the specified key.
        /// </summary>
        public int Increment
            (
                [NotNull] TKey key
            )
        {
            return Augment
                (
                    key,
                    1
                );
        }

        #endregion
    }
}
