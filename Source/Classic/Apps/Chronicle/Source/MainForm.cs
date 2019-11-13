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
using DevExpress.XtraSplashScreen;

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

namespace Chronicle
{
    public partial class MainForm
        : XtraForm
    {
        private readonly RichEditControl _richEdit;
        private readonly RichEditDocumentServer _server;
        private readonly SplashScreenManager _manager;
        private Document _document;
        private ParagraphStyle _header;
        private ParagraphStyle _para;
        private ParagraphStyle _noIndent;
        private ParagraphStyle _small;

        private MenuFile _udc;
        private MenuFile _languages;

        public MainForm()
        {
            InitializeComponent();

            _manager = new SplashScreenManager(this, typeof(MyWaitForm), true, true);
            _richEdit = new RichEditControl();
            toolStripContainer1.ContentPanel.Controls.Add(_richEdit);
            _richEdit.Dock = DockStyle.Fill;
            _richEdit.ActiveViewType = RichEditViewType.Simple;
            _server = new RichEditDocumentServer();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _FillYears();
            _CreateDocument();
        }

        private void _CreateDocument()
        {
            _server.CreateNewDocument();
            //_richEdit.CreateNewDocument();
            _document = _server.Document;

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

        private string _TranslateUdc
            (
                string original
            )
        {
            if (string.IsNullOrEmpty(original))
            {
                return "!!! Нет УДК";
            }

            MenuEntry best = null;
            foreach (var entry in _udc.Entries)
            {
                var code = entry.Code;
                if (!string.IsNullOrEmpty(code))
                {
                    if (original.StartsWith(code))
                    {
                        if (ReferenceEquals(best, null))
                        {
                            best = entry;
                        }
                        else
                        {
                            string bestCode = best.Code;
                            if (string.IsNullOrEmpty(bestCode))
                            {
                                best = entry;
                            }
                            else
                            {
                                if (code.Length > bestCode.Length)
                                {
                                    best = entry;
                                }
                            }
                        }
                    }
                }
            }

            if (ReferenceEquals(best, null))
            {
                return "!!! Нет УДК";
            }

            if (string.IsNullOrEmpty(best.Comment))
            {
                return "!!! Что за фигня?";
            }

            return best.Comment;
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
            var columns = section.Columns.CreateUniformColumns(section.Page, 100f, 1);
            section.Columns.SetColumns(columns);
            section.StartType = SectionStartType.NextPage;
            WriteH1(title);
            _document.Paragraphs.Append();
            section = _document.AppendSection();
            section.StartType = SectionStartType.Continuous;
            columns = section.Columns.CreateUniformColumns(section.Page, 100f, 2);
            section.Columns.SetColumns(columns);
            var builder = new StringBuilder();

            var ordered = dict.GroupBy(e => e.Value).OrderBy(g => g.Key);
            foreach (var group in ordered)
            {
                builder.Append(@"\par ");
                builder.Append(group.Key);
                builder.Append(separator);
                var first = true;
                foreach (var i in group.OrderBy(_ => _.Index))
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
            columns = section.Columns.CreateUniformColumns(section.Page, 100f, 1);
            section.Columns.SetColumns(columns);
            section.StartType = SectionStartType.Continuous;
            _document.Paragraphs.Append();
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

        private void _DoTheSeries
            (
                Entry entry,
                int tag
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var serie = field.GetFirstSubFieldValue('a');
                if (!string.IsNullOrEmpty(serie))
                {
                    entry.Series.Add(serie);
                }
            }
        }

        private void _DoTheCollective
            (
                Entry entry,
                int tag
            )
        {
            foreach (var field in entry.Record.Fields.GetField(tag))
            {
                var title = field.GetFirstSubFieldValue('a');
                if (string.IsNullOrEmpty(title))
                {
                    continue;
                }
                var sub = field.GetFirstSubFieldValue('b');
                var full = title;
                if (!string.IsNullOrEmpty(sub))
                {
                    full += (". " + sub);
                }
                entry.Collectives.Add(full);
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

            result.TranslatedUdc = _TranslateUdc(result.Udc);

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

            _DoTheSeries(result, 225);
            _DoTheSeries(result, 46);

            _DoTheCollective(result, 962);
            _DoTheCollective(result, 710);
            _DoTheCollective(result, 711);

            return result;
        }

        private Entry[] _DoTheSearch(int year)
        {
            var connectionString = CM.AppSettings["connection-string"];
            using (var client = new IrbisConnection(connectionString))
            {
                var spec = new FileSpecification(IrbisPath.MasterFile, client.Database, "udk.mnu");
                _udc = MenuFile.ReadFromServer(client, spec);

                spec = new FileSpecification(IrbisPath.MasterFile, client.Database, "jz.mnu");
                _languages = MenuFile.ReadFromServer(client, spec);

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

        private void _DoTheWork
            (
                int year,
                bool withMfn
            )
        {
            _CreateDocument();
            var all = _DoTheSearch(year);
            var chunks = new List<Chunk>();
            foreach (var group in all.GroupBy(one => one.TranslatedUdc).OrderBy(g => g.Key))
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
                    string mfnText = withMfn
                        ? $"{{\\ul MFN {entry.Mfn}.\\~}}"
                        : string.Empty;
                    var text = @"{\rtf1 "
                               + $"{entry.Index}.\\~"
                               + mfnText
                               + entry.Description
                               + "}";
                    _document.AppendRtfText(text);
                    _document.Paragraphs.Append();
                }
            }

            _DoTheDict(all, "Именной указатель", "\\~", entry => entry.Authors);
            _DoTheDict(all, "Географический указатель", "\\~", entry => entry.Geo);
            _DoTheDict(all, "Указатель издающих организаций", "\\~", entry => entry.Publishers);
            _DoTheDict(all, "Указатель языков", "\\~", entry =>
            {
                if (string.IsNullOrEmpty(entry.Language) || entry.Language.SameString("rus"))
                {
                    return new string[0];
                }

                var result = _languages.GetString(entry.Language);
                if (string.IsNullOrEmpty(result))
                {
                    return new string[0];
                }

                return new [] { result };
            });
            _DoTheDict(all, "Указатель ошибочных ISBN", "\\~\\emdash\\~", entry => entry.Isbn);
            _DoTheDict(all, "Указатель серий", "\\~", entry => entry.Series);
            _DoTheDict(all, "Указатель коллективов", "\\~", entry => entry.Collectives);
        }

        private async void _goButton_Click(object sender, EventArgs e)
        {
            bool withMfn = _withMfn.CheckBox.Checked;
            var year = (int)_yearBox.SelectedItem;
            _manager.ShowWaitForm();
            _manager.SetWaitFormCaption("Подождите");
            _manager.SetWaitFormDescription("Идёт формирование летописи");
            try
            {
                await Task.Run(() => _DoTheWork(year, withMfn));
            }
            finally
            {
                _manager.CloseWaitForm();
            }

            _richEdit.RtfText = _document.RtfText.Replace("  ", " ");
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            _richEdit.SaveDocumentAs(this);
        }
    }
}
