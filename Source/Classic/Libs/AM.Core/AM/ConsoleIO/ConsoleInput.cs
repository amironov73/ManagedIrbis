// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleInput.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.ConsoleIO
{
    /// <summary>
    /// Console input.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConsoleInput
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Read line.
        /// </summary>
        /// <returns><c>null</c> if nothing entered.
        /// </returns>
        [CanBeNull]
        public static string ReadLine()
        {
            return null;
        }

        #endregion
    }
}
