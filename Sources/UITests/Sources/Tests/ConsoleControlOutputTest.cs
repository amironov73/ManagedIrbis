/* ConsoleControlOutputTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class ConsoleControlOutputTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                TextBox textBox = new TextBox
                {
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(textBox);

                Button button = new Button
                {
                    Location = new Point(250, 10),
                    Text = "Enter"
                };
                form.Controls.Add(button);

                ConsoleControl console = new ConsoleControl
                {
                    Location = new Point(10, 50),
                    ForeColor = Color.Yellow
                };
                form.Controls.Add(console);

                AbstractOutput output
                    = new ConsoleControlOutput(console);

                button.Click += (sender, args) =>
                {
                    string text = textBox.Text;
                    output.WriteLine(text);
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
