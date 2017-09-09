// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubField.cs -- MARC record field.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// MARC record subfield.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("field")]
    [MoonSharpUserData]
    [DebuggerDisplay("{DebugText}")]
    public sealed class RecordField
        : IHandmadeSerializable,
        IReadOnly<RecordField>,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Нет тега, т. е. тег ещё не присвоен.
        /// </summary>
        public const int NoTag = 0;

        /// <summary>
        /// Разделитель подполей.
        /// </summary>
        public const char Delimiter = '^';

        /// <summary>
        /// Количество индикаторов поля.
        /// </summary>
        public const int IndicatorCount = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Флаг: выбрасывать исключение, если свойству Value
        /// присваивается значение, содержащее разделитель.
        /// </summary>
        public static bool BreakOnValueContainDelimiters;

        /// <summary>
        /// Флаг: удалять начальные и конечные пробелы в значении поля.
        /// </summary>
        public static bool TrimValue;

        /// <summary>
        /// Метка поля.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public int Tag
        {
            get { return _tag; }
            set { SetTag(value); }
        }

#if WITH_INDICATORS

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

#endif

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
        /// Whether the field is modified?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Modified
        {
            get { return _modified; }
            internal set { _modified = value; }
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
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Field path
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NotNull]
        public string Path
        {
            get
            {
                string result = string.Format
                    (
                        "{0}/{1}",
                        Tag.ToInvariantString(),
                        Repeat
                    );

                return result;
            }
        }

        /// <summary>
        /// Является ли поле фиксированным.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsFixed
        {
            [DebuggerStepThrough]
            get
            {
                return Tag > 0 && Tag < 10;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordField()
        {
            _subFields = new SubFieldCollection()
                ._SetField(this);

#if WITH_INDICATORS

            _indicator1 = new FieldIndicator
            {
                _field = this
            };
            _indicator2 = new FieldIndicator
            {
                _field = this
            };

#endif
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private RecordField
            (
                [NotNull] RecordField other
            )
            : this()
        {
            Code.NotNull(other, "other");

#if WITH_INDICATORS

            _indicator1 = other.Indicator1.Clone();
            _indicator2 = other.Indicator2.Clone();

#endif

            Tag = other.Tag;
            _value = other.Value;
            _subFields = other.SubFields.Clone();
            UserData = other.UserData;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordField
            (
                int tag
            )
            : this()
        {
            Code.Nonnegative(tag, "tag");

            Tag = tag;
        }

        /// <summary>
        /// Конструктор с присвоением тега поля.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag
            )
            : this()
        {
            SetTag(tag);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordField
            (
                int tag,
                [CanBeNull] string value
            )
            : this()
        {
            Code.Positive(tag, "tag");

            SetTag(tag);
            Value = value;
        }

        /// <summary>
        /// Конструктор с присвоением тега и значения поля.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag,
                [CanBeNull] string value
            )
            : this()
        {
            SetTag(tag);
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
                [CanBeNull] MarcRecord record
            )
            : this()
        {
            SetTag(tag);
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
                int tag,
                [ItemNotNull] params SubField[] subFields
            )
            : this()
        {
            SetTag(tag);
            SubFields.AddRange(subFields);
        }

        #endregion

        #region Private members

        [NonSerialized]
        private bool _modified;

        [NonSerialized]
        private object _userData;

        private string DebugText
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(Path);
                result.Append('#');
                result.Append(Value);

                foreach (SubField subField in SubFields)
                {
                    result.AppendFormat
                        (
                            "|^{0}{1}",
                            subField.Code,
                            subField.Value
                        );
                }

                return result.ToString();
            }
        }

#if WITH_INDICATORS

        private readonly FieldIndicator _indicator1, _indicator2;

#endif

        private int _tag;

        private string _value;

        private readonly SubFieldCollection _subFields;

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

        internal void SetModified()
        {
            Modified = true;
            if (!ReferenceEquals(Record, null))
            {
                Record.Modified = true;
            }
        }

        /// <summary>
        /// Sets the sub fields.
        /// </summary>
        /// <param name="encodedText">The encoded text.</param>
        /// <returns>RecordField.</returns>
        internal RecordField SetSubFields
            (
                [NotNull] string encodedText
            )
        {
            RecordField parsed = Parse
                (
                    Tag,
                    encodedText
                );
            if (!string.IsNullOrEmpty(parsed.Value))
            {
                Value = parsed.Value;
            }
            foreach (SubField subField in parsed.SubFields)
            {
                SubFields.Add(subField);
            }

            return this;
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
        /// Добавление подполя, при условии,
        /// что у него значение не false.
        /// </summary>
        [NotNull]
        public RecordField AddNonEmptySubField
            (
                char code,
                bool value,
                [NotNull] string text
            )
        {
            ThrowIfReadOnly();

            if (value)
            {
                AddSubField(code, text);
            }

            return this;
        }

        /// <summary>
        /// Добавление подполя, при условии,
        /// что у него значение не 0.
        /// </summary>
        [NotNull]
        public RecordField AddNonEmptySubField
            (
                char code,
                int value
            )
        {
            ThrowIfReadOnly();

            if (value != 0)
            {
                string text = value.ToInvariantString();
                AddSubField(code, text);
            }

            return this;
        }


        /// <summary>
        /// Добавление подполя, при условии,
        /// что у него значение не 0.
        /// </summary>
        [NotNull]
        public RecordField AddNonEmptySubField
            (
                char code,
                long value
            )
        {
            ThrowIfReadOnly();

            if (value != 0)
            {
                string text = value.ToInvariantString();
                AddSubField(code, text);
            }

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
                [CanBeNull] DateTime? value
            )
        {
            ThrowIfReadOnly();

            if (!ReferenceEquals(value, null))
            {
                string text = IrbisDate.ConvertDateToString(value.Value);
                AddSubField(code, text);
            }

            return this;
        }

        /// <summary>
        /// Assign the field from another.
        /// </summary>
        [NotNull]
        public RecordField AssignFrom
            (
                [NotNull] RecordField source
            )
        {
            Code.NotNull(source, "source");

            Value = source.Value;
            SubFields.Clear();
            foreach (SubField subField in source.SubFields)
            {
                SubFields.Add(subField.Clone());
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

            int result = field1.Tag - field2.Tag;
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

            return ReferenceEquals(result, null)
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

            return ReferenceEquals(result, null)
                ? null
                : result.Value;
        }

        /// <summary>
        /// For * specification.
        /// </summary>
        [CanBeNull]
        public string GetValueOrFirstSubField()
        {
            string result = Value;
            if (string.IsNullOrEmpty(result))
            {
                SubField subField = SubFields.FirstOrDefault();
                if (!ReferenceEquals(subField, null))
                {
                    result = subField.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Нет ни одного вхождения подполя с указанным кодом?
        /// </summary>
        public bool HaveNotSubField
            (
                char code
            )
        {
            return ReferenceEquals
                (
                    SubFields.FirstOrDefault
                        (
                            sub => sub.Code.SameChar(code)
                        ),
                    null
                );
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
            return ReferenceEquals
                (
                    SubFields.FirstOrDefault
                        (
                            sub => sub.Code.OneOf(codes)
                        ),
                    null
                );
        }

        /// <summary>
        /// Есть ли хоть одно вхождение подполя с указанным кодом?
        /// </summary>
        public bool HaveSubField
            (
                char code
            )
        {
            return !ReferenceEquals
                (
                    SubFields.FirstOrDefault
                        (
                            sub => sub.Code.SameChar(code)
                        ),
                    null
                );
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
            return !ReferenceEquals
                (
                    SubFields.FirstOrDefault
                        (
                            sub => sub.Code.OneOf(codes)
                        ),
                    null
                );
        }

        /// <summary>
        /// Mark the field as unmodified.
        /// </summary>
        /// <returns>Self.</returns>
        [NotNull]
        public RecordField NotModified()
        {
            Modified = false;

            return this;
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

            if (ReferenceEquals(subField, null))
            {
                subField = new SubField(code, text);
                SubFields.Add(subField);
            }
            subField.Value = text;

            return this;
        }

#if WITH_INDICATORS

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        public bool ShouldSerializeIndicator1()
        {
            return Indicator1.HasValue;
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        public bool ShouldSerializeIndicator2()
        {
            return Indicator2.HasValue;
        }

#endif

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        public bool ShouldSerializeSubFields()
        {
            return SubFields.Count > 0;
        }

        /// <summary>
        /// For Newtonsoft.Json.
        /// </summary>
        public bool ShouldSerializeValue()
        {
            return !string.IsNullOrEmpty(Value);
        }

        /// <summary>
        /// Парсинг строкового представления поля.
        /// </summary>
        [CanBeNull]
        public static RecordField Parse
            (
                [CanBeNull] string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] parts = line.SplitFirst('#');
            int tag = NumericUtility.ParseInt32(parts[0]);
            string body = parts[1];

            return Parse
                (
                    tag,
                    body
                );
        }

        /// <summary>
        /// Парсинг текстового представления поля
        /// </summary>
        [NotNull]
        public static RecordField Parse
            (
                int tag,
                [NotNull] string body
            )
        {
            RecordField result = new RecordField(tag);

            int first = body.IndexOf(Delimiter);
            if (first != 0)
            {
                if (first < 0)
                {
                    result.Value = body;
                    body = string.Empty;
                }
                else
                {
                    result.Value = body.Substring
                        (
                            0,
                            first
                        );
                    body = body.Substring(first);
                }
            }

            var code = SubField.NoCode;
            var value = new StringBuilder();
            foreach (char c in body)
            {
                if (c == Delimiter)
                {
                    _AddSubField
                        (
                            result,
                            code,
                            value
                        );
                    code = SubField.NoCode;
                }
                else
                {
                    if (code == SubField.NoCode)
                    {
                        code = c;
                    }
                    else
                    {
                        value.Append(c);
                    }
                }
            }

            _AddSubField
                (
                    result,
                    code,
                    value
                );

            return result;
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
            if (!ReferenceEquals(found, null))
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
                int tag
            )
        {
            ThrowIfReadOnly();

            _tag = tag;
            if (!ReferenceEquals(Record, null))
            {
                Record.Fields._RenumberFields();
            }

            return this;
        }

        /// <summary>
        /// Sets the tag for the field
        /// </summary>
        /// <returns></returns>
        public RecordField SetTag
            (
                [CanBeNull] string tag
            )
        {
            return SetTag
                (
                    string.IsNullOrEmpty(tag)
                    ? 0
                    : NumericUtility.ParseInt32(tag)
                );
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
                        Log.Error
                            (
                                "RecordField::SetValue: "
                                + "contains delimiter: "
                                + value.ToVisibleString()
                            );

                        throw new ArgumentException
                            (
                                "Contains delimiter",
                                "value"
                            );
                    }

                    SetSubFields(value);
                }
                else
                {
                    value = StringUtility
                        .ReplaceControlCharacters(value);

                    if (!string.IsNullOrEmpty(value)
                        && TrimValue)
                    {
                        value = value.Trim();
                    }

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

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            // TODO Is it necessarry?
            ThrowIfReadOnly();

            Code.NotNull(reader, "reader");

            Tag = reader.ReadPackedInt32();

#if WITH_INDICATORS

            Indicator1.RestoreFromStream(reader);
            Indicator2.RestoreFromStream(reader);

#endif

            Value = reader.ReadNullableString();
            SubFields.RestoreFromStream(reader);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(Tag);

#if WITH_INDICATORS

            Indicator1.SaveToStream(writer);
            Indicator2.SaveToStream(writer);

#endif
            writer.WriteNullable(Value);
            SubFields.SaveToStream(writer);
        }

        #endregion

        #region IReadOnly<T> members

        internal bool _readOnly;

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public RecordField AsReadOnly()
        {
            RecordField result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _readOnly = true;

#if WITH_INDICATORS

            Indicator1.SetReadOnly();
            Indicator2.SetReadOnly();

#endif
            SubFields.SetReadOnly();
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "RecordField::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RecordField> verifier = new Verifier<RecordField>
                (
                    this,
                    throwOnError
                );

            verifier
                .Assert
                    (
                        FieldTag.Verify(Tag),
                        "Field " + Path + ": Tag"
                    )
                .Assert
                    (
                        FieldValue.Verify(Value),
                        "Field " + Path + ": Value"
                    );

            foreach (SubField subField in SubFields)
            {
                verifier.Assert
                    (
                        subField.Verify(throwOnError),
                        "SubField " + subField.Path
                    );
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            ProtocolText.EncodeField(result, this);

            return result.ToString();
        }

        #endregion
    }
}
