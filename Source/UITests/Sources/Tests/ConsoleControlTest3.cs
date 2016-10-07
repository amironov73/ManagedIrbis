/* ConsoleControlTest3.cs -- 
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
    public sealed class ConsoleControlTest3
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

                Button button = new Button
                {
                    Location = new Point(10, 10),
                    Text = "Press me"
                };
                form.Controls.Add(button);

                ConsoleControl console = new ConsoleControl
                {
                    Location = new Point(10, 50),
                    ForeColor = Color.LawnGreen
                };
                form.Controls.Add(console);

                button.Click += (sender, args) =>
                {
                    for (int i = 0; i < 100; i += 5)
                    {
                        string text = string.Format
                        (
                            "\rProcessing files \x1\xE{0}%",
                            i
                        );
                        console.Write(text);
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }

                    console.WriteLine("\rProcessing files \x1\x000Fdone");
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
