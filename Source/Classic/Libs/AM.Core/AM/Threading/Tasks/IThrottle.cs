/* IThrottle.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

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

namespace AM.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IThrottle
    {
        /// <summary>
        /// Get next task.
        /// </summary>
        Task GetNext();

        /// <summary>
        /// Get next task.
        /// </summary>
        Task GetNext(out TimeSpan delay);
    }
}

#endif
