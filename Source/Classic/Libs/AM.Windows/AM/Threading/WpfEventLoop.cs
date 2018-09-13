// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WpfEventLoop.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using AM.Windows;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Event loop for WPF-based applications.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WpfEventLoop
        : EventLoop
    {
        #region EventLoop members

        /// <inheritdoc cref="EventLoop.Close" />
        public override void Close()
        {
            // Nothing to do here
        }

        /// <inheritdoc cref="EventLoop.Idle" />
        public override void Idle()
        {
            WpfUtility.DoEvents();
        }

        #endregion
    }
}
