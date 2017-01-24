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

using AM.Windows.Forms;
using IrbisUI;
using ManagedIrbis;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace EasyBinder
{
    public partial class MainForm
        : Form
    {
        #region Properties

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();

        }

        #endregion

        #region Private members

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {

        }

        private void MainForm_FormClosing
            (
                object sender,
                FormClosingEventArgs e
            )
        {
            ControlCenter.Disconnect();
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            ControlCenter.Connect();

            _magazineList.LoadMagazines(ControlCenter.Connection);
        }

        #endregion

        #region Public methods

        #endregion
    }
}
