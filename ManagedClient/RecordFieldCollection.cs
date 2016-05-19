/* RecordFieldCollection.cs -- коллекция полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Коллекция полей записи.
    /// Отличается тем, что принципиально
    /// не принимает значения <c>null</c>.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class RecordFieldCollection
        : Collection<RecordField>
    {
        #region Public methods

        /// <summary>
        /// Add range of <see cref="RecordField"/>s.
        /// </summary>
        /// <param name="fields"></param>
        public void AddRange
            (
                IEnumerable<RecordField> fields
            )
        {
            foreach (RecordField field in fields)
            {
                Add(field);
            }
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        public RecordField Find
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .FirstOrDefault(field => predicate(field));
        }

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public RecordField[] FindAll
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .Where(field => predicate(field))
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        protected override void InsertItem
            (
                int index,
                RecordField item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem
            (
                int index,
                RecordField item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.SetItem(index, item);
        }

        #endregion
    }
}
