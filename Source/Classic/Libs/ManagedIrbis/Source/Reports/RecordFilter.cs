// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordFilter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;

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
    public sealed class RecordFilter
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [XmlElement("expression")]
        [JsonProperty("expression")]
        public string Expression { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Filter records.
        /// </summary>
        [NotNull]
        public IEnumerable<MarcRecord> FilterRecords
            (
                [NotNull] IEnumerable<MarcRecord> sourceRecords
            )
        {
            Code.NotNull(sourceRecords, "sourceRecords");

            foreach (MarcRecord record in sourceRecords)
            {
                yield return record;
            }
        }

        #endregion
    }
}
