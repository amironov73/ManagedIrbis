// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioProcessor.cs -- 
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
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioProcessor
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioProcessor()
        {
            Output = new NullOutput();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioProcessor
            (
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNull(output, "output");

            Output = output;
        }

        #endregion

        #region Private members

        /// <summary>
        /// 
        /// </summary>
        private void BildDictionaries
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BildItems
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void FinalRender
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gather records from the chapter.
        /// </summary>
        protected virtual void GatherRecords
            (
                [NotNull] BiblioContext context,
                [NotNull] ChapterWithRecords chapter
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(chapter, "chapter");

            ChapterWithRecords[] children
                = chapter.Children.OfType<ChapterWithRecords>()
                    .ToArray();
            foreach (ChapterWithRecords child in children)
            {
                GatherRecords
                (
                    context,
                    child
                );
            }

            BiblioFilter chapterFilter = chapter.Filter;
            BiblioFilter documentFilter = context.Document.Filter;
            if (ReferenceEquals(chapterFilter, null)
                || string.IsNullOrEmpty(chapterFilter.SelectExpression))
            {
                if (children.Length == 0)
                {
                    Log.Warn
                    (
                        "BiblioProcessor::GatherRecords: "
                        + "chapter without filter: "
                        + chapter.Title.ToVisibleString()
                    );
                }

                return;
            }

            IrbisProvider provider = context.Provider;
            string format = chapterFilter.FormatExpression;
            if (string.IsNullOrEmpty(format)
                && !ReferenceEquals(documentFilter, null))
            {
                format = documentFilter.FormatExpression;
            }
            if (string.IsNullOrEmpty(format))
            {
                format = "@brief";
            }

            string sort = chapterFilter.SortExpression;
            if (string.IsNullOrEmpty(sort)
                && !ReferenceEquals(documentFilter, null))
            {
                sort = documentFilter.SortExpression;
            }
            if (string.IsNullOrEmpty(sort))
            {
                sort = "@brief";
            }

            int[] found = provider.Search
            (
                chapterFilter.SelectExpression
            );

            if (found.Length == 0)
            {
                Log.Warn
                (
                    "BiblioProcessor::GatherRecords: "
                    + "noting found for chapter: "
                    + chapter.Title.ToVisibleString()
                    + " with filter="
                    + chapterFilter.ToVisibleString()
                );
            }

            foreach (int mfn in found)
            {
                MarcRecord record = context.FindRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    chapter.Duplicates.Add(record);
                    continue;
                }

                record = provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    Log.Warn
                    (
                        "BiblioProcessor::GatherRecords: "
                        + "can't read record="
                        + mfn
                    );

                    continue;
                }

                record.Description = provider.FormatRecord(record, format);
                if (string.IsNullOrEmpty(record.Description))
                {
                    Log.Warn
                    (
                        "BiblioProcessor::GatherRecords: "
                        + "empty description for record="
                        + mfn
                    );
                }

                record.SortKey = provider.FormatRecord(record, sort);

                chapter.Records.Add(record);
                context.Records.Add(record);
            }

            chapter.Records.SortRecords();
        }

        /// <summary>
        /// Gather records.
        /// </summary>
        public void GatherRecords
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            IEnumerable<ChapterWithRecords> chapters
                = context.Document.Chapters.OfType<ChapterWithRecords>();
            foreach (ChapterWithRecords chapter in chapters)
            {
                GatherRecords
                (
                    context,
                    chapter
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RenderReport
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GatherTerms
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public methods

        public void BildChaptersFromMenu
            ()
        {
            
        }

        /// <summary>
        /// Build document.
        /// </summary>
        public string BuildDocument
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            GatherRecords(context);
            BildItems(context);
            GatherTerms(context);
            BildDictionaries(context);
            RenderReport(context);
            FinalRender(context);

            return string.Empty;
        }

        #endregion

        #region Object members

        #endregion
    }
}