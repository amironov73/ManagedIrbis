// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberRange.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

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
    public static class NumberRange
    {
        #region Public methods

        /// <summary>
        /// Parse the text line.
        /// </summary>
        public static int[] ParseInt32
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            List<int> result = new List<int>();
            string[] chunks = StringUtility.SplitString
                (
                    line,
                    CommonSeparators.CommaAndSemicolon,
                    StringSplitOptions.RemoveEmptyEntries
                );
            foreach (string chunk in chunks)
            {
                string trimmed = chunk.Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    continue;
                }
                bool containsMinus = false;
                for (int i = 1; i < trimmed.Length; i++)
                {
                    if (trimmed[i] == '-')
                    {
                        containsMinus = true;
                        break;
                    }
                }
                if (containsMinus)
                {
                    // TODO Handle negative values properly

                    string[] parts = StringUtility.SplitString
                        (
                            trimmed,
                            CommonSeparators.Minus,
                            2
                        );
                    if (parts.Length != 2)
                    {
                        throw new ArgumentException();
                    }
                    int start = NumericUtility.ParseInt32(parts[0]);
                    int stop = NumericUtility.ParseInt32(parts[1]);
                    for (int i = start; i <= stop; i++)
                    {
                        result.Add(i);
                    }
                }
                else
                {
                    int number = NumericUtility.ParseInt32(trimmed);
                    result.Add(number);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the text line.
        /// </summary>
        public static long[] ParseInt64
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            List<long> result = new List<long>();
            string[] chunks = StringUtility.SplitString
                (
                    line,
                    CommonSeparators.CommaAndSemicolon,
                    StringSplitOptions.RemoveEmptyEntries
                );
            foreach (string chunk in chunks)
            {
                string trimmed = chunk.Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    continue;
                }
                bool containsMinus = false;
                for (int i = 1; i < trimmed.Length; i++)
                {
                    if (trimmed[i] == '-')
                    {
                        containsMinus = true;
                        break;
                    }
                }
                if (containsMinus)
                {
                    // TODO Handle negative values properly

                    string[] parts = StringUtility.SplitString
                        (
                            trimmed,
                            CommonSeparators.Minus,
                            2
                        );
                    if (parts.Length != 2)
                    {
                        throw new ArgumentException();
                    }
                    long start = NumericUtility.ParseInt64(parts[0]);
                    long stop = NumericUtility.ParseInt64(parts[1]);
                    for (long i = start; i <= stop; i++)
                    {
                        result.Add(i);
                    }
                }
                else
                {
                    long number = NumericUtility.ParseInt64(trimmed);
                    result.Add(number);
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}
