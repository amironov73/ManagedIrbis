// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubField.cs -- MARC record subfield.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// MARC record subfield.
    /// </summary>
    [PublicAPI]
    [XmlRoot("subfield")]
    [MoonSharpUserData]
    [DebuggerDisplay("Code={Code}, Value={Value}")]
    public sealed class SubField
        : IHandmadeSerializable,
        IReadOnly<SubField>,
        IVerifiable
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
        /// Whether the subfield is modified?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Modified { get; internal set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        /// <summary>
        /// Ссылка на поле, владеющее
        /// данным подполем. Настраивается
        /// перед передачей в скрипты.
        /// Всё остальное время неактуально.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field;

        /// <summary>
        /// Subfield path.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [NotNull]
        public string Path
        {
            get
            {
                if (Code == NoCode)
                {
                    return string.Empty;
                }

                string result = "^" + Code;

                if (!ReferenceEquals(Field, null))
                {
                    result = Field.Path + result;
                }

                return result;
            }
        }

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
            NotModified();
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
            NotModified();
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
            Field = field;
            NotModified();
            if (readOnly)
            {
                SetReadOnly();
            }
        }

        #endregion

        #region Private members

        private char _code;

        private string _value;

        internal void SetModified()
        {
            Modified = true;
            if (!ReferenceEquals(Field, null))
            {
                Field.SetModified();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
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
        /// Mark the subfield as unmodified.
        /// </summary>
        /// <returns>Self.</returns>
        [NotNull]
        public SubField NotModified()
        {
            Modified = false;

            return this;
        }

        /// <summary>
        /// Sets the subfield code.
        /// </summary>
        public void SetCode
            (
                char code
            )
        {
            ThrowIfReadOnly();

            if (SubFieldCode.Verify(code))
            {
                _code = code;
                SetModified();
            }
        }

        /// <summary>
        /// Sets the subfield value.
        /// </summary>
        public void SetValue
            (
                [CanBeNull] string value
            )
        {
            ThrowIfReadOnly();

            if (SubFieldValue.Verify(value))
            {
                value = StringUtility
                    .ReplaceControlCharacters(value);
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                _value = value;
                SetModified();
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            CodeJam.Code.NotNull(reader, "reader");

            CodeString = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            CodeJam.Code.NotNull(writer, "writer");

            writer.WriteNullable(CodeString);
            writer.WriteNullable(Value);
        }

        #endregion

        #region IReadOnly<T> members

        // ReSharper disable InconsistentNaming
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        [XmlIgnore]
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public SubField AsReadOnly()
        {
            SubField result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "SubField::ThrowIfReadOnly"
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
            Verifier<SubField> verifier = new Verifier<SubField>
                (
                    this,
                    throwOnError
                );

            verifier
                .Assert
                (
                    SubFieldCode.Verify(Code),
                    "SubField " + Path + ": Code: "
                        + Code.ToVisibleString()
                )
                .Assert
                (
                    SubFieldValue.Verify(Value),
                    "SubField " + Path + ": Value: "
                        + Value.ToVisibleString()
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "^{0}{1}",
                    SubFieldCode.Normalize(Code),
                    Value
                );
        }

        #endregion
    }
}
