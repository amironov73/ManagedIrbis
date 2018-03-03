// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SourceCodeUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

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
    public static class SourceCodeUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert byte value to C# source code.
        /// </summary>
        [NotNull]
        public static string ToSourceCode
            (
                byte value
            )
        {
            return "0x" + value.ToString
                (
                    "X2",
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Convert array of bytes to C# source code.
        /// </summary>
        public static string ToSourceCode
            (
                [NotNull] byte[] array
            )
        {
            Code.NotNull(array, "array");

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < array.Length; i++)
            {
                if (i != 0)
                {
                    result.Append(", ");
                    if (i % 10 == 0)
                    {
                        result.AppendLine();
                        result.Append("  ");
                    }
                }
                result.AppendFormat
                    (
                        CultureInfo.InvariantCulture,
                        "0x{0:X2}",
                        array[i]
                    );
            }
            result.Append("}");

            return result.ToString();
        }

        /// <summary>
        /// Encode one character.
        /// </summary>
        public static string EncodeCharacter
            (
                char value
            )
        {
            switch (value)
            {
                case '\a': return "\\a";
                case '\b': return "\\b";
                case '\f': return "\\f";
                case '\n': return "\\n";
                case '\r': return "\\r";
                case '\t': return "\\t";
                case '\v': return "\\v";
                case '\\': return "\\\\";
                case '\'': return "\\'";
                case '\"': return "\\\"";
            }

            if (value < ' ')
            {
                return string.Format
                    (
                        CultureInfo.InvariantCulture,
                        "\\x{0:X2}",
                        (int)value
                    );
            }

            return value.ToString();
        }

        /// <summary>
        /// Convert the character to C# source code.
        /// </summary>
        public static string ToSourceCode
            (
                char value
            )
        {
            return "'" + EncodeCharacter(value) + "'";
        }

        /// <summary>
        /// Convert array of characters to C# source code.
        /// </summary>
        public static string ToSourceCode
            (
                [NotNull] char[] array
            )
        {
            Code.NotNull(array, "array");

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < array.Length; i++)
            {
                if (i != 0)
                {
                    result.Append(", ");
                    if (i % 10 == 0)
                    {
                        result.AppendLine();
                        result.Append("  ");
                    }
                }
                result.Append
                    (
                        "'" + EncodeCharacter(array[i]) + "'"
                    );
            }
            result.Append("}");

            return result.ToString();
        }

        /// <summary>
        /// Convert array of 32-bit integers to C# source code.
        /// </summary>
        public static string ToSourceCode
            (
                [NotNull] int[] array
            )
        {
            Code.NotNull(array, "array");

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < array.Length; i++)
            {
                if (i != 0)
                {
                    result.Append(", ");
                    if (i % 10 == 0)
                    {
                        result.AppendLine();
                        result.Append("  ");
                    }
                }
                result.Append
                    (
                        array[i].ToString
                            (
                                CultureInfo.InvariantCulture
                            )
                    );
            }
            result.Append("}");

            return result.ToString();
        }

        #endregion
    }
}
