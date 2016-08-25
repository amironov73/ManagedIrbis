/* CacheItem.cs -- cache item.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Caching
{
    /// <summary>
    /// Cache item.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CacheItem<TKey,TValue>
        where TValue: class 
    {
        #region Properties

        /// <summary>
        /// Created.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Is alive?
        /// </summary>
        public bool IsAlive { get { return _value.IsAlive; } }

        /// <summary>
        /// Key.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// Last used.
        /// </summary>
        public DateTime LastUsed { get; private set; }

        /// <summary>
        /// Usage count.
        /// </summary>
        public int UsageCount { get; private set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public TValue Value
        {
            get
            {
                TValue result = (TValue) _value.Target;

                if (!ReferenceEquals(result, null))
                {
                    UsageCount++;
                    LastUsed = DateTime.Now;
                }

                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CacheItem
            (
                TKey key,
                [NotNull] TValue value
            )
        {
            Code.NotNull(value, "value");

            LastUsed = Created = DateTime.Now;
            Key = key;
            _value = new WeakReference(value);
        }

        #endregion

        #region Private members

        private readonly WeakReference _value;

        #endregion

        #region Public methods


        #endregion
    }
}
