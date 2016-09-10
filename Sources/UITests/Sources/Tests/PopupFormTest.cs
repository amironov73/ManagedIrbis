/* PopupFormTest.cs -- 
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
    public sealed class PopupFormTest
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

                TextBox titleBox = new TextBox
                {
                    Text = "Form title",
                    Location = new Point(10, 10),
                    Width = 200
                };
                form.Controls.Add(titleBox);

                TextBox messageBox = new TextBox
                {
                    Text = "Some message <b>with formatting</b>",
                    Location = new Point(10, 40),
                    Width = 200
                };
                form.Controls.Add(messageBox);

                NumericUpDown timeoutBox = new NumericUpDown
                {
                    Minimum = 10m,
                    Maximum = 100000m,
                    Value = 2000m,
                    DecimalPlaces = 0,
                    Location = new Point(10, 70),
                    Width = 200
                };
                form.Controls.Add(timeoutBox);

                Button button = new Button
                {
                    Text = "Show popup",
                    Location = new Point(10, 100),
                    Width = 200
                };
                form.Controls.Add(button);

                button.Click += (sender, args) =>
                {
                    string title = titleBox.Text;
                    string message = messageBox.Text;
                    int timeout = Convert.ToInt32(timeoutBox.Value);

                    PopupForm.Popup
                        (
                            title,
                            message,
                            timeout
                        );
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
