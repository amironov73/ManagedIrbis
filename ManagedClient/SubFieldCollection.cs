/* SubFieldCollection.cs -- коллекция подполей
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Коллекция подполей.
    /// Отличается тем, что принципиально не принимает
    /// значения <c>null</c>.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    public sealed class SubFieldCollection
        : Collection<SubField>
    {
        #region Public methods

        /// <summary>
        /// Добавление в коллекцию нескольких подполей сразу
        /// </summary>
        public void AddRange
            (
                IEnumerable<SubField> subFields
            )
        {
            foreach (SubField subField in subFields)
            {
                Add(subField);
            }
        }

        /// <summary>
        /// Поиск с помощью предиката.
        /// </summary>
        public SubField Find
            (
                Predicate<SubField> predicate
            )
        {
            return this
                .FirstOrDefault
                (
                    subField => predicate(subField)
                );
        }

        /// <summary>
        /// Отбор с помощью предиката.
        /// </summary>
        public SubField[] FindAll
            (
                Predicate<SubField> predicate
            )
        {
            return this
                .Where(subField => predicate(subField))
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        protected override void InsertItem
            (
                int index,
                SubField item
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
                SubField item
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
