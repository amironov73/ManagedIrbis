// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConditionalBand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;

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
    public class ConditionalBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Conditional expression (PFT).
        /// </summary>
        [CanBeNull]
        [XmlAttribute("condition")]
        [JsonProperty("condition")]
        public string ConditionalExpression { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string expression = ConditionalExpression;

            if (string.IsNullOrEmpty(expression))
            {
                base.Render(context);
            }
            else
            {
                using (PftFormatter formatter
                    = context.GetFormatter(expression))
                {
                    string text = formatter.FormatRecord(null);
                    if (RecordFilter.CheckResult(text))
                    {
                        base.Render(context);
                    }
                }
            }
        }

        #endregion
    }
}
