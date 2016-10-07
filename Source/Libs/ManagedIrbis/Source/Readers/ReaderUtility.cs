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
using JetBrains.Annotations;

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
        /// Загрузка читателей из базы.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="readers"></param>
        /// <param name="dbName"></param>
        [NotNull]
        [ItemNotNull]
        public static List<ReaderInfo> LoadReaders
            (
                [NotNull] IrbisConnection client,
                [NotNull] List<ReaderInfo> readers,
                [NotNull] string dbName
            )
        {
            throw new NotImplementedException();

#if NOTDEF
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (ReferenceEquals(readers, null))
            {
                throw new ArgumentNullException("readers");
            }
            if (string.IsNullOrEmpty(dbName))
            {
                throw new ArgumentNullException("dbName");
            }

            try
            {
                client.PushDatabase(dbName);
                readers.Capacity += client.GetMaxMfn();

                BatchRecordReader batch = new BatchRecordReader
                    (
                        client,
                        1500
                    );

                Parallel.ForEach
                    (
                        batch,
                        record =>
                        {
                            if (!record.Deleted)
                            {
                                ReaderInfo reader 
                                    = ReaderInfo.Parse(record);
                                lock (readers)
                                {
                                    readers.Add(reader);
                                }
                            }
                        }
                    );
            }
            finally
            {
                client.PopDatabase();
            }

            return readers;
#endif
        }

        /// <summary>
        /// Слияние записей о читателях из разных баз.
        /// </summary>
        /// <remarks>
        /// Слияние происходит на основе читательского билета.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public static List<ReaderInfo> MergeReaders
            (
                [NotNull][ItemNotNull] List<ReaderInfo> readers
            )
        {
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
        /// Загрузка сведений о читателях из нескольких баз.
        /// </summary>
        /// <remarks>
        /// Выполняется слияние сведений на основе
        /// номера читательского билета.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public static List<ReaderInfo> LoadReaders
            (
                [NotNull] IrbisConnection client,
                [NotNull][ItemNotNull] string[] databases
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (ReferenceEquals(databases, null))
            {
                throw new ArgumentNullException("databases");
            }

            List<ReaderInfo> result = new List<ReaderInfo>();

            foreach (string database in databases)
            {
                if (string.IsNullOrEmpty(database))
                {
                    throw new ArgumentNullException("databases");
                }

                LoadReaders
                    (
                        client,
                        result,
                        database
                    );
            }
            if (databases.Length > 1)
            {
                result = MergeReaders(result);
            }

            return result;
        }

        /// <summary>
        /// Подсчёт количества событий.
        /// </summary>
        public static int CountEvents
            (
                [NotNull] List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                bool visit
            )
        {
            if (ReferenceEquals(readers, null))
            {
                throw new ArgumentNullException("readers");
            }

            string fromDayString = IrbisDate.ConvertDateToString(fromDay);
            string toDayString = IrbisDate.ConvertDateToString(toDay);
            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                    && (v.DateGivenString.SafeCompare(toDayString) <= 0)
                    && (v.IsVisit == visit));
            return result;
        }

        /// <summary>
        /// Подсчёт количества событий
        /// </summary>
        public static int CountEvents
            (
                [NotNull] List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                [NotNull] string department,
                bool visit
            )
        {
            if (ReferenceEquals(readers, null))
            {
                throw new ArgumentNullException("readers");
            }
            if (string.IsNullOrEmpty(department))
            {
                throw new ArgumentNullException("department");
            }

            string fromDayString = 
                new IrbisDate(fromDay).Text;
            string toDayString = new IrbisDate(toDay).Text;
            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                    && (v.DateGivenString.SafeCompare(toDayString) <= 0)
                    && v.Department.SameString(department)
                    && (v.IsVisit == visit));
            return result;
        }

        /// <summary>
        /// Отбор событий.
        /// </summary>
        [NotNull]
        public static VisitInfo[] GetEvents
            (
                [NotNull] this List<ReaderInfo> readers
            )
        {
            if (ReferenceEquals(readers, null))
            {
                throw new ArgumentNullException("readers");
            }

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
                [NotNull] this VisitInfo[] events,
                [NotNull] string department
            )
        {
            if (ReferenceEquals(events, null))
            {
                throw new ArgumentNullException("events");
            }
            if (string.IsNullOrEmpty(department))
            {
                throw new ArgumentNullException("department");
            }

            return events

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT

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
                [NotNull] this VisitInfo[] events,
                bool visit
            )
        {
            if (ReferenceEquals(events, null))
            {
                throw new ArgumentNullException("events");
            }

            return events

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT

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
                [NotNull] this VisitInfo[] events,
                DateTime day
            )
        {
            if (ReferenceEquals(events, null))
            {
                throw new ArgumentNullException("events");
            }

            string dayString = IrbisDate.ConvertDateToString(day);
            VisitInfo[] result = events

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT

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
                [NotNull] this VisitInfo[] events,
                DateTime fromDay,
                DateTime toDay
            )
        {
            if (ReferenceEquals(events, null))
            {
                throw new ArgumentNullException("events");
            }

            string fromDayString = IrbisDate.ConvertDateToString(fromDay);
            string toDayString = IrbisDate.ConvertDateToString(toDay);
            VisitInfo[] result = events

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT

                .AsParallel()

#endif

                .Where(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                            && (v.DateGivenString.SafeCompare(toDayString) <= 0))
                .ToArray();
            return result;
        }

#endregion
    }
}
