// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GroupBand.cs -- 
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
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

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
    public class SectionBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        /// Body bands.
        /// </summary>
        [NotNull]
        [XmlElement("body")]
        [JsonProperty("body")]
        public BandCollection<ReportBand> Body { get; internal set; }

        /// <summary>
        /// Footer band.
        /// </summary>
        [CanBeNull]
        [XmlElement("footer")]
        [JsonProperty("footer")]
        public ReportBand Footer { get; set; }

        /// <summary>
        /// Header band.
        /// </summary>
        [CanBeNull]
        [XmlElement("header")]
        [JsonProperty("header")]
        public ReportBand Header { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Evaluate"/>
        public override void Evaluate
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeEvaluation(context);

            ReportBand header = Header;
            if (!ReferenceEquals(header, null))
            {
                context.Index = -1;
                context.CurrentRecord = null;
                header.Evaluate(context);
            }

            context.Index = -1;
            context.CurrentRecord = null;
            foreach (ReportBand band in Body)
            {
                band.Evaluate(context);
            }

            ReportBand footer = Footer;
            if (!ReferenceEquals(footer, null))
            {
                context.Index = -1;
                context.CurrentRecord = null;
                footer.Evaluate(context);
            }

            OnAfterEvaluation(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
