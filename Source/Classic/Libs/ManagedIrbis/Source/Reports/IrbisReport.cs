// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisReport.cs -- 
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
    public class IrbisReport
    {
        #region Properties

        /// <summary>
        /// Report body band.
        /// </summary>
        [CanBeNull]
        [XmlElement("details")]
        [JsonProperty("details")]
        public ReportBand Body { get; set; }

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisReport()
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Render the report.
        /// </summary>
        public virtual void Evaluate
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            if (!ReferenceEquals(Header, null))
            {
                Header.Evaluate(context);
            }

            if (!ReferenceEquals(Body, null))
            {
                Body.Evaluate(context);
            }

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Evaluate(context);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
