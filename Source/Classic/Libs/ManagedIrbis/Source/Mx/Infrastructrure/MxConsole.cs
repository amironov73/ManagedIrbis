// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MxConsole.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MxConsole
        : IMxConsole
    {
        #region IMxConsole members

        /// <inheritdoc cref="IMxConsole.BackgroundColor" />
        public ConsoleColor BackgroundColor
        {
            get { return ConsoleInput.BackgroundColor; }
            set { ConsoleInput.BackgroundColor = value; }
        }

        /// <inheritdoc cref="IMxConsole.ForegroundColor" />
        public ConsoleColor ForegroundColor
        {
            get { return ConsoleInput.ForegroundColor; }
            set { ConsoleInput.ForegroundColor = value; }
        }

        /// <inheritdoc cref="IMxConsole.Write" />
        public void Write
            (
                string text
            )
        {
            ConsoleInput.Write(text);
        }

        /// <inheritdoc cref="IMxConsole.ReadLine" />
        public string ReadLine()
        {
            return ConsoleInput.ReadLine();
        }

        /// <inheritdoc cref="IMxConsole.Clear" />
        public void Clear()
        {
            ConsoleInput.Clear();
        }

        #endregion
    }
}
