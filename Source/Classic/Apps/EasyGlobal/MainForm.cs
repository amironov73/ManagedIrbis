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
using AM.Text.Output;
using AM.Windows.Forms;

#endregion

namespace EasyGlobal
{
    public partial class MainForm 
        : Form
    {
        #region Properties

        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();

            _console.Clear();
            Output = new ConsoleControlOutput(_console);
        }

        #endregion

        #region Private members

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            //Output.WriteLine("Started at: {0}", DateTime.Now);
        }

        #endregion
    }
}
