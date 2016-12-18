// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DebtorManager.cs -- работа с задолжниками
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Работа с задолжниками.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Department}")]
#endif
    public sealed class DebtorManager
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Кафедра обслуживания.
        /// </summary>
        [CanBeNull]
        public string Department { get; set; }

        /// <summary>
        /// С какой даты задолженность?
        /// </summary>
        [CanBeNull]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// По какую дату задолженность.
        /// </summary>
        [CanBeNull]
        public DateTime? ToDate { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DebtorManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Получение списка задолжников.
        /// </summary>
        public DebtorInfo[] GetDebtors()
        {
            ReaderManager manager = new ReaderManager(Connection);
            ReaderInfo[] readers = manager.GetAllReaders("RDR");
            List<DebtorInfo> result = new List<DebtorInfo>(readers.Length);
            string fromDate = null;
            if (FromDate.HasValue)
            {
                fromDate = IrbisDate.ConvertDateToString(FromDate.Value);
            }
            foreach (ReaderInfo reader in readers)
            {
                VisitInfo[] debt = ToDate.HasValue
                    ? reader.Visits.GetDebt(ToDate.Value)
                    : reader.Visits.GetDebt();

                if (FromDate.HasValue)
                {
                    debt = debt.Where
                        (
                            loan => loan.DateExpectedString.SafeCompare(fromDate) >= 0
                        )
                        .ToArray();
                }

                if (!string.IsNullOrEmpty(Department))
                {
                    debt = debt.Where
                        (
                            loan => loan.Department.SameString(Department)
                        )
                        .ToArray();
                }

                if (debt.Length != 0)
                {
                    DebtorInfo debtor = DebtorInfo.FromReader
                        (
                            reader,
                            debt
                        );
                    result.Add(debtor);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        #endregion
    }
}
