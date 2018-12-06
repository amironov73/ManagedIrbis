// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringUtility.cs -- string manipulation routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// String manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class StringUtility
    {
        #region Properties

        /// <summary>
        /// Empty array of <see cref="string"/>.
        /// </summary>
        [NotNull]
        public static readonly string[] EmptyArray = new string[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Changes the encoding of given string from one to other.
        /// </summary>
        /// <param name="fromEncoding">From encoding.</param>
        /// <param name="toEncoding">To encoding.</param>
        /// <param name="value">String to transcode.</param>
        /// <returns>Transcoded string.</returns>
        [NotNull]
        public static string ChangeEncoding
        (
            [NotNull] Encoding fromEncoding,
            [NotNull] Encoding toEncoding,
            [NotNull] string value
        )
        {
            //Code.NotNull(fromEncoding, "fromEncoding");
            //Code.NotNull(toEncoding, "toEncoding");
            //Code.NotNull(value, "value");

            if (fromEncoding.Equals(toEncoding))
            {
                return value;
            }

            byte[] bytes = fromEncoding.GetBytes(value);
            string result = toEncoding.GetString(bytes);

            return result;
        }

        #endregion
    }
}
