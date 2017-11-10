// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubArray.cs -- portion (segment) of an array
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Segment of an array.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct SubArray<T>
        : IList<T>,
        IReadOnlyList<T>,
        IEquatable<SubArray<T>>
    {
        #region Properties

        /// <summary>
        /// Array.
        /// </summary>
        [NotNull]
        public T[] Array { get; private set; }

        /// <summary>
        /// Offset.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Indexer.
        /// </summary>
        public T this[int index]
        {
            get { return Array[Offset + index]; }
            set { Array[Offset + index] = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubArray
            (
                [NotNull] T[] array,
                int offset,
                int length
            )
            : this()
        {
            Code.NotNull(array, "array");
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(length, "length");

            if (offset > array.Length)
            {
                Log.Error
                    (
                        "SubArray::Constructor: "
                        + "offset="
                        + offset
                    );

                throw new ArgumentOutOfRangeException("offset");
            }

            Array = array;
            Offset = offset;
            Length = Math.Min(length, array.Length - offset);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public SubArray
            (
                [NotNull] T[] array,
                int offset
            )
            : this()
        {
            Code.NotNull(array, "array");
            Code.Nonnegative(offset, "offset");

            if (offset > array.Length)
            {
                Log.Error
                    (
                        "SubArray::Constructor: "
                        + "offset="
                        + offset
                    );

                throw new ArgumentOutOfRangeException("offset");
            }

            Array = array;
            Offset = offset;
            Length = array.Length - offset;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Creates an array from this instance.
        /// </summary>
        [NotNull]
        public T[] ToArray()
        {
            T[] result = new T[Length];
            System.Array.Copy(Array, Offset, result, 0, Length);

            return result;
        }

        #endregion

        #region IList<T> members

        [ExcludeFromCodeCoverage]
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        [ExcludeFromCodeCoverage]
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains(T item)
        {
            int index = System.Array.IndexOf(Array, item, Offset, Length);

            return index >= 0;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo(T[] array, int arrayIndex)
        {
            System.Array.Copy(Array, Offset, array, arrayIndex, Length);
        }

        [ExcludeFromCodeCoverage]
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        [ExcludeFromCodeCoverage]
        int ICollection<T>.Count { get { return Length; } }

        [ExcludeFromCodeCoverage]
        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf(T item)
        {
            int result = System.Array.IndexOf(Array, item, Offset, Length);

            return result >= 0
                ? result - Offset
                : -1;
        }

        [ExcludeFromCodeCoverage]
        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        [ExcludeFromCodeCoverage]
        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> members

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        #endregion

        #region IEquatable<SubArray<T>> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                SubArray<T> other
            )
        {
            return ReferenceEquals(Array, other.Array)
                   && Offset == other.Offset
                   && Length == other.Length;
        }

        #endregion

        #region IReadOnlyList<T> members

        [ExcludeFromCodeCoverage]
        int IReadOnlyCollection<T>.Count
        {
            get { return Length; }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (obj is SubArray<T>)
            {
                return Equals((SubArray<T>)obj);
            }

            return false;
        }

        /// <inheritdoc cref="ValueType.GetHashCode" />
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            unchecked
            {
                return (Array.GetHashCode() * 397 + Offset) * 397 + Length;
            }
        }

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            using (IEnumerator<T> enumerator = GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    T o = enumerator.Current;
                    result.Append(o.ToVisibleString());

                    while (enumerator.MoveNext())
                    {
                        o = enumerator.Current;
                        result.Append(", ");
                        result.Append(o.ToVisibleString());
                    }
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
