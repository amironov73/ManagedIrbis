// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MonitoringSink.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    /// Абстрактный сток для данных мониторинга.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class MonitoringSink
    {
        #region Public methods

        /// <summary>
        /// Write monitoring data.
        /// </summary>
        public abstract bool WriteData
            (
                [NotNull] MonitoringData data
            );

        #endregion
    }
}
