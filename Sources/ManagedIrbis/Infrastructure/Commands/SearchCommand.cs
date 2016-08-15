/* SearchCommand.cs -- search records on IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    //
    // db_name – имя базы данных
    // search_exp – поисковое выражение на языке ISIS
    // num_records – число возвращаемых записей,
    // если параметр 0, то возвращаются
    // MAX_POSTINGS_IN_PACKET записей.
    // first_record – номер первой возвращаемой записи
    // в общем списке найденных записей если параметр
    // 0 – возвращается только количество найденных записей.
    // BRIEF – формат для форматирования найденных записей
    // format – есть 4 варианта определить формат:
    // 1-й вариант  – строка формата;
    // 2-й вариант – имя файла формата расположенного
    // на сервере по 10 пути для базы данных db_name,
    // предваряемого символом @ (например, @brief);
    // 3-й вариант – символ @ - в этом случае производится
    // ОПТИМИЗИРОВАННОЕ форматирование,
    // имя формата определяется видом записи;
    // 4-й вариант – пустая строка. В этом случае
    // форматирование не производится.
    //
    // ВОЗВРАТ
    // Список строк.
    // В 1-й строке – код возврата, который определяется
    // общим результатом выполнения команды
    // – ZERO успешно, если нет – число меньше 0.
    // Если команда выполнена успешно, далее идут строки
    // в следующем виде:
    // 2-я строка – число найденных записей
    // Далее идет список строк:
    // MFN# результат_форматирования
    //

    /// <summary>
    /// Search records on IRBIS-server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SearchCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// First record offset.
        /// </summary>
        public int FirstRecord { get; set; }

        /// <summary>
        /// Format specification.
        /// </summary>
        [CanBeNull]
        public virtual string FormatSpecification { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search query expression.
        /// </summary>
        [CanBeNull]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Specification of sequential search.
        /// </summary>
        [CanBeNull]
        public string SequentialSpecification { get; set; }

        /// <summary>
        /// Found records.
        /// </summary>
        [CanBeNull]
        public List<FoundItem> Found { get; set; }

        /// <summary>
        /// Count of found records.
        /// </summary>
        public int FoundCount { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            FirstRecord = 1;
        }

        #endregion

        #region Private members

        private bool _SubCommand;

        private void _FetchRemaining
            (
                ServerResponse mainResponse,
                int expected
            )
        {
            if (ReferenceEquals(Found, null))
            {
                return;
            }

            if (!_SubCommand && (expected > IrbisConstants.MaxPostings))
            {
                int firstRecord = FirstRecord + Found.Count;

                while (firstRecord < expected)
                {
                    SearchCommand subCommand = Clone();
                    subCommand.FirstRecord = firstRecord;
                    subCommand.NumberOfRecords =
                        Math.Min
                        (
                            expected - firstRecord + 1,
                            IrbisConstants.MaxPostings
                        );
                    subCommand._SubCommand = true;

                    ClientQuery clientQuery = subCommand.CreateQuery();
                    ServerResponse subResponse = subCommand
                        .Execute(clientQuery);
                    subCommand.CheckResponse(subResponse);

                    List<FoundItem> found = subCommand.Found
                        .ThrowIfNull("Found");
                    int count = found.Count;
                    Found.AddRange(found);

                    Debug.Assert
                        (
                            count != 0,
                            "Found.Count == 0 in SubCommand"
                        );

                    firstRecord += count;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the command.
        /// </summary>
        public SearchCommand Clone()
        {
            SearchCommand result = new SearchCommand
                (
                    Connection
                )
            {
                Database = Database,
                FirstRecord = FirstRecord,
                FormatSpecification = FormatSpecification,
                MaxMfn = MaxMfn,
                MinMfn = MinMfn,
                NumberOfRecords = NumberOfRecords,
                SearchExpression = SearchExpression,
                SequentialSpecification = SequentialSpecification
            };

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.Search;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not set");
            }

            result.Add(database);

            string preparedQuery = IrbisSearchQuery.PrepareQuery
                    (
                        SearchExpression
                    );
            result.AddUtf8(preparedQuery);

            result.Add(NumberOfRecords);
            result.Add(FirstRecord);

            string preparedFormat = IrbisFormat.PrepareFormat
                (
                    FormatSpecification
                );

            result.AddAnsi(preparedFormat);

            if (!string.IsNullOrEmpty(SequentialSpecification))
            {
                result.Add(MinMfn);
                result.Add(MaxMfn);

                string preparedSequential = IrbisFormat.PrepareFormat
                        (
                            SequentialSpecification
                        );
                if (
                    !string.IsNullOrEmpty(preparedSequential))
                {
                    if (!preparedSequential.StartsWith("!"))
                    {
                        preparedSequential = "!if "
                            + preparedSequential
                            + " then '1' else '0'";
                    }

                    result.AddUtf8(preparedSequential);
                }
            }

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery clientQuery
            )
        {
            Code.NotNull(clientQuery, "clientQuery");

            ServerResponse result = base.Execute(clientQuery);
            result.GetReturnCode();
            if (result.ReturnCode == 0)
            {
                int expected = result.RequireInt32();
                FoundCount = expected;
                Found = FoundItem.ParseServerResponse(result, expected)
                    .ThrowIfNull("Found");

                _FetchRemaining(result, expected);

                if (!_SubCommand
                    && (FirstRecord == 1)
                    && (NumberOfRecords == 0))
                {
                    Debug.Assert 
                        (
                            Found.Count == expected,
                            "Found.Count != expected in total"
                        );
                }
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<SearchCommand> verifier
                = new Verifier<SearchCommand>(this, throwOnError);

            if (!string.IsNullOrEmpty(SequentialSpecification))
            {
                verifier
                    .NotNullNorEmpty(SearchExpression, "SearchQuery");
            }

            verifier.
                Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
