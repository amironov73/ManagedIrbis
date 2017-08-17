// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChapterWithText.cs -- 
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
    public class ChapterWithText
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get { return true; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static char[] _lineDelimiters = { '\r', '\n' };

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

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");


            RenderTitle(context);

            string text = Text;
            if (!string.IsNullOrEmpty(text))
            {
                text = processor.GetText(context, text);
                if (!string.IsNullOrEmpty(text))
                {
                    string[] lines = text.Split(_lineDelimiters)
                        .NonEmptyLines()
                        .ToArray();
                    foreach (string line in lines)
                    {
                        ReportBand band = new ParagraphBand();
                        report.Body.Add(band);
                        band.Cells.Add(new SimpleTextCell(line));
                    }
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
