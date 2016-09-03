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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace UITests
{
    public partial class MainForm
        : Form
    {
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
    }
}
