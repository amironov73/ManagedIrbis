/* ConsoleFormTest3.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

namespace UITests
{
    public sealed class ConsoleFormTest3
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {

            using (ConsoleForm form = new ConsoleForm())
            {
                form.Text = "ConsoleControl in action";

                ConsoleControl console = form.Console;

                console.BackColor = Color.White;
                console.ForeColor = Color.Blue;
                console.Clear();
                console.AllowInput = true;

                form.Show(ownerWindow);

                string text = console.ReadLine();

                form.Close();
                MessageBox.Show(text);
            }
        }

        #endregion
    }
}
