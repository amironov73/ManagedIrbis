// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Profiler.cs --
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

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Profiling
{
    /// <summary>
    /// Profiler stub.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Profiler
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public static IAmProfiler Current { get; private set; }

        #endregion

        #region Construction

        static Profiler()
        {
            Current = new NullProfiler();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Start profiling the step.
        /// </summary>
        [NotNull]
        public static ProfilerStep Step
            (
                [CanBeNull] string name
            )
        {
            return new ProfilerStep();
        }

        /// <summary>
        /// Generate report.
        /// </summary>
        [NotNull]
        public static string GenerateReport()
        {
            return string.Empty;
        }

        #endregion
    }
}
