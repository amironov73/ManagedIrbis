// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Cell.cs -- 
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
    public class ReportCell
    {
        #region Properties

        /// <summary>
        /// Report.
        /// </summary>
        [NotNull]
        public Report Report { get; internal set; }

        /// <summary>
        /// Band.
        /// </summary>
        [NotNull]
        public ReportBand Band { get; internal set; }

        /// <summary>
        /// Column index.
        /// </summary>
        [XmlAttribute("column")]
        [JsonProperty("column")]
        public int Column { get; set; }

        /// <summary>
        /// Row index.
        /// </summary>
        [XmlAttribute("row")]
        [JsonProperty("row")]
        public int Row { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Render the cell.
        /// </summary>
        [CanBeNull]
        public virtual string Evaluate 
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            // TODO format record

            return null;
        }

        #endregion

        #region Object members

        #endregion
    }
}
