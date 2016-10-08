/* SubField.cs -- MARC record subfield.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Code={Code}, Value={Value}")]
#endif
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
            Field = field;
            if (readOnly)
            {
                SetReadOnly();
            }
        }

        #endregion

        #region Private members

        private char _code;

        private string _value;

        //[NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

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
            ThrowIfReadOnly();

            if (SubFieldCode.Verify(code))
            {
                _code = code;
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
                _value = value;
            }
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
            ThrowIfReadOnly();
            CodeJam.Code.NotNull(reader, "reader");

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
            CodeJam.Code.NotNull(writer, "writer");

            writer.WriteNullable(CodeString);
            writer.WriteNullable(Value);
        }

        #endregion

        #region IReadOnly<T> members

        // ReSharper disable InconsistentNaming
        //[NonSerialized]
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Whether the object is read-only?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Creates read-only clone of the field.
        /// </summary>
        public SubField AsReadOnly()
        {
            SubField result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <summary>
        /// Mark the subField as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
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

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
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
                    "SubField " + Path + ": Code"
                )
                .Assert
                (
                    SubFieldValue.Verify(Value),
                    "SubField " + Path + ": Value"
                );

            return verifier.Result;
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
