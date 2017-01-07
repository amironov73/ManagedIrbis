// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VisitInfoUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Utility routines for <see cref="VisitInfo"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class VisitInfoUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Flatten visits from given readers.
        /// </summary>
        [NotNull]
        public static VisitInfo[] AllVisits
            (
                [NotNull] this IEnumerable<ReaderInfo> readers
            )
        {
            Code.NotNull(readers, "readers");

            VisitInfo[] result = readers.SelectMany
                (
                    reader => reader.Visits
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for all dates.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetDebt
            (
                [NotNull] this IEnumerable<VisitInfo> visits
            )
        {
            Code.NotNull(visits, "visits");


            VisitInfo[] result = visits.Where
                (
                    loan => !loan.IsReturned
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetDebt
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                DateTime deadline
            )
        {
            Code.NotNull(visits, "visits");

            string date = IrbisDate.ConvertDateToString(deadline);

            VisitInfo[] result = visits.Where
                (
                    loan => !loan.IsReturned
                            && date.SafeCompare(loan.DateExpectedString) <= 0
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetDebt
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                string deadline
            )
        {
            Code.NotNull(visits, "visits");

            VisitInfo[] result = visits.Where
                (
                    loan => !loan.IsReturned
                            && deadline.SafeCompare(loan.DateExpectedString) <= 0
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetDebt
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                [NotNull] string fromDeadline,
                [NotNull] string toDeadline
            )
        {
            Code.NotNull(visits, "visits");

            VisitInfo[] result = visits.Where
                (
                    loan =>
                    {
                        string date = loan.DateExpectedString;

                        return !loan.IsVisit
                            && !loan.IsReturned
                            && date.SafeCompare(fromDeadline) >= 0
                            && date.SafeCompare(toDeadline) <= 0;
                    }
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get loans (not pure visits).
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetLoans
            (
                [NotNull] this IEnumerable<VisitInfo> visits
            )
        {
            Code.NotNull(visits, "visits");

            VisitInfo[] result = visits.Where
                (
                    visit => visit.IsVisit
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get pure visits (not loans).
        /// </summary>
        public static VisitInfo[] GetPureVisits
            (
                [NotNull] this IEnumerable<VisitInfo> visits
            )
        {
            Code.NotNull(visits, "visits");

            VisitInfo[] result = visits.Where
                (
                    visit => !visit.IsVisit
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits for given chair.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetVisits
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                [NotNull] ChairInfo chair
            )
        {
            Code.NotNull(visits, "visits");
            Code.NotNull(chair, "chair");

            string code = chair.Code;
            VisitInfo[] result = visits.Where
                (
                    visit => visit.Department.SameString(code)
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits between given dates.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetVisits
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                DateTime dateFrom,
                DateTime dateTo
            )
        {
            Code.NotNull(visits, "visits");

            string date1 = IrbisDate.ConvertDateToString(dateFrom);
            string date2 = IrbisDate.ConvertDateToString(dateTo);
            VisitInfo[] result = visits.Where
                (
                    visit =>
                    {
                        string given = visit.DateGivenString;
                        return given.SafeCompare(date1) >= 0
                               && given.SafeCompare(date2) <= 0;
                    }
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits for given date.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetVisits
            (
                [NotNull] this IEnumerable<VisitInfo> visits,
                DateTime date
            )
        {
            Code.NotNull(visits, "visits");

            string date1 = IrbisDate.ConvertDateToString(date);
            VisitInfo[] result = visits.Where
                (
                    visit => visit.DateGivenString.SafeCompare(date1) == 0
                )
                .ToArray();

            return result;
        }

        #endregion
    }
}
