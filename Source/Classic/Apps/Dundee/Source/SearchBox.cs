using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;

namespace Dundee.Source
{
    public partial class SearchBox
        : UserControl
    {
        public event EventHandler Found;

        public FoundItem[] Records;

        public SearchBox()
        {
            InitializeComponent();
        }

        private SearchScenario[] _scenarios;

        public IrbisConnection GetConnection()
        {
            return Program.Connection.ThrowIfNull("connection");
        }

        public SearchScenario GetScenario()
        {
            return (_prefixBox.SelectedItem as SearchScenario)
                .ThrowIfNull("scenario");
        }

        public string GetInput()
        {
            return _inputBox.Text;
        }

        public string GetTerm()
        {
            SearchScenario scenario = GetScenario();
            TermInfo term = _termBox.SelectedItem as TermInfo;
            string result = "\"" + scenario.Prefix + term?.Text + "\"";
            return result;
        }

        public void Setup()
        {
            IrbisConnection connection = GetConnection();
            _scenarios = SearchScenario.LoadSearchScenarios
                (
                    connection,
                    connection.Database
                );
            if (_scenarios.IsNullOrEmpty())
            {
                throw new IrbisException();
            }
            _prefixBox.Items.AddRange(_scenarios);
            _prefixBox.SelectedIndex = 0;
        }

        public void LoadTerms()
        {
            SearchScenario scenario = GetScenario();
            string prefix = scenario.Prefix ?? string.Empty;
            string input = GetInput();
            string startTerm = $"{prefix}{input}";

            IrbisConnection connection = GetConnection();

            TermParameters parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = startTerm,
                NumberOfTerms = 120
            };
            TermInfo[] terms = connection.ReadTerms(parameters);
            _termBox.Items.Clear();
            terms = TermInfo.TrimPrefix(terms, prefix);
            _termBox.Items.AddRange(terms);
        }

        public void DoSearch()
        {
            Records = EmptyArray<FoundItem>.Value;
            string expression = GetTerm();
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            IrbisConnection connection = GetConnection();
            SearchParameters parameters = new SearchParameters
            {
                Database = connection.Database,
                SearchExpression = expression,
                FormatSpecification = "@brief"
            };

            SearchCommand command = connection.CommandFactory.GetSearchCommand();
            command.ApplyParameters(parameters);
            connection.ExecuteCommand(command);
            List<FoundItem> found = command.Found.ThrowIfNull("found");
            Records = found.OrderBy(item => item.Text).ToArray();
            Found?.Invoke(this, EventArgs.Empty);
        }

        private void _inputBox_TextChanged(object sender, EventArgs e)
        {
            LoadTerms();
        }

        private void _prefixBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTerms();
        }

        private void _searchButton_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void _termBox_DoubleClick(object sender, EventArgs e)
        {
            DoSearch();
        }
    }
}
