// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Set.cs -- generic set
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace AM.Collections
{
    /// <summary>
    /// Generic set.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Count={Count}")]
    public class Set<T>
        : ICollection<T>,
          ICollection,
          ICloneable
    {
        #region Properties

        /// <summary>
        /// Count.
        /// </summary>
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _data.Count;
            }
        }

        /// <summary>
        /// Is empty.
        /// </summary>
        public bool IsEmpty
        {
            [DebuggerStepThrough]
            get
            {
                return Count == 0;
            }
        }

        /// <summary>
        /// Get array of items.
        /// </summary>
        [NotNull]
        public T[] Items
        {
            [DebuggerStepThrough]
            get
            {
                return _data.Keys.ToArray();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Set()
        {
            _data = new Dictionary<T, object>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Set
            (
                int capacity
            )
        {
            Code.Positive(capacity, "capacity");

            _data = new Dictionary<T, object>(capacity);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Set
            (
                [NotNull] Set<T> original
            )
        {
            Code.NotNull(original, "original");

            _data = new Dictionary<T, object>(original._data);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Set
            (
                [NotNull] IEnumerable<T> original
            )
        {
            Code.NotNull(original, "original");

            _data = new Dictionary<T, object>();
            AddRange(original);
        }

        #endregion

        #region Private members

        private Dictionary<T, object> _data;

        #endregion

        #region Public methods

        /// <summary>
        /// Add an element.
        /// </summary>
        public void Add
            (
                T item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            _data[item] = null;
        }

        /// <summary>
        /// Add some elements.
        /// </summary>
        [NotNull]
        public Set<T> Add
            (
                params T[] many
            )
        {
            for (int i = 0; i < many.Length; i++)
            {
                _data[many[i]] = null;
            }

            return this;
        }

        /// <summary>
        /// Add some elements.
        /// </summary>
        [NotNull]
        public Set<T> AddRange
            (
                [NotNull] IEnumerable<T> range
            )
        {
            Code.NotNull(range, "range");

            foreach (T item in range)
            {
                Add(item);
            }

            return this;
        }

        /// <summary>
        /// Convert all.
        /// </summary>
        [NotNull]
        public Set<U> ConvertAll<U>
            (
                [NotNull] Converter<T, U> converter
            )
        {
            Code.NotNull(converter, "converter");

            Set<U> result = new Set<U>(Count);

            foreach (T element in this)
            {
                result.Add(converter(element));
            }

            return result;
        }

        /// <summary>
        /// True for all.
        /// </summary>
        public bool TrueForAll
            (
                [NotNull] Predicate<T> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            bool result = false;
            foreach (T element in this)
            {
                if (!predicate(element))
                {
                    return false;
                }
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Find all.
        /// </summary>
        [NotNull]
        public Set<T> FindAll
            (
                [NotNull] Predicate<T> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            Set<T> result = new Set<T>();
            foreach (T element in this)
            {
                if (predicate(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        /// <summary>
        /// For each.
        /// </summary>
        public void ForEach
            (
                [NotNull] Action<T> action
            )
        {
            Code.NotNull(action, "action");

            foreach (T element in this)
            {
                action(element);
            }
        }

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Contains.
        /// </summary>
        public bool Contains
            (
                T item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            return _data.ContainsKey(item);
        }

        /// <summary>
        /// Copy to.
        /// </summary>
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            _data.Keys.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove an element.
        /// </summary>
        public bool Remove
            (
                T item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            return _data.Remove(item);
        }

        /// <summary>
        /// Remove some elements.
        /// </summary>
        public void Remove
            (
                params T[] range
            )
        {
            for (int i = 0; i < range.Length; i++)
            {
                _data.Remove(range[i]);
            }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        /// <summary>
        /// Is read only.
        /// </summary>
        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Union operator.
        /// </summary>
        [NotNull]
        public static Set<T> operator |
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            Set<T> result = new Set<T>(left);
            result.AddRange(right);

            return result;
        }

        /// <summary>
        /// Union.
        /// </summary>
        [NotNull]
        public Set<T> Union
            (
                [NotNull] IEnumerable<T> set
            )
        {
            Code.NotNull(set, "set");

            return this | new Set<T>(set);
        }

        /// <summary>
        /// Intersection operator.
        /// </summary>
        [NotNull]
        public static Set<T> operator &
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            Set<T> result = new Set<T>();
            foreach (T element in left)
            {
                if (right.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        /// <summary>
        /// Intersection.
        /// </summary>
        [NotNull]
        public Set<T> Intersection
            (
                [NotNull] IEnumerable<T> items
            )
        {
            Code.NotNull(items, "items");

            return this & new Set<T>(items);
        }

        /// <summary>
        /// Difference operator.
        /// </summary>
        [NotNull]
        public static Set<T> operator -
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Set<T> result = new Set<T>();
            {
                foreach (T element in left)
                {
                    if (!right.Contains(element))
                    {
                        result.Add(element);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Difference.
        /// </summary>
        [NotNull]
        public Set<T> Difference
            (
                [NotNull] IEnumerable<T> setToCompare
            )
        {
            Code.NotNull(setToCompare, "setToCompare");

            return this - new Set<T>(setToCompare);
        }

        /// <summary>
        /// Symmetric difference.
        /// </summary>
        [NotNull]
        public static Set<T> operator ^
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            Set<T> result = new Set<T>();

            foreach (T element in left)
            {
                if (!right.Contains(element))
                {
                    result.Add(element);
                }
            }

            foreach (T element in right)
            {
                if (!left.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        /// <summary>
        /// Symmetric difference.
        /// </summary>
        [NotNull]
        public Set<T> SymmetricDifference
            (
                [NotNull] IEnumerable<T> setToCompare
            )
        {
            Code.NotNull(setToCompare, "setToCompare");

            return this ^ new Set<T>(setToCompare);
        }

        /// <summary>
        /// Empty.
        /// </summary>
        [NotNull]
        public static Set<T> Empty
        {
            get
            {
                return new Set<T>();
            }
        }

        /// <summary>
        /// Less or equal.
        /// </summary>
        public static bool operator <=
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            foreach (T element in left)
            {
                if (!right.Contains(element))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Less.
        /// </summary>
        public static bool operator <
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.Count < right.Count
                && left <= right;
        }

        /// <summary>
        /// Equal.
        /// </summary>
        public static bool operator ==
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.Count == right.Count
                && left <= right;
        }

        /// <summary>
        /// More.
        /// </summary>
        public static bool operator >
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return right < left;
        }

        /// <summary>
        /// More or equal.
        /// </summary>
        public static bool operator >=
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return right <= left;
        }

        /// <summary>
        /// Not equal.
        /// </summary>
        public static bool operator !=
            (
                [NotNull] Set<T> left,
                [NotNull] Set<T> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return !(left == right);
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            Set<T> a = this;
            Set<T> b = obj as Set<T>;

            return !ReferenceEquals(b, null) && a == b;
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            int hashcode = 0;

            foreach (T element in this)
            {
                unchecked
                {
                    hashcode = hashcode * 17 + element.GetHashCode();
                }
            }

            return hashcode;
        }

        /// <inheritdoc cref="ICollection.CopyTo" />
        void ICollection.CopyTo
            (
                Array array,
                int index
            )
        {
            ((ICollection)_data.Keys).CopyTo(array, index);
        }

        /// <inheritdoc cref="ICollection.SyncRoot" />
        [ExcludeFromCodeCoverage]
        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)_data.Keys).SyncRoot;
            }
        }

        /// <inheritdoc cref="ICollection.IsSynchronized" />
        [ExcludeFromCodeCoverage]
        bool ICollection.IsSynchronized
        {
            get
            {
                return ((ICollection)_data.Keys).IsSynchronized;
            }
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data.Keys).GetEnumerator();
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            return new Set<T>(this);
        }

        #endregion
    }
}

