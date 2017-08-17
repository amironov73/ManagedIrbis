// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithStatistics.cs -- 
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

using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ChapterWithStatistics
        : BiblioChapter
    {
        #region Properties

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get { return true; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private int _total;

        private void _ProcessChapter
            (
                [NotNull] IrbisReport report,
                [NotNull] BiblioChapter chapter
            )
        {
            ItemCollection items = chapter.Items;
            if (!ReferenceEquals(items, null))
            {
                int count = items.Count;
                if (!chapter.IsServiceChapter)
                {
                    string text = string.Format
                    (
                        "{0}\\tab\\~ {{\\b {1}}}",
                        chapter.Title,
                        count
                    );
                    ParagraphBand band = new ParagraphBand(text);
                    report.Body.Add(band);
                    _total += count;
                }
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                _ProcessChapter(report, child);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render {0}", this);
            BiblioDocument document = context.Document;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");
            RecordCollection badRecords = context.BadRecords;

            RenderTitle(context);

            _total = 0;
            string text;
            if (badRecords.Count != 0)
            {
                text = string.Format
                    (
                        "ВНЕ РАЗДЕЛОВ:\\tab\\~ {{\\b {0}}}",
                        badRecords.Count.ToInvariantString()
                    );
                report.Body.Add(new ParagraphBand(text));
            }
            foreach (BiblioChapter chapter in document.Chapters)
            {
                _ProcessChapter(report, chapter);
            }
            report.Body.Add(new ParagraphBand());
            text = string.Format
                (
                    "ВСЕГО:\\tab\\~ {{\\b {0}}}",
                    _total.ToInvariantString()
                );
            report.Body.Add(new ParagraphBand(text));

            RenderChildren(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
