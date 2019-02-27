// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.Text;
using AM.Text.Output;

using DevExpress.XtraSpreadsheet;
using IrbisUI;
using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using CM = System.Configuration.ConfigurationManager;
using DataGridViewRow = System.Windows.Forms.DataGridViewRow;

#endregion

// ReSharper disable StringLiteralTypo

namespace FastGazettes
{
    public partial class MainForm
        : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
    }
}
