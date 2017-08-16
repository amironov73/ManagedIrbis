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
        public string OrderBy { get; set; }

        /// <summary>
        /// Select expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("select")]
        public string Select { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterWithDictionary()
        {
            Dictionary = new BiblioDictionary();
            Terms = new TermCollection();
        }

        #endregion

        #region Private members

        private static char[] _charactersToTrim = {'[', ']'};

        private void _ChapterToTerms
            (
                [NotNull] BiblioContext context,
                [NotNull] BiblioChapter chapter
            )
        {
            AbstractOutput log = context.Log;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            ItemCollection items = chapter.Items;
            if (!ReferenceEquals(items, null)
                && items.Count != 0)
            {
                log.WriteLine("Gather terms from chapter {0}", chapter);

                int termCount = 0;
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    string select = Select.ThrowIfNull("Select");
                    string format = processor.GetText(context, select)
                        .ThrowIfNull("Select");
                    formatter.ParseProgram(format);

                    for (int i = 0; i < items.Count; i++)
                    {
                        log.Write(".");
                        BiblioItem item = items[i];
                        MarcRecord record = item.Record;
                        string formatted = formatter.FormatRecord(record);
                        if (!string.IsNullOrEmpty(formatted))
                        {
                            string[] lines = formatted.SplitLines()
                                .TrimLines()
                                .TrimLines(_charactersToTrim)
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            foreach (string line in lines)
                            {
                                BiblioTerm term = new BiblioTerm
                                {
                                    Title = line,
                                    Dictionary = Terms,
                                    Item = item
                                };
                                Terms.Add(term);
                                termCount++;
                            }
                        }
                    }
                }

                log.WriteLine(string.Empty);
                log.WriteLine("Term count: {0}", termCount);
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                _ChapterToTerms(context, child);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter" />
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
                BiblioItem item = term.Item.ThrowIfNull("term.Item");
                Dictionary.Add(title, item.Number);
            }

            log.WriteLine(string.Empty);
            log.WriteLine(Title);
            log.WriteLine(string.Empty);
            Dictionary.Dump(log);
            log.WriteLine(string.Empty);

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

        #endregion

        #region IVerifiable mebers

        /// <inheritdoc cref="IVerifiable.Verify" />
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

