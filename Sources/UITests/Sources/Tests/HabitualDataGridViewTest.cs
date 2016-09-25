/* HabitualDataGridViewTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
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
    public sealed class HabitualDataGridViewTest
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

                HabitualDataGridView grid = new HabitualDataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(600, 300)
                };
                DataGridViewColumn column1 = new DataGridViewTextBoxColumn
                {
                    HeaderText = "Column1",
                    DataPropertyName = "Column1",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                grid.Columns.Add(column1);
                DataGridViewColumn column2 = new DataGridViewTextBoxColumn
                {
                    HeaderText = "Column2",
                    DataPropertyName = "Column2",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                grid.Columns.Add(column2);
                form.Controls.Add(grid);

                DataTable table = new DataTable();
                DataColumn column3 = new DataColumn("Column1", typeof(int));
                table.Columns.Add(column3);
                DataColumn column4 = new DataColumn("Column2", typeof(int));
                table.Columns.Add(column4);

                int counter = 0;
                for (int i = 0; i < 100; i++)
                {
                    DataRow row = table.NewRow();
                    row[0] = ++counter;
                    row[1] = ++counter;
                    table.Rows.Add(row);
                }
                grid.DataSource = table;

                form.ShowDialog(ownerWindow);
            }
        }

        #endregion
    }
}
