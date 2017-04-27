// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectionStatistics.cs --
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

namespace ManagedIrbis.Infrastructure.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectionStatistics
    {
        #region Properties

        /// <summary>
        /// Count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Maximum, seconds.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Minimum, seconds.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Time spent on network, seconds.
        /// </summary>
        public double Total { get; set; }

        #endregion
    }
}
