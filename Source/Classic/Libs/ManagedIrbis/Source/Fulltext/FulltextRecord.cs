// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FulltextRecord.cs --
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
    /// Запись во вложенной базе данных TEXT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FulltextRecord
    {
        #region Properties

        /// <summary>
        /// GUID.
        /// </summary>
        /// <remarks>
        /// See <see cref="IrbisGuid"/>.
        /// </remarks>
        [XmlAttribute("guid")]
        [JsonProperty("guid", NullValueHandling = NullValueHandling.Ignore)]
        public string Guid { get; set; }

        /// <summary>
        /// Число слов в тексте.
        /// Поле 20.
        /// </summary>
        [Field(20)]
        [CanBeNull]
        [XmlAttribute("wordCount")]
        [JsonProperty("wordCount", NullValueHandling = NullValueHandling.Ignore)]
        public string WordCount { get; set; }

        /// <summary>
        /// Индекс естественно-тематического рубрикатора.
        /// Поле 21.
        /// </summary>
        [Field(21)]
        [CanBeNull]
        [XmlAttribute("subject")]
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        /// <summary>
        /// Первые строки текста.
        /// Поле 22.
        /// </summary>
        [Field(22)]
        [CanBeNull]
        [XmlAttribute("brief")]
        [JsonProperty("brief", NullValueHandling = NullValueHandling.Ignore)]
        public string BriefText { get; set; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        [Field(23)]
        [CanBeNull]
        [XmlAttribute("remarks")]
        [JsonProperty("remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }

        /// <summary>
        /// Дата ввода записи в базу данных.
        /// Поле 24.
        /// </summary>
        [Field(24)]
        [CanBeNull]
        [XmlAttribute("entryDate")]
        [JsonProperty("entryDate", NullValueHandling = NullValueHandling.Ignore)]
        public string EntryDate { get; set; }

        /// <summary>
        /// Размер файла полного текста в байтах.
        /// Поле 25.
        /// </summary>
        [Field(25)]
        [CanBeNull]
        [XmlAttribute("fileSize")]
        [JsonProperty("fileSize", NullValueHandling = NullValueHandling.Ignore)]
        public string FileSize { get; set; }

        /// <summary>
        /// Дата создания полного текста.
        /// Поле 26.
        /// </summary>
        [Field(26)]
        [CanBeNull]
        [XmlAttribute("fileDate")]
        [JsonProperty("fileDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FileDate { get; set; }

        /// <summary>
        /// Строки текста.
        /// Поле 27.
        /// </summary>
        [Field(27)]
        [CanBeNull]
        [XmlElement("line")]
        [JsonProperty("lines", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Lines { get; set; }

        /// <summary>
        /// Данные о переносе записи из ЭК.
        /// Поле 66.
        /// </summary>
        [Field(66)]
        [CanBeNull]
        [XmlAttribute("transfer")]
        [JsonProperty("transfer", NullValueHandling = NullValueHandling.Ignore)]
        public string Transfer { get; set; }

        /// <summary>
        /// Исходные данные из ЭК.
        /// Поле 951.
        /// </summary>
        [CanBeNull]
        [Field(951)]
        [XmlAttribute("initialData")]
        [JsonProperty("initialData", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialData { get; set; }

        /// <summary>
        /// Ссылка на объект полнотекстового поиска.
        /// Поле 952.
        /// </summary>
        [CanBeNull]
        [Field(952)]
        [XmlElement("reference")]
        [JsonProperty("reference", NullValueHandling = NullValueHandling.Ignore)]
        public FullTextReference TextReference { get; set; }

        /// <summary>
        /// Ссылка на библиографическую запись.
        /// Поле 999.
        /// </summary>
        [Field(999)]
        [CanBeNull]
        [XmlAttribute("refGuid")]
        [JsonProperty("refGuid", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceGuid { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

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
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static FulltextRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            FulltextRecord result = new FulltextRecord
            {
                Guid = record.FM(IrbisGuid.Tag),
                WordCount = record.FM(20),
                Subject = record.FM(21),
                BriefText = record.FM(22),
                Remarks = record.FM(23),
                EntryDate = record.FM(24),
                FileSize = record.FM(25),
                FileDate = record.FM(26),
                Lines = record.FMA(27),
                Transfer = record.FM(66),
                InitialData = record.FM(951),
                TextReference = FullTextReference
                    .Parse(record.Fields.GetFirstField(952)),
                ReferenceGuid = record.FM(999),
                Record = record
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return BriefText.ToVisibleString();
        }

        #endregion
    }
}