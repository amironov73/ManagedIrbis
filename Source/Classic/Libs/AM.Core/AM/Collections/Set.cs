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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Generic set.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Count={Count}")]
#endif
    public class Set<T>
        : ICollection<T>,
          ICollection
#if !NETCORE && !SILVERLIGHT && !UAP && !WIN81 && !PORTABLE
        , ICloneable
#endif
    {
        #region Properties

        /// <summary>
        /// Count.
        /// </summary>
        /// <value></value>
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
        /// <value></value>
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
        public T[] Items
        {
            [DebuggerStepThrough]
            get
            {
                return new List<T>(_data.Keys).ToArray();
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
                Set<T> original
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
            _data[item] = null;
        }

        /// <summary>
        /// Add some elements.
        /// </summary>
        public void Add
            (
                params T[] many
            )
        {
            for (int i = 0; i < many.Length; i++)
            {
                _data[many[i]] = null;
            }
        }

        /// <summary>
        /// Add some elements.
        /// </summary>
        public void AddRange
            (
                IEnumerable<T> range
            )
        {
            foreach (T item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Convert all.
        /// </summary>
        public Set<U> ConvertAll<U>
            (
                Converter<T, U> converter
            )
        {
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
                Predicate<T> predicate
            )
        {
            foreach (T element in this)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Find all.
        /// </summary>
        public Set<T> FindAll
            (
                Predicate<T> predicate
            )
        {
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
                Action<T> action
            )
        {
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
        public bool Contains(T item)
        {
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

        /// <summary>
        /// Get enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        /// <summary>
        /// Is read only.
        /// </summary>
        /// <value></value>
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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Set<T> operator |
            (
                Set<T> left,
                Set<T> right
            )
        {
            Set<T> result = new Set<T>(left);
            result.AddRange(right);

            return result;
        }

        /// <summary>
        /// Union.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Set<T> Union(IEnumerable<T> set)
        {
            return this | new Set<T>(set);
        }

        /// <summary>
        /// Intersection operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Set<T> operator &(Set<T> left, Set<T> right)
        {
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
        public Set<T> Intersection
            (
                IEnumerable<T> set
            )
        {
            return this & new Set<T>(set);
        }

        /// <summary>
        /// Difference operator.
        /// </summary>
        public static Set<T> operator -
            (
                Set<T> left,
                Set<T> right
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
        public Set<T> Difference
            (
                IEnumerable<T> setToCompare
            )
        {
            return this - new Set<T>(setToCompare);
        }

        /// <summary>
        /// Symmetric difference.
        /// </summary>
        public static Set<T> operator ^
            (
                Set<T> left,
                Set<T> right
            )
        {
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
        public Set<T> SymmetricDifference
            (
            IEnumerable<T> setToCompare
            )
        {
            return this ^ new Set<T>(setToCompare);
        }

        /// <summary>
        /// Empty.
        /// </summary>
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
            // Fix PVS-Studio issue
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (ReferenceEquals(left, null)
                || ReferenceEquals(right, null))
            {
                return false;
            }
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

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
            return right <= left;
        }

        /// <summary>
        /// Not equal.
        /// </summary>
        public static bool operator !=
            (
                Set<T> left,
                Set<T> right
            )
        {
            return !(left == right);
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
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
                hashcode ^= element.GetHashCode();
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
        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)_data.Keys).SyncRoot;
            }
        }

        /// <inheritdoc cref="ICollection.IsSynchronized" />
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

