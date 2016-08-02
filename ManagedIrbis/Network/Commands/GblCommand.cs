/* GblCommand.cs -- global correction
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    //
    // Исходные данные:
    // Adbn – имя базы данных;
    // Aifupdate - параметр принимает два значения:
    // 1 – актуализировать записи после корректировки;
    // 0 – не актуализировать запись.
    // Asexp – поисковое выражение для прямого поиска на языке ИРБИС
    // Amin,Amax – диапазон MFN, ограничивающий результат
    // последовательного поиска; если в качестве Amin задан
    // 0 – в качестве нижней границы принимается 1; если
    // в качестве Amax задан 0 – в качестве верхней границы
    // принимается максимальный mfn в базе данных;
    // данные параметры учитываются только в том случае,
    // когда задан последовательный поиск (т.е. когда Aseq
    // не пустое выражение);
    // Amfnlist – список номеров (mfn) записей, организованный
    // одним из трех следующих способов:
    // 1) диапазон номеров – в виде трех строк следующей структуры:
    // 0
    // minmfn
    // maxmfn
    // 2) список номеров – в виде набора строк:
    // N
    // mfn1
    // mfn2
    // ...
    // mfnN
    // 3) отрицательный список номеров («кроме  указанных») – в виде
    // набора строк:
    // -N
    // mfn1
    // mfn2
    // ...
    // mfnN
    // Aseq – поисковое выражение для последовательного поиска
    // (представляет собой явный формат, который возвращает одно
    // из двух значений: 0 – документ не соответствует критерию
    // поиска, 1 – соответствует). Если задается выражение для
    // прямого поиска (Asexp) – последовательный поиск ведется
    // по его результату (с учетом Amin и Amax).
    // Agbl – задание на глобальную корректировку, которое может
    // задаваться двумя способами:
    // 1) имя файла задания с предшествующим символом
    // @ (например: @glob);
    // 2) в виде набора строк задания, в котором реальные
    // разделители строк $0D0A заменены на псевдоразделители $3130.
    // Описание структуры задания на глобальную корректировку
    // см. в Общем описании системы.
    //
    // Возвращаемые данные:
    // Код возврата – ZERO или код ошибки;
    // answer – содержит протокол выполнения глобальной корректировки.
    //

    /// <summary>
    /// Global correction.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Server file name for GBL.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Update index?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Apply formal check?
        /// </summary>
        public bool FormalControl { get; set; }

        /// <summary>
        /// Execute autoin.gbl?
        /// </summary>
        public bool AutoIn { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [CanBeNull]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// First record number.
        /// </summary>
        public int FirstRecord { get; set; }

        /// <summary>
        /// Number of records.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        [CanBeNull]
        public int[] MfnList { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        [CanBeNull]
        public GblItem[] Items { get; set; }

        /// <summary>
        /// Protocol.
        /// </summary>
        [CanBeNull]
        public string Protocol { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            FirstRecord = 1;
            FormalControl = false;
            AutoIn = false;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.GlobalCorrection;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisException("database not specified");
            }
            result.AddAnsi(database);

            result.Add(Actualize);

            string items = GblUtility.EncodeGbl
                (
                    Items.ThrowIfNull("Items")
                );
            result.AddUtf8(items);

            string preparedSearch = IrbisSearchQuery.PrepareQuery
                (
                    SearchExpression
                );
            result.AddUtf8(preparedSearch);

            result.Add(FirstRecord);
            result.Add(NumberOfRecords);

            result.Add(string.Empty);

            if (ReferenceEquals(MfnList, null))
            {
                result.Add(0);
                result.Add(MinMfn);
                result.Add(MaxMfn);
            }
            else
            {
                if (MfnList.Length == 0)
                {
                    throw new IrbisException("MfnList.Length == 0");
                }
                result.Add(MfnList.Length);
                foreach (var mfn in MfnList)
                {
                    result.Add(mfn);
                }
            }

            if (!FormalControl)
            {
                result.Add("*");
            }

            if (!AutoIn)
            {
                result.Add("&");
            }

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);
            CheckResponse(result);

            // Palliative
            Protocol = result.RemainingAnsiText();

            return result;
        }

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<GblCommand> verifier = new Verifier<GblCommand>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNull(Items, "Items")
                .Assert(Items.Length > 0, "Items.Length > 0");

            foreach (GblItem item in Items)
            {
                verifier.VerifySubObject(item, "item");
            }

            return verifier.Result;
        }

        #endregion
    }
}
