/* ButtonedTextBoxTest.cs -- 
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
    public sealed class ButtonedTextBoxTest
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
                    Text = "Press",
                    Padding = new Padding(0),
                    FlatStyle = FlatStyle.Flat,
                    Width = 50
                };
                button.Click += (sender, args) =>
                {
                    MessageBox.Show("Test");
                };
                ButtonedTextBox textBox = new ButtonedTextBox(button)
                {
                    Text = "Some text",
                    Location = new Point(10, 10),
                    AutoSize = false,
                    Size = new Size(150, 26)
                };
                form.Controls.Add(textBox);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
