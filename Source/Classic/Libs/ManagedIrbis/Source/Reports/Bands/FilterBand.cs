// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FilterBand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
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
    public class FilterBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Filter expression.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("filter")]
        [JsonProperty("filter")]
        public string FilterExpression { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            string expression = FilterExpression;
            if (string.IsNullOrEmpty(expression))
            {
                RenderOnce(context);
            }
            else
            {
                List<MarcRecord> list;

                using (RecordFilter filter = new RecordFilter
                    (
                        context.Provider,
                        expression
                    ))
                {
                    list = filter
                        .FilterRecords(context.Records)
                        .ToList();
                }

                ReportContext cloneContext 
                    = context.Clone(list);

                RenderOnce(cloneContext);
            }

            OnAfterRendering(context);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="CompositeBand.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportBand> verifier
                = new Verifier<ReportBand>(this, throwOnError);

            verifier.Assert(base.Verify(throwOnError));

            verifier.NotNullNorEmpty
                (
                    FilterExpression,
                    "FilterExpression"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
