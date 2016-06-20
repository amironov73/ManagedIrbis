/* RecordFieldCollection.cs -- коллекция полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AM.IO;
using AM.Runtime;
using CodeJam;
using JetBrains.Annotations;

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
        : Collection<RecordField>,
        IHandmadeSerializable
    {
        #region Properties

        [CanBeNull]
        public IrbisRecord Record { get { return _record; } }

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
        internal IrbisRecord _record;
        // ReSharper restore InconsistentNaming


        #endregion

        #region Public methods

        /// <summary>
        /// Add range of <see cref="RecordField"/>s.
        /// </summary>
        /// <param name="fields"></param>
        public void AddRange
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            foreach (RecordField field in fields)
            {
                Add(field);
            }
        }

        /// <summary>
        /// Create read-only clone of the collection.
        /// </summary>
        public RecordFieldCollection AsReadOnly()
        {
            RecordFieldCollection result = Clone();
            result._readOnly = true;

            return result;
        }

        /// <summary>
        /// Создание клона коллекции.
        /// </summary>
        [NotNull]
        public RecordFieldCollection Clone()
        {
            RecordFieldCollection result = new RecordFieldCollection
            {
                _record = Record
            };

            foreach (RecordField field in this)
            {
                RecordField clone = field.Clone();
                clone.Record = Record;
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        [CanBeNull]
        public RecordField Find
            (
                [NotNull] Predicate<RecordField> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.FirstOrDefault
                (
                    field => predicate(field)
                );
        }

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public RecordField[] FindAll
            (
                [NotNull] Predicate<RecordField> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.Where
                (
                    field => predicate(field)
                )
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        /// <summary>
        /// Inserts an element into the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />
        /// at the specified index.
        /// </summary>
        protected override void InsertItem
            (
                int index,
                [NotNull] RecordField item
            )
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        protected override void SetItem
            (
                int index,
                [NotNull] RecordField item
            )
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }

            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.SetItem(index, item);
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
            RecordField[] array = reader.ReadArray<RecordField>();
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
