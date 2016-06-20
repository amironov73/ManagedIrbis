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

using AM;
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
        IHandmadeSerializable,
        IReadOnly<RecordFieldCollection>
    {
        #region Properties

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        public IrbisRecord Record { get { return _record; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        [NonSerialized]
        private IrbisRecord _record;

        internal RecordFieldCollection _SetRecord
            (
                IrbisRecord newRecord
            )
        {
            _record = newRecord;

            foreach (RecordField field in this)
            {
                field.Record = newRecord;
            }

            return this;
        }

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
            ThrowIfReadOnly();
            Code.NotNull(fields, "fields");
            
            foreach (RecordField field in fields)
            {
                Add(field);
            }
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
        /// Removes all elements from the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (RecordField field in this)
            {
                field.Record = null;
            }

            base.ClearItems();
        }

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
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Record = Record;

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index of the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        /// <param name="index">The zero-based index
        /// of the element to remove.</param>
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if ((index >= 0) && (index < Count))
            {
                RecordField field = this[index];
                if (field != null)
                {
                    field.Record = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to replace.</param>
        /// <param name="item">The new value for the element
        /// at the specified index. The value can't be null.</param>
        protected override void SetItem
            (
                int index,
                [NotNull] RecordField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Record = Record;

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(reader, "reader");

            ClearItems();
            RecordField[] array = reader.ReadArray<RecordField>();
            AddRange(array);
        }

        /// <summary>
        /// Save the object state to the given stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteArray(this.ToArray());
        }

        #endregion

        #region IReadOnly<T> members

        [NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the collection is read-only?
        /// </summary>
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Create read-only clone of the collection.
        /// </summary>
        public RecordFieldCollection AsReadOnly()
        {
            RecordFieldCollection result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <summary>
        /// Marks the object as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
            foreach (RecordField field in this)
            {
                field.SetReadOnly();
            }
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }
        }

        #endregion
    }
}
