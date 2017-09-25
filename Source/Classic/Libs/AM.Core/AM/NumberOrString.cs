// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberOrString.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [JsonConverter(typeof(NosConverter))]
    public struct NumberOrString
        : IHandmadeSerializable,
        IXmlSerializable
    {
        #region Inner classes

        class NosConverter
            : JsonConverter
        {
            public override void WriteJson
                (
                    JsonWriter writer,
                    object value,
                    JsonSerializer serializer
                )
            {
                NumberOrString number = (NumberOrString) value;
                JValue jValue = new JValue(number._value);
                jValue.WriteTo(writer);
            }

            public override object ReadJson
                (
                    JsonReader reader,
                    Type objectType,
                    object existingValue,
                    JsonSerializer serializer
                )
            {
                JValue token = (JValue) JToken.Load(reader);
                NumberOrString number = (NumberOrString) existingValue;
                number._value = ReferenceEquals(token.Value, null)
                    ? null
                    : token.ToString(CultureInfo.InvariantCulture);

                return number;
            }

            [ExcludeFromCodeCoverage]
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return objectType == typeof(NumberOrString);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value converted to <see cref="decimal"/>.
        /// </summary>
        public decimal AsDecimal
        {
            get
            {
                return ReferenceEquals(_value, null)
                    ? 0.0m
                    : NumericUtility.ParseDecimal(_value);
            }
            set { _value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Value converted to <see cref="double"/>.
        /// </summary>
        public double AsDouble
        {
            get
            {
                return ReferenceEquals(_value, null)
                    ? 0.0
                    : NumericUtility.ParseDouble(_value);
            }
            set { _value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Value converted to <see cref="short" />.
        /// </summary>
        public short AsInt16
        {
            get
            {
                return ReferenceEquals(_value, null)
                    ? (short)0
                    : NumericUtility.ParseInt16(_value);
            }
            set { _value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Value converted to <see cref="int"/>.
        /// </summary>
        public int AsInt32
        {
            get
            {
                return ReferenceEquals(_value, null)
                    ? 0
                    : NumericUtility.ParseInt32(_value);
            }
            set { _value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Value converted to <see cref="long"/>.
        /// </summary>
        public long AsInt64
        {
            get
            {
                return ReferenceEquals(_value, null)
                    ? 0L
                    : NumericUtility.ParseInt64(_value);
            }
            set { _value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Value itself.
        /// </summary>
        public string AsString
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Whether the value can represent <see cref="decimal"/>?
        /// </summary>
        public bool IsDecimal
        {
            get
            {
                decimal value;
                return NumericUtility.TryParseDecimal(_value, out value);
            }
        }

        /// <summary>
        /// Whether the value can represent <see cref="double"/>?
        /// </summary>
        public bool IsDouble
        {
            get
            {
                double value;
                return NumericUtility.TryParseDouble(_value, out value);
            }
        }

        /// <summary>
        /// Whether the value can represent <see cref="short"/>?
        /// </summary>
        public bool IsInt16
        {
            get
            {
                short value;
                return NumericUtility.TryParseInt16(_value, out value);
            }
        }

        /// <summary>
        /// Whether the value can represent <see cref="int"/>?
        /// </summary>
        public bool IsInt32
        {
            get
            {
                int value;
                return NumericUtility.TryParseInt32(_value, out value);
            }
        }

        /// <summary>
        /// Whether the value can represent <see cref="long"/>?
        /// </summary>
        public bool IsInt64
        {
            get
            {
                long value;
                return NumericUtility.TryParseInt64(_value, out value);
            }
        }

        /// <summary>
        /// Whether the value is <c>null</c>?
        /// </summary>
        public bool IsNull
        {
            get { return ReferenceEquals(_value, null); }
        }

        /// <summary>
        /// Whether the value is <c>null</c> or empty?
        /// </summary>
        public bool IsNullOrEmpty
        {
            get { return string.IsNullOrEmpty(_value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                [CanBeNull] string value
            )
            : this()
        {
            _value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                short value
            )
            : this()
        {
            _value = value.ToInvariantString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                int value
            )
            : this()
        {
            _value = value.ToInvariantString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                long value
            )
            : this()
        {
            _value = value.ToInvariantString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                decimal value
            )
            : this()
        {
            _value = value.ToInvariantString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberOrString
            (
                double value
            )
            : this()
        {
            _value = value.ToVisibleString();
        }

        #endregion

        #region Private members

        private string _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(string value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(short value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(int value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(long value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(decimal value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberOrString(double value)
        {
            return new NumberOrString(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator string(NumberOrString value)
        {
            return value.AsString;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator short(NumberOrString value)
        {
            return value.AsInt16;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator int(NumberOrString value)
        {
            return value.AsInt32;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator long(NumberOrString value)
        {
            return value.AsInt64;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator decimal(NumberOrString value)
        {
            return value.AsDecimal;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator double(NumberOrString value)
        {
            return value.AsDouble;
        }

        /// <summary>
        /// Represent as visible string.
        /// </summary>
         public string ToVisibleString()
        {
            if (ReferenceEquals(_value, null))
            {
                return "(null)";
            }

            if (_value == string.Empty)
            {
                return "(empty)";
            }

            return _value;
        }

        #endregion

        #region IHandmadeSerialization members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            _value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(_value);
        }

        #endregion

        #region IXmlSerializable members

        /// <inheritdoc cref="IXmlSerializable.GetSchema" />
        [ExcludeFromCodeCoverage]
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <inheritdoc cref="IXmlSerializable.ReadXml" />
        void IXmlSerializable.ReadXml
            (
                XmlReader reader
            )
        {
            Code.NotNull(reader, "reader");

#if NETCORE

            throw new NotImplementedException();

#else

            string value = reader.ReadString();
            _value = value == "(null)"
                ? null
                : value;
            reader.ReadEndElement();

#endif
        }

        /// <inheritdoc cref="IXmlSerializable.WriteXml" />
        void IXmlSerializable.WriteXml
            (
                XmlWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            if (ReferenceEquals(_value, null))
            {
                writer.WriteString("(null)");
            }
            else
            {
                writer.WriteString(_value);
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Compare with other <see cref="NumberOrString"/>.
        /// </summary>
        public bool Equals
            (
                NumberOrString other
            )
        {
            return string.Equals(_value, other._value);
        }

        /// <inheritdoc cref="ValueType.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is NumberOrString
                && Equals((NumberOrString) obj);
        }

        /// <inheritdoc cref="ValueType.GetHashCode" />
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode

            return ReferenceEquals(_value, null)
                ? 0
                : _value.GetHashCode();
        }

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return _value;
        }

        #endregion
    }
}
