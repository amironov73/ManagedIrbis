/* IUITest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace UITests.Sources
{
    /// <summary>
    /// UI test interface
    /// </summary>
    [PublicAPI]
    // ReSharper disable once InconsistentNaming
    public interface IUITest
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="parentWindow">Parent window
        /// (can be <c>null</c>).</param>
        void Run
            (
                [CanBeNull] IWin32Window parentWindow
            );
    }
}
