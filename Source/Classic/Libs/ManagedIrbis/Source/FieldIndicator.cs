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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

//#if FW4
//using System.ComponentModel;
//#endif

using AM;
using AM.Logging;
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
    [Serializable]
    [MoonSharpUserData]
    [XmlRoot("indicator")]
    [DebuggerDisplay("Value = '{Value}'")]
#if !WINMOBILE && !PocketPC
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

            /// <inheritdoc />
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

            /// <inheritdoc />
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
                if (ReferenceEquals(indicator, null))
                {
                    indicator = new FieldIndicator();
                }
                indicator.Value = token.ToString();

                return indicator;
            }

            /// <inheritdoc />
            #if FW4
            [ExcludeFromCodeCoverage]
            #endif
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

        ///// <summary>
        ///// Field.
        ///// </summary>
        //[CanBeNull]
        //[XmlIgnore]
        //[JsonIgnore]
        //public RecordField Field { get { return _field; } }

        /// <summary>
        /// Whether value is set?
        /// </summary>
        [JsonIgnore]
        public bool HasValue
        {
            get
            {
                return !string.IsNullOrEmpty(Value)
                    && !Value.SameString(NoValue);
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

        //// ReSharper disable InconsistentNaming
        //internal RecordField _field;
        //// ReSharper restore InconsistentNaming

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

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
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

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
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

        // ReSharper disable InconsistentNaming
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        [JsonIgnore]
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public FieldIndicator AsReadOnly()
        {
            FieldIndicator result = Clone();
            result._readOnly = true;

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "FieldIndicator::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return ToText();
        }

        #endregion
    }
}
