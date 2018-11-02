// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FulltextDublin.cs --
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

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Dublin Core для полнотекстовой записи.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FulltextDublin
    {
        #region Properties

        /// <summary>
        /// Заглавие.
        /// Поле 1.
        /// </summary>
        [Field(1)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Создатель (автор).
        /// Поле 2.
        /// </summary>
        [Field(2)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Creator { get; set; }

        /// <summary>
        /// Тема.
        /// Поле 3.
        /// </summary>
        [Field(3)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        /// <summary>
        /// Описание.
        /// Поле 4.
        /// </summary>
        [Field(4)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Издатель.
        /// Поле 5.
        /// </summary>
        [Field(5)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Publisher { get; set; }

        /// <summary>
        /// Внесший вклад.
        /// Поле 6.
        /// </summary>
        [Field(6)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Contributor { get; set; }

        /// <summary>
        /// Дата.
        /// Поле 7.
        /// </summary>
        [Field(7)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Тип.
        /// Поле 8.
        /// </summary>
        [Field(8)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Формат документа.
        /// Поле 9.
        /// </summary>
        [Field(9)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        /// <summary>
        /// Идентификатор.
        /// Поле 10.
        /// </summary>
        [Field(10)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Identifier { get; set; }

        /// <summary>
        /// Источник.
        /// Поле 11.
        /// </summary>
        [Field(11)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        /// <summary>
        /// Язык.
        /// Поле 12.
        /// </summary>
        [Field(12)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        /// <summary>
        /// Отношения.
        /// Поле 13.
        /// </summary>
        [Field(13)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Relation { get; set; }

        /// <summary>
        /// Покрытие
        /// Поле 14.
        /// </summary>
        [Field(14)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Coverage { get; set; }

        /// <summary>
        /// Авторские права.
        /// Поле 15.
        /// </summary>
        [Field(15)]
        [CanBeNull]
        [XmlAttribute("")]
        [JsonProperty("", NullValueHandling = NullValueHandling.Ignore)]
        public string Rights { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        public static FulltextDublin Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            FulltextDublin result = new FulltextDublin
            {
                Title = record.FM(1),
                Creator = record.FM(2),
                Subject = record.FM(3),
                Description = record.FM(4),
                Publisher = record.FM(5),
                Contributor = record.FM(6),
                Date = record.FM(7),
                Type = record.FM(8),
                Format = record.FM(9),
                Identifier = record.FM(10),
                Source = record.FM(11),
                Language = record.FM(12),
                Relation = record.FM(13),
                Coverage = record.FM(14),
                Rights = record.FM(15),
                Record = record
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}