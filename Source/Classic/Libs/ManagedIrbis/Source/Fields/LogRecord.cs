// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LogRecord.cs -- record in the LOGDB database
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
    /// Record in the LOGDB database.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LogRecord
    {
        #region Properties

        /// <summary>
        /// Число найденных записей.
        /// Поле 1001.
        /// </summary>
        [CanBeNull]
        [Field(1001)]
        [XmlAttribute("foundCount")]
        [JsonProperty("foundCount", NullValueHandling = NullValueHandling.Ignore)]
        public string FoundCount { get; set; }

        /// <summary>
        /// Время запроса.
        /// Поле 907.
        /// </summary>
        [CanBeNull]
        [Field(907)]
        [XmlAttribute("moment")]
        [JsonProperty("moment", NullValueHandling = NullValueHandling.Ignore)]
        public string Moment { get; set; }

        /// <summary>
        /// Идентификатор читателя.
        /// Поле 1002.
        /// </summary>
        [CanBeNull]
        [Field(1002)]
        [XmlAttribute("ticket")]
        [JsonProperty("ticket", NullValueHandling = NullValueHandling.Ignore)]
        public string Ticket { get; set; }

        /// <summary>
        /// IP-адрес текущего запроса.
        /// Поле 1100.
        /// </summary>
        [CanBeNull]
        [Field(1100)]
        [XmlAttribute("ipAddress")]
        [JsonProperty("ipAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }

        /// <summary>
        /// Профиль базы данных.
        /// Поле 2221.
        /// </summary>
        [CanBeNull]
        [Field(2221)]
        [XmlAttribute("databaseProfile")]
        [JsonProperty("databaseProfile", NullValueHandling = NullValueHandling.Ignore)]
        public string DatabaseProfile { get; set; }

        /// <summary>
        /// Команда шлюза.
        /// Поле 2222.
        /// </summary>
        [CanBeNull]
        [Field(2222)]
        [XmlAttribute("gatewayCommand")]
        [JsonProperty("gatewayCommand", NullValueHandling = NullValueHandling.Ignore)]
        public string GatewayCommand { get; set; }

        /// <summary>
        /// Стартовый номер.
        /// Поле 2223.
        /// </summary>
        [CanBeNull]
        [Field(2223)]
        [XmlAttribute("startNumber")]
        [JsonProperty("startNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string StartNumber { get; set; }

        /// <summary>
        /// Порция.
        /// Поле 2224.
        /// </summary>
        [CanBeNull]
        [Field(2224)]
        [XmlAttribute("portion")]
        [JsonProperty("portion", NullValueHandling = NullValueHandling.Ignore)]
        public string Portion { get; set; }

        /// <summary>
        /// Зашифрованный идентификатор читателя.
        /// Поле 2225.
        /// </summary>
        [CanBeNull]
        [Field(2225)]
        [XmlAttribute("encodedId")]
        [JsonProperty("encodedId", NullValueHandling = NullValueHandling.Ignore)]
        public string EncodedId { get; set; }

        /// <summary>
        /// Поисковый запрос.
        /// Поле 2226.
        /// </summary>
        [CanBeNull]
        [Field(2226)]
        [XmlAttribute("searchQuery")]
        [JsonProperty("searchQuery", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Слова для раскраски.
        /// Поле 2227.
        /// </summary>
        [CanBeNull]
        [Field(2227)]
        [XmlAttribute("words")]
        [JsonProperty("words", NullValueHandling = NullValueHandling.Ignore)]
        public string Words { get; set; }

        /// <summary>
        /// Префикс словаря.
        /// Поле 2228.
        /// </summary>
        [CanBeNull]
        [Field(2228)]
        [XmlAttribute("prefix")]
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public string Prefix { get; set; }

        /// <summary>
        /// Ключ словаря.
        /// Поле 2229.
        /// </summary>
        [CanBeNull]
        [Field(2229)]
        [XmlAttribute("key")]
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// Порядок сортировки.
        /// Поле 1007.
        /// </summary>
        [CanBeNull]
        [Field(1007)]
        [XmlAttribute("sortOrder")]
        [JsonProperty("sortOrder", NullValueHandling = NullValueHandling.Ignore)]
        public string SortOrder { get; set; }

        /// <summary>
        /// Вид сортировки.
        /// Поле 1008.
        /// </summary>
        [CanBeNull]
        [Field(1008)]
        [XmlAttribute("sortKind")]
        [JsonProperty("sortKind", NullValueHandling = NullValueHandling.Ignore)]
        public string SortKind { get; set; }

        /// <summary>
        /// Формат показа найденных.
        /// Поле 1009.
        /// </summary>
        [CanBeNull]
        [Field(1009)]
        [XmlAttribute("format")]
        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        /// <summary>
        /// Число ссылок далее.
        /// Поле 1010.
        /// </summary>
        [CanBeNull]
        [Field(1010)]
        [XmlAttribute("linkCount")]
        [JsonProperty("linkCount", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkCount { get; set; }

        /// <summary>
        /// MFN заказа.
        /// Поле 1011.
        /// </summary>
        [CanBeNull]
        [Field(1011)]
        [XmlAttribute("requestMfn")]
        [JsonProperty("requestMfn", NullValueHandling = NullValueHandling.Ignore)]
        public string RequestMfn { get; set; }

        /// <summary>
        /// Порция словаря.
        /// Поле 1012.
        /// </summary>
        [CanBeNull]
        [Field(1012)]
        [XmlAttribute("dictionaryPortion")]
        [JsonProperty("dictionaryPortion", NullValueHandling = NullValueHandling.Ignore)]
        public string DictionaryPortion { get; set; }

        /// <summary>
        /// Последовательный поиск.
        /// Поле 1013.
        /// </summary>
        [CanBeNull]
        [Field(1013)]
        [XmlAttribute("sequentialSearch")]
        [JsonProperty("sequentialSearch", NullValueHandling = NullValueHandling.Ignore)]
        public string SequentialSearch { get; set; }

        /// <summary>
        /// Имя текущей базы данных.
        /// Поле 3331.
        /// </summary>
        [CanBeNull]
        [Field(3331)]
        [XmlAttribute("databaseName")]
        [JsonProperty("databaseName", NullValueHandling = NullValueHandling.Ignore)]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Место выдачи заказа.
        /// Поле 3334.
        /// </summary>
        [CanBeNull]
        [Field(3334)]
        [XmlAttribute("department")]
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

        /// <summary>
        /// Полнотекстовый запрос.
        /// Поле 3335.
        /// </summary>
        [CanBeNull]
        [Field(3335)]
        [XmlAttribute("fulltextQuery")]
        [JsonProperty("fulltextQuery", NullValueHandling = NullValueHandling.Ignore)]
        public string FulltextQuery { get; set; }

        /// <summary>
        /// Полнотекстовый префикс.
        /// Поле 3337.
        /// </summary>
        [CanBeNull]
        [Field(3337)]
        [XmlAttribute("fulltextPrefix")]
        [JsonProperty("filltextPrefix", NullValueHandling = NullValueHandling.Ignore)]
        public string FulltextPrefix { get; set; }

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
        /// Apply to the record.
        /// </summary>
        public void ApplyTo
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            record
                .ApplyField(1001, FoundCount)
                .ApplyField(907, Moment)
                .ApplyField(1002, Ticket)
                .ApplyField(1100, IpAddress)
                .ApplyField(2221, DatabaseProfile)
                .ApplyField(2222, GatewayCommand)
                .ApplyField(2223, StartNumber)
                .ApplyField(2224, Portion)
                .ApplyField(2225, EncodedId)
                .ApplyField(2226, SearchQuery)
                .ApplyField(2227, Words)
                .ApplyField(2228, Prefix)
                .ApplyField(2229, Key)
                .ApplyField(1007, SortOrder)
                .ApplyField(1008, SortKind)
                .ApplyField(1009, Format)
                .ApplyField(1010, LinkCount)
                .ApplyField(1011, RequestMfn)
                .ApplyField(1012, DictionaryPortion)
                .ApplyField(1013, SequentialSearch)
                .ApplyField(3331, DatabaseName)
                .ApplyField(3334, Department)
                .ApplyField(3335, FulltextQuery)
                .ApplyField(3337, FulltextPrefix);
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static LogRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            LogRecord result = new LogRecord
            {
                FoundCount = record.FM(1001),
                Moment = record.FM(907),
                Ticket = record.FM(1002),
                IpAddress = record.FM(1100),
                DatabaseProfile = record.FM(2221),
                GatewayCommand = record.FM(2222),
                StartNumber = record.FM(2223),
                Portion = record.FM(2224),
                EncodedId = record.FM(2225),
                SearchQuery = record.FM(2226),
                Words = record.FM(2227),
                Prefix = record.FM(2228),
                Key = record.FM(2229),
                SortOrder = record.FM(1007),
                SortKind = record.FM(1008),
                Format = record.FM(1009),
                LinkCount = record.FM(1010),
                RequestMfn = record.FM(1011),
                DictionaryPortion = record.FM(1012),
                SequentialSearch = record.FM(1013),
                DatabaseName = record.FM(3331),
                Department = record.FM(3334),
                FulltextQuery = record.FM(3335),
                FulltextPrefix = record.FM(3337),
                Record = record
            };

            return result;
        }

        /// <summary>
        /// Convert to the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            MarcRecord result = new MarcRecord()
                .AddNonEmptyField(1001, FoundCount)
                .AddNonEmptyField(907, Moment)
                .AddNonEmptyField(1002, Ticket)
                .AddNonEmptyField(1100, IpAddress)
                .AddNonEmptyField(2221, DatabaseProfile)
                .AddNonEmptyField(2222, GatewayCommand)
                .AddNonEmptyField(2223, StartNumber)
                .AddNonEmptyField(2224, Portion)
                .AddNonEmptyField(2225, EncodedId)
                .AddNonEmptyField(2226, SearchQuery)
                .AddNonEmptyField(2227, Words)
                .AddNonEmptyField(2228, Prefix)
                .AddNonEmptyField(2229, Key)
                .AddNonEmptyField(1007, SortOrder)
                .AddNonEmptyField(1008, SortKind)
                .AddNonEmptyField(1009, Format)
                .AddNonEmptyField(1010, LinkCount)
                .AddNonEmptyField(1011, RequestMfn)
                .AddNonEmptyField(1012, DictionaryPortion)
                .AddNonEmptyField(1013, SequentialSearch)
                .AddNonEmptyField(3331, DatabaseName)
                .AddNonEmptyField(3334, Department)
                .AddNonEmptyField(3335, FulltextQuery)
                .AddNonEmptyField(3337, FulltextPrefix);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return SearchQuery.ToVisibleString();
        }

        #endregion
    }
}
