using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AM.Configuration;

using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace KpioChronicle
{
    public partial class MainForm : RibbonForm
    {
        private string _conectionString, _format;

        public MainForm()
        {
            InitializeComponent();
            InitializeRichEditControl();
        }

        void InitializeRichEditControl()
        {
        }

        private void _buildButtonItem_ItemClick
            (
                object sender,
                ItemClickEventArgs e
            )
        {
            var expression = string.Empty;
            var prefix = _prefixEditItem.EditValue as string ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                expression = prefix;
            }

            var opName = _operatorEditItem.EditValue as string ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(opName))
            {
                expression = expression + opName + "-";
            }

            var stage = _stageEditItem.EditValue as string ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(stage))
            {
                expression = expression + stage + "-";
            }

            var year = Convert.ToInt32(_yearEditItem.EditValue);
            expression += year.ToString(CultureInfo.InvariantCulture);

            var month = Convert.ToInt32(_monthEditItem.EditValue);
            expression += month.ToString("00", CultureInfo.InvariantCulture);

            using (var connection = new IrbisConnection(_conectionString))
            {
                var found = connection
                    .SearchFormat(expression, _format)
                    .Select(item => item.Text)
                    .OrderBy(line => line)
                    .ToArray();
                var builder = new StringBuilder(100 * 1024);
                builder.AppendLine(@"{\rtf1");

                var index = 1;
                foreach (var item in found)
                {
                    builder.AppendFormat(@"{{\b {0}) }}", index++);
                    builder.Append("{");
                    builder.AppendLine(item);
                    builder.AppendLine("}");
                    builder.AppendLine(@"\par\pard");
                    builder.AppendLine(@"\par\pard");
                }
                builder.Append("}");

                _richEditControl.RtfText = builder.ToString();
            }
        }

        private void InitialFill()
        {
            using (var connection = new IrbisConnection(_conectionString))
            {
                var ini = connection.ReadIniFile("ibis.ini");
                var scenarios = SearchScenario.ParseIniFile(ini);
                _prefixComboBox.Items.AddRange(scenarios);

                var users = connection.ListUsers();
                var logins = users.Select(user => user.Name)
                    .OrderBy(user => user)
                    .ToArray();
                _operatorComboBox.Items.AddRange(logins);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _ribbonControl.SelectPage(_kpioPage);
            _conectionString = ConfigurationUtility.GetString("irbis");
            _format = ConfigurationUtility.GetString("format");

            InitialFill();
        }
    }
}