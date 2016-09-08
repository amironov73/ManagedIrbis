/* BindingListSourceTest.cs -- 
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
    public sealed class BindingListSourceTest
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

                BindingListSource<string> source
                    = new BindingListSource<string>();

                Button button = new Button
                {
                    Location = new Point(10, 10),
                    Width = 200,
                    Text = "Add an item"
                };
                form.Controls.Add(button);

                ListBox listBox = new ListBox
                {
                    Location = new Point(220, 10),
                    Size = new Size(200, 200),
                    DataSource = source
                };
                form.Controls.Add(listBox);

                button.Click += (sender, args) =>
                {
                    string item = DateTime.Now.Ticks.ToString();
                    source.Add(item);
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
