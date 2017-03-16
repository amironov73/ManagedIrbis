// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs -- 
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Search;

using Newtonsoft.Json.Linq;

#endregion

namespace Shortage
{
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void CheckBarcode
            (
                [NotNull] string barcode
            )
        {
            Code.NotNullNorEmpty(barcode, "barcode");

            List<ExemplarInfo> exemplars = ControlCenter.Exemplars;
            if (ReferenceEquals(exemplars, null))
            {
                return;
            }

            ExemplarInfo current = exemplars.FirstOrDefault
                (
                    exemplar => exemplar.Barcode.SameString(barcode)
                );

            if (ReferenceEquals(current, null))
            {
                WriteLine
                    (
                        "Метка не входит в партию: {0}",
                        barcode
                    );
                return;
            }

            current.Marked = true;

            WriteLine
                (
                    "Подтверждён экземпляр {0}: {1}",
                    current,
                    current.Description
                );

            UpdateTop();
            _bookGrid.ResetBindings();
            _bookGrid.Invalidate();
        }

        private void LoadRegisters()
        {
            string year = _yearBox.Value.ToString
                (
                    "0",
                    CultureInfo.InvariantCulture
                );
            TermInfo[] terms = ControlCenter.GetTerms(year);
            WriteLine
                (
                    "Year: {0}, registers: {1}",
                    year,
                    terms.Length
                );
            _ksuGrid.DataSource = terms;
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            _ksuGrid.AutoGenerateColumns = false;
            _bookGrid.AutoGenerateColumns = false;

            this.ShowVersionInfoInTitle();
            _logBox.Output.PrintSystemInformation();

            ControlCenter.Initialize();
            ControlCenter.Output = _logBox.Output;

            _yearBox.Value = DateTime.Today.Year;

            ControlCenter.WriteLine("Ready");
            ControlCenter.WriteLine(string.Empty);

            _barcodeBox.Focus();
        }

        private async void _newPartyBox_Click
            (
                object sender,
                EventArgs e
            )
        {
            DataGridViewRow currentRow = _ksuGrid.CurrentRow;
            if (ReferenceEquals(currentRow, null))
            {
                return;
            }

            TermInfo register = currentRow.DataBoundItem as TermInfo;
            if (ReferenceEquals(register, null))
            {
                return;
            }

            try
            {
                _busyStripe.Moving = true;
                _busyStripe.Visible = true;

                string ksu = register.Text.ThrowIfNull("ksu");

                WriteLine("Партия: {0}", ksu);

                List<ExemplarInfo> exemplars = await Task.Run
                    (
                        () => ControlCenter.GetExemplars(ksu)
                    );

                SetExemplars(exemplars);
            }
            finally
            {
                _busyStripe.Visible = false;
                _busyStripe.Moving = false;
            }
        }

        private void SetExemplars
            (
                [NotNull] List<ExemplarInfo> exemplars
            )
        {
            Code.NotNull(exemplars, "exemplars");

            ControlCenter.Exemplars = exemplars;
            _bookGrid.DataSource = exemplars;

            ExemplarInfo[] emptyBarcode = exemplars
                .Where
                (
                    exemplar => string.IsNullOrEmpty(exemplar.Barcode)
                )
                .ToArray();
            if (emptyBarcode.Length != 0)
            {
                WriteLine("Имеются экземпляры без радиометок:");
                foreach (ExemplarInfo exemplar in emptyBarcode)
                {
                    WriteLine
                        (
                            "{0}: {1}",
                            exemplar, exemplar.Description
                        );
                }
            }

            int total = exemplars.Count;
            int marked = exemplars.Count
                (
                    exemplar => exemplar.Marked
                );
            int remaining = total - marked;
            string text = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "Всего экземпляров: {0}, отмечено: {1}, осталось: {2}",
                    total,
                    marked,
                    remaining
                );
            WriteLine(text);

            UpdateTop();
        }

        private void UpdateTop()
        {
            List<ExemplarInfo> exemplars = ControlCenter.Exemplars;
            int total = exemplars.Count;
            int marked = exemplars.Count
                (
                    exemplar => exemplar.Marked
                );
            int remaining = total - marked;
            string text = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "Всего экземпляров: {0}, отмечено: {1}, осталось: {2}",
                    total,
                    marked,
                    remaining
                );

            if (remaining == 0)
            {
                text += ". ПОЗДРАВЛЯЕМ!";
                WriteLine("Все экземпляры подтверждены");
            }

            _topLabel.Text = text;
        }

        /// <summary>
        /// Write debug line.
        /// </summary>
        private static void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            ControlCenter.WriteLine(format, arguments);
        }

        private void _yearBox_ValueChanged
            (
                object sender,
                EventArgs e
            )
        {
            LoadRegisters();
        }

        #endregion

        private void _ksuGrid_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            _newPartyBox_Click(sender, e);
        }

        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (ReferenceEquals(ControlCenter.Exemplars, null))
            {
                return;
            }

            if (ControlCenter.Exemplars.Count == 0)
            {
                return;
            }

            string fileName = ControlCenter.Exemplars[0].KsuNumber1
                .ThrowIfNull("ksu not set")
                .Replace('/', '-')
                .Replace('\\', '-')
                .Replace(':', '-')
                .Replace('|', '-')
                + ".json";

            _saveFileDialog.FileName = fileName;

            if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                ControlCenter.SaveExemplars(_saveFileDialog.FileName);
            }
        }

        private void toolStripButton1_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                List<ExemplarInfo> exemplars = ControlCenter.ReadExemplars
                    (
                        _openFileDialog.FileName
                    );
                SetExemplars(exemplars);
            }
        }

        private void _barcodeBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    _barcodeBox.Clear();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Enter:

                    string barcode = _barcodeBox.Text.Trim();
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        CheckBarcode(barcode);
                    }

                    _barcodeBox.Clear();

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void _unmarkedButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            List<ExemplarInfo> unmarked = ControlCenter.ListUnmarked();

            if (unmarked.Count == 0)
            {
                MessageBox.Show("Нет неподтвержденных");
                return;
            }

            StringBuilder builder = new StringBuilder();

            foreach (ExemplarInfo exemplar in unmarked)
            {
                builder.AppendLine
                    (
                        string.Format
                        (
                            "{0}: {1}",
                            exemplar,
                            exemplar.Description
                        )
                    );
            }

            string text = builder.ToString();
            using (PlainTextViewer viewer = new PlainTextViewer())
            {
                viewer.SetText(text);
                viewer.ShowDialog(this);
            }
        }
    }
}
