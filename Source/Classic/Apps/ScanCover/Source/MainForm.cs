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

using ManagedIrbis;

#endregion

namespace ScanCover
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

        // private IrbisConnection _connection;

        public IrbisConnection GetConnection()
        {
            IrbisConnection result
                = IrbisConnectionUtility.GetClientFromConfig();

            return result;
        }

        #endregion

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();
        }

    }
}
