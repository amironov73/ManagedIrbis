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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Environment;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace PftBench
{
    public partial class MainForm
        : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private MarcRecord _record;
        private PftTokenList _tokenList;
        private PftProgram _program;

        private void Clear()
        {
            _resutlBox.Clear();
            _pftTreeView.Clear();
            _tokenGrid.Clear();
            _recordGrid.Clear();
            _varsGrid.Clear();
        }

        private void Parse()
        {
            string recordText = _recordBox.Text;
            StringReader reader = new StringReader(recordText);
            _record = PlainText.ReadRecord(reader);
            _recordGrid.SetRecord(_record);

            PftLexer lexer = new PftLexer();
            _tokenList = lexer.Tokenize(_pftBox.Text);

            _tokenGrid.SetTokens(_tokenList);

            PftParser parser = new PftParser(_tokenList);
            _program = parser.Parse();
            _pftTreeView.SetNodes(_program);
        }

        private void Run()
        {
            PftFormatter formatter = new PftFormatter
            {
                Program = _program
            };

            string rootPath = CM.AppSettings["rootPath"];
            PftEnvironmentAbstraction environment
                = new PftLocalEnvironment
                    (
                        rootPath
                    );
            formatter.SetEnvironment(environment);

            string result = formatter.Format(_record);
            _resutlBox.Text = result;
            _recordGrid.SetRecord(_record);

            _varsGrid.SetVariables(formatter.Context.Variables);
        }

        private void _goButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Clear();
                Parse();
                Run();
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }
        }

        private void _parseButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Clear();
                Parse();
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }
        }

        private void MainForm_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyCode)
            {
                case Keys.F4:
                    _parseButton_Click(sender, e);
                    break;

                case Keys.F5:
                    _goButton_Click(sender, e);
                    break;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F4:
                    _parseButton_Click(sender, e);
                    e.Handled = true;
                    break;

                case Keys.F5:
                    _goButton_Click(sender, e);
                    e.Handled = true;
                    break;
            }
        }

    }
}
