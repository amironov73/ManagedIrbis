// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IConsoleOutputReceiver.cs -- receives console output
 *  ArsMagna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
