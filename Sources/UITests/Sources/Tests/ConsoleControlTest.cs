/* ConsoleControlTest.cs -- 
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
    public sealed class ConsoleControlTest
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
                    Width = 200,
                    Text = "Hello, world! "
                };
                form.Controls.Add(textBox);

                Button button1 = new Button
                {
                    Location = new Point(230, 10),
                    Width = 100,
                    Text = "Write"
                };
                form.Controls.Add(button1);

                Button button2 = new Button
                {
                    Location = new Point(340, 10),
                    Width = 100,
                    Text = "Many"
                };
                form.Controls.Add(button2);

                Button button3 = new Button
                {
                    Location = new Point(450, 10),
                    Width = 100,
                    Text = "Clear"
                };
                form.Controls.Add(button3);

                ConsoleControl console = new ConsoleControl
                {
                    Location = new Point(10, 50),
                };
                form.Controls.Add(console);

                for (int row = 0; row < 4; row++)
                {
                    for (int column = 0; column < 80; column++)
                    {
                        if (column%10 == 0)
                        {
                            console.Write
                                (
                                    row,
                                    column,
                                    '0',
                                    Color.Red,
                                    Color.White
                                );
                        }
                        else
                        {
                            char c = (char)('0' + column%10);
                            console.Write
                                (
                                    row,
                                    column,
                                    c,
                                    Color.Blue, 
                                    Color.GreenYellow
                                );
                        }

                    }
                }
                console.CursorTop = 4;

                button1.Click += (sender, args) =>
                {
                    console.Write(textBox.Text);
                };

                button2.Click += (sender, args) =>
                {
                    console.ForeColor = Color.LimeGreen;
                    for (int i = 0; i < 100; i++)
                    {
                        console.Write("Mary has a little lamb. ");
                        Application.DoEvents();
                    }
                };

                button3.Click += (sender, args) =>
                {
                    console.Clear();
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
