// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HairbrushPanel.cs -- 
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
using ManagedIrbis.Search;
using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Hairbrush
{
    public partial class HairbrushPanel
        : UniversalCentralControl
    {
        #region Properties

        [NotNull]
        public BusyController Controller
        {
            get { return MainForm.Controller; }
        }

        [NotNull]
        public string Prefix { get; set; }

        [CanBeNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        public HairbrushPanel()
            : base(null)
        {
            InitializeComponent();

            Prefix = "A=";
            _clearButton_Click(this, EventArgs.Empty);
        }

        public HairbrushPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            Prefix = "A=";
            _clearButton_Click(this, EventArgs.Empty);
        }

        #endregion

        #region Private members



        #endregion

        #region Public methods

        [NotNull]
        public IrbisProvider GetProvider()
        {
            //return MainForm.Provider;
            return MainForm.GetIrbisProvider();
        }

        public void ReleaseProvider()
        {
            MainForm.ReleaseProvider();
            Provider = null;
        }

        #endregion

        private void _keyBox_ButtonClick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                Provider = GetProvider();

                string startTerm = Prefix + _keyBox.Text;
                TermParameters parameters = new TermParameters
                {
                    Database = Provider.Database,
                    StartTerm = startTerm,
                    NumberOfTerms = 100
                };
                TermData[] termData = null;
                Controller.Run
                    (
                        () =>
                        {
                            TermInfo[] rawTerms
                                = Provider.ReadTerms(parameters);
                            termData = TermData.FromRawTerms
                            (
                                rawTerms,
                                Prefix
                            );
                        }
                    );
                _termDataBindingSource.DataSource = termData;
            }
            finally
            {
                ReleaseProvider();
            }
        }

        private void _keyBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                _keyBox_ButtonClick(sender, e);
            }
        }

        private void _termGrid_CellContentClick
            (
                object sender,
                DataGridViewCellEventArgs e
            )
        {
            DataGridView grid = sender as DataGridView;
            if (ReferenceEquals(grid, null))
            {
                return;
            }
            if (e.ColumnIndex < 0
                || e.ColumnIndex >= grid.ColumnCount
                || e.RowIndex < 0
                || e.RowIndex >= grid.RowCount)
            {
                return;
            }
            DataGridViewButtonColumn column
                = grid.Columns[e.ColumnIndex] as DataGridViewButtonColumn;
            if (ReferenceEquals(column, null))
            {
                return;
            }
            if (ReferenceEquals(grid.CurrentRow, null))
            {
                return;
            }
            TermData term = grid.CurrentRow.DataBoundItem as TermData;
            if (ReferenceEquals(term, null))
            {
                return;
            }
            string text = term.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            string familyName = AuthorInfo.ExtractFamilyName(text);
            if (string.IsNullOrEmpty(familyName))
            {
                return;
            }

            _propertyGrid.SelectedObject = null;

            string query = Prefix + text;
            try
            {
                Provider = GetProvider();
                AuthorInfo theAuthor = null;
                Controller.Run
                    (
                        () =>
                        {
                            TermLink[] links
                                = Provider.ExactSearchLinks(query);
                            if (links.Length != 0)
                            {
                                int mfn = links[0].Mfn;
                                MarcRecord record
                                    = Provider.ReadRecord(mfn);
                                if (!ReferenceEquals(record, null))
                                {
                                    AuthorInfo[] authors
                                        = AuthorInfo.ParseRecord
                                        (
                                            record,
                                            AuthorInfo.AllKnownTags
                                        );
                                    theAuthor = authors.FirstOrDefault
                                        (
                                            a => a.FamilyName
                                                .SameString(familyName)
                                        );
                                }
                            }
                        }
                    );

                _propertyGrid.SelectedObject = theAuthor;
                WriteLine
                    (
                        "Задан эталон: {0}",
                        theAuthor.Field
                    );
            }
            finally
            {
                ReleaseProvider();
            }
        }

        private void _applyButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            AuthorInfo ethalon = _propertyGrid.SelectedObject as AuthorInfo;
            if (ReferenceEquals(ethalon, null))
            {
                WriteLine("Не задан эталон");
                return;
            }
            string familyName = ethalon.FamilyName;
            if (string.IsNullOrEmpty(familyName))
            {
                WriteLine("Эталон пуст");
                return;
            }
            TermData[] terms = _termDataBindingSource.DataSource as TermData[];
            if (ArrayUtility.IsNullOrEmpty(terms))
            {
                WriteLine("Нет записей для коррекции");
                return;
            }
            string[] selectedTerms = terms
                .Where(term => term.Selected)
                .Select(term => term.Text)
                .ToArray();
            if (ArrayUtility.IsNullOrEmpty(selectedTerms))
            {
                WriteLine("Нет записей для коррекции");
                return;
            }

            StringBuilder query = new StringBuilder();
            bool first = true;
            foreach (string term in selectedTerms)
            {
                if (!first)
                {
                    query.Append(" + ");
                }
                query.AppendFormat
                    (
                        "\"{0}{1}\"",
                        Prefix,
                        term
                    );

                first = false;
            }

            try
            {
                Provider = GetProvider();
                Controller.Run
                    (
                        () =>
                        {
                            int[] found = Provider.Search(query.ToString());
                            WriteLine
                                (
                                    "Отобрано для коррекции: {0}",
                                    found.Length
                                );
                            foreach (int mfn in found)
                            {
                                MarcRecord record
                                    = Provider.ReadRecord(mfn);
                                if (!ReferenceEquals(record, null))
                                {
                                    AuthorInfo[] authors
                                        = AuthorInfo.ParseRecord
                                        (
                                            record,
                                            AuthorInfo.AllKnownTags
                                        );
                                    AuthorInfo[] selected = authors.Where
                                        (
                                            a => a.FamilyName
                                                .SameString(familyName)
                                        )
                                        .ToArray();
                                    if (selected.Length == 0)
                                    {
                                        WriteLine
                                            (
                                                "MFN {0} пропущен",
                                                mfn
                                            );
                                    }
                                    else
                                    {
                                        foreach (AuthorInfo theAuthor in selected)
                                        {
                                            WriteLine
                                                (
                                                    "MFN {0}: {1}",
                                                    mfn,
                                                    theAuthor.Field
                                                );
                                            ethalon.ApplyToField
                                                (
                                                    theAuthor.Field
                                                        .ThrowIfNull("theAuthor.Field")
                                                );
                                        }
                                        Provider.WriteRecord(record);
                                    }
                                }
                            }
                        }
                    );
            }
            finally
            {
                ReleaseProvider();
            }

            _keyBox_ButtonClick(sender, e);
            WriteLine("Обработка завершена");
        }

        private void _clearButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _propertyGrid.SelectedObject = new AuthorInfo();
        }
    }
}
