/* ConsoleFormTest2.cs -- 
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
    public sealed class ConsoleFormTest2
        : IUITest
    {
        static string[] knownCommands =
        {
                "Clear-Screen",
                "Show-Help",
                "Current-Date",
                "Set-Time",
                "Print-Directory",
                "Exit"
        };

        static void HandleTabKey
            (
                object sender,
                ConsoleInputEventArgs eventArgs
            )
        {
            ConsoleControl console = (ConsoleControl) sender;
            string text = eventArgs.Text;

            if (string.IsNullOrEmpty(text))
            {
                console.DropInput();
                console.WriteLine
                    (
                        Color.Gray,
                        "Available commands are: "
                        + string.Join(", ", knownCommands)
                    );
                return;
            }

            foreach (string command in knownCommands)
            {
                if (command.ToLower().StartsWith(text.ToLower()))
                {
                    console.SetInput(command);
                    return;
                }
            }

            console.DropInput();
            console.WriteLine
                (
                    Color.Red,
                    "No suggestions found"
                );
            console.SetInput(text);
        }

        static void HandleInput
            (
                object sender,
                ConsoleInputEventArgs eventArgs
            )
        {
            ConsoleControl console = (ConsoleControl)sender;
            string text = eventArgs.Text;

            if (string.IsNullOrEmpty(text))
            {
                console.WriteLine
                    (
                        Color.Red,
                        "Idle command"
                    );
                return;
            }

            Color color = Color.Green;

            console.WriteLine
                (
                    color,
                    "Command received: '" + text + "'"
                );
            console.Write
                (
                    color,
                    "Processing... "
                );
            Application.DoEvents();
            Thread.Sleep(2000);
            console.WriteLine
                (
                    color,
                    "done"
                );

            console.WriteLine
                (
                    color,
                    new string('=', 70)
                );
            console.WriteLine();

            if (text.SameString("Clear-Screen"))
            {
                console.Clear();
            }
            if (text.SameString("Exit"))
            {
                Form form = console.FindForm().ThrowIfNull();
                form.Close();
            }
        }

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
                console.WriteLine
                    (
                        Color.Black,
                        "NekoShell 10.1.2.522 ready"
                    );
                console.WriteLine();

                console.AllowInput = true;
                console.TabPressed += HandleTabKey;
                console.Input += HandleInput;

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
