#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Design;
using DevExpress.XtraRichEdit.Forms.Design;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraRichEdit.UI;

using ManagedIrbis;

using ParagraphAlignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment;
using ParagraphFirstLineIndent = DevExpress.XtraRichEdit.API.Native.ParagraphFirstLineIndent;
using ParagraphStyle = DevExpress.XtraRichEdit.API.Native.ParagraphStyle;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace Cronicle
{
    public partial class MainForm
        : XtraForm
    {
        private readonly RichEditControl _richEdit;
        private Document _document;
        private ParagraphStyle _header;
        private ParagraphStyle _para;
        private ParagraphStyle _noIndent;
        private ParagraphStyle _small;

        public MainForm()
        {
            InitializeComponent();

            _richEdit = new RichEditControl();
            toolStripContainer1.ContentPanel.Controls.Add(_richEdit);
            _richEdit.Dock = DockStyle.Fill;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _FillYears();
            _CreateDocument();
        }

        private void _CreateDocument()
        {
            _richEdit.CreateNewDocument();
            _document = _richEdit.Document;

            _header = _document.ParagraphStyles.CreateNew();
            _header.Name = "Заголовочек";
            _header.KeepLinesTogether = true;
            _header.OutlineLevel = 1;
            _header.SuppressHyphenation = true;
            _header.Alignment = ParagraphAlignment.Center;
            _header.FirstLineIndent = 0.0f;
            _header.SpacingBefore = 36f;
            _header.SpacingAfter = 36f;
            _header.FontName = "Times New Roman";
            _header.FontSize = 14f;
            _header.Bold = true;
            _document.ParagraphStyles.Add(_header);

            _para = _document.ParagraphStyles.CreateNew();
            _para.Name = "Текстик";
            _para.KeepLinesTogether = true;
            _para.OutlineLevel = 2;
            _para.SuppressHyphenation = false;
            _para.Alignment = ParagraphAlignment.Justify;
            _para.FirstLineIndentType = ParagraphFirstLineIndent.Indented;
            _para.FirstLineIndent = 100f;
            _para.SpacingBefore = 36f;
            _para.SpacingAfter = 0.0f;
            _para.FontName = "Times New Roman";
            _para.FontSize = 9.5f;
            _document.ParagraphStyles.Add(_para);

            _noIndent = _document.ParagraphStyles.CreateNew();
            _noIndent.Name = "Без отступчика";
            _noIndent.SuppressHyphenation = true;
            _noIndent.Alignment = ParagraphAlignment.Left;
            _noIndent.FirstLineIndentType = ParagraphFirstLineIndent.Indented;
            _noIndent.FirstLineIndent = 100f;
            _noIndent.SpacingBefore = 0.0f;
            _noIndent.SpacingAfter = 0.0f;
            _noIndent.FontName = "Times New Roman";
            _noIndent.FontSize = 8f;
            _document.ParagraphStyles.Add(_noIndent);

            _small = _document.ParagraphStyles.CreateNew();
            _small.Name = "Малюсенький";
            _small.SuppressHyphenation = true;
            _small.Alignment = ParagraphAlignment.Left;
            _small.FirstLineIndentType = ParagraphFirstLineIndent.None;
            _small.FirstLineIndent = 0.0f;
            _small.SpacingBefore = 0.0f;
            _small.SpacingAfter = 0.0f;
            _small.FontName = "Times New Roman";
            _small.FontSize = 8f;
            _document.ParagraphStyles.Add(_small);

            //WritePara("Привет, мир", string.Empty, string.Empty);
        }

        private void WriteH1
            (
                string text
            )
        {
            _document.Paragraphs.Append();
            var range = _document.AppendText(text);
            var subDocument = range.BeginUpdateDocument();
            var properties = subDocument.BeginUpdateParagraphs(range);
            properties.Style = _header;
            subDocument.EndUpdateParagraphs(properties);
            subDocument.EndUpdate();
        }

        private void WritePara
            (
                string start,
                string bold,
                string end
            )
        {
            _document.Paragraphs.Append();
            var range = _document.AppendText(start);
            var subDocument = range.BeginUpdateDocument();
            var properties = subDocument.BeginUpdateParagraphs(range);
            properties.Style = _para;
            subDocument.EndUpdateParagraphs(properties);
            subDocument.EndUpdate();
            if (!string.IsNullOrEmpty(bold))
            {
                var range2 = _document.AppendText(bold);
                var subDocument2 = range2.BeginUpdateDocument();
                var properties2 = subDocument2.BeginUpdateCharacters(range2);
                properties2.Bold = new bool?(true);
                subDocument2.EndUpdateCharacters(properties2);
                subDocument2.EndUpdate();
            }

            if (string.IsNullOrEmpty(end))
            {
                return;
            }

            var range3 = _document.AppendText(end);
            var subDocument3 = range3.BeginUpdateDocument();
            var properties3 = subDocument3.BeginUpdateCharacters(range3);
            properties3.Bold = new bool?(false);
            subDocument3.EndUpdateCharacters(properties3);
            subDocument3.EndUpdate();
        }

        private void WriteNoIndent(string text)
        {
            _document.Paragraphs.Append();
            var range = _document.AppendText(text);
            var subDocument = range.BeginUpdateDocument();
            var properties = subDocument.BeginUpdateParagraphs(range);
            properties.Style = _noIndent;
            subDocument.EndUpdateParagraphs(properties);
            subDocument.EndUpdate();
        }

        private void WriteSmall(string text)
        {
            _document.Paragraphs.Append();
            var range = _document.AppendText(text);
            var subDocument = range.BeginUpdateDocument();
            var properties = subDocument.BeginUpdateParagraphs(range);
            properties.Style = _small;
            subDocument.EndUpdateParagraphs(properties);
            subDocument.EndUpdate();
        }

        private void _FillYears()
        {
            const int capacity = 20;
            var thisYear = DateTime.Today.Year;
            _yearBox.Items.Add(thisYear + 1);
            for (var i = 0; i < capacity; i++)
            {
                var year = thisYear - i;
                _yearBox.Items.Add(year);
            }

            _yearBox.SelectedIndex = 2;
        }

        private One[] _DoTheSearch()
        {
            var year = _yearBox.SelectedItem;
            var connectionString = CM.AppSettings["connection-string"];
            using (var client = new IrbisConnection(connectionString))
            {
                var found = client.SearchRead($"KNL={year} ^ V=ZL");
                var result = new List<One>(found.Length);
                foreach (var record in found)
                {
                    var one = new One
                    {
                        Mfn = record.Mfn,
                        Record = record
                    };
                    result.Add(one);
                }

                var formatted = client.FormatRecords
                    (
                        client.Database,
                        "@magna",
                        result.Select(o => o.Mfn).ToArray()
                    );
                for (var i = 0; i < formatted.Length; i++)
                {
                    var one = result[i];
                    one.Description = formatted[i];
                    one.Cleaned = RichTextStripper
                        .StripRichTextFormat(one.Description)
                        ?.Replace("\"", string.Empty);
                }

                return result.ToArray();
            }
        }

        private void _DoTheWork()
        {
            _CreateDocument();
            var all = _DoTheSearch();
            var sorted = all.OrderBy(one => one.Cleaned);
            foreach (var one in sorted)
            {
                _document.Paragraphs.Append();
                var text = @"{\rtf1"
                           + $"{{\\ul MFN {one.Mfn}.}} "
                           + one.Description
                           + "}";
                _document.AppendRtfText(text);
                _document.Paragraphs.Append();
            }
        }

        private void _goButton_Click(object sender, EventArgs e)
        {
            _DoTheWork();
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            _richEdit.SaveDocumentAs(this);
        }
    }
}
