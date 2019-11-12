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

using AM;
using AM.Text;

using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

using ManagedIrbis;
using ManagedIrbis.Identifiers;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ParagraphAlignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment;
using ParagraphFirstLineIndent = DevExpress.XtraRichEdit.API.Native.ParagraphFirstLineIndent;
using ParagraphStyle = DevExpress.XtraRichEdit.API.Native.ParagraphStyle;
using SectionStartType = DevExpress.XtraRichEdit.API.Native.SectionStartType;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo

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

        private MenuFile _udk;

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

        private void _DoTheDict
            (
                Entry[] array,
                string title,
                string separator,
                Func<Entry, IEnumerable<string>> selector
            )
        {
            var dict = new List<DictEntry>();
            foreach (var entry in array)
            {
                foreach (var value in selector(entry))
                {
                    var dictEntry = new DictEntry
                    {
                        Index = entry.Index,
                        Value = value
                    };
                    dict.Add(dictEntry);
                }
            }

            var section = _document.AppendSection();
            section.StartType = SectionStartType.NextPage;
            WriteH1(title);
            _document.Paragraphs.Append();
            section = _document.AppendSection();
            section.StartType = SectionStartType.Continuous;
            var columns = section.Columns.CreateUniformColumns(section.Page, 100f, 2);
            section.Columns.SetColumns(columns);
            var builder = new StringBuilder();

            var ordered = dict.GroupBy(e => e.Value).OrderBy(g => g.Key);
            foreach (var group in ordered)
            {
                builder.Append(@"\par ");
                builder.Append(group.Key);
                builder.Append(separator);
                var first = true;
                foreach (var i in group)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(i.Index.ToInvariantString());
                    first = false;
                }
            }

            _document.AppendRtfText(@"{\rtf1 " + builder + "}");

            section = _document.AppendSection();
            section.StartType = SectionStartType.Continuous;
        }

        private void _DoTheAuthor
            (
                Entry entry,
                int tag
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var familyName = field.GetFirstSubFieldValue('a');
                var initials = field.GetFirstSubFieldValue('b');
                var author = string.IsNullOrEmpty(initials)
                    ? familyName
                    : familyName + " " + initials;
                entry.Authors.Add(author);
            }
        }

        private void _DoTheGeo
            (
                Entry entry,
                int tag,
                char code
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var place = field.GetFirstSubFieldValue(code);
                if (!string.IsNullOrEmpty(place))
                {
                    place = place.TrimStart('[').TrimEnd(']');
                    entry.Geo.Add(place);
                }
            }
        }

        private void _DoThePublisher
            (
                Entry entry,
                int tag,
                char code
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var publisher = field.GetFirstSubFieldValue(code);
                if (!string.IsNullOrEmpty(publisher))
                {
                    publisher = publisher.TrimStart('[').TrimEnd(']');
                    entry.Publishers.Add(publisher);
                }
            }
        }

        private void _DoTheIsbn
            (
                Entry entry,
                int tag,
                char code1,
                char code2
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var isbn = field.GetFirstSubFieldValue(code1);
                if (!string.IsNullOrEmpty(isbn))
                {
                    if (!Isbn.CheckControlDigit(isbn))
                    {
                        entry.Isbn.Add(isbn);
                    }
                }

                isbn = field.GetFirstSubFieldValue(code2);
                if (!string.IsNullOrEmpty(isbn))
                {
                    entry.Isbn.Add(isbn);
                }
            }
        }

        private Entry _DoTheEntry
            (
                MarcRecord record
            )
        {
            var result = new Entry
            {
                Mfn = record.Mfn,
                Record = record,
                Bbk = record.FM(621),
                Udc = record.FM(675),
                Language = record.FM(101)
            };

            _DoTheAuthor(result, 700);
            _DoTheAuthor(result, 701);
            _DoTheAuthor(result, 702);
            _DoTheAuthor(result, 961);

            _DoTheGeo(result, 210, 'a');
            _DoTheGeo(result, 461, 'd');

            _DoThePublisher(result, 210, 'c');
            _DoThePublisher(result, 461, 'g');

            _DoTheIsbn(result, 10, 'a', 'z');
            _DoTheIsbn(result, 461, 'i', '1');

            return result;
        }

        private Entry[] _DoTheSearch()
        {
            var year = _yearBox.SelectedItem;
            var connectionString = CM.AppSettings["connection-string"];
            using (var client = new IrbisConnection(connectionString))
            {
                var spec = new FileSpecification(IrbisPath.MasterFile, client.Database, "udk.mnu");
                _udk = MenuFile.ReadFromServer(client, spec);
                var found = client.SearchRead($"KNL={year} ^ V=ZL");
                var result = new List<Entry>(found.Length);
                foreach (var record in found)
                {
                    var one = _DoTheEntry(record);
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
            var chunks = new List<Chunk>();
            foreach (var group in all.GroupBy(one => one.Udc).OrderBy(g => g.Key))
            {
                var chunk = new Chunk
                {
                    Key = group.Key,
                    Entries = group.OrderBy(one => one.Cleaned).ToArray()
                };
                chunks.Add(chunk);
            }

            var index = 1;
            foreach (var chunk in chunks)
            {
                foreach (var entry in chunk.Entries)
                {
                    entry.Index = index;
                    index++;
                }
            }

            foreach (var chunk in chunks)
            {
                _document.Paragraphs.Append();
                WriteH1(chunk.Key);
                _document.Paragraphs.Append();
                foreach (var entry in chunk.Entries)
                {
                    var text = @"{\rtf1 "
                               + $"{entry.Index}.\\~"
                               + $"{{\\ul MFN {entry.Mfn}.}} "
                               + entry.Description
                               + "}";
                    _document.AppendRtfText(text);
                    _document.Paragraphs.Append();
                }
            }

            _DoTheDict(all, "Именной указатель", "\\~", entry => entry.Authors);
            _DoTheDict(all, "Географический указатель", "\\~", entry => entry.Geo);
            _DoTheDict(all, "Указатель издающих организаций", "\\~", entry => entry.Publishers);
            _DoTheDict(all, "Указатель ошибочных ISBN", "\\~\\emdash\\~", entry => entry.Isbn);
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
