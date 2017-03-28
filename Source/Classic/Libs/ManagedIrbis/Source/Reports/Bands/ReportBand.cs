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

using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Generic (non-repeating) band.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ReportBand
        : IAttributable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Attributes.
        /// </summary>
        [NotNull]
        [XmlArray("attr")]
        [JsonProperty("attr")]
        public ReportAttributes Attributes { get; private set; }

        /// <summary>
        /// Cells.
        /// </summary>
        [NotNull]
        [XmlArray("cells")]
        [JsonProperty("cells")]
        public CellCollection Cells { get; private set; }

        /// <summary>
        /// Report.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public IrbisReport Report
        {
            get { return _report; }
            internal set
            {
                _report = value;
                Cells.SetReport(value);
            }
        }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public ReportBand()
        {
            Attributes = new ReportAttributes();
            Cells = new CellCollection
            {
                Band = this
            };
        }

        #endregion

        #region Private members

        private IrbisReport _report;

        private void _Evaluate
            (
                [NotNull] ReportContext context
            )
        {
            ReportDriver driver = context.Driver;
            driver.BeginRow(context, this);
            foreach (ReportCell cell in Cells)
            {
                cell.Evaluate(context);
            }
            driver.EndRow(context, this);
        }

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

            EvaluateOnce(context, null);
        }

        /// <summary>
        /// Evaluate the band once (ignore records).
        /// </summary>
        public void EvaluateOnce
            (
                [NotNull] ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            context.SetVariables(formatter);

            _Evaluate(context);
        }

        /// <summary>
        /// 
        /// </summary>
        public void EvaluateRecords
            (
                [NotNull] ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            context.SetVariables(formatter);

            int index = 0;
            foreach (MarcRecord record in context.Records)
            {
                context.CurrentRecord = record;
                context.Index = index;

                _Evaluate(context);

                index++;
            }

            context.Index = -1;
            context.CurrentRecord = null;
        }

        /// <summary>
        /// Should serialize <see cref="Attributes"/>?
        /// </summary>
        public bool ShouldSerializeAttributes()
        {
            return Attributes.Count != 0;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Cells.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
