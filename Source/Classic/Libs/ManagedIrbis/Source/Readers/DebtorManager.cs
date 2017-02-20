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

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
        #region Events

        /// <summary>
        /// Fired on batch read.
        /// </summary>
        public event EventHandler BatchRead;

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

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

            Database = "RDR";
            Connection = connection;
        }

        #endregion

        #region Private members

        private string _fromDate, _toDate;

        private void HandleBatchRead
            (
                object sender,
                EventArgs eventArgs
            )
        {
            BatchRead.Raise(sender, eventArgs);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get debt from the reader
        /// </summary>
        [NotNull]
        public VisitInfo[] GetDebt
            (
                [NotNull] ReaderInfo reader
            )
        {
            VisitInfo[] result = reader.Visits.GetDebt
                (
                    _fromDate, 
                    _toDate
                );

            if (!string.IsNullOrEmpty(Department))
            {
                result = result.Where
                    (
                        item => item.Department.SameString
                            (
                                Department
                            )
                    )
                    .ToArray();
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get debtor from the reader.
        /// </summary>
        /// <returns><c>null</c> if reader is not debtor.
        /// </returns>
        [CanBeNull]
        public DebtorInfo GetDebtor
            (
                [NotNull] ReaderInfo reader
            )
        {
            VisitInfo[] debt = GetDebt(reader);

            if (debt.Length == 0)
            {
                return null;
            }

            DebtorInfo result = DebtorInfo.FromReader
                (
                    reader,
                    debt
                );

            return result;
        }

        /// <summary>
        /// Получение списка задолжников.
        /// </summary>
        [NotNull]
        public DebtorInfo[] GetDebtors()
        {
            SetupDates();

            ReaderManager manager = new ReaderManager(Connection);
            manager.BatchRead += HandleBatchRead;

            string database = Database.ThrowIfNull("Database");
            ReaderInfo[] readers = manager.GetAllReaders(database);
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

                if (!string.IsNullOrEmpty(fromDate))
                {
                    debt = debt.Where
                        (
                            loan => loan.DateExpectedString
                                .SafeCompare(fromDate) >= 0
                        )
                        .ToArray();
                }

                if (!string.IsNullOrEmpty(Department))
                {
                    debt = debt.Where
                        (
                            loan => loan.Department.SameString
                                (
                                    Department
                                )
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

            manager.BatchRead -= HandleBatchRead;

            return result.ToArray();
        }

        /// <summary>
        /// Получение списка задолжников.
        /// </summary>
        [NotNull]
        public DebtorInfo[] GetDebtors
            (
                [NotNull] IEnumerable<int> mfns
            )
        {
            Code.NotNull(mfns, "mfns");

            SetupDates();

            ReaderManager manager = new ReaderManager(Connection);
            manager.BatchRead += HandleBatchRead;

            string database = Database.ThrowIfNull("Database");
            ReaderInfo[] readers = manager.GetReaders(database, mfns);
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

                if (!string.IsNullOrEmpty(fromDate))
                {
                    debt = debt.Where
                        (
                            loan => loan.DateExpectedString
                                .SafeCompare(fromDate) >= 0
                        )
                        .ToArray();
                }

                if (!string.IsNullOrEmpty(Department))
                {
                    debt = debt.Where
                        (
                            loan => loan.Department.SameString
                                (
                                    Department
                                )
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

            manager.BatchRead -= HandleBatchRead;

            return result.ToArray();
        }

        /// <summary>
        /// Setup <see cref="FromDate"/> and
        /// <see cref="ToDate"/>.
        /// </summary>
        /// <returns><c>true</c> on success,
        /// <c>false</c> otherwise.</returns>
        public bool SetupDates()
        {
            if (!FromDate.HasValue
                || !ToDate.HasValue)
            {
                return false;
            }

            _fromDate = IrbisDate.ConvertDateToString(FromDate.Value);
            _toDate = IrbisDate.ConvertDateToString(ToDate.Value);

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
