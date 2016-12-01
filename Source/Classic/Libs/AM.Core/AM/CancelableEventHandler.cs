// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CancelableEventHandler.cs -- Handler of cancellable event.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM
{
    /// <summary>
    /// Handler of event whose handling can be cancelled.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    public delegate void CancelableEventHandler
        (
            object sender,
            CancelableEventArgs ea
        );
}