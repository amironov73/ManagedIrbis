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
using System.Diagnostics.CodeAnalysis;

using AM.Logging;

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
        public class Node
        {
            #region Public properties

            /// <summary>
            /// Предыдущий элемент.
            /// </summary>
            [CanBeNull]
            [JsonIgnore]
            public Node Previous { get; set; }

            /// <summary>
            /// Последующий элемент.
            /// </summary>
            [CanBeNull]
            [JsonIgnore]
            public Node Next { get; set; }

            /// <summary>
            /// Хранимое значение.
            /// </summary>
            [CanBeNull]
            [JsonProperty("value")]
            public T Value { get; set; }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
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
        [CanBeNull]
        public Node FirstNode { get; protected set; }

        /// <summary>
        /// Последний элемент.
        /// </summary>
        [CanBeNull]
        public Node LastNode { get; protected set; }

        #endregion

        #region Private members

        /// <summary>
        /// Get node at specified index.
        /// </summary>
        [CanBeNull]
        [CLSCompliant(false)]
        protected Node _NodeAt
            (
                int index
            )
        {
            if (index < 0)
            {
                return null;
            }

            Node result = FirstNode;

            for (int i = 0; i < index; i++)
            {
                if (ReferenceEquals(result, null))
                {
                    break;
                }
                result = result.Next;
            }

            return result;
        }

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                T item
            )
        {
            using (IEnumerator<T> enumerator = GetEnumerator())
            {
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    T current = enumerator.Current;
                    if (!ReferenceEquals(current, null)
                        && current.Equals(item))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            Code.Nonnegative(index, "index");

            Node existingNode = _NodeAt(index);
            if (!ReferenceEquals(existingNode, null))
            {
                Node newNode = new Node { Value = item };
                Node nextNode = existingNode.Next;
                Node previousNode = existingNode.Previous;
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

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            Code.Nonnegative(index, "index");

            Node nodeToRemove = _NodeAt(index);
            if (!ReferenceEquals(nodeToRemove, null))
            {
                Node nextNode = nodeToRemove.Next;
                Node previousNode = nodeToRemove.Previous;
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

        /// <inheritdoc cref="IList{T}.this"/>
        public T this[int index]
        {
            get
            {
                Node node = _NodeAt(index);

                if (ReferenceEquals(node, null))
                {
                    Log.Error
                        (
                            "DoublyLinkedList::Indexer: "
                            + "index="
                            + index
                            + " is out of range"
                        );

                    throw new IndexOutOfRangeException();
                }

                return node.Value;
            }
            set
            {
                Node node = _NodeAt(index);

                if (ReferenceEquals(node, null))
                {
                    Log.Error
                    (
                        "DoublyLinkedList::Indexer: "
                        + "index="
                        + index
                        + " is out of range"
                    );

                    throw new IndexOutOfRangeException();
                }

                node.Value = value;
            }
        }

        #endregion

        #region ICollection<T> members

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            Node node = new Node { Value = item };
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

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            FirstNode = null;
            LastNode = null;
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            foreach (T current in this)
            {
                if (current.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            using (IEnumerator<T> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext()
                       && arrayIndex < array.Length)
                {
                    array[arrayIndex] = enumerator.Current;
                    arrayIndex++;
                }
            }
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get
            {
                using (IEnumerator<T> enumerator = GetEnumerator())
                {
                    int result = 0;
                    while (enumerator.MoveNext())
                    {
                        result++;
                    }

                    return result;
                }
            }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
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

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (Node node = FirstNode;
                  !ReferenceEquals(node, null);
                  node = node.Next)
            {
                yield return node.Value;
            }
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            for (Node node = FirstNode;
                  !ReferenceEquals(node, null);
                  node = node.Next)
            {
                yield return node.Value;
            }
        }

        #endregion
    }
}
