/* BusyControllerTest.cs -- 
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Threading;
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
    public sealed class BusyControllerTest
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

                Button firstButton = new Button
                {
                    Text = "Push me 1",
                    Location = new Point(10, 100)
                };
                form.Controls.Add(firstButton);

                Button secondButton = new Button
                {
                    Text = "Push me 2",
                    Location = new Point(200, 100)
                };
                form.Controls.Add(secondButton);

                BusyState state = new BusyState();

                BusyStripe stripe = new BusyStripe
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.LimeGreen,
                    Height = 20,
                    Text = "I am very busy"
                };
                stripe.SubscribeTo(state);
                form.Controls.Add(stripe);

                BusyController controller = new BusyController
                {
                    State = state
                };
                controller.Controls.Add(firstButton);
                controller.Controls.Add(secondButton);
                controller.ExceptionOccur += (sender, args) =>
                {
                    ExceptionBox.Show(ownerWindow, args.Exception);
                };

                Action action = () =>
                {
                    Thread.Sleep(3000);
                };

                firstButton.Click += (sender, args) =>
                {
                    controller.Run(action);
                };

                secondButton.Click += (sender, args) =>
                {
                    controller.RunAsync(action);
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
