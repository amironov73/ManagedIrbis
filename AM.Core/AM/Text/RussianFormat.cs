/* RussianFormat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RussianFormat
    {
        #region Private members

        private static readonly IFormatProvider _formatProvider;

        static RussianFormat()
        {
            _formatProvider = CultureInfo.GetCultureInfo("ru-RU");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Format string.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                [CanBeNull] object arg0
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                [CanBeNull] object arg0,
                [CanBeNull] object arg1
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    arg0,
                    arg1
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                [CanBeNull] object arg0,
                [CanBeNull] object arg1,
                [CanBeNull] object arg2
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    arg0,
                    arg1,
                    arg2
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                params object[] args
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    args
                );

            return result;
        }

        /// <summary>
        /// Format integer.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                int arg0
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format double.
        /// </summary>
        [NotNull]
        public static string Format
            (
                [NotNull] string format,
                double arg0
            )
        {
            string result = string.Format
                (
                    _formatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format integer.
        /// </summary>
        [NotNull]
        public static string Format
            (
                int arg0
            )
        {
            string result = arg0.ToString(_formatProvider);

            return result;
        }

        /// <summary>
        /// Format double.
        /// </summary>
        [NotNull]
        public static string Format
            (
                double arg0
            )
        {
            string result = arg0.ToString(_formatProvider);

            return result;
        }

        #endregion
    }
}
