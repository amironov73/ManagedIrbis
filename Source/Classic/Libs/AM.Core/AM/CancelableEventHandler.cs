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