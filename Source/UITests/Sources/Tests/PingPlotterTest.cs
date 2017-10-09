/* PingPlotterTest.cs -- 
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
using IrbisUI.Source.Statistics;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Statistics;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class PingPlotterTest
        : IUITest
    {
        #region Private members

        private void _FillStatistics
            (
                [NotNull] PingStatistics statistics
            )
        {
            statistics.Clear();
            Random random = new Random();
            const int min = 10, max = 1000;
            int current = random.Next(min, max);
            int howMany = random.Next(10, 1000);
            for (int i = 0; i < howMany; i++)
            {
                PingData item = new PingData
                {
                    Moment = DateTime.Now,
                    RoundTripTime = current,
                    Success = random.Next(1000) % 17 != 0
                };
                statistics.Add(item);
                current = Math.Max(min, Math.Min
                    (
                        max,
                        current + random.Next(11)) - 5
                    );
            }
        }

        #endregion

        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            IrbisConnection connection = new IrbisConnection();
            IrbisPing pinger = new IrbisPing(connection);
            _FillStatistics(pinger.Statistics);

            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                PingPlotter plotter = new PingPlotter
                {
                    Location = new Point(10, 10),
                    Size = new Size(600, 300),
                    Statistics = pinger.Statistics
                };
                form.Controls.Add(plotter);

                Button button = new Button
                {
                    Location = new Point(620, 10),
                    Width = 75,
                    Text = "Обновить"
                };
                form.Controls.Add(button);
                button.Click += (sender, args) =>
                {
                    _FillStatistics(pinger.Statistics);
                    plotter.Invalidate();
                };

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
