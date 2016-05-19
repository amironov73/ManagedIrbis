/* SubFieldCollection.cs -- коллекция подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Коллекция подполей.
    /// Отличается тем, что принципиально не принимает
    /// значения <c>null</c>.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Count={Count}")]
    public sealed class SubFieldCollection
        : Collection<SubField>
    {
        #region Public methods

        /// <summary>
        /// Добавление в коллекцию нескольких подполей сразу
        /// </summary>
        public void AddRange
            (
                [NotNull] IEnumerable<SubField> subFields
            )
        {
            Code.NotNull(subFields, "subFields");

            foreach (SubField subField in subFields)
            {
                Add(subField);
            }
        }

        /// <summary>
        /// Поиск с помощью предиката.
        /// </summary>
        [CanBeNull]
        public SubField Find
            (
                [NotNull] Predicate<SubField> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this
                .FirstOrDefault
                (
                    subField => predicate(subField)
                );
        }

        /// <summary>
        /// Отбор с помощью предиката.
        /// </summary>
        [NotNull]
        public SubField[] FindAll
            (
                [NotNull] Predicate<SubField> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this
                .Where(subField => predicate(subField))
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        /// <summary>
        /// Вставка элемента в коллекцию в заданной позиции.
        /// </summary>
        protected override void InsertItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            Code.NotNull(item, "item");

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Присвоение элемента в данной позиции коллекции.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            Code.NotNull(item, "item");

            base.SetItem(index, item);
        }

        #endregion
    }
}
