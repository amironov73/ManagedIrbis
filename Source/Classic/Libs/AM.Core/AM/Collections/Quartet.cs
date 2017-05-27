// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Quartet.cs -- holds four objects of given types
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

using AM.Logging;

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
    [DebuggerDisplay("{First};{Second};{Third};{Fourth}")]
    //[TypeConverter(typeof(IndexableConverter))]
    public class Quartet<T1, T2, T3, T4>
        : IList,
          IIndexable<object>,
          IReadOnly<Quartet<T1, T2, T3, T4>>
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
        public Quartet
            (
                T1 first
            )
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
        public Quartet
            (
                T1 first,
                T2 second
            )
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
        public Quartet
            (
                T1 first,
                T2 second,
                T3 third
            )
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

        /// <inheritdoc cref="IList.Add" />
        int IList.Add
            (
                object value
            )
        {
            Log.Error
                (
                    "Quartet::Add: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.Contains" />
        bool IList.Contains
            (
                object value
            )
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

        /// <inheritdoc cref="IList.Clear" />
        void IList.Clear()
        {
            Log.Error
                (
                    "Quartet::Clear: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.IndexOf" />
        int IList.IndexOf
            (
                object value
            )
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

        /// <inheritdoc cref="IList.Insert" />
        void IList.Insert
            (
                int index,
                object value
            )
        {
            Log.Error
                (
                    "Quartet::Insert: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.Remove" />
        void IList.Remove
            (
                object value
            )
        {
            Log.Error
                (
                    "Quartet::Insert: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.RemoveAt" />
        void IList.RemoveAt
            (
                int index
            )
        {
            Log.Error
                (
                    "Quartet::Add: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.this" />
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
                        Log.Error
                            (
                                "Quartet::Indexer: "
                                + "index="
                                + index
                            );

                        throw new ArgumentOutOfRangeException("index");
                }
            }
            set
            {
                if (ReadOnly)
                {
                    Log.Error
                        (
                            "Quartet::Indexer: "
                            + "read-only"
                        );

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
                        Log.Error
                            (
                                "Quartet::Indexer: "
                                + "index="
                                + index
                            );

                        throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        private bool _isReadOnly;

        /// <inheritdoc cref="IList.IsReadOnly" />
        bool IList.IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        /// <inheritdoc cref="IList.IsFixedSize" />
        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc cref="ICollection.CopyTo" />
        void ICollection.CopyTo
            (
                Array array,
                int index
            )
        {
            array.SetValue(First, index);
            array.SetValue(Second, index + 1);
            array.SetValue(Third, index + 2);
            array.SetValue(Fourth, index + 3);
        }

        /// <inheritdoc cref="ICollection.Count" />
        public int Count
        {
            get
            {
                return 4;
            }
        }

        /// <inheritdoc cref="ICollection.SyncRoot" />
        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc cref="ICollection.IsSynchronized" />
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
            yield return Fourth;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            return new Quartet<T1, T2, T3, T4>
                (
                    First,
                    Second,
                    Third,
                    Fourth,
                    ReadOnly
                );
        }

        #endregion

        #region IReadOnly<T> members

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
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

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _isReadOnly = true;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "Quartet::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
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


        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Quartet<T1, T2, T3, T4>)obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            unchecked
            {
                var hashCode = EqualityComparer<T1>.Default.GetHashCode(_first);
                hashCode = (hashCode * 397)
                    ^ EqualityComparer<T2>.Default.GetHashCode(_second);
                hashCode = (hashCode * 397)
                    ^ EqualityComparer<T3>.Default.GetHashCode(_third);
                hashCode = (hashCode * 397)
                    ^ EqualityComparer<T4>.Default.GetHashCode(_fourth);
                return hashCode;
            }
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0};{1};{2};{3}",
                    First.ToVisibleString(),
                    Second.ToVisibleString(),
                    Third.ToVisibleString(),
                    Fourth.ToVisibleString()
                );
        }

        #endregion
    }
}