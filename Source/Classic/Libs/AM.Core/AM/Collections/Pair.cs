// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM.Logging;

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
        /// Constructor.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
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
        /// Constructor.
        /// </summary>
        public Pair
            (
                [CanBeNull] T1 first
            )
        {
            First = first;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
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
        /// Constructor.
        /// </summary>
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

        ///<inheritdoc cref="IList.Add" />
        int IList.Add
            (
                object value
            )
        {
            Log.Error
                (
                    "Pair::Add: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        ///<inheritdoc cref="IList.Contains" />
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

        ///<inheritdoc cref="IList.Clear" />
        void IList.Clear()
        {
            Log.Error
            (
                "Pair::Clear: "
                + "not supported"
            );

            throw new NotSupportedException();
        }

        ///<inheritdoc cref="IList.IndexOf" />
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

        ///<inheritdoc cref="IList.Insert" />
        void IList.Insert
            (
                int index,
                object value
            )
        {
            Log.Error
                (
                    "Pair::Insert: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        ///<inheritdoc cref="IList.Remove" />
        void IList.Remove
            (
                object value
            )
        {
            Log.Error
                (
                    "Pair::Remove: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        ///<inheritdoc cref="IList.RemoveAt" />
        void IList.RemoveAt
            (
                int index
            )
        {
            Log.Error
                (
                    "Pair::RemoveAt: "
                    + "not supported"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList.this"/>
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
                        Log.Error
                            (
                                "Pair::Indexer: "
                                + "index="
                                + index
                                + " is out of range"
                            );

                        throw new ArgumentOutOfRangeException("index");
                }
            }
            set
            {
                if (_isReadOnly)
                {
                    Log.Error
                        (
                            "Pair::Indexer: "
                            + "is read-only"
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

                    default:
                        Log.Error
                            (
                                "Pair::Indexer: "
                                + "index="
                                + index
                                + " is out of range"
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
        }

        /// <inheritdoc cref="ICollection.Count" />
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return 2;
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
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            return new Pair<T1, T2>(First, Second, _isReadOnly);
        }

        #endregion

        #region IReadOnly<T> members

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public Pair<T1, T2> AsReadOnly()
        {
            return new Pair<T1, T2>(First, Second, true);
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
                        "Pair::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region IEquatable<T> members

        ///<inheritdoc cref="IEquatable{T}.Equals(T)" />
        protected bool Equals
        (
            Pair<T1, T2> other
        )
        {
            return EqualityComparer<T1>.Default.Equals(_first, other._first)
                   && EqualityComparer<T2>.Default.Equals(_second, other._second);
        }

        #endregion

        #region Object members

        ///<inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
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

            return Equals((Pair<T1, T2>)obj);
        }

        ///<inheritdoc cref="object.GetHashCode" />
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

        ///<inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0};{1}",
                    First.ToVisibleString(),
                    Second.ToVisibleString()
                );
        }

        #endregion
    }
}
