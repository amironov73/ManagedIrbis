// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FastNumber.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Fast routines for integer numbers.
    /// </summary>
    [PublicAPI]
    public static class FastNumber
    {
        #region Public methods

        // ==========================================================

        /// <summary>
        /// Convert integer to string.
        /// </summary>
        public static string Int32ToString
            (
                int number
            )
        {
            char[] buffer = new char[10];
            int offset = 9;
            if (number == 0)
            {
                buffer[offset] = '0';
                offset--;
            }
            else
            {
                for (; number != 0; offset--)
                {
                    int remainder;
                    number = Math.DivRem(number, 10, out remainder);
                    buffer[offset] = (char) ('0' + remainder);
                }
            }

            return new string(buffer, offset + 1, 9 - offset);
        }

        // ==========================================================

        /// <summary>
        /// Convert integer to string.
        /// </summary>
        public static string Int64ToString
            (
                long number
            )
        {
            char[] buffer = new char[20];
            int offset = 19;
            if (number == 0)
            {
                buffer[offset] = '0';
                offset--;
            }
            else
            {
                for (; number != 0; offset--)
                {
                    long remainder;
                    number = Math.DivRem(number, 10, out remainder);
                    buffer[offset] = (char) ('0' + remainder);
                }
            }

            return new string(buffer, offset + 1, 19 - offset);
        }

        // ==========================================================

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                [NotNull] string text
            )
        {
            int result = 0;
            unchecked
            {
                foreach (char c in text)
                {
                    result = result * 10 + c - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                [NotNull] string text,
                int offset,
                int length
            )
        {
            int result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                [NotNull] char[] text,
                int offset,
                int length
            )
        {
            int result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                [NotNull] byte[] text,
                int offset,
                int length
            )
        {
            int result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        // ==========================================================

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                [NotNull] string text
            )
        {
            long result = 0;
            unchecked
            {
                foreach (char c in text)
                {
                    result = result * 10 + c - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                [NotNull] string text,
                int offset,
                int length
            )
        {
            long result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                [NotNull] char[] text,
                int offset,
                int length
            )
        {
            long result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                [NotNull] byte[] text,
                int offset,
                int length
            )
        {
            long result = 0;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;
        }

        // ==========================================================

        #endregion
    }
}
