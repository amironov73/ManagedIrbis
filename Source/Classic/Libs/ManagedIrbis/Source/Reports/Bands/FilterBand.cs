// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FilterBand.cs -- 
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
using AM.Text;

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
                int count = context.Records.Count;

                using (PftFormatter formatter
                    = context.GetFormatter(expression))
                {
                    List<MarcRecord> list = new List<MarcRecord>(count);
                    for (int i = 0; i < count; i++)
                    {
                        context.SetVariables(formatter);

                        string formatted = formatter.Format
                        (
                            context.Records[i]
                        );
                        if (formatted.SameString("1"))
                        {
                            list.Add(context.Records[i]);
                        }
                    }

                    ReportContext cloneContext = context.Clone
                        (
                            list
                        );

                    RenderOnce(cloneContext);
                }
            }

            OnAfterRendering(context);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
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
