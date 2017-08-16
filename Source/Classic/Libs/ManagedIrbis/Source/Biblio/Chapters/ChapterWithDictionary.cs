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
        }

        #endregion

        #region Private members

        private void _ProcessChapter
            (
                [NotNull] BiblioContext context,
                [NotNull] BiblioChapter chapter
            )
        {
            AbstractOutput log = context.Log;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            ItemCollection items = chapter.Items;
            if (items.Count != 0)
            {
                log.WriteLine("Gather terms from chapter {0}", chapter);

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
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            foreach (string line in lines)
                            {
                                BiblioTerm term = new BiblioTerm
                                {
                                    Data = line,
                                    Dictionary = Dictionary,
                                    Item = item
                                };
                                Dictionary.Add(term);
                            }
                        }
                    }
                }
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                _ProcessChapter(context, child);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.GatherTerms" />
        public override void GatherTerms
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather terms {0}", this);

            try
            {
                IrbisProvider provider = context.Provider;
                BiblioDocument document = context.Document;



            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
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
                .VerifySubObject(Dictionary, "Dictionary");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
