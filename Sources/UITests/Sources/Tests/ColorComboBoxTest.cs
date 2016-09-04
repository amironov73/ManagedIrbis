/* ColorComboBoxTest.cs -- 
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
    public sealed class ColorComboBoxTest
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

                ColorComboBox colorBox = new ColorComboBox
                {
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(colorBox);

                TextBox textBox = new TextBox
                {
                    Location = new Point(310, 10),
                    Width = 300
                };
                form.Controls.Add(textBox);

                colorBox.SelectedIndexChanged += (sender, args) =>
                {
                    textBox.Text = colorBox.SelectedColor.ToString();
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
