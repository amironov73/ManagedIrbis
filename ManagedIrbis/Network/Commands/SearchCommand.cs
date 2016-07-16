/* SearchCommand.cs -- search records on IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
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
    public sealed class SearchCommand
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
        public string FormatSpecification { get; set; }

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
        public string SearchQuery { get; set; }

        /// <summary>
        /// Specification of sequential search.
        /// </summary>
        [CanBeNull]
        public string SequentialSpecification { get; set; }

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

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.Search;

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery clientQuery
            )
        {
            Code.NotNull(clientQuery, "clientQuery");

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not set");
            }

            clientQuery.Add(database);

            string preparedQuery = IrbisSearchQuery.PrepareQuery
                    (
                        SearchQuery
                    );
            clientQuery.Add
                (
                    new TextWithEncoding
                        (
                            preparedQuery,
                            IrbisEncoding.Utf8
                        )
                );

            clientQuery.Add(NumberOfRecords);
            clientQuery.Add(FirstRecord);

            string preparedFormat = IrbisFormat.PrepareFormat
                (
                    FormatSpecification
                );

            clientQuery.Add
                (
                    new TextWithEncoding
                        (
                            preparedFormat,
                            IrbisEncoding.Ansi
                        )
                );

            IrbisServerResponse result = base.Execute(clientQuery);

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
            verifier
                .NotNullNorEmpty(SearchQuery, "SearchQuery")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
