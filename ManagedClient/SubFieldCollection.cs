/* SubFieldCollection.cs -- коллекция подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM.IO;
using AM.Runtime;

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
        : Collection<SubField>,
        IHandmadeSerializable
    {
        #region Properties

        [CanBeNull]
        public RecordField Field { get { return _field; } }

        /// <summary>
        /// Whether the collection is read-only?
        /// </summary>
        public bool ReadOnly { get { return _readOnly; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        [NonSerialized]
        internal bool _readOnly;

        [NonSerialized]
        internal RecordField _field;
        // ReSharper restore InconsistentNaming

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление в коллекцию нескольких подполей сразу
        /// </summary>
        [NotNull]
        public SubFieldCollection AddRange
            (
                [NotNull] IEnumerable<SubField> subFields
            )
        {
            Code.NotNull(subFields, "subFields");
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            foreach (SubField subField in subFields)
            {
                Add(subField);
            }

            return this;
        }

        /// <summary>
        /// Create read-only clone of the collection.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public SubFieldCollection AsReadOnly()
        {
            SubFieldCollection result = Clone();
            result._readOnly = true;

            return result;
        }

        /// <summary>
        /// Создание "глубокой" копии коллекции.
        /// </summary>
        [NotNull]
        public SubFieldCollection Clone()
        {
            SubFieldCollection result = new SubFieldCollection
            {
                _field = Field
            };

            foreach (SubField subField in this)
            {
                SubField clone = subField.Clone();
                clone.Field = Field;
                result.Add(clone);
            }

            return result;
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
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            item.Field = Field;

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
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            base.SetItem(index, item);

            item.Field = Field;
        }

        protected override void ClearItems()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            base.ClearItems();
        }

        protected override void RemoveItem
            (
                int index
            )
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            base.RemoveItem(index);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

            ClearItems();
            SubField[] array = reader.ReadArray<SubField>();
            AddRange(array);
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(() => writer);

            writer.WriteArray(this.ToArray());
        }

        #endregion
    }
}
