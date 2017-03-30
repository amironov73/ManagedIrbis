// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TotalCell.cs --
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
    public class TotalCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Band index.
        /// </summary>
        [XmlAttribute("band")]
        [JsonProperty("band")]
        public int BandIndex { get; set; }

        /// <summary>
        /// Cell index.
        /// </summary>
        [XmlAttribute("cell")]
        [JsonProperty("cell")]
        public int CellIndex
        {
            get; set;
        }

        /// <summary>
        /// Function.
        /// </summary>
        [XmlAttribute("function")]
        [JsonProperty("function")]
        public TotalFunction Function { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeCompute(context);

            ReportBand band = Band
                .ThrowIfNull("Band not set");
            CompositeBand composite = (CompositeBand)band;
            band = composite.Body[BandIndex];
            ReportCell cell = band.Cells[CellIndex];

            string result = null;

            int count = context.Records.Count;
            for (int i = 0; i < count; i++)
            {
                context.Index = i;
                context.CurrentRecord = context.Records[i];

                string value = cell.Compute(context);

                // TODO implement functions
                result = value;
            }

            context.CurrentRecord = null;
            context.Index = -1;

            OnAfterCompute(context);

            return result;
        }

        /// <inheritdoc cref="ReportCell.Evaluate"/>
        public override void Evaluate
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            ReportDriver driver = context.Driver;

            driver.BeginCell(context, this);

            string text = Compute(context);
            driver.Write(context, text);

            driver.EndCell(context, this);
        }

        #endregion

        #region Public methods

        #endregion
    }
}
