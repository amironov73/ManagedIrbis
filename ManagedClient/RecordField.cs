/* SubField.cs -- MARC record field.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AM;
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
    public sealed class RecordField
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Флаг: выбрасывать исключение, если свойству Text
        /// присваивается значение, содержащее разделитель.
        /// </summary>
        public static bool BreakOnTextContainDelimiters;

        /// <summary>
        /// Метка поля.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

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
        [XmlAttribute("text")]
        [JsonProperty("text")]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
                else
                {
                    if (value.IndexOf(SubField.Delimiter) >= 0)
                    {
                        if (BreakOnTextContainDelimiters)
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
                        _text = value;
                    }
                }
            }
        }

        /// <summary>
        /// Список подполей.
        /// </summary>
        [XmlElement("subfield")]
        [JsonProperty("subfields")]
        public SubFieldCollection SubFields
        {
            get { return _subFields; }
        }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
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
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public IrbisRecord Record;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        public RecordField()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public RecordField
            (
                string tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordField" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="text">The text.</param>
        public RecordField
            (
                string tag,
                string text
            )
        {
            Tag = tag;
            Text = text;
        }

        #endregion

        #region Private members

        private string _text;

        private readonly SubFieldCollection _subFields
            = new SubFieldCollection();

        [NonSerialized]
        private object _userData;

        private static void AddSubField
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

            if (!string.IsNullOrEmpty(field.Text))
            {
                builder.Append(field.Text);
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


        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
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
