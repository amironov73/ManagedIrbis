// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionUtility.cs --
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

using AM.Logging;

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
    public static class ExceptionUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Throw exception with message.
        /// </summary>
        public static void Throw
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            string message = string.Format
                (
                    format,
                    args
                );

            Log.Trace
                (
                    "ExceptionUtility::Throw: "
                    + message
                );

            throw new Exception(message);
        }

        #endregion
    }
}
