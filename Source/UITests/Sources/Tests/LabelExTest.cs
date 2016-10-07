/* LabelExTest.cs -- 
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
    public sealed class LabelExTest
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

                LabelEx label = new LabelEx
                {
                    Text = "This is label",
                    Location = new Point(10, 10),
                };
                form.Controls.Add(label);

                TextBox textBox1 = new TextBox
                {
                    Text = "This is text box",
                    Location = new Point(10, 40)
                };
                form.Controls.Add(textBox1);
                TextBox textBox2 = new TextBox
                {
                    Text = "This is another text box",
                    Location = new Point(10, 70)
                };
                form.Controls.Add(textBox2);
                textBox2.Focus();

                label.BuddyControl = textBox1;

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
