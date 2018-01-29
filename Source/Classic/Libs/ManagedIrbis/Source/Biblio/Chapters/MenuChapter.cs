// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuChapter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MenuChapter
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Leaf nodes only can contain records.
        /// </summary>
        [JsonProperty("leafOnly")]
        public bool LeafOnly { get; set; }

        /// <summary>
        /// Menu name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("menuName")]
        public string MenuName { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [CanBeNull]
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        /// <summary>
        /// Record selector.
        /// </summary>
        [CanBeNull]
        [JsonProperty("recordSelector")]
        public string RecordSelector { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("search")]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Title format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("titleFormat")]
        public string TitleFormat { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [CanBeNull]
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// List of settings.
        /// </summary>
        [NotNull]
        [JsonProperty("menuSettings")]
        public List<SpecialSettings> MenuSettings { get; private set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuChapter()
        {
            MenuSettings = new List<SpecialSettings>();
        }

        #endregion

        #region Private members

        private static char[] _lineDelimiters
            = { '\r', '\n', '\u001E', '\u001F' };

        private MenuSubChapter _CreateChapter
            (
                [NotNull] IPftFormatter formatter,
                [NotNull] IrbisTreeFile.Item item
            )
        {
            string key = item.Prefix.Trim();
            SpecialSettings settings = MenuSettings.FirstOrDefault
                (
                    s => s.Name == key
                );
            string value = item.Suffix;

            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(1, key));
            record.Fields.Add(new RecordField(2, value));
            string title = formatter.FormatRecord(record);

            string className = null;
            if (!ReferenceEquals(settings, null))
            {
                className = settings.GetSetting("type");
            }

            MenuSubChapter result;
            if (string.IsNullOrEmpty(className))
            {
                result = new MenuSubChapter();
            }
            else
            {
                if (!className.Contains("."))
                {
                    className = "ManagedIrbis.Biblio." + className;
                }
                Type type = Type.GetType(className, true)
                    .ThrowIfNull("Type.GetType");
                result = (MenuSubChapter)Activator.CreateInstance(type);
            }
            result.Key = key;
            result.MainChapter = this;
            result.Title = title;
            result.Value = value;
            result.Settings = settings;

            foreach (IrbisTreeFile.Item child in item.Children)
            {
                MenuSubChapter subChapter
                    = _CreateChapter(formatter, child);
                result.Children.Add(subChapter);
            }

            return result;
        }

        private void _RemoveSubField
            (
                [NotNull] MarcRecord record,
                int tag,
                char code
            )
        {
            RecordField[] fields = record.Fields.GetField(tag);
            foreach (RecordField field in fields)
            {
                field.RemoveSubField(code);
            }
        }

        private void _BeautifyRecord
            (
                [NotNull] MarcRecord record
            )
        {
            // Украшаем запись согласно вкусам библиографов

            foreach (RecordField field in record.Fields)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    field.Value = MenuSubChapter.Enhance(field.Value);
                }

                foreach (SubField subField in field.SubFields)
                {
                    if (!string.IsNullOrEmpty(subField.Value))
                    {
                        subField.Value = MenuSubChapter.Enhance(subField.Value);
                    }
                }
            }

            // Источник библиографической записи
            record.RemoveField(488);

            // Сведения об автографах
            record.RemoveField(391);

            string worksheet = record.FM(920);
            if (!worksheet.SameString("ASP"))
            {
                return;
            }

            // Подзаголовочные сведения журналов
            _RemoveSubField(record, 463, '7');
            //_RemoveSubField(record, 963, 'e');

            // Издательство в статьях
            _RemoveSubField(record, 463, 'g');

            // Сведения об автографах
            record.RemoveField(391);

            // Из аннотаций брать только первое повторение
            RecordField[] annotations = record.Fields.GetField(331);
            for (int i = 1; i < annotations.Length; i++)
            {
                record.Fields.Remove(annotations[i]);
            }
        }

        private static readonly Regex _regex463 = new Regex(@"^№.*\((?<date>.*?)\)$");

        private void _FixDate
            (
                [CanBeNull] SubField subField
            )
        {
            if (!ReferenceEquals(subField, null))
            {
                string value = subField.Value;
                if (!string.IsNullOrEmpty(value))
                {

                    Match match = _regex463.Match(value);
                    if (match.Success)
                    {

                        string date = match.Groups["date"].Value;
                        if (!string.IsNullOrEmpty(date)
                            && date.Contains(" "))
                        {
                            subField.Value = date.Replace(" ", "\\~");
                        }
                    }
                }
            }
        }

        private void _Fix463
            (
                [NotNull] MarcRecord record
            )
        {
            //
            // Переделываем даты у газет, как нравится библиографам
            // из "№ 11 (5 мая)" в просто "5 мая".
            //
            // Всё, что не походит под шаблон, оставляем как есть.
            // Пусть библиографы правят сами, ручками
            //

            RecordField[] fields = record.Fields.GetField(463);
            foreach (RecordField field in fields)
            {
                SubField subField = field.SubFields.GetFirstSubField('h');
                _FixDate(subField);

                subField = field.SubFields.GetFirstSubField('v');
                _FixDate(subField);
            }
        }

        private static int _GetYear
            (
                [NotNull] MarcRecord record
            )
        {
            string result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(463, 'j');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(934);
            }
            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }

            Match match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }
            return result.SafeToInt32();
        }

        private void _GatherSame
            (
                [NotNull] BiblioContext context
            )
        {
            //
            // Собираем вместе записи с одинаковым полем 2025
            //

            RecordCollection records = Records;
            if (ReferenceEquals(records, null))
            {
                return;
            }

            MarcRecord[] allMarked = records.Where(r => r.HaveField(2025)).ToArray();
            var grouped = allMarked.GroupBy(r => r.FM(2025));
            List<MarcRecord> toRemove = new List<MarcRecord>();
            foreach (var oneGroup in grouped)
            {
                MarcRecord[] array = oneGroup.ToArray();
                if (array.Length == 1)
                {
                    continue;
                }

                foreach (MarcRecord record in array)
                {
                    record.UserData = _GetYear(record);
                }

                array = array.OrderBy(r => (int) r.UserData).ToArray();

                MarcRecord firstRecord = array[0];
                RecordCollection same = new RecordCollection();
                for (int i = 1; i < array.Length; i++)
                {
                    MarcRecord record = array[i];
                    record.RemoveField(200);
                    record.RemoveField(922);
                    record.RemoveField(925);
                    record.RemoveField(700);
                    record.RemoveField(701);
                    record.RemoveField(702);

                    record.Fields.Add(new RecordField(200).AddSubField('a', "То же"));

                    same.Add(record);
                    toRemove.Add(record);
                }

                firstRecord.UserData = same;
            }

            foreach (MarcRecord recordToRemove in toRemove)
            {
                records.Remove(recordToRemove);
                context.Records.Remove(recordToRemove);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather records {0}", this);
            RecordCollection badRecords = context.BadRecords;
            Records = new RecordCollection();
            MarcRecord record = null;

            try
            {
                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    IrbisProvider provider = context.Provider;
                    RecordCollection records = Records
                        .ThrowIfNull("Records");

                    string searchExpression = SearchExpression
                        .ThrowIfNull("SearchExpression");
                    formatter.ParseProgram(searchExpression);
                    record = new MarcRecord();
                    searchExpression = formatter.FormatRecord(record);

                    int[] found = provider.Search(searchExpression);
                    log.WriteLine("Found: {0} record(s)", found.Length);

                    log.Write("Reading records");

                    for (int i = 0; i < found.Length; i++)
                    {
                        log.Write(".");
                        record = provider.ReadRecord(found[i]);
                        if (!ReferenceEquals(record, null))
                        {
                            _BeautifyRecord(record);
                            _Fix463(record);
                        }
                        records.Add(record);
                        context.Records.Add(record);
                    }

                    _GatherSame(context);

                    //// Пробуем не загружать записи,
                    //// а предоставить заглушки

                    //for (int i = 0; i < found.Length; i++)
                    //{
                    //    log.Write(".");
                    //    record = new MarcRecord
                    //    {
                    //        Mfn = found[i]
                    //    };
                    //    records.Add(record);
                    //    context.Records.Add(record);
                    //}

                    log.WriteLine(" done");

                    CleanRecords(context, records);

                    Dictionary<string, MenuSubChapter> dictionary
                        = new Dictionary<string, MenuSubChapter>();
                    Action<BiblioChapter> action = chapter =>
                    {
                        MenuSubChapter subChapter = chapter as MenuSubChapter;
                        if (!ReferenceEquals(subChapter, null))
                        {
                            string key = subChapter.Key
                                .ThrowIfNull("subChapter.Key");
                            dictionary.Add(key, subChapter);
                        }
                    };
                    Walk(action);

                    string recordSelector = RecordSelector
                        .ThrowIfNull("RecordSelector");
                    formatter.ParseProgram(recordSelector);
                    log.Write("Distributing records");

                    int[] mfns = records.Select(r => r.Mfn).ToArray();
                    string[] formatted = formatter.FormatRecords(mfns);
                    if (formatted.Length != mfns.Length)
                    {
                        throw new IrbisException();
                    }

                    for (int i = 0; i < records.Count; i++)
                    {
                        log.Write(".");

                        record = records[i];
                        //string key
                        //    = formatter.FormatRecord(record);
                        string key = formatted[i];
                        if (string.IsNullOrEmpty(key))
                        {
                            badRecords.Add(record);
                        }
                        else
                        {
                            string[] keys = key.Trim()
                                .Split(_lineDelimiters)
                                .TrimLines()
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            key = keys.FirstOrDefault();
                            if (string.IsNullOrEmpty(key))
                            {
                                badRecords.Add(record);
                            }
                            else
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(key, out subChapter))
                                {
                                    subChapter.Records.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }

                            foreach (string nextKey in keys.Skip(1))
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(nextKey, out subChapter))
                                {
                                    subChapter.Duplicates.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }
                        }
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                log.WriteLine(" done");
                log.WriteLine("Bad records: {0}", badRecords.Count);

                // Do we really need this?

                foreach (BiblioChapter child in Children)
                {
                    child.GatherRecords(context);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format
                    (
                        "Exception: {0}",
                        exception
                    );

                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0}{1}{2}",
                            record.Mfn,
                            Environment.NewLine,
                            message
                        );
                }

                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End gather records {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.Initialize" />
        public override void Initialize
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
            try
            {
                string menuName = MenuName.ThrowIfNull("MenuName");

                IrbisProvider provider = context.Provider;

                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        provider.Database,
                        menuName
                    );
                MenuFile menu = provider.ReadMenuFile(specification);
                if (ReferenceEquals(menu, null))
                {
                    throw new IrbisException();
                }
                IrbisTreeFile tree = menu.ToTree();

                // Create Formatter

                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    string titleFormat = TitleFormat
                        .ThrowIfNull("TitleFormat");
                    formatter.ParseProgram(titleFormat);

                    foreach (IrbisTreeFile.Item root in tree.Roots)
                    {
                        MenuSubChapter chapter
                            = _CreateChapter(formatter, root);
                        Children.Add(chapter);
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.Initialize(context);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            processor.Report.Body.Add(new NewPageBand());

            base.Render(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}

