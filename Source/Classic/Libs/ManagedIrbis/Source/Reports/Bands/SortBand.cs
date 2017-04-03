// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SortBand.cs -- 
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
    public class SortBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Sort expression.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("sort")]
        [JsonProperty("sort")]
        public string SortExpression { get; set; }

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

            string expression = SortExpression;
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
                    List<Pair<string, int>> list
                        = new List<Pair<string, int>>(count);
                    for (int i = 0; i < count; i++)
                    {
                        string formatted = formatter.Format
                        (
                            context.Records[i]
                        );
                        Pair<string, int> pair = new Pair<string, int>
                        (
                            formatted,
                            i
                        );
                        list.Add(pair);
                    }

                    list.Sort
                    (
                        (left, right) => NumberText.Compare
                        (
                            left.First,
                            right.First
                        )
                    );
                    ReportContext cloneContext = context.Clone
                    (
                        list.Select
                        (
                            pair => context.Records[pair.Second]
                        )
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
                    SortExpression,
                    "SortExpression"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
