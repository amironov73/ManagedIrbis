// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GlobalCounter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    //
    // Для хранения глобальных счетчиков служит специальная база
    // данных с именем COUNT (зарезервированное имя).
    // Каждая запись базы данных служит для хранения и описания
    // одного глобального счетчика и содержит три обязательных поля:
    //
    // * Индекс глобального счетчика - уникальный идентификатор
    // счетчика (в простейшем случае - номер); метка поля - 1;
    //
    // * Текущее значение глобального счетчика; метка поля - 2;
    //
    // * Шаблон глобального счетчика - определяет структуру (маску)
    // счетчика; метка поля - 3.
    //
    // Шаблон глобального счетчика в общем случае может содержать
    // три части:
    //
    // * Префиксная часть - любой набор символов (кроме символа *),
    // в частном случае может отсутствовать;
    //
    // * Числовая часть - обязательная, обозначается символами *;
    //
    // * Суффиксная часть - любой набор символов (кроме символа *),
    // в частном случае может отсутствовать.
    //
    // Если числовая часть счетчика не имеет фиксированной длины
    // (т.е. не имеет лидирующих нулей), то она обозначается одним
    // символом *. Если числовая часть имеет фиксированную длину
    // (с лидирующими нулями), то она обозначается соответствующим
    // количеством символов *.

    /// <summary>
    /// Экземпляр глобального счетчика.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("counter")]
    public sealed class GlobalCounter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Индекс глобального счетчика. Поле 1.
        /// </summary>
        [CanBeNull]
        [Field("1")]
        [XmlAttribute("index")]
        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }

        /// <summary>
        /// Текущее значение глобального счетчика. Поле 2.
        /// </summary>
        [CanBeNull]
        [Field("2")]
        [XmlAttribute("value")]
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Numeric value of the <see cref="Value"/> property.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int NumericValue
        {
            get { return Value.SafeToInt32(); }
            set { Value = value.ToInvariantString(); }
        }

        /// <summary>
        /// Шаблон глобального счетчика. Поле 3.
        /// </summary>
        [CanBeNull]
        [Field("3")]
        [XmlAttribute("template")]
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public string Template { get; set; }

        /// <summary>
        /// Associated record.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the counter to the <see cref="MarcRecord"/>.
        /// </summary>
        public void ApplyTo
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            record.Fields
                .ApplyFieldValue(1, Index)
                .ApplyFieldValue(2, Value)
                .ApplyFieldValue(3, Template);
        }

        /// <summary>
        /// Increment the counter value.
        /// </summary>
        [NotNull]
        public GlobalCounter Increment()
        {
            int value = Value.SafeToInt32();
            value++;
            Value = value.ToInvariantString();

            return this;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        public static GlobalCounter Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            GlobalCounter result = new GlobalCounter
            {
                Index = record.FM(1),
                Value = record.FM(2),
                Template = record.FM(3),
                Record = record
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Index"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIndex()
        {
            return !string.IsNullOrEmpty(Index);
        }

        /// <summary>
        /// Should serialize the <see cref="Template"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTemplate()
        {
            return !string.IsNullOrEmpty(Template);
        }

        /// <summary>
        /// Should serialize the <see cref="Value"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeValue()
        {
            return !string.IsNullOrEmpty(Value);
        }

        /// <summary>
        /// Convert the counter back to the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            MarcRecord result = new MarcRecord()
                .AddNonEmptyField(1, Index)
                .AddNonEmptyField(2, Value)
                .AddNonEmptyField(3, Template);

            return result;
        }

        /// <summary>
        /// Returns text representation of the value.
        /// </summary>
        [NotNull]
        public string ToText()
        {
            int value = Value.SafeToInt32();
            string template = Template ?? string.Empty;
            TextNavigator navigator = new TextNavigator(template);
            string prefix = navigator.ReadUntil("*");
            string stars = navigator.ReadWhile('*') ?? string.Empty;
            int width = stars.Length;
            string suffix = navigator.GetRemainingText();

            string result = prefix
                + value.ToInvariantString().PadLeft(width, '0')
                + suffix;

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Index = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Template = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Index)
                .WriteNullable(Value)
                .WriteNullable(Template);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<GlobalCounter> verifier
                = new Verifier<GlobalCounter>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Index, "Index")
                .NotNullNorEmpty(Value, "Value")
                .NotNullNorEmpty(Template, "Template");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Index.ToVisibleString() + ":" + Value.ToVisibleString();
        }

        #endregion
    }
}
