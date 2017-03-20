// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Band.cs -- 
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
    public class ReportBand
    {
        #region Properties

        /// <summary>
        /// Cells.
        /// </summary>
        [NotNull]
        [XmlArray("cells")]
        [JsonProperty("cells")]
        public CellCollection Cells { get; private set; }

        /// <summary>
        /// Group band.
        /// </summary>
        [CanBeNull]
        public GroupBand Group { get; internal set; }

        /// <summary>
        /// Report.
        /// </summary>
        [CanBeNull]
        public IrbisReport Report { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public ReportBand()
        {
            Cells = new CellCollection();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the band.
        /// </summary>
        public virtual ReportBand Clone()
        {
            return (ReportBand)MemberwiseClone();
        }

        /// <summary>
        /// Render the band.
        /// </summary>
        public virtual void Evaluate
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            foreach (ReportCell cell in Cells)
            {
                cell.Evaluate(context);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
