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

        /// <summary>
        /// Constructor.
        /// </summary>
        public DoublyLinkedList()
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// _s the node at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        [CanBeNull]
        [CLSCompliant(false)]
        protected Node<T> _NodeAt(int index)
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

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            IEnumerator<T> enumerator = GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                if (enumerator.Current.Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void Insert(int index, T item)
        {
            Node<T> exisingNode = _NodeAt(index);
            Node<T> newNode = _CreateNode(item);
            Node<T> nextNode = exisingNode.Next;
            newNode.Previous = exisingNode;
            newNode.Next = nextNode;
            exisingNode.Next = exisingNode;
            if (nextNode != null)
            {
                nextNode.Previous = newNode;
            }
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void RemoveAt(int index)
        {
            Node<T> nodeToRemove = _NodeAt(index);
            if (nodeToRemove != null)
            {
                Node<T> nextNode = nodeToRemove.Next;
                Node<T> previousNode = nodeToRemove.Previous;
                if (previousNode != null)
                {
                    previousNode.Next = nextNode;
                }
                if (nextNode != null)
                {
                    nextNode.Previous = previousNode;
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

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item)
        {
            Node<T> node = _CreateNode(item);
            if (LastNode != null)
            {
                LastNode.Next = node;
            }
            node.Previous = LastNode;
            LastNode = node;
            if (FirstNode == null)
            {
                FirstNode = node;
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            FirstNode = null;
            LastNode = null;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
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

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
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