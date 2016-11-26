/* Form1.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IrbisUI.Grid;

#endregion

namespace SiberianGrider
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1
        : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SiberianTextColumn _firstColumn;
        private SiberianButtonColumn _secondColumn;
        private SiberianTextColumn _thirdColumn;

        private void Form1_Load
            (
                object sender,
                EventArgs e
            )
        {
            _firstColumn = (SiberianTextColumn) _grid
                .CreateColumn<SiberianTextColumn>();
            _secondColumn = (SiberianButtonColumn) _grid
                .CreateColumn<SiberianButtonColumn>();
            _secondColumn.Width = 20;
            _thirdColumn = (SiberianTextColumn) _grid
                .CreateColumn<SiberianTextColumn>();

            _firstColumn.ReadOnly = true;
            _secondColumn.ReadOnly = true;

            for (int i = 0; i < 7; i++)
            {
                _grid.CreateRow(null);
            }

            _grid.Goto(2, 0);
        }
    }
}
