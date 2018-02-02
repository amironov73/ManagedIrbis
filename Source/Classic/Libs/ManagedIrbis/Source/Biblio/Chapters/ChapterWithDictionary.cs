// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithDictionary.cs -- 
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
using System.Text.RegularExpressions;
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

using ManagedIrbis.Client;
using ManagedIrbis.Pft;
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
    public class ChapterWithDictionary
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Dictionary.
        /// </summary>
        [NotNull]
        public BiblioDictionary Dictionary { get; private set; }

        /// <summary>
        /// Dictionary.
        /// </summary>
        [NotNull]
        public TermCollection Terms { get; private set; }

        /// <summary>
        /// OrderBy expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("orderBy")]
        public string OrderByClause { get; set; }

        /// <summary>
        /// Select expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("select")]
        public string SelectClause { get; set; }

        /// <summary>
        /// Extended format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("extended")]
        public string ExtendedFormat { get; set; }

        /// <summary>
        /// Entries to exclude.
        /// </summary>
        [NotNull]
        [JsonProperty("exclude")]
        public List<string> ExcludeList { get; private set; }

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
        public ChapterWithDictionary()
        {
            Dictionary = new BiblioDictionary();
            Terms = new TermCollection();
            ExcludeList = new List<string>();
        }

        #endregion

        #region Private members

        private static char[] _charactersToTrim = { '[', ']' };

        private static char[] _lineDelimiters = { '\r', '\n', '\u001F' };

        private void _ChapterToTerms
            (
                [NotNull] BiblioContext context,
                [NotNull] BiblioChapter chapter
            )
        {
            AbstractOutput log = context.Log;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            SpecialSettings settings = Settings;
            if (!ReferenceEquals(settings, null))
            {
                string pattern = settings.GetSetting("chapterFilter");
                if (!string.IsNullOrEmpty(pattern))
                {
                    string title = chapter.Title;
                    if (!ReferenceEquals(title, null))
                    {
                        if (!Regex.IsMatch(title, pattern))
                        {
                            log.WriteLine("Filtered");

                            return;
                        }
                    }
                }
            }

            ItemCollection items = chapter.Items;
            if (!ReferenceEquals(items, null)
                && items.Count != 0)
            {
                log.WriteLine("Gather terms from chapter {0}", chapter);

                int[] mfns = items.Select(i => i.Record.Mfn)
                    .Where(mfn => mfn > 0)
                    .ToArray();
                if (mfns.Length == 0)
                {
                    goto DONE;
                }
                if (mfns.Length != items.Count)
                {
                    throw new IrbisException();
                }

                int termCount = 0;
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    string select = SelectClause
                        .ThrowIfNull("SelectClause");
                    string format = processor.GetText(context, select)
                        .ThrowIfNull("SelectClause");
                    formatter.ParseProgram(format);

                    string[] formatted = formatter.FormatRecords(mfns);
                    string[] formatted2 = new string[mfns.Length];

                    string extendedFormat = ExtendedFormat;
                    if (!string.IsNullOrEmpty(extendedFormat))
                    {
                        extendedFormat = processor.GetText
                            (
                                context,
                                extendedFormat
                            )
                            .ThrowIfNull("ExtendedFormat");
                        formatter.ParseProgram(extendedFormat);
                        formatted2 = formatter.FormatRecords(mfns);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(formatted[i]))
                        {
                            string[] lines = formatted[i]
                                .Split(_lineDelimiters)
                                .TrimLines()
                                .TrimLines(_charactersToTrim)
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            string[] lines2 = new string[lines.Length];
                            if (!string.IsNullOrEmpty(formatted2[i]))
                            {
                                lines2 = formatted2[i]
                                    .Split(_lineDelimiters)
                                    .TrimLines()
                                    .TrimLines(_charactersToTrim)
                                    .NonEmptyLines()
                                    .Distinct()
                                    .ToArray();
                            }
                            for (int j = 0; j < lines.Length; j++)
                            {
                                string line1 = lines[j];
                                string line2 = lines2[j];
                                if (!ExcludeList.Contains(line1))
                                {
                                    BiblioTerm term = new BiblioTerm
                                    {
                                        Title = line1,
                                        Extended = line2,
                                        Dictionary = Terms,
                                        Item = items[i]
                                    };
                                    Terms.Add(term);
                                    termCount++;
                                }
                            }
                        }
                    }
                }

                log.WriteLine(" done");
                log.WriteLine("Term count: {0}", termCount);
            }

            DONE:

            foreach (BiblioChapter child in chapter.Children)
            {
                _ChapterToTerms(context, child);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.BuildDictionary" />
        public override void BuildDictionary
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build dictionary {0}", this);

            foreach (BiblioTerm term in Terms)
            {
                string title = term.Title.ThrowIfNull("term.Title");
                BiblioItem item = term.Item;
                if (ReferenceEquals(item, null))
                {
                    continue;
                }
                Dictionary.Add(title, item.Number);
            }

            log.WriteLine("End build dictionary {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.GatherTerms" />
        public override void GatherTerms
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather terms {0}", this);

            if (Active)
            {
                try
                {
                    BiblioDocument document = context.Document;

                    foreach (BiblioChapter chapter in document.Chapters)
                    {
                        _ChapterToTerms(context, chapter);
                    }

                }
                catch (Exception exception)
                {
                    log.WriteLine("Exception: {0}", exception);
                    throw;
                }
            }


            log.WriteLine("End gather terms {0}", this);
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

            report.Body.Add(new NewPageBand());
            RenderTitle(context);

            string[] keys = Dictionary.Keys.ToArray();
            string[] items = keys.Select(k => CleanOrder(k)).ToArray();
            Array.Sort(items, keys);
            StringBuilder builder = new StringBuilder();
            foreach (string key in keys)
            {
                log.Write(".");
                builder.Clear();
                DictionaryEntry entry = Dictionary[key];
                ParagraphBand band = new ParagraphBand();
                report.Body.Add(band);

                string title = entry.Title;
                if (string.IsNullOrEmpty(title))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(ExtendedFormat))
                {
                    int maxLength = 0;
                    string longest = null;
                    foreach (BiblioTerm term in Terms)
                    {
                        if (term.Title == title)
                        {
                            string candidate = term.Extended;
                            if (!string.IsNullOrEmpty(candidate))
                            {
                                int length = candidate.Length;
                                if (length > maxLength)
                                {
                                    maxLength = length;
                                    longest = candidate;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(longest)
                        && longest.StartsWith(title))
                    {
                        title = longest;
                    }
                }

                builder.Append(title);
                builder.Append(" {\\i ");
                int[] refs = entry.References
                    .Where(item => item > 0)
                    .ToArray();
                Array.Sort(refs);
                bool first = true;
                foreach (int reference in refs)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(reference);
                    first = false;
                }
                builder.Append('}');

                band.Cells.Add(new SimpleTextCell
                    (
                        // TODO implement properly!!!
                        //RichText.Encode2(builder.ToString(), UnicodeRange.Russian)
                        RichText.Encode3(builder.ToString(), UnicodeRange.Russian, "\\f2")
                    ));
            }

            log.WriteLine(" done");

            RenderChildren(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region IVerifiable mebers

        /// <inheritdoc cref="BiblioChapter.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ChapterWithDictionary> verifier
                = new Verifier<ChapterWithDictionary>(this, throwOnError);

            verifier
                .Assert(base.Verify(throwOnError))
                .VerifySubObject(Terms, "Dictionary");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}

