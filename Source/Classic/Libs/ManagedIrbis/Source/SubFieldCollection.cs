// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldCollection.cs -- коллекция подполей
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

#if FW4
using System.Diagnostics.CodeAnalysis;
#endif

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Коллекция подполей.
    /// Отличается тем, что принципиально не принимает
    /// значения <c>null</c>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("subfields")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Count={Count}")]
#endif
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

        private RecordField _field;

#if FW4
        [ExcludeFromCodeCoverage]
#endif
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

        internal void SetModified()
        {
            if (!ReferenceEquals(Field, null))
            {
                Field.SetModified();
            }
        }

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

        /// <inheritdoc />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (SubField subField in this)
            {
                subField.Field = null;
            }

            SetModified();

            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void InsertItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Field = Field;

            SetModified();

            base.InsertItem(index, item);
        }

        /// <inheritdoc />
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

                SetModified();
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem
            (
                int index,
                [NotNull] SubField item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Field = Field;

            SetModified();

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        //[NonSerialized]
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

#if !WINMOBILE && !PocketPC

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

#endif

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
