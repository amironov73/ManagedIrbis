// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportCell.cs -- 
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
    public abstract class ReportCell
    {
        #region Properties

        /// <summary>
        /// Band.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReportBand Band { get; internal set; }

        /// <summary>
        /// Report.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public IrbisReport Report { get; internal set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the cell.
        /// </summary>
        public virtual ReportCell Clone()
        {
            return (ReportCell) MemberwiseClone();
        }

        /// <summary>
        /// Render the cell.
        /// </summary>
        public virtual void Evaluate 
            (
                [NotNull] ReportContext context
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Object members

        #endregion
    }
}
