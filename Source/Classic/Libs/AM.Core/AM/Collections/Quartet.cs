/* Quartet.cs -- holds four objects of given types
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Simple container that holds four objects of given types.
    /// </summary>
    /// <typeparam name="T1">Type of first object.</typeparam>
    /// <typeparam name="T2">Type of second object.</typeparam>
    /// <typeparam name="T3">Type of third object.</typeparam>
    /// <typeparam name="T4">Type of fourth object.</typeparam>
    /// <seealso cref="Pair{T1,T2}"/>
    /// <seealso cref="Triplet{T1,T2,T3}"/>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{First};{Second};{Third};{Fourth}")]
#endif
    //[TypeConverter(typeof(IndexableConverter))]
    public class Quartet<T1, T2, T3, T4>
        : IList,
          IIndexable<object>,
          //ICloneable,
          IReadOnly<Quartet<T1,T2,T3,T4>>
    {
        #region Properties

        private T1 _first;

        /// <summary>
        /// First element of the quartet.
        /// </summary>
        /// <value>First element.</value>
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
        /// Second element of the quartet.
        /// </summary>
        /// <value>Second element.</value>
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

        private T3 _third;

        /// <summary>
        /// Third element of the quartet.
        /// </summary>
        /// <value>Third element.</value>
        [CanBeNull]
        [XmlElement("third")]
        [JsonProperty("third")]
        public T3 Third
        {
            [DebuggerStepThrough]
            get
            {
                return _third;
            }
            [DebuggerStepThrough]
            set
            {
                ThrowIfReadOnly();
                _third = value;
            }
        }

        private T4 _fourth;

        /// <summary>
        /// Fourth element of the quartet.
        /// </summary>
        /// <value>Fourth element.</value>
        [CanBeNull]
        [XmlElement("fourth")]
        [JsonProperty("fourth")]
        public T4 Fourth
        {
            [DebuggerStepThrough]
            get
            {
                return _fourth;
            }
            [DebuggerStepThrough]
            set
            {
                ThrowIfReadOnly();
                _fourth = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Quartet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="quartet">The quartet.</param>
        public Quartet
            (
                [NotNull] Quartet<T1, T2, T3, T4> quartet
            )
        {
            Code.NotNull(quartet, "quartet");

            First = quartet.First;
            Second = quartet.Second;
            Third = quartet.Third;
            Fourth = quartet.Fourth;
            _isReadOnly = quartet._isReadOnly;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// Constructs quartet without 3 last elements.
        /// </summary>
        /// <param name="first">First element.</param>
        public Quartet(T1 first)
        {
            First = first;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// Constructs quartet without 2 last elements.
        /// </summary>
        /// <param name="first">First element.</param>
        /// <param name="second">Second element.</param>
        public Quartet(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// Constructs quartet without last element.
        /// </summary>
        /// <param name="first">First element.</param>
        /// <param name="second">Second element.</param>
        /// <param name="third">Third element.</param>
        public Quartet(T1 first, T2 second, T3 third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="first">Initial value for first element.
        /// </param>
        /// <param name="second">Initial value for second element.
        /// </param>
        /// <param name="third">Initial value for third element.
        /// </param>
        /// <param name="fourth">Initial value for fourth element.
        /// </param>
        public Quartet
            (
            T1 first,
            T2 second,
            T3 third,
            T4 fourth
            )
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Quartet{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="first">Initial value for first element.
        /// </param>
        /// <param name="second">Initial value for second element.
        /// </param>
        /// <param name="third">Initial value for third element.
        /// </param>
        /// <param name="fourth">Initial value for fourth element.
        /// </param>
        /// <param name="readOnly">Specifies whether the quartet
        /// should be read-only or not.</param>
        public Quartet
            (
                T1 first,
                T2 second,
                T3 third,
                T4 fourth,
                bool readOnly
            )
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            _isReadOnly = readOnly;
        }

        #endregion

        #region Public methods

        #endregion

        #region IList members

        ///<summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        ///</summary>
        ///<returns>
        /// The position into which the new element was inserted.
        ///</returns>
        ///<param name="value">The <see cref="T:System.Object"/>
        /// to add to the <see cref="T:System.Collections.IList"/>.
        /// </param>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception><filterpriority>2</filterpriority>
        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        ///<summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/>
        /// contains a specific value.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the <see cref="T:System.Object"/> 
        /// is found in the <see cref="T:System.Collections.IList"/>;
        /// otherwise, <c>false</c>.
        ///</returns>
        ///<param name="value">The <see cref="T:System.Object"/>
        /// to locate in the <see cref="T:System.Collections.IList"/>.
        /// </param>
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

        ///<summary>
        /// Removes all items from the 
        /// <see cref="T:System.Collections.IList"/>.
        ///</summary>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception>
        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        ///<summary>
        /// Determines the index of a specific item in the 
        /// <see cref="T:System.Collections.IList"/>.
        ///</summary>
        ///
        ///<returns>
        /// The index of value if found in the list; otherwise, -1.
        ///</returns>
        ///<param name="value">The <see cref="T:System.Object"/>
        /// to locate in the <see cref="T:System.Collections.IList" />.
        /// </param>
        int IList.IndexOf(object value)
        {
            int index = 0;
            foreach (object o in this)
            {
                if (!ReferenceEquals(o, null)
                    && o.Equals(value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        ///<summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/>
        /// at the specified index.
        ///</summary>
        ///<param name="value">The <see cref="T:System.Object"/>
        /// to insert into the <see cref="T:System.Collections.IList"/>.
        /// </param>
        ///<param name="index">The zero-based index at which value 
        /// should be inserted.</param>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception>
        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        ///<summary>
        /// Removes the first occurrence of a specific object from the 
        /// <see cref="T:System.Collections.IList"/>.
        ///</summary>
        ///<param name="value">The <see cref="T:System.Object"/>
        /// to remove from the <see cref="T:System.Collections.IList"/>.
        /// </param>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception>
        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        ///<summary>
        ///Removes the <see cref="T:System.Collections.IList"/>
        /// item at the specified index.
        ///</summary>
        ///<param name="index">The zero-based index of the 
        /// item to remove. </param>
        ///<exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </exception><filterpriority>2</filterpriority>
        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        ///<summary>
        /// Gets or sets the element at the specified index.
        ///</summary>
        ///<returns>
        /// The element at the specified index.
        ///</returns>
        ///<param name="index">The zero-based index of the 
        /// element to get or set. </param>
        ///<exception cref="T:System.ArgumentOutOfRangeException">
        /// index is not a valid index in the 
        /// <see cref="T:System.Collections.IList"/>.
        /// </exception>
        /// <exception cref="T:System.InvalidCastException">
        /// Can't cast <c>value</c> to element type.
        /// </exception>
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

                    case 2:
                        return Third;

                    case 3:
                        return Fourth;

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

                    case 2:
                        Third = (T3)value;
                        break;

                    case 3:
                        Fourth = (T4)value;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        private bool _isReadOnly;

        ///<summary>
        /// Gets a value indicating whether the 
        /// <see cref="T:System.Collections.IList"/>
        /// is read-only.
        ///</summary>
        ///<returns>
        /// <c>true</c> if the <see cref="T:System.Collections.IList"/>
        /// is read-only; otherwise, <c>false</c>.
        ///</returns>
        bool IList.IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        ///<summary>
        /// Gets a value indicating whether the 
        /// <see cref="T:System.Collections.IList"/>
        /// has a fixed size.
        ///</summary>
        ///<returns>
        /// <c>true</c>if the <see cref="T:System.Collections.IList"/>
        /// has a fixed size; otherwise, <c>false</c>.
        ///</returns>
        /// <remarks><see cref="Quartet{T1,T2,T3,T4}"/>
        /// returns <c>true</c>.
        /// </remarks>
        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        ///<summary>
        ///Copies the elements of the 
        /// <see cref="T:System.Collections.ICollection" />
        /// to an <see cref="T:System.Array"/>, starting at a particular 
        /// <see cref="T:System.Array"/> index.
        ///</summary>
        ///<param name="array">The one-dimensional 
        /// <see cref="T:System.Array" /> that is the destination 
        /// of the elements copied from
        ///  <see cref="T:System.Collections.ICollection"/>.
        /// The <see cref="T:System.Array"/>
        /// must have zero-based indexing. </param>
        ///<param name="index">The zero-based index in array 
        /// at which copying begins.</param>
        /// <exception cref="T:System.NotImplementedException">
        /// Method not implemented.
        /// </exception>
        /// <remarks>Method not implemented.</remarks>
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Gets the number of elements contained in the 
        /// <see cref="T:System.Collections.ICollection"/>.
        ///</summary>
        ///<returns>
        ///The number of elements contained in the 
        /// <see cref="T:System.Collections.ICollection"/>.
        ///</returns>
        /// <remarks><see cref="Quartet{T1,T2,T3,T4}"/>
        /// returns <c>4</c>.
        /// </remarks>
        public int Count
        {
            get
            {
                return 4;
            }
        }

        ///<summary>
        ///Gets an object that can be used to synchronize 
        /// access to the <see cref="T:System.Collections.ICollection"/>.
        ///</summary>
        ///<returns>
        ///An object that can be used to synchronize access to the 
        /// <see cref="T:System.Collections.ICollection"/>.
        ///</returns>
        /// <remarks><see cref="Quartet{T1,T2,T3,T4}"/>
        /// returns <c>this</c>.
        /// </remarks>
        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        ///<summary>
        ///Gets a value indicating whether access to the 
        /// <see cref="T:System.Collections.ICollection"/>
        /// is synchronized (thread safe).
        ///</summary>
        ///<returns>
        ///<c>true</c> if access to the 
        /// <see cref="T:System.Collections.ICollection"/>
        /// is synchronized (thread safe); otherwise, <c>false</c>.
        ///</returns>
        /// <remarks><c>false</c> for 
        /// <see cref="Quartet{T1,T2,T3,T4}"/>.
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
        /// <remarks><see cref="Quartet{T1,T2,T3,T4}"/>
        /// yields sequentially: <see cref="First"/>, 
        /// <see cref="Second"/>, <see cref="Third"/>,
        /// <see cref="Fourth"/>.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
            yield return Fourth;
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
            return new Quartet<T1, T2, T3, T4>(First, Second, Third, Fourth);
        }

        #endregion

        #region IReadOnly<T> members

        /// <summary>
        /// Gets a value indicating whether [read only].
        /// </summary>
        public bool ReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Returns read-only copy of the quartet.
        /// </summary>
        /// <returns>Read-only copy of the quartet.</returns>
        public Quartet<T1, T2, T3, T4> AsReadOnly()
        {
            return new Quartet<T1, T2, T3, T4>
                (
                    First,
                    Second,
                    Third,
                    Fourth,
                    true
                );
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

        /// <summary>
        /// 
        /// </summary>
        protected bool Equals
            (
                Quartet<T1, T2, T3, T4> other
            )
        {
            return EqualityComparer<T1>.Default.Equals(_first, other._first)
                && EqualityComparer<T2>.Default.Equals(_second, other._second)
                && EqualityComparer<T3>.Default.Equals(_third, other._third)
                && EqualityComparer<T4>.Default.Equals(_fourth, other._fourth);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Quartet<T1, T2, T3, T4>) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use
        /// in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(_first);
                hashCode = (hashCode*397) ^ EqualityComparer<T2>.Default.GetHashCode(_second);
                hashCode = (hashCode*397) ^ EqualityComparer<T3>.Default.GetHashCode(_third);
                hashCode = (hashCode*397) ^ EqualityComparer<T4>.Default.GetHashCode(_fourth);
                return hashCode;
            }
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents 
        /// the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents 
        /// the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0};{1};{2};{3}",
                    First,
                    Second,
                    Third,
                    Fourth
                );
        }

        #endregion
    }
}