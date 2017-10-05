// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChooseReaderForm.cs -- 
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
using System.Windows.Forms;

using ManagedIrbis.Readers;

#endregion

namespace AM.Istu
{
    public partial class ChooseReaderForm 
        : Form
    {
        public ChooseReaderForm()
        {
            InitializeComponent();
        }


        public static ReaderInfo ChooseReader 
            (
                IWin32Window owner,
                IEnumerable<ReaderInfo> readers
            )
        {
            using (ChooseReaderForm form = new ChooseReaderForm ())
            {
                form._grid.AutoGenerateColumns = false;
                form._bindingSource.DataSource = readers;
                form._grid.DataSource = form._bindingSource;

                DialogResult result = form.ShowDialog (owner);

                if (result == DialogResult.OK)
                {
                    return (ReaderInfo) form._bindingSource.Current;
                }

                return null;
            }
        }
    }
}
