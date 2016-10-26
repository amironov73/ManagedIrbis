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

using CM=System.Configuration.ConfigurationManager;

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

        private void _goButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                _resutlBox.Clear();

                string recordText = _recordBox.Text;
                StringReader reader = new StringReader(recordText);
                MarcRecord record = PlainText.ReadRecord(reader)
                    .ThrowIfNull("record!");

                PftLexer lexer = new PftLexer();
                PftTokenList tokenList = lexer.Tokenize(_pftBox.Text);

                _tokenGrid.SetTokens(tokenList);

                PftParser parser = new PftParser(tokenList);
                PftProgram program = parser.Parse();

                _pftTreeView.SetNodes(program);

                PftFormatter formatter = new PftFormatter
                {
                    Program = program
                };

                string rootPath = CM.AppSettings["rootPath"];
                PftEnvironmentAbstraction environment
                    = new PftLocalEnvironment
                        (
                            rootPath
                        );
                formatter.SetEnvironment(environment);

                string result = formatter.Format(record);
                _resutlBox.Text = result;
            }
            catch (Exception exception)
            {
                _resutlBox.Text = exception.ToString();
            }
        }
    }
}
