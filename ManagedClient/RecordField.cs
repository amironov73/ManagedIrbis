/* SubField.cs -- MARC record field.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
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
        : IHandmadeSerializable
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
        public string Tag { get; set; }

        /// <summary>
        /// Первый индикатор.
        /// </summary>
        [NotNull]
        public FieldIndicator Indicator1
        {
            get { return _indicator1; }
        }

        /// <summary>
        /// Второй индикатор.
        /// </summary>
        [NotNull]
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
        }

        /// <summary>
        /// Конструктор с присвоением тега поля.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag
            )
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
        {
            Tag = tag;
            Value = value;
        }

        /// <summary>
        /// Конструктор с подполями.
        /// </summary>
        public RecordField
            (
                [CanBeNull] string tag,
                [ItemNotNull] params SubField[] subFields
            )
        {
            Tag = tag;
            SubFields.AddRange(subFields);
        }

        #endregion

        #region Private members

        private readonly FieldIndicator
            _indicator1 = new FieldIndicator(),
            _indicator2 = new FieldIndicator();

        private string _value;

        private readonly SubFieldCollection _subFields
            = new SubFieldCollection();

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
        /// Установка нового значения.
        /// </summary>
        [NotNull]
        public RecordField SetValue
            (
                [CanBeNull] string value
            )
        {
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

            Tag = reader.ReadNullableString();
            Indicator1.RestoreFromStream(reader);
            Indicator2.RestoreFromStream(reader);
            Value = reader.ReadNullableString();
            SubFields.RestoreFromStream(reader);
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

            writer.WriteNullable(Tag);
            Indicator1.SaveToStream(writer);
            Indicator2.SaveToStream(writer);
            writer.WriteNullable(Value);
            SubFields.SaveToStream(writer);
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
