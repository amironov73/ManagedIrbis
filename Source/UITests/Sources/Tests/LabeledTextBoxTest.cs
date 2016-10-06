/* LabeledTextBoxTest.cs -- 
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
    public sealed class LabeledTextBoxTest
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

                LabeledTextBox comboBox = new LabeledTextBox
                {
                    Location = new Point(10, 10),
                    Size = new Size(100, 100)
                };
                comboBox.Label.Text = "Labeled TextBox";
                comboBox.TextBox.Text = "Some text";
                form.Controls.Add(comboBox);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
