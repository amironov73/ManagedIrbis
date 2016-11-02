/* MxUtility.cs -- 
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
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx
{
    public static class MxUtility
    {
        #region Properties

        public static string[] AssemblyReferences =
        {
            "AM.Core.dll",
            "JetBrains.Annotations.dll",
            "ManagedIrbis.dll",
            "Microsoft.CSharp.dll",
            "MoonSharp.Interpreter.dll",
            "Newtonsoft.Json.dll",
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Data.DataSetExtensions.dll",
            "System.Drawing.dll",
            "System.Windows.Forms.dll",
            "System.Xml.dll",
            "System.Xml.Linq.dll"
        };

        #endregion
    }
}
