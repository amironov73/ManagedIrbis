// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IApplicationUpdater.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Deployment
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public interface IApplicationUpdater
    {
        /// <summary>
        /// Updates the application.
        /// </summary>
        /// <returns><c>true</c> if application needs to restart.
        /// </returns>
        bool UpdateApplication();
    }
}
