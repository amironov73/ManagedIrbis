// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithErrors.cs -- 
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

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ChapterWithErrors
        : BiblioChapter
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

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

            RecordCollection badRecords = context.BadRecords;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            RenderTitle(context);

            if (badRecords.Count != 0)
            {
                ParagraphBand title = new ParagraphBand
                    (
                        "Следующие записи не входят ни в один раздел"
                    );
                report.Body.Add(title);
                report.Body.Add(new ParagraphBand());

                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    string briefFormat = processor
                        .GetText(context, "*brief.pft")
                        .ThrowIfNull("processor.GetText");
                    formatter.ParseProgram(briefFormat);

                    foreach (MarcRecord record in badRecords)
                    {
                        log.Write(".");
                        string description =
                            "MFN " + record.Mfn + " "
                            + formatter.FormatRecord(record.Mfn);
                        ParagraphBand band 
                            = new ParagraphBand(description);
                        report.Body.Add(band);
                        report.Body.Add(new ParagraphBand());
                    }

                    log.WriteLine(" done");

                    processor.ReleaseFormatter(context, formatter);
                }
            }

            RenderChildren(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
