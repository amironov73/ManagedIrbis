/* BindingListSource.cs -- combination of BindingSource and BindingList
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Combination of <see cref="BindingSource"/>
    /// and <see cref="BindingList{T}"/>.
    /// </summary>
    /// <remarks>Values added through <see cref="Add"/>
    /// or <see cref="Insert"/> methods can't be <c>null</c>.
    /// </remarks>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class BindingListSource<T>
        : BindingSource
    {
        #region Properties

        private BindingList<T> _innerList;

        /// <summary>
        /// Gets the inner list.
        /// </summary>
        public BindingList<T> InnerList
        {
            [DebuggerStepThrough]
            get
            {
                return _innerList;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BindingListSource{T}"/> class.
        /// </summary>
        public BindingListSource()
        {
            _innerList = new BindingList<T>();
            DataSource = InnerList;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region InnerList delegating members

        /// <summary>
        /// Raises a 
        /// <see cref="E:System.ComponentModel.BindingList`1.ListChanged"/>
        /// event of type 
        /// <see cref="F:System.ComponentModel.ListChangedType.Reset"/>.
        /// </summary>
        public void ResetBindings()
        {
            InnerList.ResetBindings();
        }

        /// <summary>
        /// Discards a pending new item.
        /// </summary>
        /// <param name="itemIndex">The index of the of the new item 
        /// to be added</param>
        public void CancelNew(int itemIndex)
        {
            InnerList.CancelNew(itemIndex);
        }

        /// <summary>
        /// Commits a pending new item to the collection.
        /// </summary>
        /// <param name="itemIndex">The index of the new item 
        /// to be added.</param>
        public void EndNew(int itemIndex)
        {
            InnerList.EndNew(itemIndex);
        }

        /// <summary>
        /// Adds an object to the end of the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>. 
        /// </summary>
        /// <param name="item">The object to be added to the end of the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// The value can't be null for reference types.</param>
        public void Add(T item)
        {
            object value = item;
            if (value == null)
            {
                throw new ArgumentNullException("item");
            }
            InnerList.Add(item);
        }

        /// <summary>
        /// Copies the entire 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/> 
        /// to a compatible one-dimensional <see cref="T:System.Array"/>,
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional 
        /// <see cref="T:System.Array"/> that is the destination of the 
        /// elements copied from 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in array at which 
        /// copying begins.</param>
        /// <exception cref="T:System.ArgumentException">index is equal 
        /// to or greater than the length of array.-or-The number of elements 
        /// in the source 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>
        /// is greater than the available space from index to the end of the 
        /// destination array.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// index is less than zero.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// array is null.</exception>
        public void CopyTo(T[] array, int index)
        {
            InnerList.CopyTo(array, index);
        }

        /// <summary>
        /// Determines whether an element is in the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <returns>
        /// true if item is found in the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>; 
        /// otherwise, false.
        ///</returns>
        ///
        ///<param name="item">The object to locate in the <see cref="T:System.Collections.ObjectModel.Collection`1"/>>. The value can be null for reference types.</param>
        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based 
        /// index of the first occurrence within the entire 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the entire 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>, 
        /// if found; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the 
        /// <see cref="T:System.Collections.Generic.List`1"/>. 
        /// The value can be null for reference types.</param>
        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an element into the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>
        /// at the specified index.
        /// </summary>
        /// <param name="item">The object to insert. The value can 
        /// be null for reference types.</param>
        /// <param name="index">The zero-based index at which item 
        /// should be inserted.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// index is less than zero.-or-index is greater than 
        /// <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        public void Insert(int index, T item)
        {
            InnerList.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <returns>
        /// true if item is successfully removed; otherwise, false.  
        /// This method also returns false if item was not found 
        /// in the original 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the 
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// The value can be null for reference types.</param>
        public bool Remove(T item)
        {
            return InnerList.Remove(item);
        }

        #endregion
    }
}
