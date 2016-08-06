/* IConsoleOutputReceiver.cs -- receives console output
 *  ArsMagna project, http://arsmagna.ru
 */

namespace AM.IO
{
    /// <summary>
    /// Receives console output.
    /// </summary>
    public interface IConsoleOutputReceiver
    {
        /// <summary>
        /// Receives the console line.
        /// </summary>
        void ReceiveConsoleOutput 
            (
                string text
            );
    }
}
