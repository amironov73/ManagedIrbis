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
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    [XmlRoot("subfields")]
    [DebuggerDisplay("Count={Count}")]
    public sealed class SubFieldCollection
        : Collection<SubField>,
        IHandmadeSerializable,
        IReadOnly<SubFieldCollection>
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get { return _field; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        [NonSerialized]
        private RecordField _field;

        internal SubFieldCollection _SetField
            (
                RecordField newField
            )
        {
            _field = newField;

            foreach (SubField subField in this)
            {
                subField.Field = newField;
            }

            return this;
        }

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
            ThrowIfReadOnly();

            foreach (SubField subField in subFields)
            {
                Add(subField);
            }

            return this;
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
        /// Removes all elements from the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (SubField subField in this)
            {
                subField.Field = null;
            }

            base.ClearItems();
        }

        /// <summary>
        /// Inserts an element into the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />
        /// at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which
        /// <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can't
        /// be <c>null</c>.</param>
        protected override void InsertItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Field = Field;

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index of the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to remove.</param>
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if ((index >= 0) && (index < Count))
            {
                SubField subField = this[index];
                if (subField != null)
                {
                    subField.Field = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the
        /// element to replace.</param>
        /// <param name="item">The new value for the element
        /// at the specified index. The value can't be <c>null</c>.
        /// </param>
        protected override void SetItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Field = Field;

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
            SubField[] array = reader.ReadArray<SubField>();
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

        // ReSharper disable InconsistentNaming
        [NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the collection is read-only?
        /// </summary>
        public bool ReadOnly { get { return _readOnly; } }

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Create read-only clone of the collection.
        /// </summary>
        public SubFieldCollection AsReadOnly()
        {
            SubFieldCollection result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        /// <exception cref="System.Data.ReadOnlyException"></exception>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }
        }

        /// <summary>
        /// Mark the collection as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
            foreach (SubField subField in this)
            {
                subField.SetReadOnly();
            }
        }

        /// <summary>
        /// Convert the collection to JSON.
        /// </summary>
        [NotNull]
        public string ToJson()
        {
            string result = JArray.FromObject(this).ToString();

            return result;
        }

        /// <summary>
        /// Restore the collection from JSON.
        /// </summary>
        [NotNull]
        public static SubFieldCollection FromJson
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            SubFieldCollection result
                = JsonConvert.DeserializeObject<SubFieldCollection>
                (
                    text
                );

            return result;
        }

        /// <summary>
        /// Assign.
        /// </summary>
        [NotNull]
        public SubFieldCollection Assign
            (
                [NotNull] SubFieldCollection other
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(other, "other");

            Clear();
            _field = other.Field;
            AddRange(other);

            return this;
        }

        /// <summary>
        /// Assign clone.
        /// </summary>
        [NotNull]
        public SubFieldCollection AssignClone
            (
                [NotNull] SubFieldCollection other
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(other, "other");

            Clear();
            _field = other.Field;
            foreach (SubField subField in other)
            {
                Add(subField.Clone());
            }

            return this;
        }

        #endregion
    }
}
