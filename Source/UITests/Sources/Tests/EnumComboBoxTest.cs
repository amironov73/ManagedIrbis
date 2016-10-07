/* EnumComboBoxTest.cs -- 
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
using ManagedIrbis.Marc;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class EnumComboBoxTest
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

                EnumComboBox comboBox = new EnumComboBox
                {
                    EnumType = typeof(MarcBibliographicalIndex),
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(comboBox);

                TextBox textBox = new TextBox
                {
                    Location = new Point(220, 10),
                    Width = 200
                };
                form.Controls.Add(textBox);

                comboBox.SelectedValueChanged += (sender, args) =>
                {
                    int? value = comboBox.Value;
                    string text = value == null
                        ? "(null)"
                        : ((MarcBibliographicalIndex)value).ToString();

                    textBox.Text = text;
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
