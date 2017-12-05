// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DateTimeUtility.cs -- set of date/time manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Set of date/time manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class DateTimeUtility
    {
        #region Properties

        /// <summary>
        /// Gets the date of next month first day.
        /// </summary>
        /// <value>Next month first day.</value>
        public static DateTime NextMonth
        {
            [DebuggerStepThrough]
            get
            {
                return ThisMonth.AddMonths(1);
            }
        }

        /// <summary>
        /// Gets the date of next year first day.
        /// </summary>
        /// <value>Next year first day.</value>
        public static DateTime NextYear
        {
            [DebuggerStepThrough]
            get
            {
                return ThisYear.AddYears(1);
            }
        }

        /// <summary>
        /// Gets the date of previous month first day.
        /// </summary>
        /// <value>Previous month first day.</value>
        public static DateTime PreviousMonth
        {
            [DebuggerStepThrough]
            get
            {
                return ThisMonth.AddMonths(-1);
            }
        }

        /// <summary>
        /// Gets the date of previous year first day.
        /// </summary>
        /// <value>Previous year first day.</value>
        public static DateTime PreviousYear
        {
            [DebuggerStepThrough]
            get
            {
                return ThisYear.AddYears(-1);
            }
        }

        /// <summary>
        /// Gets the date of current month first day.
        /// </summary>
        /// <value>Current month first day.</value>
        public static DateTime ThisMonth
        {
            [DebuggerStepThrough]
            get
            {
                DateTime today = DateTime.Today;
                return new DateTime(today.Year, today.Month, 1);
            }
        }

        /// <summary>
        /// Gets the date of current year first day.
        /// </summary>
        /// <value>Current year first day.</value>
        public static DateTime ThisYear
        {
            [DebuggerStepThrough]
            get
            {
                return new DateTime(DateTime.Today.Year, 1, 1);
            }
        }

        /// <summary>
        /// Gets the date for tomorrow.
        /// </summary>
        /// <value>Tomorrow date.</value>
        public static DateTime Tomorrow
        {
            [DebuggerStepThrough]
            get
            {
                return DateTime.Today.AddDays(1.0);
            }
        }

        /// <summary>
        /// Gets the for yesterday.
        /// </summary>
        /// <value>Yesterday date.</value>
        public static DateTime Yesterday
        {
            [DebuggerStepThrough]
            get
            {
                return DateTime.Today.AddDays(-1.0);
            }
        }

        #endregion

        #region Private members

        private static readonly DateTime UnixStart
            = new DateTime(1970, 1, 1);

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public static bool Between
            (
                this DateTime date,
                DateTime start,
                DateTime end
            )
        {
            return date >= start && date <= end;
        }

        /// <summary>
        /// Finds maximal date/time from given ones.
        /// </summary>
        /// <param name="first">First date/time.</param>
        /// <param name="other">Other dates/times.</param>
        /// <returns>Maximum.</returns>
        public static DateTime MaxDate
            (
                DateTime first,
                params DateTime[] other
            )
        {
            DateTime result = first;
            foreach (DateTime time in other)
            {
                if (time > result)
                {
                    result = time;
                }
            }

            return result;
        }

        /// <summary>
        /// Finds minimal date/time from given ones.
        /// </summary>
        /// <param name="first">First date/time.</param>
        /// <param name="other">Other dates/times.</param>
        /// <returns>Minimum.</returns>
        public static DateTime MinDate
            (
                DateTime first,
                params DateTime[] other
            )
        {
            DateTime result = first;
            foreach (DateTime time in other)
            {
                if (time < result)
                {
                    result = time;
                }
            }
            return result;
        }

#if CLASSIC

        // Borrowed from: https://mikearnett.wordpress.com/2011/09/13/c-convert-julian-date/

        /// <summary>
        /// Convert date to Julian calendar.
        /// </summary>
        public static double ToJulianDate
            (
                this DateTime date
            )
        {
            return date.ToOADate() + 2415018.5;
        }

        /// <summary>
        /// Convert to Julian date.
        /// </summary>
        public static long ToJulian
            (
                DateTime dateTime
            )
        {
            int day = dateTime.Day;
            long month = dateTime.Month;
            long year = dateTime.Year;

            if (month < 3)
            {
                month = month + 12;
                year = year - 1;
            }

            return day
                + (153 * month - 457) / 5
                + 365 * year
                + year / 4
                - year / 100
                + year / 400
                + 1721119;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FromJulian
            (
                long julianDate,
                string format
            )
        {
            long L = julianDate + 68569;
            long N = (long)(4 * L / 146097);
            L = L - (long)((146097 * N + 3) / 4);
            long I = (long)(4000 * (L + 1) / 1461001);
            L = L - (long)(1461 * I / 4) + 31;
            long J = (long)(80 * L / 2447);
            int Day = (int)(L - (long)(2447 * J / 80));
            L = (long)(J / 11);
            int Month = (int)(J + 2 - 12 * L);
            int Year = (int)(100 * (N - 49) + I + L);

            // example format "dd/MM/yyyy"
            return new DateTime(Year, Month, Day).ToString(format);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FromJulianDate
            (
                DateTime gregorian
            )
        {
            JulianCalendar calendar = new JulianCalendar();
            var dateInJulian = calendar.ToDateTime
                (
                    gregorian.Year,
                    gregorian.Month,
                    gregorian.Day,
                    gregorian.Hour,
                    gregorian.Minute,
                    gregorian.Second,
                    gregorian.Millisecond
                );

            return dateInJulian.ToString("yyyyMMdd");
        }

#endif

        /// <summary>
        /// Универсальное длинное представление.
        /// </summary>
        [NotNull]
        public static string ToLongUniformString
            (
                this DateTime dateTime
            )
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Универсальное короткое представление.
        /// </summary>
        [NotNull]
        public static string ToShortUniformString
            (
                this DateTime dateTime
            )
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Переводит указанную дату в формат Unix.
        /// </summary>
        public static long ToUnixTime
            (
                this DateTime dateTime
            )
        {
            return (long)(dateTime - UnixStart).TotalSeconds;
        }

        #endregion
    }
}
