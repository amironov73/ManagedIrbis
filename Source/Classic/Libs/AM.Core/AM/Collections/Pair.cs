/* Pair.cs -- holds pair of objects of given types
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Simple container that holds pair of objects of given types.
    /// </summary>
    /// <typeparam name="T1">Type of first object.</typeparam>
    /// <typeparam name="T2">Type of second object.</typeparam>
    /// <seealso cref="Triplet{T1,T2,T3}"/>
    /// <seealso cref="Quartet{T1,T2,T3,T4}"/>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{First};{Second}")]
#endif
    //[TypeConverter(typeof(IndexableConverter))]
    public class Pair<T1, T2>
        : IList,
          IIndexable<object>,
          IReadOnly<Pair<T1, T2>>
    {
        #region Properties

        private T1 _first;

        /// <summary>
        /// First element of the pair.
        /// </summary>
        /// <value>Value of first element.</value>
        [CanBeNull]
        [XmlElement("first")]
        [JsonProperty("first")]
        public T1 First
        {
            [DebuggerStepThrough]
            get
            {
                return _first;
            }
            [DebuggerStepThrough]
            set
            {
                ThrowIfReadOnly();
                _first = value;
            }
        }

        private T2 _second;

        /// <summary>
        /// Second element of the pair.
        /// </summary>
        /// <value>Value of second element.</value>
        [CanBeNull]
        [XmlElement("second")]
        [JsonProperty("second")]
        public T2 Second
        {
            [DebuggerStepThrough]
            get
            {
                return _second;
            }
            [DebuggerStepThrough]
            set
            {
                ThrowIfReadOnly();
                _second = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Pair{T1, T2}"/> class.
        /// Both <see cref="First"/> and <see cref="Second"/>
        /// members of the pair remains unassigned.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Pair{T1, T2}"/> class.
        /// </summary>
        /// <param name="pair">The pair.</param>
        public Pair
            (
                [NotNull] Pair<T1, T2> pair
            )
        {
            Code.NotNull(pair, "pair");

            First = pair.First;
            Second = pair.Second;
            _isReadOnly = pair._isReadOnly;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Pair{T1, T2}"/> class
        /// and assigns <see cref="First"/> member of the pair.
        /// <see cref="Second"/> member remains unassigned.
        /// </summary>
        /// <param name="first">Initial value for <see cref="First"/>
        /// member of the pair.</param>
        public Pair
            (
                [CanBeNull] T1 first
            )
        {
            First = first;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Pair{T1, T2}"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        public Pair
            (
                [CanBeNull] T1 first,
                [CanBeNull] T2 second
            )
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Pair{T1, T2}"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="readOnly">Specifies whether the pair
        /// should be read-only.</param>
        public Pair
            (
                [CanBeNull] T1 first,
                [CanBeNull] T2 second,
                bool readOnly
            )
        {
            First = first;
            Second = second;
            _isReadOnly = readOnly;
        }

        #endregion

        #region Public methods

        #endregion

        #region IList members

        ///<inheritdoc/>
        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        bool IList.Contains(object value)
        {
            foreach (object o in this)
            {
                if (!ReferenceEquals(o, null)
                    && o.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        ///<inheritdoc/>
        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        int IList.IndexOf
            (
                object value
            )
        {
            int index = 0;
            foreach (object o in this)
            {
                if (!ReferenceEquals(o, null))
                {
                    if (o.Equals(value))
                    {
                        return index;
                    }
                }
                index++;
            }

            return -1;
        }

        ///<inheritdoc/>
        void IList.Insert
            (
                int index,
                object value
            )
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        void IList.Remove
            (
                object value
            )
        {
            throw new NotSupportedException();
        }

        ///<inheritdoc/>
        void IList.RemoveAt
            (
                int index
            )
        {
            throw new NotSupportedException();
        }

        ///<summary>
        /// Gets or sets the element at the specified index.
        ///</summary>
        ///<returns>
        /// The element at the specified index.
        ///</returns>
        ///<param name="index">The zero-based index of 
        /// the element to get or set. </param>
        ///<exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the 
        /// <see cref="T:System.Collections.IList"/>.</exception>
        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return First;

                    case 1:
                        return Second;

                    default:
                        throw new ArgumentOutOfRangeException("index");
                }
            }
            set
            {
                if (_isReadOnly)
                {
                    throw new NotSupportedException();
                }
                switch (index)
                {
                    case 0:
                        First = (T1)value;
                        break;

                    case 1:
                        Second = (T2)value;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        private bool _isReadOnly;

        ///<inheritdoc/>
        bool IList.IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        ///<summary>
        /// Gets a value indicating whether the 
        /// <see cref="T:System.Collections.IList" /> has a fixed size.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the <see cref="T:System.Collections.IList" />
        /// has a fixed size; otherwise, <c>false</c>.
        ///</returns>
        ///<remarks><see cref="Pair{T1,T2}"/>
        /// yields <c>true</c>.
        /// </remarks>
        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        ///<inheritdoc/>
        void ICollection.CopyTo
            (
                Array array,
                int index
            )
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// Gets the number of elements contained in the 
        /// <see cref="T:System.Collections.ICollection"/>.
        ///</summary>
        ///<returns>
        /// The number of elements contained in the 
        /// <see cref="T:System.Collections.ICollection" />.
        ///</returns>
        ///<remarks><see cref="Pair{T1,T2}"/>
        /// yields <c>2</c>.
        /// </remarks>
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return 2;
            }
        }

        ///<summary>
        /// Gets an object that can be used to synchronize 
        /// access to the <see cref="T:System.Collections.ICollection"/>.
        ///</summary>
        ///<returns>
        /// An object that can be used to synchronize access to the 
        /// <see cref="T:System.Collections.ICollection"/>.
        ///</returns>
        ///<remarks><see cref="Pair{T1,T2}"/>
        /// yields <c>this</c>.
        /// </remarks>
        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        ///<summary>
        /// Gets a value indicating whether access to the 
        /// <see cref="T:System.Collections.ICollection"/>
        /// is synchronized (thread safe).
        ///</summary>
        ///<returns>
        /// <c>true</c> if access to the 
        /// <see cref="T:System.Collections.ICollection"/>
        /// is synchronized (thread safe); otherwise, <c>false</c>.
        ///</returns>
        /// <remarks><see cref="Pair{T1,T2}"/>
        /// yields <c>false</c>.
        /// </remarks>
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        ///<summary>
        /// Returns an enumerator that iterates through a collection.
        ///</summary>
        ///<returns>
        /// An <see cref="T:System.Collections.IEnumerator"/>
        /// object that can be used to iterate through the collection.
        ///</returns>
        ///<remarks><see cref="Pair{T1,T2}"/>
        /// yields sequentially: <see cref="First"/>,
        /// <see cref="Second"/>.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return First;
            yield return Second;
        }

        #endregion

        #region ICloneable members

        ///<summary>
        /// Creates a new object that is a copy of the current instance.
        ///</summary>
        ///<returns>
        /// A new object that is a copy of this instance.
        ///</returns>
        public object Clone()
        {
            return new Pair<T1, T2>(First, Second, _isReadOnly);
        }

        #endregion

        #region IReadOnly<T> members

        /// <summary>
        /// Gets a value indicating whether object is read only.
        /// </summary>
        public bool ReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Returns read-only copy of the pair.
        /// </summary>
        /// <returns>Read-only copy of the pair.</returns>
        public Pair<T1, T2> AsReadOnly()
        {
            return new Pair<T1, T2>(First, Second, true);
        }

        /// <summary>
        /// Sets the read only.
        /// </summary>
        public void SetReadOnly()
        {
            _isReadOnly = true;
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }
        }

        #endregion

        #region Object members

        ///<inheritdoc/>
        protected bool Equals
            (
                Pair<T1, T2> other
            )
        {
            return EqualityComparer<T1>.Default.Equals(_first, other._first)
                && EqualityComparer<T2>.Default.Equals(_second, other._second);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pair<T1, T2>)obj);
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            unchecked
            {
                return (EqualityComparer<T1>.Default.GetHashCode(_first) * 397)
                    ^ EqualityComparer<T2>.Default.GetHashCode(_second);
            }
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "{0};{1}",
                    First,
                    Second
                );
        }

        #endregion
    }
}
