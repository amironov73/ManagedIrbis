/* SubField.cs -- MARC record field.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// MARC record subfield.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("field")]
    [MoonSharpUserData]
    [DebuggerDisplay("Tag={Tag} Value={Value}")]
    public sealed class RecordField
        : IHandmadeSerializable,
        IReadOnly<RecordField>
    {
        #region Constants

        /// <summary>
        /// Нет тега, т. е. тег ещё не присвоен.
        /// </summary>
        public const string NoTag = null;

        #endregion

        #region Properties

        /// <summary>
        /// Флаг: выбрасывать исключение, если свойству Value
        /// присваивается значение, содержащее разделитель.
        /// </summary>
        public static bool BreakOnValueContainDelimiters;

        /// <summary>
        /// Метка поля.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag
        {
            get { return _tag; }
            set { SetTag(value); }
        }

        /// <summary>
        /// Первый индикатор.
        /// </summary>
        [NotNull]
        [XmlElement("indicator1")]
        [JsonProperty("indicator1")]
        public FieldIndicator Indicator1
        {
            get { return _indicator1; }
        }

        /// <summary>
        /// Второй индикатор.
        /// </summary>
        [NotNull]
        [XmlElement("indicator2")]
        [JsonProperty("indicator2")]
        public FieldIndicator Indicator2
        {
            get { return _indicator2; }
        }

        /// <summary>
        /// Повторение поля.
        /// Настраивается перед передачей
        /// в скрипты.
        /// Не используется в большинстве сценариев.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public int Repeat;

        /// <summary>
        /// Значение поля до первого разделителя подполей.
        /// </summary>
        /// <remarks>
        /// Внимание! Если присваиваемое значение содержит
        /// разделитель, то происходит и присвоение подполей!
        /// Имеющиеся в SubFields значения при этом пропадают
        /// и замещаются на вновь присваиваемые!
        /// </remarks>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        /// <summary>
        /// Список подполей.
        /// </summary>
        [NotNull]
        [XmlElement("subfield")]
        [JsonProperty("subfields")]
        public SubFieldCollection SubFields
        {
            get { return _subFields; }
        }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        /// <summary>
        /// Ссылка на запись, владеющую
        /// данным полем. Настраивается
        /// перед передачей в скрипты.
        /// Всё остальное время неактуально.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public IrbisRecord Record;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RecordField()
        {
            _subFields = new SubFieldCollection()
                ._SetField(this);

            _indicator1 = new FieldIndicator
            {
                _field = this
            };
            _indicator2 = new FieldIndicator
            {
                _field = this
            };
        }

        /// <summary>
        /// Конструктор для клонирования.
        /// </summary>
        private RecordField
            (
                [NotNull] RecordField other
            )
            : this ()
        {
            _indicator1 = other.Indicator1.Clone();
            _indicator2 = other.Indicator2.Clone();
            _value = other.Value;
            _subFields = other.SubFields.Clone();
            _userData = other.UserData;
        }

        /// <summary>
        /// Конструктор с присвоением тега поля.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag
            )
            : this ()
        {
            Tag = tag;
        }

        /// <summary>
        /// Конструктор с присвоением тега и значения поля.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag,
                [CanBeNull] string value
            )
            : this ()
        {
            Tag = tag;
            Value = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag,
                [CanBeNull] string value,
                bool readOnly,
                [CanBeNull] IrbisRecord record
            )
            : this ()
        {
            Tag = tag;
            Value = value;
            Record = record;
            if (readOnly)
            {
                SetReadOnly();
            }
        }

        /// <summary>
        /// Конструктор с подполями.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag,
                [ItemNotNull] params SubField[] subFields
            )
            : this ()
        {
            Tag = tag;
            SubFields.AddRange(subFields);
        }

        #endregion

        #region Private members

        private readonly FieldIndicator _indicator1, _indicator2;

        private string _tag;

        private string _value;

        private readonly SubFieldCollection _subFields;

        [NonSerialized]
        private object _userData;

        private static void _AddSubField
            (
                RecordField field,
                char code,
                StringBuilder value
            )
        {
            if (code != 0)
            {
                if (value.Length == 0)
                {
                    field.SubFields.Add(new SubField(code));
                }
                else
                {
                    field.SubFields.Add(new SubField
                        (
                            code,
                            value.ToString()
                        ));
                }
            }
            value.Length = 0;
        }

        private static void _EncodeSubField
            (
                StringBuilder builder,
                SubField subField
            )
        {
            builder.AppendFormat
                (
                    "{0}{1}{2}",
                    SubField.Delimiter,
                    subField.Code,
                    subField.Value
                );
        }

        internal static void _EncodeField
            (
                StringBuilder builder,
                RecordField field
            )
        {
            int fieldNumber = field.Tag.SafeToInt32();
            if (fieldNumber != 0)
            {
                builder.AppendFormat
                    (
                        "{0}#",
                        fieldNumber
                    );
            }
            else
            {
                builder.AppendFormat
                    (
                        "{0}#",
                        field.Tag
                    );
            }

            if (!string.IsNullOrEmpty(field.Value))
            {
                builder.Append(field.Value);
            }

            foreach (SubField subField in field.SubFields)
            {
                _EncodeSubField
                    (
                        builder,
                        subField
                    );
            }
            builder.Append("\x001F\x001E");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        [NotNull]
        public RecordField AddSubField
            (
                char code,
                [CanBeNull] object value
            )
        {
            ThrowIfReadOnly();

            string text = value.NullableToString();

            SubFields.Add(new SubField(code, text));

            return this;
        }

        /// <summary>
        /// Добавление подполя, при условии,
        /// что у него не пустое значение.
        /// </summary>
        [NotNull]
        public RecordField AddNonEmptySubField
            (
                char code,
                [CanBeNull] object value
            )
        {
            ThrowIfReadOnly();

            string text = value.NullableToString();

            if (!string.IsNullOrEmpty(text))
            {
                AddSubField(code, text);
            }

            return this;
        }

        /// <summary>
        /// Создание "глубокой" копии поля.
        /// </summary>
        [NotNull]
        public RecordField Clone()
        {
            RecordField result = new RecordField(this);

            return result;
        }

        /// <summary>
        /// Compares the specified fields.
        /// </summary>
        public static int Compare
            (
                [NotNull] RecordField field1,
                [NotNull] RecordField field2
            )
        {
            Code.NotNull(field1, "field1");
            Code.NotNull(field2, "field2");

            int result = string.CompareOrdinal
                (
                    field1.Tag,
                    field2.Tag
                );
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal
                (
                    field1.Value,
                    field2.Value
                );
            if (result != 0)
            {
                return result;
            }

            result = field1.SubFields.Count
                - field2.SubFields.Count;
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < field1.SubFields.Count; i++)
            {
                SubField subField1 = field1.SubFields[i];
                SubField subField2 = field2.SubFields[i];

                result = SubField.Compare
                    (
                        subField1,
                        subField2
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Перечень подполей с указанным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <remarks>Сравнение кодов происходит без учета
        /// регистра символов.</remarks>
        /// <returns>Найденные подполя.</returns>
        [NotNull]
        [ItemNotNull]
        public SubField[] GetSubField
            (
                char code
            )
        {
            SubField[] result = SubFields
                .Where(_ => _.Code.SameChar(code))
                .ToArray();

            return result;
        }

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Найденное подполе или <c>null</c>.</returns>
        [CanBeNull]
        public SubField GetSubField
            (
                char code,
                int occurrence
            )
        {
            SubField[] found = GetSubField(code);
            SubField result = found.GetOccurrence(occurrence);

            return result;
        }

        /// <summary>
        /// Gets the first subfield.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>SubField.</returns>
        [CanBeNull]
        public SubField GetFirstSubField
            (
                char code
            )
        {
            return SubFields.FirstOrDefault
                (
                    subField => subField.Code.SameChar(code)
                );
        }

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Текст найденного подполя или <c>null</c>.</returns>
        public string GetSubFieldValue
            (
                char code,
                int occurrence
            )
        {
            SubField result = GetSubField(code, occurrence);
            return (result == null)
                       ? null
                       : result.Value;
        }

        /// <summary>
        /// Получает значение первого появления подполя
        /// с указанным кодом.
        /// </summary>
        public string GetFirstSubFieldValue
            (
                char code
            )
        {
            SubField result = GetFirstSubField(code);
            return (result == null)
                ? null
                : result.Value;
        }

        /// <summary>
        /// Нет ни одного вхождения подполя с указанным кодом?
        /// </summary>
        public bool HaveNotSubField
            (
                char code
            )
        {
            return SubFields.FirstOrDefault
                (
                    sub => sub.Code.SameChar(code)
                )
                == null;
        }

        /// <summary>
        /// Нет ни одного вхождения подполя с одним из
        /// перечисленных кодов?
        /// </summary>
        public bool HaveNotSubField
            (
                params char[] codes
            )
        {
            return SubFields.FirstOrDefault
                (
                    sub => sub.Code.OneOf(codes)
                )
                == null;
        }

        /// <summary>
        /// Есть ли хоть одно вхождение подполя с указанным кодом?
        /// </summary>
        public bool HaveSubField
            (
                char code
            )
        {
            return SubFields.FirstOrDefault
                (
                    sub => sub.Code.SameChar(code)
                )
                != null;
        }

        /// <summary>
        /// Есть ли хоть одно вхождение подполя с одним из
        /// перечисленных кодов?
        /// </summary>
        public bool HaveSubField
            (
                params char[] codes
            )
        {
            return SubFields.FirstOrDefault
                (
                    sub => sub.Code.OneOf(codes)
                )
                != null;
        }

        /// <summary>
        /// Устанавливает значение подполя.
        /// Если подполя с указанным кодом нет,
        /// оно создаётся.
        /// </summary>
        /// <remarks>Устанавливает значение только первого
        /// подполя с указанным кодом (если в поле их несколько)!
        /// </remarks>
        [NotNull]
        public RecordField SetSubField
            (
                char code,
                [CanBeNull] object value
            )
        {
            ThrowIfReadOnly();

            string text = value.NullableToString();

            SubField subField = SubFields.GetFirstSubField(code);

            if (subField == null)
            {
                subField = new SubField(code, text);
                SubFields.Add(subField);
            }
            subField.Value = text;

            return this;
        }

        /// <summary>
        /// Удаляет все повторения подполей
        /// с указанным кодом.
        /// </summary>
        [NotNull]
        public RecordField RemoveSubField
            (
                char code
            )
        {
            ThrowIfReadOnly();

            SubField[] found = SubFields.GetSubField(code);

            foreach (SubField subField in found)
            {
                SubFields.Remove(subField);
            }

            return this;
        }

        /// <summary>
        /// Заменяет значение подполя.
        /// </summary>
        [NotNull]
        public RecordField ReplaceSubField
            (
                char code,
                [CanBeNull] string oldValue,
                [CanBeNull] object newValue
            )
        {
            ThrowIfReadOnly();

            SubField found = SubFields.GetFirstSubField
                (
                    code,
                    oldValue
                );
            if (found != null)
            {
                string text = newValue.NullableToString();
                found.Value = text;
            }

            return this;
        }

        /// <summary>
        /// Sets the tag for the field.
        /// </summary>
        [NotNull]
        public RecordField SetTag
            (
                [CanBeNull] string tag
            )
        {
            ThrowIfReadOnly();

            _tag = tag;

            return this;
        }

        /// <summary>
        /// Sets the value for the field.
        /// </summary>
        [NotNull]
        public RecordField SetValue
            (
                [CanBeNull] string value
            )
        {
            ThrowIfReadOnly();

            if (string.IsNullOrEmpty(value))
            {
                _value = value;
            }
            else
            {
                if (value.IndexOf(SubField.Delimiter) >= 0)
                {
                    if (BreakOnValueContainDelimiters)
                    {
                        throw new ArgumentException
                            (
                                "Contains delimiter",
                                "value"
                            );
                    }
                    //SetSubFields(value);
                }
                else
                {
                    _value = value;
                }
            }

            return this;
        }

        /// <summary>
        /// Builds text representation of the field.
        /// </summary>
        [NotNull]
        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(Value))
            {
                result.Append(Value);
            }
            foreach (SubField subField in SubFields)
            {
                string subText = subField.ToString();
                if (!string.IsNullOrEmpty(subText))
                {
                    result.Append(subText);
                }
            }

            return result.ToString();
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

            Tag = reader.ReadNullableString();
            Indicator1.RestoreFromStream(reader);
            Indicator2.RestoreFromStream(reader);
            Value = reader.ReadNullableString();
            SubFields.RestoreFromStream(reader);
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

            writer.WriteNullable(Tag);
            Indicator1.SaveToStream(writer);
            Indicator2.SaveToStream(writer);
            writer.WriteNullable(Value);
            SubFields.SaveToStream(writer);
        }

        #endregion

        #region IReadOnly<T> members

        [NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the field is read-only?
        /// </summary>
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Creates read-only clone of the field.
        /// </summary>
        public RecordField AsReadOnly()
        {
            RecordField result = Clone();
            result._readOnly = true;
            result.SubFields._readOnly = true;
            foreach (SubField subField in result.SubFields)
            {
                subField._readOnly = true;
            }

            return result;
        }

        /// <summary>
        /// Marks the record as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
            Indicator1.SetReadOnly();
            Indicator2.SetReadOnly();
            SubFields.SetReadOnly();
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

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" />
        /// that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            _EncodeField(result, this);
            return result.ToString();
        }

        #endregion
    }
}
