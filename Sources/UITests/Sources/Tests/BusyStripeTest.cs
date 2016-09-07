/* BusyStripeTest.cs -- 
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
    public sealed class BusyStripeTest
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

                BusyStripe stripe = new BusyStripe
                {
                    Size = new Size(10, 30),
                    Dock = DockStyle.Top,
                    Text = "Some text"
                };
                form.Controls.Add(stripe);

                Button button = new Button
                {
                    Text = "Toggle on/off",
                    Location = new Point(10, 40),
                    Width = 100
                };
                button.Click += (sender, args) =>
                {
                    stripe.Moving = !stripe.Moving;
                };
                form.Controls.Add(button);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
