/* SubField.cs -- MARC record subfield.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

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
    [XmlRoot("subfield")]
    [MoonSharpUserData]
    [DebuggerDisplay("Code={Code}, Value={Value}")]
    public sealed class SubField
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// </summary>
        public const char NoCode = '\0';

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// </summary>
        public const string NoCodeString = "\0";

        /// <summary>
        /// Subfield delimiter.
        /// </summary>
        public const char Delimiter = '^';

        #endregion

        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public char Code
        {
            get { return _code; }
            set { SetCode(value); }
        }

        /// <summary>
        /// Код подполя.
        /// </summary>
        /// <remarks>
        /// Для XML-сериализации.
        /// </remarks>
        [XmlAttribute("code")]
        [JsonProperty("code")]
        [CanBeNull]
        public string CodeString
        {
            get
            {
                return Code.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Code = value[0];
                }
            }
        }

        /// <summary>
        /// Значение подполя.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set
            {
                SetValue(value);
            }
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
        /// Is read only?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Ссылка на поле, владеющее
        /// данным подполем. Настраивается
        /// перед передачей в скрипты.
        /// Всё остальное время неактуально.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NonSerialized]
        public RecordField Field;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubField()
        {
            _readOnly = false;
        }

        /// <summary>
        /// Конструктор с присвоением кода подполя.
        /// </summary>
        public SubField
            (
                char code
            )
        {
            _readOnly = false;
            Code = code;
        }

        /// <summary>
        /// Конструктор с присвоением кода и значения подполя.
        /// </summary>
        public SubField
            (
                char code,
                [CanBeNull] string value
            )
        {
            _readOnly = false;
            Code = code;
            Value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubField
            (
                char code,
                [CanBeNull] string value,
                bool readOnly,
                [CanBeNull] RecordField field
            )
        {
            Code = code;
            Value = value;
            _readOnly = readOnly;
            Field = field;
        }

        #endregion

        #region Private members

        private char _code;

        private string _value;

        [NonSerialized]
        // ReSharper disable InconsistentNaming
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Creates read-only clone of the field.
        /// </summary>
        [NotNull]
        public SubField AsReadOnly()
        {
            SubField result = Clone();
            result._readOnly = true;

            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public SubField Clone()
        {
            SubField result = new SubField
            {
                Code = Code,
                Value = Value
            };

            return result;
        }

        /// <summary>
        /// Compares the specified subfields.
        /// </summary>
        public static int Compare
            (
                [NotNull] SubField subField1,
                [NotNull] SubField subField2
            )
        {
            CodeJam.Code.NotNull(subField1, "subField1");
            CodeJam.Code.NotNull(subField2, "subField2");

            int result = subField1.Code.CompareTo(subField2.Code);
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal
                (
                    subField1.Value,
                    subField2.Value
                );

            return result;
        }

        /// <summary>
        /// Sets the subfield code.
        /// </summary>
        public void SetCode
            (
                char code
            )
        {
            if (_readOnly)
            {
                throw new ReadOnlyException();
            }

            _code = code;
        }

        /// <summary>
        /// Sets the subfield value.
        /// </summary>
        public void SetValue
            (
                [CanBeNull] string value
            )
        {
            if (_readOnly)
            {
                throw new ReadOnlyException();
            }

            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf(Delimiter) >= 0)
                {
                    throw new ArgumentException
                        (
                            "Value contains delimiter",
                            "value"
                        );
                }
            }
            _value = value;
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
            CodeJam.Code.NotNull(() => reader);

            CodeString = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            CodeJam.Code.NotNull(() => writer);

            writer.WriteNullable(CodeString);
            writer.WriteNullable(Value);
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
            return string.Format
                (
                    "^{0}{1}",
                    Code,
                    Value
                );
        }

        #endregion
    }
}
