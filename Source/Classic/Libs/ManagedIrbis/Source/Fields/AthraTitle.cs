// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthraTitle.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Заголовок записи в базе данных ATHRA.
    /// Поле 210.
    /// </summary>
    [PublicAPI]
    [XmlRoot("title")]
    [MoonSharpUserData]
    public sealed class AthraTitle
    {
        #region Properties

        /// <summary>
        /// Начальный элемент ввода (фамилия или имя).
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("surname")]
        [JsonProperty("surname", NullValueHandling = NullValueHandling.Ignore)]
        public string Surname { get; set; }

        /// <summary>
        /// Инициалы.
        /// Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("initials")]
        [JsonProperty("intials", NullValueHandling = NullValueHandling.Ignore)]
        public string Initials { get; set; }

        /// <summary>
        /// Расширение инициалов.
        /// Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("extension")]
        [JsonProperty("extension", NullValueHandling = NullValueHandling.Ignore)]
        public string Extension { get; set; }

        /// <summary>
        /// Роль (инвертирование ФИО допустимо?).
        /// Подполе &lt;.
        /// </summary>
        [CanBeNull]
        [SubField('<')]
        [XmlElement("role")]
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (выводится в скобках).
        /// Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlElement("integral")]
        [JsonProperty("integral", NullValueHandling = NullValueHandling.Ignore)]
        public string IntegralPart { get; set; }

        /// <summary>
        /// Идентифицирующие признаки имени.
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("identifying")]
        [JsonProperty("identifying", NullValueHandling = NullValueHandling.Ignore)]
        public string IdentifyingSigns { get; set; }

        /// <summary>
        /// Римские цифры.
        /// Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("roman")]
        [JsonProperty("roman", NullValueHandling = NullValueHandling.Ignore)]
        public string RomanNumerals { get; set; }

        /// <summary>
        /// Даты.
        /// Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("dates")]
        [JsonProperty("dates", NullValueHandling = NullValueHandling.Ignore)]
        public string Dates { get; set; }

        /// <summary>
        /// Требуется редактирование.
        /// Подполе !.
        /// </summary>
        [CanBeNull]
        [SubField('!')]
        [XmlElement("correction")]
        [JsonProperty("correction", NullValueHandling = NullValueHandling.Ignore)]
        public string CorrectionNeeded { get; set; }

        /// <summary>
        /// Графика.
        /// Подполе 7.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlElement("graphics")]
        [JsonProperty("graphics", NullValueHandling = NullValueHandling.Ignore)]
        public string Graphics { get; set; }

        /// <summary>
        /// Язык заголовка.
        /// Подполе 8.
        /// </summary>
        [CanBeNull]
        [SubField('8')]
        [XmlElement("language")]
        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        /// <summary>
        /// Признак ввода имени лица.
        /// Подполе 9.
        /// </summary>
        [CanBeNull]
        [SubField('9')]
        [XmlElement("mark")]
        [JsonProperty("mark", NullValueHandling = NullValueHandling.Ignore)]
        public string Mark { get; set; }

        /// <summary>
        /// Код отношения.
        /// Подполе 4.
        /// </summary>
        [CanBeNull]
        [SubField('4')]
        [XmlElement("relation")]
        [JsonProperty("relation", NullValueHandling = NullValueHandling.Ignore)]
        public string RelationCode { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ApplyTo
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field.ApplySubField('a', Surname)
                .ApplySubField('b', Initials)
                .ApplySubField('g', Extension)
                .ApplySubField('<', Role)
                .ApplySubField('1', IntegralPart)
                .ApplySubField('c', IdentifyingSigns)
                .ApplySubField('d', RomanNumerals)
                .ApplySubField('f', Dates)
                .ApplySubField('!', CorrectionNeeded)
                .ApplySubField('7', Graphics)
                .ApplySubField('8', Language)
                .ApplySubField('9', Mark)
                .ApplySubField('4', RelationCode);

            return field;
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static AthraTitle Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            AthraTitle result = new AthraTitle
            {
                Surname = field.GetFirstSubFieldValue('a'),
                Initials = field.GetFirstSubFieldValue('b'),
                Extension = field.GetFirstSubFieldValue('g'),
                Role = field.GetFirstSubFieldValue('<'),
                IntegralPart = field.GetFirstSubFieldValue('1'),
                IdentifyingSigns = field.GetFirstSubFieldValue('c'),
                RomanNumerals = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                CorrectionNeeded = field.GetFirstSubFieldValue('!'),
                Graphics = field.GetFirstSubFieldValue('7'),
                Language = field.GetFirstSubFieldValue('8'),
                Mark = field.GetFirstSubFieldValue('9'),
                RelationCode = field.GetFirstSubFieldValue('4'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(210)
                .AddNonEmptySubField('a', Surname)
                .AddNonEmptySubField('b', Initials)
                .AddNonEmptySubField('g', Extension)
                .AddNonEmptySubField('<', Role)
                .AddNonEmptySubField('1', IntegralPart)
                .AddNonEmptySubField('c', IdentifyingSigns)
                .AddNonEmptySubField('d', RomanNumerals)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('!', CorrectionNeeded)
                .AddNonEmptySubField('7', Graphics)
                .AddNonEmptySubField('8', Language)
                .AddNonEmptySubField('9', Mark)
                .AddNonEmptySubField('4', RelationCode);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Surname.ToVisibleString();
        }

        #endregion
    }
}