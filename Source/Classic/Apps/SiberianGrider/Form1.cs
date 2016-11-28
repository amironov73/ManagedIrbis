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
        private SiberianTextColumn _fourthColumn;
        private SiberianTextColumn _fifthColumn;

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
            _thirdColumn = (SiberianTextColumn) _grid
                .CreateColumn<SiberianTextColumn>();
            _fourthColumn = (SiberianTextColumn) _grid
                .CreateColumn<SiberianTextColumn>();
            _fifthColumn = (SiberianTextColumn) _grid
                .CreateColumn<SiberianTextColumn>();

            _firstColumn.Title = "Title";
            _firstColumn.ReadOnly = true;
            _firstColumn.BackColor = Color.DarkViolet;
            _firstColumn.Member = "Title";

            _secondColumn.ReadOnly = true;
            _secondColumn.BackColor = Color.DarkGreen;
            _secondColumn.Width = 20;

            _thirdColumn.Title = "Value";
            _thirdColumn.FillWidth = 100;
            _thirdColumn.Member = "Value";

            _fourthColumn.Title = "Appendix1";
            _fourthColumn.FillWidth = 100;

            _fifthColumn.Title = "Appendix2";
            _fifthColumn.FillWidth = 100;

            for (int i = 0; i < 70; i++)
            {
                SubFieldDescription description
                    = new SubFieldDescription
                    {
                        Title = "Subfield " + (i + 1),
                        Value = "Value " + (i + 1)
                    };

                SiberianRow row = _grid.CreateRow(null);
                row.Data = description;
                row.GetData();
            }

            _grid.Goto(2, 0);
        }
    }
}
