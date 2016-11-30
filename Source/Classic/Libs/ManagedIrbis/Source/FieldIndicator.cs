// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldIndicator.cs -- индикатор поля
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
    /// Индикатор поля.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("indicator")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Value = '{Value}'")]
    [JsonConverter(typeof(FieldIndicatorConverter))]
#endif
    public sealed class FieldIndicator
        : IHandmadeSerializable,
        IReadOnly<FieldIndicator>
    {
        #region Constants

        /// <summary>
        /// Значение не установлено.
        /// </summary>
        public const string NoValue = " ";

        /// <summary>
        /// For visual only.
        /// </summary>
        public const string EmptyValue = "#";

        #endregion

        #region Nested classes

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// JSON converter for <see cref="FieldIndicator"/>
        /// </summary>
        public class FieldIndicatorConverter
            : JsonConverter
        {
            #region JsonConverter members

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The
            /// <see cref="T:Newtonsoft.Json.JsonWriter" />
            /// to write to.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The calling serializer.
            /// </param>
            public override void WriteJson
                (
                    JsonWriter writer,
                    object value,
                    JsonSerializer serializer
                )
            {
                FieldIndicator indicator = (FieldIndicator) value;

                JToken token = new JValue(indicator.Value);
                token.WriteTo(writer);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The
            /// <see cref="T:Newtonsoft.Json.JsonReader" />
            /// to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value
            /// of object being read.</param>
            /// <param name="serializer">The calling serializer.
            /// </param>
            /// <returns>The object value.</returns>
            public override object ReadJson
                (
                    JsonReader reader,
                    Type objectType,
                    object existingValue,
                    JsonSerializer serializer
                )
            {
                JToken token = JToken.Load(reader);
                FieldIndicator indicator = (FieldIndicator) existingValue;
                indicator.Value = token.ToString();

                return indicator;
            }

            /// <summary>
            /// Determines whether this instance can convert
            /// the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns><c>true</c> if this instance can convert
            /// the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return objectType == typeof(FieldIndicator);
            }

            #endregion
        }

#endif

        #endregion

        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get { return _field; } }

        /// <summary>
        /// Whether value is set?
        /// </summary>
        [JsonIgnore]
        public bool HasValue
        {
            get
            {
                return !Value.SameString(NoValue)
                    && !string.IsNullOrEmpty(Value);
            }
        }

        /// <summary>
        /// Value of the indicator.
        /// </summary>
        [XmlText]
        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator()
        {
            _value = NoValue;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator
            (
                [CanBeNull] string value
            )
        {
            SetValue(value);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator
            (
                [CanBeNull] string value,
                bool readOnly
            )
        {
            SetValue(value);
            if (readOnly)
            {
                SetReadOnly();
            }
        }

        #endregion

        #region Private members

        private string _value;

        //[NonSerialized]
        internal RecordField _field;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание клона индикатора.
        /// </summary>
        [NotNull]
        public FieldIndicator Clone()
        {
            FieldIndicator result = new FieldIndicator(Value);

            return result;
        }

        /// <summary>
        /// Marks the indicator as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <summary>
        /// Установка значения.
        /// </summary>
        [NotNull]
        public FieldIndicator SetValue
            (
                [CanBeNull] string value
            )
        {
            ThrowIfReadOnly();

            _value = string.IsNullOrEmpty(value) 
                ? NoValue
                : value.Substring(0, 1);
            return this;
        }

        /// <summary>
        /// Text representation of the field.
        /// </summary>
        [NotNull]
        public string ToText()
        {
            return (string.IsNullOrEmpty(Value))
                ? EmptyValue
                : Value;
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

            string value = reader.ReadString();
            SetValue(value);
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

            writer.Write(Value);
        }

        #endregion

        #region IReadOnly<T> members

        //[NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the indicator is read-only?
        /// </summary>
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Creates read-only clone of the indicator.
        /// </summary>
        public FieldIndicator AsReadOnly()
        {
            FieldIndicator result = Clone();
            result._readOnly = true;

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

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return ToText();
        }

        #endregion
    }
}
