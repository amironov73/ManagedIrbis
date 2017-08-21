// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GroupingSubChapter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Группировка документов, например, в авторские комплексы.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class GroupingSubChapter
        : MenuSubChapter
    {
        #region Nested classes

        /// <summary>
        /// Group of books.
        /// </summary>
        public class BookGroup
            : List<BiblioItem>
        {
            #region Properties

            /// <summary>
            /// Name.
            /// </summary>
            [CanBeNull]
            public string Name { get; set; }

            /// <summary>
            /// Group for "other" records.
            /// </summary>
            public bool OtherGroup { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Groups.
        /// </summary>
        [NotNull]
        public List<BookGroup> Groups { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupingSubChapter()
        {
            Groups = new List<BookGroup>();
        }

        #endregion

        #region Private members

        private static char[] _lineDelimiters = { '\r', '\n' };

        private void _OrderGroup
            (
                [NotNull] BiblioContext context,
                [NotNull] BookGroup bookGroup
            )
        {
            if (!bookGroup.OtherGroup)
            {
                SpecialSettings settings = SpecialSettings;
                if (ReferenceEquals(settings, null))
                {
                    return;
                }
                string orderFormat = settings.GetSetting("groupedOrder");
                if (string.IsNullOrEmpty(orderFormat))
                {
                    return;
                }

                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    orderFormat = processor.GetText(context, orderFormat)
                        .ThrowIfNull("orderFormat");
                    formatter.ParseProgram(orderFormat);

                    foreach (BiblioItem item in bookGroup)
                    {
                        MarcRecord record = item.Record
                            .ThrowIfNull("item.Record");
                        string order = formatter.FormatRecord(record.Mfn);
                        item.Order = order;
                    }
                }
            }

            BiblioItem[] items = bookGroup
                .OrderBy(item => item.Order)
                .ToArray();
            bookGroup.Clear();
            bookGroup.AddRange(items);
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

            base.BuildItems(context);

            SpecialSettings settings = SpecialSettings;
            if (ReferenceEquals(settings, null))
            {
                return;
            }

            string allValues = settings.GetSetting("values");
            if (string.IsNullOrEmpty(allValues))
            {
                return;
            }

            string[] values = StringUtility
                .SplitString(allValues, ";")
                .NonEmptyLines()
                .ToArray();
            if (values.Length == 0)
            {
                return;
            }

            string groupBy = settings.GetSetting("groupBy");
            if (string.IsNullOrEmpty(groupBy))
            {
                return;
            }

            AbstractOutput log = context.Log;
            log.WriteLine("Begin grouping {0}", this);

            string otherName = settings.GetSetting("others");
            BookGroup others = new BookGroup
            {
                Name = otherName,
                OtherGroup = true
            };

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                groupBy = processor.GetText(context, groupBy)
                    .ThrowIfNull("groupBy");
                formatter.ParseProgram(groupBy);

                foreach (BiblioItem item in Items)
                {
                    MarcRecord record = item.Record
                        .ThrowIfNull("item.Record");
                    string text = formatter.FormatRecord(record.Mfn);
                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }
                    string[] keys = text.Trim()
                        .Split(_lineDelimiters)
                        .TrimLines()
                        .NonEmptyLines()
                        .Distinct()
                        .ToArray();
                    bool found = false;
                    foreach (string key in keys)
                    {
                        string theKey = key;
                        if (theKey.OneOf(values))
                        {
                            BookGroup bookGroup = Groups.FirstOrDefault
                                (
                                    g => g.Name == theKey
                                );
                            if (ReferenceEquals(bookGroup, null))
                            {
                                bookGroup = new BookGroup
                                {
                                    Name = theKey
                                };
                                Groups.Add(bookGroup);
                            }
                            bookGroup.Add(item);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        others.Add(item);
                    }
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Name).ToList();
            foreach (BookGroup bookGroup in Groups)
            {
                _OrderGroup(context, bookGroup);
            }
            _OrderGroup(context, others);
            Groups.Add(others);

            log.WriteLine("End grouping {0}", this);
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

            RenderTitle(context);

            foreach (BookGroup bookGroup in Groups)
            {
                string name = bookGroup.Name;
                log.WriteLine(name);

                report.Body.Add(new ParagraphBand());
                ReportBand band = new ParagraphBand
                    (
                        "\\b "
                        + name
                        + "\\b0"
                    );
                report.Body.Add(band);
                report.Body.Add(new ParagraphBand());

                for (int i = 0; i < bookGroup.Count; i++)
                {
                    log.Write(".");
                    BiblioItem item = bookGroup[i];
                    int number = item.Number;
                    string description = item.Description
                        .ThrowIfNull("item.Description");

                    band = new ParagraphBand
                        (
                            number.ToInvariantString() + ") "
                        );
                    report.Body.Add(band);
                    band.Cells.Add(new SimpleTextCell(description));
                }
                log.WriteLine(" done");
            }

            RenderDuplicates(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
