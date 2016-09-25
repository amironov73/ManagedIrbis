/* DataGridViewTest.cs -- 
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
    public sealed class DataGridViewTest
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

                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(grid);

                DataGridViewCalendarColumn calendarColumn
                    = new DataGridViewCalendarColumn
                    {
                        HeaderText = "Calendar"
                    };
                grid.Columns.Add(calendarColumn);

                DataGridViewColorColumn colorColumn
                    = new DataGridViewColorColumn
                    {
                        HeaderText = "Color"
                    };
                grid.Columns.Add(colorColumn);

                DataGridViewNumericColumn numericColumn
                    = new DataGridViewNumericColumn
                    {
                        HeaderText = "Numeric"
                    };
                grid.Columns.Add(numericColumn);

                DataGridViewProgressColumn progressColumn
                    = new DataGridViewProgressColumn
                    {
                        HeaderText = "Progress"
                    };
                grid.Columns.Add(progressColumn);

                DataGridViewRatingColumn ratingColumn
                    = new DataGridViewRatingColumn
                    {
                        HeaderText = "Rating"
                    };
                grid.Columns.Add(ratingColumn);

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
