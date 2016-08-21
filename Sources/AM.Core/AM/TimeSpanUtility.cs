/* TimeSpanUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TimeSpanUtility
    {
        #region Properties

        /// <summary>
        /// One day.
        /// </summary>
        public static TimeSpan OneDay
        {
            get { return new TimeSpan(1, 0, 0, 0); }
        }

        /// <summary>
        /// One hour.
        /// </summary>
        public static TimeSpan OneHour
        {
            get { return new TimeSpan(1, 0, 0);}
        }

        /// <summary>
        /// One minute.
        /// </summary>
        public static TimeSpan OneMinute
        {
            get { return new TimeSpan(0, 1, 0); }
        }

        /// <summary>
        /// One second.
        /// </summary>
        public static TimeSpan OneSecond
        {
            get { return new TimeSpan(0, 0, 1); }
        }

        #endregion

        #region Private members

        private static IFormatProvider _FormatProvider
        {
            get { return CultureInfo.InvariantCulture; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Is zero-length time span?
        /// </summary>
        public static bool IsZero
            (
            this TimeSpan timeSpan
            )
        {
            return TimeSpan.Compare(timeSpan, TimeSpan.Zero) == 0;
        }

        /// <summary>
        /// Is zero-length or less?
        /// </summary>
        public static bool IsZeroOrLess
            (
            this TimeSpan timeSpan
            )
        {
            return TimeSpan.Compare(timeSpan, TimeSpan.Zero) <= 0;
        }

        /// <summary>
        /// Is length of the time span less than zero?
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool LessThenZero
            (
            this TimeSpan timeSpan
            )
        {
            return TimeSpan.Compare(timeSpan, TimeSpan.Zero) < 0;
        }

        /// <summary>
        /// Converts time span to string
        /// automatically selecting format
        /// according duration of the span.
        /// </summary>
        public static string ToAutoString
            (
                this TimeSpan span
            )
        {
            if (span >= OneDay)
            {
                return span.ToDayString();
            }
            if (span >= OneHour)
            {
                return span.ToHourString();
            }
            if (span >= OneMinute)
            {
                return span.ToMinuteString();
            }
            return span.ToSecondString();
        }

        /// <summary>
        /// Converts time span using format 'dd:hh:mm:ss'
        /// </summary>
        public static string ToDayString
            (
                this TimeSpan span
            )
        {
            return string.Format
                (
                    _FormatProvider,
                    "{0:00} d {1:00} h {2:00} m {3:00} s",
                    span.Days,
                    span.Hours,
                    span.Minutes,
                    span.Seconds
                );
        }

        /// <summary>
        /// Converts time span using format 'hh:mm:ss'
        /// </summary>
        public static string ToHourString
            (
                this TimeSpan span
            )
        {
            return string.Format
                (
                    _FormatProvider,
                    "{0:00}:{1:00}:{2:00}",
                    span.Hours + span.Days * 60,
                    span.Minutes,
                    span.Seconds
                );
        }

        /// <summary>
        /// Converts time span using format 'mm:ss'
        /// </summary>
        public static string ToMinuteString
            (
                this TimeSpan span
            )
        {
            double totalMinutes = span.TotalMinutes;
            int minutes = (int)totalMinutes;
            int seconds = (int)((totalMinutes - minutes) * 60.0);

            return string.Format
                (
                    _FormatProvider,
                    "{0:00}:{1:00}",
                    minutes,
                    seconds
                );
        }

        /// <summary>
        /// Converts time span using format 's.ff'
        /// </summary>
        public static string ToSecondString
            (
                this TimeSpan span
            )
        {
            return span.TotalSeconds.ToString
                (
                    "F2",
                    _FormatProvider
                );
        }

        /// <summary>
        /// Converts time span using format 's'
        /// </summary>
        public static string ToWholeSecondsString
            (
                this TimeSpan span
            )
        {
            return span.TotalSeconds.ToString
                (
                    "F0",
                    _FormatProvider
                );
        }

        #endregion
    }
}
