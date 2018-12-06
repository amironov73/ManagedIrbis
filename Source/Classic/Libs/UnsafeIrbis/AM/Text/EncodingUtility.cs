// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EncodingUtility.cs -- text encoding related routines
 *  Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Text
{
    /// <summary>
    /// Text encoding related routines.
    /// </summary>
    [PublicAPI]
    public static class EncodingUtility
    {
        #region Public methods

        /// <summary>
        /// Get string from bytes.
        /// </summary>
        /// <remarks>
        /// Reduce if/else preprocessing.
        /// </remarks>
        [NotNull]
        public static string GetString
            (
                [NotNull] Encoding encoding,
                [NotNull] byte[] bytes
            )
        {
            //Code.NotNull(encoding, "encoding");
            //Code.NotNull(bytes, "bytes");

            string result = encoding.GetString(bytes);


            return result;
        }


        #endregion
    }
}
