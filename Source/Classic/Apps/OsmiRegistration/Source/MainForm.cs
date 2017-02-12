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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace OsmiRegistration
{
    /// <summary>
    /// 
    /// </summary>
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

        private void MainForm_FormClosing
            (
                object sender, 
                FormClosingEventArgs e
            )
        {
            ControlCenter.WriteLine("Shutdown");
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            ControlCenter.Initialize();
            ControlCenter.Output = _logBox.Output;

            ControlCenter.Ping();

            ControlCenter.WriteLine("Ready to work");
        }

        #endregion
    }
}
