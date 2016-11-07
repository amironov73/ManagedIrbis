/* PathFinder.cs -- 
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

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Finds a file on abstract path.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PathFinder
    {
        #region Public methods

        /// <summary>
        /// Find file.
        /// </summary>
        [CanBeNull]
        public virtual string FindFile
            (
                [NotNull] string path,
                [NotNull] string fileName
            )
        {
            Code.NotNull(path, "path");
            Code.NotNull(fileName, "fileName");

            return null;
        }

        #endregion
    }
}
