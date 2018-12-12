// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordFieldCollection.cs -- коллекция полей записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using UnsafeAM;
using UnsafeAM.Collections;
using UnsafeAM.IO;
using UnsafeAM.Logging;
using UnsafeAM.Runtime;

using UnsafeCode;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Коллекция полей записи.
    /// Отличается тем, что принципиально
    /// не принимает значения <c>null</c>.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class RecordFieldCollection
        : Collection<RecordField>//,
//        IHandmadeSerializable,
//        IReadOnly<RecordFieldCollection>
    {
        #region Properties

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity
        {
            get
            {
                List<RecordField> innerList = _GetInnerList();

                return innerList.Capacity;
            }
        }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public MarcRecord Record { get { return _record; } }

        #endregion

        #region Private members

        [NotNull]
        private List<RecordField> _GetInnerList()
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            List<RecordField> result = (List<RecordField>)Items;
            // ReSharper restore SuspiciousTypeConversion.Global

            return result;
        }

        private MarcRecord _record;

        private bool _dontRenumber;

        // ReSharper disable InconsistentNaming

        internal void _RenumberFields()
        {
            if (_dontRenumber)
            {
                return;
            }

            DictionaryCounterInt32<int> seen = new DictionaryCounterInt32<int>();

            foreach (RecordField field in this)
            {
                int tag = field.Tag;
                field.Repeat = tag <= 0
                    ? 0
                    : seen.Increment(tag);
            }
        }

        [NotNull]
        internal RecordFieldCollection _SetRecord
            (
                [CanBeNull] MarcRecord newRecord
            )
        {
            _record = newRecord;

            foreach (RecordField field in this)
            {
                field.Record = newRecord;
            }

            return this;
        }

        internal void SetModified()
        {
            if (!ReferenceEquals(Record, null))
            {
                Record.Modified = true;
            }
        }

        // ReSharper restore InconsistentNaming

        #endregion

        #region Public methods

        /// <summary>
        /// Add capacity to eliminate reallocations.
        /// </summary>
        public void AddCapacity
            (
                int delta
            )
        {
            List<RecordField> innerList = _GetInnerList();
            int newCapacity = innerList.Count + delta;
            if (newCapacity > innerList.Capacity)
            {
                innerList.Capacity = newCapacity;
            }
        }

        /// <summary>
        /// Add range of <see cref="RecordField"/>s.
        /// </summary>
        public void AddRange
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            ThrowIfReadOnly();
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                Add(field);
            }
        }

        /// <summary>
        /// Apply the field value.
        /// </summary>
        /// <remarks>
        /// For non-repeating fields only.
        /// </remarks>
        [NotNull]
        public RecordFieldCollection ApplyFieldValue
            (
                int tag,
                [CanBeNull] string value
            )
        {
            RecordField targetField = null;
            foreach (RecordField field in this)
            {
                if (field.Tag == tag)
                {
                    targetField = field;
                    break;
                }
            }

            if (string.IsNullOrEmpty(value))
            {
                if (!ReferenceEquals(targetField, null))
                {
                    Remove(targetField);
                }
            }
            else
            {
                if (ReferenceEquals(targetField, null))
                {
                    targetField = new RecordField(tag);
                    Add(targetField);
                }
                targetField.Value = value;
            }

            return this;
        }

        /// <summary>
        /// Begin record update.
        /// </summary>
        public void BeginUpdate()
        {
            _dontRenumber = true;
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
        /// End record update.
        /// </summary>
        public void EndUpdate()
        {
            _dontRenumber = false;
            _RenumberFields();
        }

        /// <summary>
        /// Ensure the capacity.
        /// </summary>
        public void EnsureCapacity
            (
                int capacity
            )
        {
            List<RecordField> innerList = _GetInnerList();
            if (innerList.Capacity < capacity)
            {
                innerList.Capacity = capacity;
            }
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

            foreach (RecordField field in Items)
            {
                if (predicate(field))
                {
                    return field;
                }
            }

            return null;
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

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in this)
            {
                if (predicate(field))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (RecordField field in this)
            {
                field.Record = null;
            }

            SetModified();

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                RecordField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Record = Record;

            base.InsertItem(index, item);

            SetModified();

            _RenumberFields();
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if (index >= 0 && index < Count)
            {
                RecordField field = this[index];
                if (field != null)
                {
                    field.Record = null;
                }
            }

            base.RemoveItem(index);

            SetModified();

            _RenumberFields();
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                RecordField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Record = Record;

            base.SetItem(index, item);

            SetModified();

            _RenumberFields();
        }

        #endregion

        #region IHandmadeSerializable members

        ///// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        //public void RestoreFromStream
        //    (
        //        BinaryReader reader
        //    )
        //{
        //    ThrowIfReadOnly();
        //    Code.NotNull(reader, "reader");

        //    ClearItems();
        //    RecordField[] array = reader.ReadArray<RecordField>();
        //    AddRange(array);
        //}

        ///// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        //public void SaveToStream
        //    (
        //        BinaryWriter writer
        //    )
        //{
        //    Code.NotNull(writer, "writer");

        //    writer.WriteArray(this.ToArray());
        //}

        #endregion

        #region IReadOnly<T> members

        internal bool _readOnly;

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public RecordFieldCollection AsReadOnly()
        {
            RecordFieldCollection result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _readOnly = true;
            foreach (RecordField field in this)
            {
                field.SetReadOnly();
            }
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "RecordFieldCollection::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region Object members

        ///// <inheritdoc cref="Object.ToString" />
        //public override string ToString()
        //{
        //    return StringUtility.Join
        //        (
        //            Environment.NewLine,
        //            this
        //        );
        //}

        #endregion
    }
}
