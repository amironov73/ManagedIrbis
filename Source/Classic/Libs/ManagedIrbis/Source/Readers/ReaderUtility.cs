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
using System.Text;
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
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabaseName = "RDR";

        /// <summary>
        /// Default reader identifier search prefix.
        /// </summary>
        public const string DefaultIdentifierPrefix = "RI=";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public static NonNullValue<string> DatabaseName { get; set; }

        /// <summary>
        /// Reader identifier search prefix.
        /// </summary>
        public static NonNullValue<string> IdentifierPrefix { get; set; }

        #endregion

        #region Construction

        static ReaderUtility()
        {
            DatabaseName = DefaultDatabaseName;
            IdentifierPrefix = DefaultIdentifierPrefix;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fix the reader email.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public static string FixEmail
            (
                [CanBeNull] string email
            )
        {
            if (string.IsNullOrEmpty(email))
            {
                return email;
            }

            email = email.Replace(" ", string.Empty);

            return email;
        }

        /// <summary>
        /// Fix the reader name: remove extra spaces.
        /// </summary>
        [CanBeNull]
        public static string FixName
            (
                [CanBeNull] string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            name = name.Trim();
            name = name.Replace(',', ' ');
            name = name.Replace('.', ' ');
            while (name.Contains("  "))
            {
                name = name.Replace("  ", " ");
            }

            return name;
        }

        /// <summary>
        /// Fix the phone number: remove spaces
        /// and bad characters.
        /// </summary>
        [CanBeNull]
        public static string FixPhone
            (
                [CanBeNull] string phone
            )
        {
            if (string.IsNullOrEmpty(phone))
            {
                return phone;
            }

            phone = phone.Trim();
            if (phone.StartsWith("+7"))
            {
                phone = "8" + phone.Substring(2);
            }

            StringBuilder result = new StringBuilder(phone.Length);
            foreach (char c in phone)
            {
                if (c.IsArabicDigit())
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Fix the ticket number: remove spaces,
        /// convert cyrillic characters to latin equivalents.
        /// </summary>
        [CanBeNull]
        public static string FixTicket
            (
                [CanBeNull] string ticket
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return ticket;
            }

            StringBuilder result = new StringBuilder(ticket.Length);
            foreach (char c in ticket)
            {
                if (c <= ' ')
                {
                    continue;
                }

                switch (c)
                {
                    case 'А':
                    case 'а':
                        result.Append('A');
                        break;

                    case 'В':
                    case 'в':
                        result.Append('B');
                        break;

                    case 'Е':
                    case 'е':
                        result.Append('E');
                        break;

                    case 'О':
                    case 'о':
                        result.Append('0');
                        break;

                    case 'С':
                    case 'с':
                        result.Append('C');
                        break;

                    default:
                        if (c < (int) 256)
                        {
                            result.Append(char.ToUpper(c));
                        }
                        break;
                }
            }

            return result.ToString();
        }

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

#if !WINMOBILE && !PocketPC

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

#if !WINMOBILE && !PocketPC

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

#if !WINMOBILE && !PocketPC

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

#if !WINMOBILE && !PocketPC

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
