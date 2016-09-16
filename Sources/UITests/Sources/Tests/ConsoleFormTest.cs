/* ConsoleFormTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

namespace UITests
{
    public sealed class ConsoleFormTest
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

                form.Console.BackColor = Color.White;
                form.Console.ForeColor = Color.Blue;
                form.Console.Clear();

                form.Console.WriteLine
                    (
                        Color.Green,
                        @"#include <stdio.h>

int main ( int argc, char** argv )
{
    printf (""Hello, world!"");
    return 0;
}"
                    );

                form.Console.AllowInput = true;
                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
