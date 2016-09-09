/* MeterTest.cs -- 
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
    public sealed class MeterTest
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

                Meter meter = new Meter
                {
                    Location = new Point(10, 10),
                    Size = new Size(400, 200),
                    MinValue = 0.0f,
                    MaxValue = 100.0f,
                    Value = 50.0f
                };
                form.Controls.Add(meter);

                TrackBar trackBar = new TrackBar
                {
                    Location = new Point(10, 230),
                    Size = new Size(400, 30),
                    Minimum = 0,
                    Maximum = 100,
                    Value = 50
                };
                form.Controls.Add(trackBar);
                trackBar.ValueChanged += (sender, args) =>
                {
                    meter.Value = trackBar.Value;
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
