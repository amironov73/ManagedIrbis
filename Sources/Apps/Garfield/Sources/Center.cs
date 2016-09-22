/* Center.cs --
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

using ManagedIrbis;
using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace Garfield
{
    public static class Center
    {
        public static IrbisConnection Connection { get; set; }

        public static LocalCatalogerIniFile IniFile { get; set; }

        public static MainForm MainForm { get; set; }
    }
}
