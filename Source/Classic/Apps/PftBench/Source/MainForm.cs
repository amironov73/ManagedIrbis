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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
using CodeJam;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

using IrbisUI;
using IrbisUI.Pft;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

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

            string rootPath = CM.AppSettings["rootPath"];
            _provider = new LocalProvider
                    (
                        rootPath
                    );
        }

        private MarcRecord _record;
        private PftTokenList _tokenList;
        private PftProgram _program;
        private readonly IrbisProvider _provider;

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

        private void ParseHtml()
        {
            string recordText = _recordBox.Text;
            StringReader reader = new StringReader(recordText);
            _record = PlainText.ReadRecord(reader);
            _recordGrid.SetRecord(_record);

            PftHtmlFormatter formatter = new PftHtmlFormatter();
            formatter.ParseProgram(_pftBox.Text);
            _program = formatter.Program;
            _pftTreeView.SetNodes(_program);
        }

        private void Run()
        {
            PftFormatter formatter = new PftFormatter
            {
                Program = _program
            };

            PftUiDebugger debugger
                = new PftUiDebugger(formatter.Context);
            formatter.Context.Debugger = debugger;

            DatabaseInfo database = _databaseBox.SelectedItem
                as DatabaseInfo;
            if (!ReferenceEquals(database, null))
            {
                _provider.Database = database.Name
                    .ThrowIfNull("database.Name");
            }
            formatter.SetProvider(_provider);

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
            try
            {
                _htmlBox.DocumentText =
                    "<html>"
                    + result
                    + "</html>";
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Nothing to do
            }

            _recordGrid.SetRecord(_record);

            _warningBox.Text = formatter.Warning;

            _varsGrid.SetVariables(formatter.Context.Variables);
            _globalsGrid.SetGlobals(formatter.Context.Globals);
        }


        private void CompileAndRun()
        {
            DatabaseInfo database = _databaseBox.SelectedItem
                as DatabaseInfo;
            if (!ReferenceEquals(database, null))
            {
                _provider.Database = database.Name
                    .ThrowIfNull("database.Name");
            }

            PftContext context = new PftContext(null);
            context.SetProvider(_provider);
            PftProgram program = (PftProgram)_program.Clone();
            program.Optimize();

            if (!Directory.Exists("Out"))
            {
                Directory.CreateDirectory("Out");
            }

            PftCompiler compiler = new PftCompiler
            {
                Debug = true,
                KeepSource = true,
                OutputPath = "Out"
            };
            string className = compiler.CompileProgram(program);
            AbstractOutput output = new TextOutput();
            string assemblyPath = compiler.CompileToDll
                (
                    output,
                    className
                );
            string result = string.Empty;
            if (!ReferenceEquals(assemblyPath, null))
            {
                Assembly assembly
                    = Assembly.LoadFile(assemblyPath);
                Func<PftContext, PftPacket> creator
                    = CompilerUtility.GetEntryPoint(assembly);
                PftPacket packet = creator(context);
                result = packet.Execute(_record);
            }

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
            try
            {
                _htmlBox.DocumentText =
                    "<html>"
                    + result
                    + "</html>";
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Nothing to do
            }

            _recordGrid.SetRecord(_record);
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

                case Keys.F6:
                    _prettyPrintButton_Click(sender, e);
                    break;

                case Keys.F7:
                    _compileButton_Click(sender, e);
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

                case Keys.F6:
                    _prettyPrintButton_Click(sender, e);
                    break;

                case Keys.F7:
                    _compileButton_Click(sender, e);
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
                = _splitContainer1.Height / 2;
            _splitContainer2.SplitterDistance
                = _splitContainer2.Width / 2;
            _splitContainer3.SplitterDistance
                = _splitContainer3.Width / 2;

            DatabaseInfo[] databases = _provider.ListDatabases();
            _databaseBox.Items.AddRange(databases);
            if (databases.Length != 0)
            {
                _databaseBox.SelectedIndex = 0;
            }
        }

        private void _databaseBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            DatabaseInfo database = _databaseBox.SelectedItem as DatabaseInfo;

            if (!ReferenceEquals(database, null))
            {
                _provider.Database = database.Name.ThrowIfNull();
                int maxMfn = _provider.GetMaxMfn();

                _maxMfnLabel.Text = string.Format
                    (
                        "Max MFN={0}",
                        maxMfn
                    );
            }
        }

        private void _newButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _pftBox.Text = string.Empty;
        }

        private void _openButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string text = File.ReadAllText
                    (
                        _openFileDialog.FileName,
                        IrbisEncoding.Ansi
                    );
                _pftBox.Text = text;
            }
        }

        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string text = _pftBox.Text;
                string fileName = _saveFileDialog.FileName;

                File.WriteAllText
                    (
                        fileName,
                        text,
                        IrbisEncoding.Ansi
                    );
            }
        }

        private void _tokenGrid_CellDoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            TextAreaControl _editor = _pftBox.ActiveTextAreaControl;

            PftTokenGrid grid = sender as PftTokenGrid;
            if (ReferenceEquals(grid, null))
            {
                return;
            }

            PftToken token = grid.SelectedToken;
            if (ReferenceEquals(token, null))
            {
                return;
            }

            string text = token.Text;
            int line = token.Line - 1, column = token.Column - 1;
            if (line < 0
                || string.IsNullOrEmpty(text))
            {
                return;
            }

            TextLocation start = new TextLocation(column, line);
            TextLocation end = new TextLocation(column + text.Length, line);
            _editor.SelectionManager.SetSelection(start, end);
            _editor.Caret.Position = start;
        }

        private void _pftTreeView_CurrentNodeChanged
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            PftNodeInfo currentNode = _pftTreeView.CurrentNode;
            if (ReferenceEquals(currentNode, null))
            {
                return;
            }

            PftNode pftNode = currentNode.Node;
            if (ReferenceEquals(pftNode, null))
            {
                return;
            }

            string text = pftNode.Text ?? string.Empty;
            int line = pftNode.LineNumber - 1;
            int column = pftNode.Column - 1;
            if (line < 0 || column < 0)
            {
                return;
            }

            TextLocation start = new TextLocation(column, line);
            TextLocation end = new TextLocation(column + text.Length, line);
            TextAreaControl editor = _pftBox.ActiveTextAreaControl;
            editor.SelectionManager.SetSelection(start, end);
            editor.Caret.Position = start;
        }

        private void _pftTreeView_NodeChecked
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            TreeNode treeNode = e.Node;
            if (ReferenceEquals(treeNode, null))
            {
                return;
            }

            PftNodeInfo nodeInfo = treeNode.Tag as PftNodeInfo;
            if (ReferenceEquals(nodeInfo, null))
            {
                return;
            }

            PftNode node = nodeInfo.Node;
            if (ReferenceEquals(node, null))
            {
                return;
            }

            node.Breakpoint = treeNode.Checked;
        }

        private void _goHtmlButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                ParseHtml();
                Run();
                _outputTabControl.SelectTab(_htmlPage);
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }
        }

        private void _prettyPrintButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Clear();
                Parse();

                string text = _program.ToString();
                _pftBox.Text = text;
                _pftBox.Refresh();
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }
        }

        private void _compileButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Clear();
                Parse();
                CompileAndRun();
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }

        }
    }
}
