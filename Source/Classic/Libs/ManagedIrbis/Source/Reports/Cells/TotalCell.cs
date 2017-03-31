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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
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

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TotalCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TotalCell
            (
                int bandIndex,
                int cellIndex,
                TotalFunction function
            )
        {
            BandIndex = bandIndex;
            CellIndex = cellIndex;
            Function = function;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TotalCell
            (
                int bandIndex,
                int cellIndex,
                TotalFunction function,
                [CanBeNull] string format
            )
        {
            BandIndex = bandIndex;
            CellIndex = cellIndex;
            Function = function;
            Format = format;
        }

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
            CompositeBand parent = (CompositeBand) band.Parent
                .ThrowIfNull("Parent not set");
            band = parent.Body[BandIndex];
            ReportCell cell = band.Cells[CellIndex];
            string format = Format;

            string result = null;

            int count = context.Records.Count;

            if (Function == TotalFunction.Count)
            {
                result = context.Records.Count.ToInvariantString();
            }
            else
            {
                int countNonEmpty = 0;
                double accumulator = 0;

                for (int i = 0; i < count; i++)
                {
                    context.Index = i;
                    context.CurrentRecord = context.Records[i];

                    string value = cell.Compute(context);

                    switch (Function)
                    {
                        case TotalFunction.CountNonEmpty:
                            if (!string.IsNullOrEmpty(value))
                            {
                                countNonEmpty++;
                            }
                            result 
                                = countNonEmpty.ToInvariantString();
                            break;

                        case TotalFunction.Maximum:
                            if (string.IsNullOrEmpty(result))
                            {
                                result = value;
                            }
                            if (NumberText.Compare
                                (
                                    result,
                                    value
                                ) < 0)
                            {
                                result = value;
                            }
                            break;

                        case TotalFunction.Minimum:
                            if (string.IsNullOrEmpty(result))
                            {
                                result = value;
                            }
                            if (NumberText.Compare
                                (
                                    result,
                                    value
                                ) > 0)
                            {
                                result = value;
                            }
                            break;

                        case TotalFunction.Sum:
                            double number;
                            if (NumericUtility.TryParseDouble
                                (
                                    value,
                                    out number
                                ))
                            {
                                accumulator += number;
                                if (string.IsNullOrEmpty(format))
                                {
                                    result = accumulator
                                        .ToInvariantString();
                                }
                                else
                                {
                                    result = accumulator
                                        .ToString
                                        (
                                            format,
                                            CultureInfo.InvariantCulture
                                        );
                                }
                            }
                            break;
                    }
                }
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
