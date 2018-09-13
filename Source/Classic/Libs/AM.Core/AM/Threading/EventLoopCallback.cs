// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventLoopCallback.cs -- callback function for EventLoop
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Callback function for <see cref="EventLoop"/>.
    /// </summary>
    [PublicAPI]
    public delegate void EventLoopCallback<T>(EventLoop loop, T argument);
}
