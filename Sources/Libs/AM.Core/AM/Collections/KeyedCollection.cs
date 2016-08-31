/* KeyedCollection.cs -- collection with key=value support
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Collection with key=value support.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class KeyedCollection<TKey, TValue>
        : IEnumerable<KeyedCollection<TKey, TValue>.Element>
    {
        #region Nested classes

        /// <summary>
        /// Collection item.
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
        [DebuggerDisplay("{Key}={Value}")]
        public sealed class Element
        {
            #region Properties

            /// <summary>
            /// Key.
            /// </summary>
            [CanBeNull]
            public TKey Key { get; private set; }

            /// <summary>
            /// Value.
            /// </summary>
            [CanBeNull]
            public TValue Value { get; internal set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Element
                (
                    TKey key,
                    TValue value
                )
            {
                Key = key;
                Value = value;
            }

            #endregion

            #region Object members

            /// <inheritdoc />
            public override string ToString()
            {
                return string.Format
                    (
                        "{0}={1}",
                        Key,
                        Value
                    );
            }

            #endregion
        }

        #endregion

        // =========================================================

        #region Properties

        /// <summary>
        /// Gets the generic equality comparer that
        /// is used to determine equality of keys
        /// in the collection.
        /// </summary>
        public IEqualityComparer<TKey> Comparer
        {
            get { return _comparer; }
        }

        /// <summary>
        /// Gets the number of elements actually contained
        /// in the <see cref="KeyedCollection{TKey,TValue}"/>.
        /// </summary>
        public int Count
        {
            get { return _elements.Count; }
        }

        /// <summary>
        /// Gets the element with the specified key.
        /// </summary>
        [CanBeNull]
        public TValue this[TKey key]
        {
            get
            {
                Element element = GetElement(key);
                if (ReferenceEquals(element, null))
                {
                    throw new KeyNotFoundException();
                }

                return element.Value;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        [CanBeNull]
        public TValue this[int index]
        {
            get
            {
                // ReSharper disable once InconsistentlySynchronizedField
                return _elements[index].Value;
            }
            set
            {
                // ReSharper disable once InconsistentlySynchronizedField
                _elements[index].Value = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public KeyedCollection()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public KeyedCollection
            (
                [NotNull] IEqualityComparer<TKey> comparer
            )
        {
            Code.NotNull(comparer, "comparer");

            _elements = new List<Element>();
            _comparer = comparer;
            _lock = new object();
        }

        #endregion

        #region Private members

        private readonly List<Element> _elements;

        private readonly IEqualityComparer<TKey> _comparer;

        private readonly object _lock;

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an object to the end of the
        /// <see cref="KeyedCollection{TKey,TValue}"/>.
        /// </summary>
        [NotNull]
        public KeyedCollection<TKey, TValue> Add
            (
                TKey key,
                TValue value
            )
        {
            lock (_lock)
            {
                Element element = new Element(key, value);
                _elements.Add(element);

                return this;

            }
        }

        /// <summary>
        /// Add or replace element.
        /// </summary>
        [NotNull]
        public KeyedCollection<TKey, TValue> AddOrReplace
            (
                TKey key,
                TValue value
            )
        {
            lock (_lock)
            {
                Element element = GetElement(key);
                if (ReferenceEquals(element, null))
                {
                    element = new Element(key, value);
                    _elements.Add(element);
                }
                element.Value = value;

                return this;
            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        [NotNull]
        public KeyedCollection<TKey, TValue> Clear()
        {
            lock (_lock)
            {
                _elements.Clear();

                return this;
            }
        }

        /// <summary>
        /// Determines whether the collection contains
        /// an element with the specified key.
        /// </summary>
        public bool Contains
            (
                TKey key
            )
        {
            lock (_lock)
            {
                foreach (Element element in _elements)
                {
                    if (_comparer.Equals(key, element.Key))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Get first matching element for the key.
        /// </summary>
        /// <returns><c>null</c> if no matching element found.
        /// </returns>
        [CanBeNull]
        public Element GetElement
            (
                [CanBeNull] TKey key
            )
        {
            lock (_lock)
            {
                foreach (Element element in _elements)
                {
                    if (_comparer.Equals(key, element.Key))
                    {
                        return element;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Get all matching elements for the key.
        /// </summary>
        [NotNull]
        public TValue[] GetValues
            (
                [CanBeNull] TKey key
            )
        {
            lock (_lock)
            {
                List<TValue> result = new List<TValue>();

                foreach (Element element in _elements)
                {
                    if (_comparer.Equals(key, element.Key))
                    {
                        result.Add(element.Value);
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Remove all matching elements with specified key.
        /// </summary>
        [NotNull]
        public KeyedCollection<TKey, TValue> Remove
            (
                [CanBeNull] TKey key
            )
        {
            lock (_lock)
            {
                List<Element> found = new List<Element>();
                foreach (Element element in _elements)
                {
                    if (_comparer.Equals(key, element.Key))
                    {
                        found.Add(element);
                    }
                }

                foreach (Element element in found)
                {
                    _elements.Remove(element);
                }

                return this;
            }
        }

        #endregion

        #region IEnumerable members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<Element> GetEnumerator()
        {
            // ReSharper disable once InconsistentlySynchronizedField
            return _elements.GetEnumerator();
        }

        #endregion
    }
}
