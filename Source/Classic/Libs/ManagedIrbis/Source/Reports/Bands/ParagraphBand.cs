// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ParagraphBand.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ParagraphBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        /// Style name.
        /// </summary>
        [CanBeNull]
        public string StyleSpecification { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParagraphBand()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParagraphBand
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            Cells.Add(new SimpleTextCell(text));
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            ReportDriver driver = context.Driver;
            driver.BeginParagraph(context, this);
            if (!string.IsNullOrEmpty(StyleSpecification))
            {
                driver.WriteServiceText(context, StyleSpecification);
            }
            foreach (ReportCell cell in Cells)
            {
                cell.Render(context);
            }
            driver.EndParagraph(context, this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
