/* ScrollControlTest.cs -- 
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
    public sealed class ScrollControlTest
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
                form.AutoScaleMode = AutoScaleMode.None;
                form.Size = new Size(800, 600);

                ScrollControl scroll = new ScrollControl
                {
                    Location = new Point(10, 10),
                    BackColor = Color.DarkGray
                };
                form.Controls.Add(scroll);

                TextBox textBox = new TextBox
                {
                    Location = new Point(40, 10),
                    Size = new Size(400, 200),
                    Multiline = true
                };
                form.Controls.Add(textBox);

                scroll.Scroll += (sender, args) =>
                {
                    string text = string.Format
                        (
                            "Type: {0}, Value: {1}\r\n",
                            args.Type,
                            args.NewValue
                        );
                    textBox.AppendText(text);
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
