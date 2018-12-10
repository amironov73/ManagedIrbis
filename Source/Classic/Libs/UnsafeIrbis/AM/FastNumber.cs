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

namespace UnsafeAM
{
    /// <summary>
    /// Fast and dirty routines for integer numbers.
    /// </summary>
    [PublicAPI]
    public static unsafe class FastNumber
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
            // TODO не работает с отрицательными числами!

            char* buffer = stackalloc char[10];
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
                    unchecked
                    {
                        number = Math.DivRem(number, 10, out int remainder);
                        buffer[offset] = (char) ('0' + remainder);
                    }
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
            char* buffer = stackalloc char[20];
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
                    unchecked
                    {
                        number = Math.DivRem(number, 10, out long remainder);
                        buffer[offset] = (char) ('0' + remainder);
                    }
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
                fixed (char* ptr = text)
                {
                    char* stop = ptr + text.Length;
                    for (char* p = ptr; p < stop; p++)
                    {
                        result = result * 10 + *p - '0';
                    }
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
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
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
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
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
                fixed (byte* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                ReadOnlyMemory<char> text
            )
        {
            int result = 0;
            var span = text.Span;
            unchecked
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static int ParseInt32
            (
                ReadOnlyMemory<byte> text
            )
        {
            int result = 0;
            var span = text.Span;
            unchecked
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
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
                fixed (char* ptr = text)
                {
                    char* stop = ptr + text.Length;
                    for (char* p = ptr; p < stop; p++)
                    {
                        result = result * 10 + *p - '0';
                    }
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
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
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
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
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
                fixed (byte* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                ReadOnlyMemory<char> text
            )
        {
            long result = 0;
            var span = text.Span;
            unchecked
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                ReadOnlyMemory<byte> text
            )
        {
            long result = 0;
            var span = text.Span;
            unchecked
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;
        }

        // ==========================================================

        #endregion
    }
}
