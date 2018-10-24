// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LogRecord.cs -- запись в базе данных LOGDB.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    /// Запись в базе данных LOGDB.
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
        public string FoundCount { get; set; }

        /// <summary>
        /// Время запроса.
        /// Поле 907.
        /// </summary>
        [CanBeNull]
        [Field(907)]
        public string Moment { get; set; }

        /// <summary>
        /// Идентификатор читателя.
        /// Поле 1002.
        /// </summary>
        [CanBeNull]
        [Field(1002)]
        public string Ticket { get; set; }

        /// <summary>
        /// IP-адрес текущего запроса.
        /// Поле 1100.
        /// </summary>
        [CanBeNull]
        [Field(1100)]
        public string IpAddress { get; set; }

        /// <summary>
        /// Профиль базы данных.
        /// Поле 2221.
        /// </summary>
        [CanBeNull]
        [Field(2221)]
        public string DatabaseProfile { get; set; }

        /// <summary>
        /// Команда шлюза.
        /// Поле 2222.
        /// </summary>
        [CanBeNull]
        [Field(2222)]
        public string GatewayCommand { get; set; }

        /// <summary>
        /// Стартовый номер.
        /// Поле 2223.
        /// </summary>
        [CanBeNull]
        [Field(2223)]
        public string StartNumber { get; set; }

        /// <summary>
        /// Порция.
        /// Поле 2224.
        /// </summary>
        [CanBeNull]
        [Field(2224)]
        public string Portion { get; set; }

        /// <summary>
        /// Зашифрованный идентификатор читателя.
        /// Поле 2225.
        /// </summary>
        [CanBeNull]
        [Field(2225)]
        public string EncodedId { get; set; }

        /// <summary>
        /// Поисковый запрос.
        /// Поле 2226.
        /// </summary>
        [CanBeNull]
        [Field(2226)]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Слова для раскраски.
        /// Поле 2227.
        /// </summary>
        [CanBeNull]
        [Field(2227)]
        public string Words { get; set; }

        /// <summary>
        /// Префикс словаря.
        /// Поле 2228.
        /// </summary>
        [CanBeNull]
        [Field(2228)]
        public string Prefix { get; set; }

        /// <summary>
        /// Ключ словаря.
        /// Поле 2229.
        /// </summary>
        [CanBeNull]
        [Field(2229)]
        public string Key { get; set; }

        /// <summary>
        /// Порядок сортировки.
        /// Поле 1007.
        /// </summary>
        [CanBeNull]
        [Field(1007)]
        public string SortOrder { get; set; }

        /// <summary>
        /// Вид сортировки.
        /// Поле 1008.
        /// </summary>
        [CanBeNull]
        [Field(1008)]
        public string SortKind { get; set; }

        /// <summary>
        /// Формат показа найденных.
        /// Поле 1009.
        /// </summary>
        [CanBeNull]
        [Field(1009)]
        public string Format { get; set; }

        /// <summary>
        /// Число ссылок далее.
        /// Поле 1010.
        /// </summary>
        [CanBeNull]
        [Field(1010)]
        public string LinkCount { get; set; }

        /// <summary>
        /// MFN заказа.
        /// Поле 1011.
        /// </summary>
        [CanBeNull]
        [Field(1011)]
        public string RequestMfn { get; set; }

        /// <summary>
        /// Порция словаря.
        /// Поле 1012.
        /// </summary>
        [CanBeNull]
        [Field(1012)]
        public string DictionaryPortion { get; set; }

        /// <summary>
        /// Последовательный поиск.
        /// Поле 1013.
        /// </summary>
        [CanBeNull]
        [Field(1013)]
        public string SequentialSearch { get; set; }

        /// <summary>
        /// Имя текущей базы данных.
        /// Поле 3331.
        /// </summary>
        [CanBeNull]
        [Field(3331)]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Место выдачи заказа.
        /// Поле 3334.
        /// </summary>
        [CanBeNull]
        [Field(3334)]
        public string Department { get; set; }

        /// <summary>
        /// Полнотекстовый запрос.
        /// Поле 3335.
        /// </summary>
        [CanBeNull]
        [Field(3335)]
        public string FulltextQuery { get; set; }

        /// <summary>
        /// Полнотекстовый префикс.
        /// Поле 3337.
        /// </summary>
        [CanBeNull]
        [Field(3337)]
        public string FulltextPrefix { get; set; }

        #endregion

        #region Public methods

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
                FulltextPrefix = record.FM(3337)
            };

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
