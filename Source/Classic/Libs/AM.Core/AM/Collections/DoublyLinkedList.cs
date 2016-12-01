// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DoublyLinkedList.cs -- 
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

using Newtonsoft.Json;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Дважды-связанный список
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DoublyLinkedList<T>
        : IList<T>
    {
        #region Nested classes

        /// <summary>
        /// Элемент дважды-связанного списка.
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        public class Node<T2>
        {
            #region Public properties

            /// <summary>
            /// Список-владелец.
            /// </summary>
            /// <value></value>
            [NotNull]
            [JsonIgnore]
            public DoublyLinkedList<T2> List { get; internal set; }

            /// <summary>
            /// Предыдущий элемент.
            /// </summary>
            /// <value></value>
            [CanBeNull]
            [JsonIgnore]
            public Node<T2> Previous { get; set; }

            /// <summary>
            /// Последующий элемент.
            /// </summary>
            /// <value></value>
            [CanBeNull]
            [JsonIgnore]
            public Node<T2> Next { get; set; }

            /// <summary>
            /// Хранимое значение.
            /// </summary>
            /// <value></value>
            [CanBeNull]
            [JsonProperty("value")]
            public T2 Value { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Node(T2 value)
            {
                Value = value;
            }

            #endregion

            #region Object members

            /// <inheritdoc/>
            public override string ToString()
            {
                return Value.ToVisibleString();
            }

            #endregion
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Первый элемент.
        /// </summary>
        /// <value></value>
        public Node<T> FirstNode { get; protected set; }

        /// <summary>
        /// Последний элемент.
        /// </summary>
        /// <value></value>
        public Node<T> LastNode { get; protected set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Get node at specified index.
        /// </summary>
        [CanBeNull]
        [CLSCompliant(false)]
        protected Node<T> _NodeAt
            (
                int index
            )
        {
            Node<T> result = FirstNode;

            for (int i = 0; i < index; i++)
            {
                if (result == null)
                {
                    break;
                }
                result = result.Next;
            }

            return result;
        }

        /// <summary>
        /// _s the create node.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [NotNull]
        [CLSCompliant(false)]
        protected Node<T> _CreateNode(T value)
        {
            Node<T> result
                = new Node<T>(value)
                {
                    List = this
                };

            return result;
        }

        #endregion

        #region IList<T> members

        /// <inheritdoc/>
        public int IndexOf
            (
                T item
            )
        {
            IEnumerator<T> enumerator = GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                T current = enumerator.Current;
                if (!ReferenceEquals(current, null)
                    && current.Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Insert
            (
                int index,
                T item
            )
        {
            Code.Nonnegative(index, "index");

            Node<T> existingNode = _NodeAt(index);
            if (!ReferenceEquals(existingNode, null))
            {
                Node<T> newNode = _CreateNode(item);
                Node<T> nextNode = existingNode.Next;
                Node<T> previousNode = existingNode.Previous;
                newNode.Next = existingNode;
                newNode.Previous = previousNode;
                existingNode.Previous = newNode;
                if (!ReferenceEquals(nextNode, null))
                {
                    nextNode.Previous = newNode;
                }
                if (!ReferenceEquals(previousNode, null))
                {
                    previousNode.Next = newNode;
                }
                if (ReferenceEquals(existingNode, FirstNode))
                {
                    FirstNode = newNode;
                }
            }
        }

        /// <inheritdoc/>
        public void RemoveAt
            (
                int index
            )
        {
            Code.Nonnegative(index, "index");

            Node<T> nodeToRemove = _NodeAt(index);
            if (!ReferenceEquals(nodeToRemove, null))
            {
                Node<T> nextNode = nodeToRemove.Next;
                Node<T> previousNode = nodeToRemove.Previous;
                if (!ReferenceEquals(previousNode, null))
                {
                    previousNode.Next = nextNode;
                }
                if (!ReferenceEquals(nextNode, null))
                {
                    nextNode.Previous = previousNode;
                }
                if (ReferenceEquals(nodeToRemove, FirstNode))
                {
                    FirstNode = nextNode;
                }
                if (ReferenceEquals(nodeToRemove, LastNode))
                {
                    LastNode = previousNode;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:T"/> at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get
            {
                Node<T> node = _NodeAt(index);

                if (ReferenceEquals(node, null))
                {
                    throw new IndexOutOfRangeException();
                }

                return node.Value;
            }
            set
            {
                Node<T> node = _NodeAt(index);

                if (ReferenceEquals(node, null))
                {
                    throw new IndexOutOfRangeException();
                }

                node.Value = value;
            }
        }

        #endregion

        #region ICollection<T> members

        /// <inheritdoc/>
        public void Add
            (
                T item
            )
        {
            Node<T> node = _CreateNode(item);
            if (!ReferenceEquals(LastNode, null))
            {
                LastNode.Next = node;
            }
            node.Previous = LastNode;
            LastNode = node;
            if (ReferenceEquals(FirstNode, null))
            {
                FirstNode = node;
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            FirstNode = null;
            LastNode = null;
        }

        /// <inheritdoc/>
        public bool Contains
            (
                T item
            )
        {
            IEnumerator<T> enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void CopyTo
            (
                T[] array, 
                int arrayIndex
            )
        {
            IEnumerator<T> enumerator = GetEnumerator();
            while (enumerator.MoveNext()
                && arrayIndex < array.Length)
            {
                array[arrayIndex] = enumerator.Current;
                arrayIndex++;
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                IEnumerator<T> enumerator = GetEnumerator();
                int result = 0;
                while (enumerator.MoveNext())
                {
                    result++;
                }

                return result;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Remove
            (
                T item
            )
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable members

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (Node<T> node = FirstNode;
                  node != null;
                  node = node.Next)
            {
                yield return node.Value;
            }
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (Node<T> node = FirstNode;
                  node != null;
                  node = node.Next)
            {
                yield return node.Value;
            }
        }

        #endregion
    }
}
