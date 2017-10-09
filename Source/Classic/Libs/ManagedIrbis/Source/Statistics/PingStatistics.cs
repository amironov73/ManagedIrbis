// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PingStatistics.cs -- 
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

namespace ManagedIrbis.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PingStatistics
    {
        #region Properties

        /// <summary>
        /// Average roundtrip time.
        /// </summary>
        public int AverageTime
        {
            get
            {
                return (int)Data
                    .Where(item => item.Success)
                    .Select(item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Average();
            }
        }

        /// <summary>
        /// Maximum roundtrip time.
        /// </summary>
        public int MaxTime
        {
            get
            {
                return Data
                    .Where(item => item.Success)
                    .Select(item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Max();
            }
        }

        /// <summary>
        /// Minimum roundtrip time.
        /// </summary>
        public int MinTime
        {
            get
            {
                return Data
                    .Where(item => item.Success)
                    .Select(item => item.RoundTripTime)
                    .DefaultIfEmpty()
                    .Min();
            }
        }

        /// <summary>
        /// Data.
        /// </summary>
        [NotNull]
        public Queue<PingData> Data { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PingStatistics()
        {
            Data = new Queue<PingData>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Add entry.
        /// </summary>
        public void Add
            (
                PingData item
            )
        {
            Data.Enqueue(item);
        }

        /// <summary>
        /// Clear the statistics.
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        #endregion

        #region Object members

        #endregion
    }
}
