// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IMxConsole.cs -- 
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
    /// Generic console for MX.
    /// </summary>
    public interface IMxConsole
    {
        /// <summary>
        /// Фон консоли.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Цвет символов.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Вывод.
        /// </summary>
        void Write([NotNull] string text);

        /// <summary>
        /// Вввод.
        /// </summary>
        string ReadLine();

        void Clear();
    }
}
