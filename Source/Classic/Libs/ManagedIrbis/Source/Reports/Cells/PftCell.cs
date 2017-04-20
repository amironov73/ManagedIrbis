// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCell.cs -- 
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

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
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
    public class PftCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Script text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCell
            (
                string format
            )
        {
            Text = format;
        }

        #endregion

        #region Private members

        private PftFormatter _formatter;

        #endregion

        #region Public methods

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeCompute(context);

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return null;
            }

            string result = null;

            ConnectedClient connected 
                = context.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                MarcRecord record = context.CurrentRecord;
                if (!ReferenceEquals(record, null))
                {
                    result = connected.FormatRecord
                    (
                        record,
                        text
                    );
                }
            }
            else
            {
                if (ReferenceEquals(_formatter, null))
                {
                    _formatter = context.GetFormatter(text);
                }

                context.SetVariables(_formatter);

                result
                    = _formatter.Format(context.CurrentRecord);

                OnAfterCompute(context);
            }
            return result;
        }

        /// <inheritdoc cref="ReportCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return;
            }

            ReportDriver driver = context.Driver;
            string formatted = Compute(context);

            driver.BeginCell(context, this);
            driver.Write(context, formatted);
            driver.EndCell(context, this);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            base.Dispose();

            if (!ReferenceEquals(_formatter, null))
            {
                _formatter.Dispose();
                _formatter = null;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
