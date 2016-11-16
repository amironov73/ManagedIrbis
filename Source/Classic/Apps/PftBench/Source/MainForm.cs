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
using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Pft.Infrastructure;

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
            _globalsGrid.Clear();
            _rtfBox.Clear();
            _htmlBox.DocumentText = string.Empty;
            _warningBox.Clear();
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
            AbstractClient environment = new LocalClient
                    (
                        rootPath
                    );
            formatter.SetEnvironment(environment);

            string result = formatter.Format(_record);
            _resutlBox.Text = result;
            try
            {
                _rtfBox.Rtf = result;
            }
            catch
            {
                _rtfBox.Text = result;
            }

            if (ReferenceEquals(_htmlBox.Document, null))
            {
                _htmlBox.Navigate("about:blank");
                while (_htmlBox.IsBusy)
                {
                    Application.DoEvents();
                }
            }
            if (!ReferenceEquals(_htmlBox.Document, null))
            {
                _htmlBox.Document.Write(result);
            }
            //_htmlBox.DocumentText = result;

            _recordGrid.SetRecord(_record);

            _warningBox.Text = formatter.Warning;

            _varsGrid.SetVariables(formatter.Context.Variables);
            _globalsGrid.SetGlobals(formatter.Context.Globals);
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
            switch (e.KeyData)
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
            switch (e.KeyData)
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

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            _splitContainer1.SplitterDistance 
                = _splitContainer1.Height/2;
            _splitContainer2.SplitterDistance
                = _splitContainer2.Width/2;
            _splitContainer3.SplitterDistance
                = _splitContainer3.Width/2;
        }
    }
}
