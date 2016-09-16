/* ConsoleControlTest2.cs -- 
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
    public sealed class ConsoleControlTest2
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
                    Width = 100
                };
                form.Controls.Add(textBox);

                ConsoleControl console = new ConsoleControl
                {
                    Location = new Point(10, 50),
                    ForeColor = Color.Yellow,
                    AllowInput = true
                };
                form.Controls.Add(console);

                console.WriteLine
                    (
                        Color.LawnGreen,
                        @"#include <stdio.h>

int main ( int argc, char** argv )
{
    printf (""Hello, world!"");
    return 0;
}"
                    );

                console.Input += (sender, args) =>
                {
                    console.WriteLine
                        (
                            Color.DeepSkyBlue,
                            "You entered: " + args.Text
                        );
                };

                console.TabPressed += (sender, args) =>
                {
                    string text = DateTime.Now.ToShortTimeString()
                                  + ": " + args.Text;
                    console.SetInput(text);
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
