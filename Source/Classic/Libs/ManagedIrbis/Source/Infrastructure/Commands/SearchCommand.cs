// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchCommand.cs -- search records on IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;

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

    //
    // Поисковое выражение для последовательного поиска
    // как правило имеет вид
    //
    // !(if V910^D='ФКХ' then '1' else '0' fi)
    //
    // где восклицательный знак в начале означает использование
    // кодировки UTF8 (без восклицательного знака -- CP1251).
    //
    // Также в начале строки может стоять звёздочка, означающая
    // требование "для всех повторений поля". В этом случае
    // ИРБИС64-сервер расформатирует запись и
    // то результирующая строка должна быть непустой и состоять
    // только из единиц, чтобы запись считалась удовлетворяющей
    // условию.
    //
    // В отсутствие звёздочки достаточно наличия хотя бы одной
    // единицы в результирующей строке.
    //

    //
    // Если пользователь ввёл поисковое выражение в текстбокс
    // "Свободный поиск", то АРМ автоматически обрамляет его
    // if ... then '1' else '0' fi
    //

    //
    // Если "Свободный поиск" начать с символа |, то автоматически
    // формируется конструкция
    // (if ... then '1' else '0' fi)
    //
    // Если "Свободный поиск" начать с выражения |MMM, где MMM-метка
    // поля, то автоматически формируется конструкция
    // (if p(vMMM) then if ... then '1' else '0' fi fi), '0'
    //

    //
    // Пример поискового выражения, сформированного АРМ "Каталогизатор":
    //
    // *!(if p(v910) then if v910^d='ФКХ' then '1' else '0' fi fi),'0'
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
        /// Use UTF8 for <see cref="FormatSpecification"/>?
        /// </summary>
        public bool UtfFormat { get; set; }

        /// <summary>
        /// Условие должно выполняться для каждого повторения поля.
        /// </summary>
        public bool ForEachRepeat { get; set; }

        /// <summary>
        /// Переписывать поисковое выражение для последовательного поиска
        /// согласно традициям ИРБИС.
        /// </summary>
        public bool RewriteSequential { get; set; }

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
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
            FirstRecord = 1;
        }

        #endregion

        #region Private members

        private bool _subCommand;

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

            if (!_subCommand && expected > IrbisConstants.MaxPostings)
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
                    subCommand._subCommand = true;

                    ClientQuery clientQuery = subCommand.CreateQuery();
                    ServerResponse subResponse = subCommand
                        .Execute(clientQuery);
                    subCommand.CheckResponse(subResponse);

                    List<FoundItem> found = subCommand.Found
                        .ThrowIfNull("Found");
                    int count = found.Count;
                    Found.ThrowIfNull().AddRange(found);
                    if (count == 0)
                    {
                        break;
                    }

                    // FIXME
                    //Debug.Assert
                    //    (
                    //        count != 0,
                    //        "Found.Count == 0 in SubCommand"
                    //    );

                    firstRecord += count;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply <see cref="SearchParameters"/>.
        /// </summary>
        public void ApplyParameters
            (
                [NotNull] SearchParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            Database = parameters.Database;
            FirstRecord = parameters.FirstRecord;
            FormatSpecification = parameters.FormatSpecification;
            MaxMfn = parameters.MaxMfn;
            MinMfn = parameters.MinMfn;
            NumberOfRecords = parameters.NumberOfRecords;
            SearchExpression = parameters.SearchExpression;
            SequentialSpecification = parameters.SequentialSpecification;
            UtfFormat = parameters.UtfFormat;
        }

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
                SequentialSpecification = SequentialSpecification,
                UtfFormat = UtfFormat
            };

            return result;
        }

        /// <summary>
        /// Gather <see cref="SearchParameters"/>
        /// from the record.
        /// </summary>
        [NotNull]
        public SearchParameters GatherParameters()
        {
            SearchParameters result = new SearchParameters
            {
                Database = Database,
                FirstRecord = FirstRecord,
                FormatSpecification = FormatSpecification,
                MaxMfn = MaxMfn,
                MinMfn = MinMfn,
                NumberOfRecords = NumberOfRecords,
                SearchExpression = SearchExpression,
                SequentialSpecification = SequentialSpecification,
                UtfFormat = UtfFormat
            };

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery"/>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.Search;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                Log.Error
                    (
                        "SearchCommand::CreateQuery: "
                        + "database not set"
                    );

                throw new IrbisException("database not set");
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

            result.Add
                (
                    new TextWithEncoding
                        (
                            UtfFormat
                            ? "!" + preparedFormat
                            : preparedFormat,
                            UtfFormat
                            ? IrbisEncoding.Utf8
                            : IrbisEncoding.Ansi
                        )
                );

            if (!string.IsNullOrEmpty(SequentialSpecification))
            {
                result.Add(MinMfn);
                result.Add(MaxMfn);

                string preparedSequential = IrbisFormat.PrepareFormat
                        (
                            SequentialSpecification
                        );
                if (!string.IsNullOrEmpty(preparedSequential))
                {
                    if (!preparedSequential.StartsWith("!if"))
                    {
                        preparedSequential = "!if "
                            + preparedSequential
                            + " then '1' else '0'";
                    }

                    Encoding encoding = preparedSequential.StartsWith("!")
                        ? IrbisEncoding.Utf8
                        : IrbisEncoding.Ansi;

                    result.Add
                        (
                            new TextWithEncoding
                                (
                                    preparedSequential,
                                    encoding
                                )
                        );
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
                List<FoundItem> foundList = FoundItem
                    .ParseServerResponse(result, expected)
                    .ThrowIfNull("Found");
                Found = foundList;

                if (FirstRecord > 0)
                {
                    _FetchRemaining(result, expected);
                }

                if (!_subCommand
                    && FirstRecord == 1
                    && NumberOfRecords == 0)
                {
                    Debug.Assert
                        (
                            foundList.Count == expected,
                            "Found.Count != expected in total"
                        );
                }
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<SearchCommand> verifier
                = new Verifier<SearchCommand>(this, throwOnError);

            //if (!string.IsNullOrEmpty(SequentialSpecification))
            //{
            //    verifier
            //        .NotNullNorEmpty(SearchExpression, "SearchExpression");
            //}

            verifier.
                Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
