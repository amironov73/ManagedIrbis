// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuSubChapter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AM;
using AM.Collections;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MenuSubChapter
        : ChapterWithRecords
    {
        #region Properties

        /// <summary>
        /// Key.
        /// </summary>
        [CanBeNull]
        public string Key { get; set; }

        /// <summary>
        /// Main chapter.
        /// </summary>
        [CanBeNull]
        public MenuChapter MainChapter { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get
            {
                if (Children.Count == 0)
                {
                    return false;
                }

                MenuChapter mainChapter = MainChapter;
                if (ReferenceEquals(mainChapter, null))
                {
                    return Records.Count == 0;
                }

                return mainChapter.LeafOnly && Records.Count == 0;
            }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        [CanBeNull]
        private BiblioItem _FindItem
            (
                [NotNull] MenuSubChapter chapter,
                [NotNull] MarcRecord record
            )
        {
            foreach (BiblioItem item in chapter.Items)
            {
                if (ReferenceEquals(item.Record, record))
                {
                    return item;
                }
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                MenuSubChapter subChapter = child as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    BiblioItem found = _FindItem(subChapter, record);
                    if (!ReferenceEquals(found, null))
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        [CanBeNull]
        private BiblioItem _FindItem
            (
                [NotNull] MarcRecord record
            )
        {
            MenuChapter mainChapter = MainChapter
                .ThrowIfNull("MainChapter");
            foreach (BiblioChapter child in mainChapter.Children)
            {
                MenuSubChapter chapter = child as MenuSubChapter;
                if (!ReferenceEquals(chapter, null))
                {
                    BiblioItem found = _FindItem(chapter, record);
                    if (!ReferenceEquals(found, null))
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        [NotNull]
        protected virtual string GetDescriptionFormat()
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                MenuSubChapter subChapter = chapter as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    SpecialSettings settings = subChapter.Settings;

                    string result = settings.GetSetting("format");
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }

                chapter = chapter.Parent;
            }

            return MainChapter
                .ThrowIfNull("MainChapter")
                .Format
                .ThrowIfNull("MainChapter.Format");
        }

        /// <summary>
        /// Render duplicates.
        /// </summary>
        protected void RenderDuplicates
            (
                [NotNull] BiblioContext context
            )
        {
            AbstractOutput log = context.Log;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            if (Duplicates.Count != 0)
            {
                List<BiblioItem> items
                    = new List<BiblioItem>(Duplicates.Count);
                foreach (MarcRecord dublicate in Duplicates)
                {
                    BiblioItem item = _FindItem(dublicate);
                    if (!ReferenceEquals(item, null))
                    {
                        items.Add(item);
                    }
                    else
                    {
                        log.WriteLine
                            (
                                "Проблема с дубликатом MFN="
                                + dublicate.Mfn
                            );
                    }
                }
                items = items
                    .OrderBy(x => x.Number)
                    .Distinct()
                    .ToList();

                StringBuilder builder = new StringBuilder();
                builder.Append("См. также: {\\i ");
                bool first = true;
                foreach (BiblioItem item in items)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(item.Number.ToInvariantString());
                    first = false;
                }
                builder.Append('}');

                report.Body.Add(new ParagraphBand());
                report.Body.Add(new ParagraphBand(builder.ToString()));
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            MarcRecord record = null;
            IPftFormatter formatter = null;

            log.WriteLine("Begin build items {0}", this);
            Items = new ItemCollection();

            try
            {
                if (Records.Count != 0)
                {
                    BiblioProcessor processor = context.Processor
                        .ThrowIfNull("context.Processor");
                    MenuChapter mainChapter = MainChapter
                        .ThrowIfNull("MainChapter");

                    using (formatter = processor.AcquireFormatter(context))
                    {
                        string descriptionFormat = GetDescriptionFormat();
                        descriptionFormat = processor.GetText
                            (
                                context,
                                descriptionFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(descriptionFormat);

                        int[] mfns = Records.Select(r => r.Mfn).ToArray();
                        string[] formatted = formatter.FormatRecords(mfns);

                        for (int i = 0; i < Records.Count; i++)
                        {
                            log.Write(".");
                            record = Records[i];
                            //string description = 
                            //    "MFN " + record.Mfn + " "
                            //    + formatter.FormatRecord(record);
                            //string description
                            //    = formatter.FormatRecord(record.Mfn);
                            string description = formatted[i]
                                .TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(description)

                            description
                                = BiblioUtility.AddTrailingDot(description);

                            BiblioItem item = new BiblioItem
                            {
                                Chapter = this,
                                Record = record,
                                Description = description
                            };
                            Items.Add(item);
                        }
                    }

                    log.WriteLine(" done");

                    using (formatter = processor.AcquireFormatter(context))
                    {
                        string orderFormat = mainChapter.OrderBy
                            .ThrowIfNull("mainChapter.OrderBy");
                        orderFormat = processor.GetText
                            (
                                context,
                                orderFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(orderFormat);

                        int[] mfns = Records.Select(r => r.Mfn).ToArray();
                        string[] formatted = formatter.FormatRecords(mfns);

                        for (int i = 0; i < Items.Count; i++)
                        {
                            log.Write(".");
                            BiblioItem item = Items[i];
                            //record = item.Record
                            //    .ThrowIfNull("item.Record");
                            //string order
                            //    = formatter.FormatRecord(record.Mfn);
                            string order = formatted[i].TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(order)

                            item.Order = order;
                        }
                    }

                    log.WriteLine(" done");

                    Items.SortByOrder();

                    log.WriteLine("Items: {0}", Items.Count);
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.BuildItems(context);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format
                    (
                        "Exception: {0}", exception
                    );
                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0} : {1}",
                            record.Mfn,
                            message
                        );
                }

                log.WriteLine(string.Empty);
                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End build items {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render {0}", this);

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            if (Records.Count != 0
                || Duplicates.Count != 0
                || Children.Count != 0)
            {
                RenderTitle(context);

                for (int i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    BiblioItem item = Items[i];
                    int number = item.Number;
                    string description = item.Description
                        .ThrowIfNull("item.Description");

                    ReportBand band = new ParagraphBand
                        (
                            number.ToInvariantString() + ") "
                        );
                    report.Body.Add(band);
                    band.Cells.Add(new SimpleTextCell(description));
                }

                log.WriteLine(" done");

            }

            RenderDuplicates(context);

            RenderChildren(context);

            log.WriteLine(string.Empty);
            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = base.ToString()
                + " [:] "
                + Records.Count.ToInvariantString();

            return result;
        }

        #endregion
    }
}
