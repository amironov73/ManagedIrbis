// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CumulatingSubChapter.cs -- 
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
    /// Сводное описание многотомного издания.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CumulatingSubChapter
        : MenuSubChapter
    {
        #region Nested classes

        /// <summary>
        /// Multivolume document.
        /// </summary>
        public class Multivolume
            : List<BiblioItem>
        {
            #region Properties

            /// <summary>
            /// Header part.
            /// </summary>
            [CanBeNull]
            public string Header { get; set; }

            /// <summary>
            /// Order
            /// </summary>
            [CanBeNull]
            public string Order { get; set; }

            /// <summary>
            /// Item.
            /// </summary>
            [CanBeNull]
            public BiblioItem Item { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Groups.
        /// </summary>
        [NotNull]
        public List<Multivolume> Groups { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CumulatingSubChapter()
        {
            Groups = new List<Multivolume>();
        }

        #endregion

        #region Private members

        private static char[] _lineDelimiters = { '\r', '\n' };

        private static void _OrderGroup
            (
                [NotNull] Multivolume bookGroup
            )
        {
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

        /// <inheritdoc cref="MenuSubChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            base.BuildItems(context);

            SpecialSettings settings = Settings;
            if (ReferenceEquals(settings, null))
            {
                return;
            }

            string generalFormat = settings.GetSetting("general");
            string orderFormat = settings.GetSetting("order");
            if (string.IsNullOrEmpty(generalFormat)
                || string.IsNullOrEmpty(orderFormat))
            {
                return;
            }

            AbstractOutput log = context.Log;
            log.WriteLine("Begin grouping {0}", this);

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                generalFormat = processor.GetText(context, generalFormat)
                    .ThrowIfNull("generalFormat");
                formatter.ParseProgram(generalFormat);

                foreach (BiblioItem item in Items)
                {
                    MarcRecord record = item.Record
                        .ThrowIfNull("item.Record");
                    string header = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(header))
                    {
                        header = header.Trim();
                    }
                    Multivolume bookGroup = Groups.FirstOrDefault
                        (
                            g => g.Header == header
                        );
                    if (ReferenceEquals(bookGroup, null))
                    {
                        bookGroup = new Multivolume
                        {
                            Header = header
                        };
                        Groups.Add(bookGroup);
                    }
                    bookGroup.Add(item);
                }

                orderFormat = processor.GetText(context, orderFormat)
                    .ThrowIfNull("orderFormat");
                formatter.ParseProgram(orderFormat);

                foreach (Multivolume bookGroup in Groups)
                {
                    MarcRecord record = bookGroup.First().Record
                        .ThrowIfNull("bookGroup.Record");
                    string order = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(order))
                    {
                        order = order.Trim();
                    }
                    bookGroup.Order = order;
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Order).ToList();
            Items.Clear();
            foreach (Multivolume bookGroup in Groups)
            {
                _OrderGroup(bookGroup);
                BiblioItem item = new BiblioItem
                {
                    Description = bookGroup.Header,
                    Record = new MarcRecord(), // TODO ???
                    UserData = bookGroup
                };
                bookGroup.Item = item;
                Items.Add(item);
            }


            log.WriteLine("End grouping {0}", this);
        }

        /// <inheritdoc cref="MenuSubChapter.Render" />
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

            foreach (Multivolume bookGroup in Groups)
            {
                string header = bookGroup.Header;
                log.WriteLine(header);

                header = RichText.Encode3(header, UnicodeRange.Russian, "\\f2");

                report.Body.Add(new ParagraphBand());
                BiblioItem item = bookGroup.Item
                    .ThrowIfNull("bookGroup.Item");
                int number = item.Number;
                ReportBand band = new ParagraphBand
                    (
                        number.ToInvariantString() + ".\\~\\~"
                        + header
                    );
                report.Body.Add(band);

                for (int i = 0; i < bookGroup.Count; i++)
                {
                    log.Write(".");
                    item = bookGroup[i];
                    string description = item.Description
                        .ThrowIfNull("item.Description");
                    band = new ParagraphBand(description);
                    report.Body.Add(band);
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
