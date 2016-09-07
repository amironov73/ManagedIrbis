/* ToolStripTest.cs -- 
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
    public sealed class ToolStripTest
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

                ToolStripContainer container = new ToolStripContainer
                {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(container);
                ToolStrip toolStrip = new ToolStrip
                {
                    Dock = DockStyle.Top
                };
                container.TopToolStripPanel.Controls.Add(toolStrip);

                ToolStripCheckBox checkBox = new ToolStripCheckBox
                {
                    Text = "CheckBox"
                };
                toolStrip.Items.Add(checkBox);

                ToolStripColorComboBox colorBox = new ToolStripColorComboBox();
                toolStrip.Items.Add(colorBox);

                ToolStripDateTimePicker datePicker = new ToolStripDateTimePicker();
                toolStrip.Items.Add(datePicker);

                ToolStripNumericUpDown upDown = new ToolStripNumericUpDown();
                toolStrip.Items.Add(upDown);

                ToolStripOrdinaryButton ordinaryButton = new ToolStripOrdinaryButton
                {
                    Text = "Button"
                };
                toolStrip.Items.Add(ordinaryButton);

                ToolStripTrackBar trackBar = new ToolStripTrackBar
                {
                    Height = toolStrip.ClientSize.Height
                };
                toolStrip.Items.Add(trackBar);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
