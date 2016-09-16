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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM.Windows.Forms;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace UITests
{
    /// <summary>
    /// Main form.
    /// </summary>
    public partial class MainForm
        : Form
    {
        #region Properties

        [CanBeNull]
        public UITest CurrentTest
        {
            get
            {
                return _listBox.SelectedItem as UITest;
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private void _exitItem_Click
            (
                object sender,
                EventArgs e
            )
        {
            Close();
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            UITest[] tests = UITest
                .LoadFromFile("config.json")
                .OrderBy(test => test.Title)
                .ToArray();

            _listBox.Items.AddRange(tests);
        }

        private void _listBox_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                UITest test = CurrentTest;
                if (test != null)
                {
                    test.Run(this);
                }
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
            }
        }

        private void _listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                _listBox_DoubleClick(sender, e);
            }
        }
    }
}
