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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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

                PftParser parser = new PftParser(tokenList);
                PftProgram program = parser.Parse();
                PftFormatter formatter = new PftFormatter
                {
                    Program = program
                };
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
