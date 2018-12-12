// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryCounterDouble.cs -- simple dictionary to count values
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
    public sealed class DictionaryCounterDouble<TKey>
        : Dictionary<TKey, double>
    {
        #region Properties

        /// <summary>
        /// Gets the total.
        /// </summary>
        [JsonIgnore]
        public double Total
        {
            get
            {
                lock (_syncRoot)
                {
                    double result = 0.0;
                    foreach (double value in Values)
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
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterDouble{TKey}"/> class.
        /// </summary>
        public DictionaryCounterDouble()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DictionaryCounterDouble
        (
            [NotNull] IEqualityComparer<TKey> comparer
        )
            : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterDouble{TKey}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public DictionaryCounterDouble(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DictionaryCounterDouble{TKey}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DictionaryCounterDouble
            (
                [NotNull] DictionaryCounterDouble<TKey> dictionary
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
        public double Augment
            (
                [NotNull] TKey key,
                double increment
            )
        {
            lock (_syncRoot)
            {
                double value;
                TryGetValue(key, out value);
                value += increment;
                this[key] = value;
                return value;
            }
        }

        /// <summary>
        /// Get accumulated value for the specified key.
        /// </summary>
        public double GetValue
            (
                [NotNull] TKey key
            )
        {
            double result;

            TryGetValue(key, out result);

            return result;
        }

        /// <summary>
        /// Increment the specified key.
        /// </summary>
        public double Increment
            (
                [NotNull] TKey key
            )
        {
            return Augment
                (
                    key,
                    1.0
                );
        }

        #endregion
    }
}
