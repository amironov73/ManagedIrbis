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
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int ch = Console.Read();
                if (ch < 0)
                {
                    break;
                }

                if (ch == '\r')
                {
                    if (Console.In.Peek() == '\n')
                    {
                        Console.Read();
                    }
                    return result.ToString();
                }

                if (ch == '\n')
                {
                    return result.ToString();
                }

                result.Append((char) ch);
            }

            if (result.Length != 0)
            {
                return result.ToString();
            }

            return null;
        }

        #endregion
    }
}
