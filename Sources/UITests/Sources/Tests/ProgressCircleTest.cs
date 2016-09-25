/* ProgressCircleTest.cs -- 
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
    public sealed class ProgressCircleTest
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

                float percent = 0f;

                ProgressCircle circle = new ProgressCircle()
                {
                    Location = new Point(10, 10),
                    Size = new Size(100, 100)
                };
                form.Controls.Add(circle);

                Timer timer = new Timer
                {
                    Enabled = true,
                    Interval = 100
                };
                timer.Tick += (sender, args) =>
                {
                    percent += 1f;
                    if (percent >= 100f)
                    {
                        percent = 0f;
                    }
                    circle.Percent = percent;
                };

                form.ShowDialog(ownerWindow);
                timer.Dispose();
            }
        }

        #endregion
    }
}
