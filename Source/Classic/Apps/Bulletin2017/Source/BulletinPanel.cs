// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListPanel.cs -- 
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Configuration;
using AM.Data;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Reflection;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;
using AM.UI;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Bulletin2017
{
    public partial class BulletinPanel
        : UniversalCentralControl
    {
        #region Properties

        /// <summary>
        /// Busy state controller.
        /// </summary>
        [NotNull]
        public BusyController Controller
        {
            get
            {
                return MainForm
                    .ThrowIfNull("MainForm")
                    .Controller
                    .ThrowIfNull("MainForm.Controller");
            }
        }

        [NotNull]
        public IIrbisConnection Connection
        {
            get { return GetConnection(); }
        }

        public DateTime Period
        {
            get { return _period; }
            set
            {
                _period = value;
                _periodLabel.Text = value.ToString("Y");
            }
        }

        #endregion

        #region Construction

        public BulletinPanel()
            : base(null)
        {
            InitializeComponent();
        }

        public BulletinPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            mainForm.Icon = Properties.Resources.Bulletin;
            Period = DateTimeUtility.ThisMonth;
        }

        #endregion

        #region Private members

        private DateTime _period;

        [NotNull]
        private IIrbisConnection GetConnection()
        {
            UniversalForm mainForm = MainForm.ThrowIfNull("MainForm");
            mainForm.GetIrbisProvider();
            IIrbisConnection result = mainForm.Connection
                .ThrowIfNull("connection");

            return result;
        }

        private void _prevMonthButton_Click(object sender, EventArgs e)
        {
            Period = Period.AddMonths(-1);
        }

        private void _nextMonthButton_Click(object sender, EventArgs e)
        {
            Period = Period.AddMonths(1);
        }

        #endregion
    }
}
