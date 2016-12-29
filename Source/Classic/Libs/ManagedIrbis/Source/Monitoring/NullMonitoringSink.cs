// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullMonitoringSink.cs -- 
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NullMonitoringSink
        : MonitoringSink
    {
        #region MonitoringSink members

        /// <inheritdoc/>
        public override bool WriteData
            (
                MonitoringData data
            )
        {
            // Nothing to do here.

            return true;
        }

        #endregion
    }
}
