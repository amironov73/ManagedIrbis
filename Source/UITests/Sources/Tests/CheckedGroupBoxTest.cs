/* CheckedGroupBoxTest.cs -- 
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
    public sealed class CheckedGroupBoxTest
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

                CheckedGroupBox group = new CheckedGroupBox
                {
                    Text = "Checked group box",
                    Location = new Point(10, 10),
                    Size = new Size(250, 100)
                };
                form.Controls.Add(group);

                Label label = new Label
                {
                    Text = "Label",
                    Location = new Point(10, 20)
                };
                group.Controls.Add(label);

                TextBox textBox = new TextBox
                {
                    Text = "Text box",
                    Location = new Point(10, 50)
                };
                group.Controls.Add(textBox);

                Button button = new Button
                {
                    Text = "Button",
                    Location = new Point(120, 20)
                };
                group.Controls.Add(button);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
