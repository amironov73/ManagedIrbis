// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderUtility.cs -- методы для работы с БД читателей.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Методы для работы с БД читателей.
    /// </summary>
    [PublicAPI]
    public static class ReaderUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Слияние записей о читателях из разных баз.
        /// </summary>
        /// <remarks>
        /// Слияние происходит на основе номера читательского билета.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public static List<ReaderInfo> MergeReaders
            (
                [NotNull][ItemNotNull] List<ReaderInfo> readers
            )
        {
            Code.NotNull(readers, "readers");

            var grouped = readers
                .Where(r => !string.IsNullOrEmpty(r.Ticket))
                .GroupBy(r => r.Ticket);

            List<ReaderInfo> result = new List<ReaderInfo>(readers.Count);

            foreach (var grp in grouped)
            {
                ReaderInfo first = grp.First();
                first.Visits = grp
                    .SelectMany(r => r.Visits)
                    .ToArray();
                result.Add(first);
            }

            return result;
        }

        /// <summary>
        /// Подсчёт количества событий.
        /// </summary>
        public static int CountEvents
            (
                [NotNull][ItemNotNull] List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                bool visit
            )
        {
            Code.NotNull(readers, "readers");

            string fromDayString = IrbisDate.ConvertDateToString(fromDay);
            string toDayString = IrbisDate.ConvertDateToString(toDay);
            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => v.DateGivenString.SafeCompare(fromDayString) >= 0
                    && v.DateGivenString.SafeCompare(toDayString) <= 0
                    && v.IsVisit == visit);

            return result;
        }

        /// <summary>
        /// Подсчёт количества событий
        /// </summary>
        public static int CountEvents
            (
                [NotNull][ItemNotNull] List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                [NotNull] string department,
                bool visit
            )
        {
            Code.NotNull(readers, "readers");
            Code.NotNullNorEmpty(department, "department");

            string fromDayString = IrbisDate.ConvertDateToString(fromDay);
            string toDayString = IrbisDate.ConvertDateToString(toDay);

            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => v.DateGivenString.SafeCompare(fromDayString) >= 0
                    && v.DateGivenString.SafeCompare(toDayString) <= 0
                    && v.Department.SameString(department)
                    && v.IsVisit == visit);

            return result;
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull][ItemNotNull] this List<ReaderInfo> readers
            )
        {
            Code.NotNull(readers, "readers");

            return readers
                .SelectMany(r => r.Visits)
                .ToArray();
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull][ItemNotNull] this VisitInfo[] events,
                [NotNull] string department
            )
        {
            Code.NotNull(events, "events");
            Code.NotNullNorEmpty(department, "department");

            return events

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                .AsParallel()

#endif
                .Where(v => v.Department.SameString(department))
                .ToArray();
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull][ItemNotNull] this VisitInfo[] events,
                bool visit
            )
        {
            Code.NotNull(events, "events");

            return events

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                .AsParallel()

#endif

                .Where(v => v.IsVisit == visit)
                .ToArray();
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull][ItemNotNull] this VisitInfo[] events,
                DateTime day
            )
        {
            Code.NotNull(events, "events");

            string dayString = IrbisDate.ConvertDateToString(day);
            VisitInfo[] result = events

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                .AsParallel()

#endif

                .Where(v => v.DateGivenString.SameString(dayString))
                .ToArray();

            return result;
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull][ItemNotNull] this VisitInfo[] events,
                DateTime fromDay,
                DateTime toDay
            )
        {
            Code.NotNull(events, "events");

            string fromDayString = IrbisDate.ConvertDateToString(fromDay);
            string toDayString = IrbisDate.ConvertDateToString(toDay);
            VisitInfo[] result = events

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                .AsParallel()

#endif

                .Where(v => v.DateGivenString.SafeCompare(fromDayString) >= 0
                            && v.DateGivenString.SafeCompare(toDayString) <= 0)
                .ToArray();

            return result;
        }

#endregion
    }
}
