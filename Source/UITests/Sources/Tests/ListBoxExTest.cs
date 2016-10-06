/* ListBoxExTest.cs -- 
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
    public sealed class ListBoxExTest
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

                ListBoxEx listBox = new ListBoxEx
                {
                    Location = new Point(10, 10),
                    Width = 200
                };
                listBox.Items.AddRange(new object[]
                {
                    "Item1",
                    "Item2",
                    "Item3",
                    "Item4",
                    "Item5",
                    "Item6",
                    "Item7",
                    "Item8",
                    "Item9",
                    "Item10"
                });
                form.Controls.Add(listBox);

                TextBox textBox = new TextBox
                {
                    Location = new Point(310, 10),
                    Width = 300
                };
                form.Controls.Add(textBox);

                listBox.SelectedIndexChanged += (sender, args) =>
                {
                    textBox.Text = listBox.SelectedItem
                    .NullableToVisibleString();
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
