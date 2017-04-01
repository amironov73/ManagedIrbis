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
        #region Events

        /// <summary>
        /// Raised after rendering.
        /// </summary>
        public event EventHandler<ReportRenderingEventArgs> AfterRendering;

        /// <summary>
        /// Raised before rendering.
        /// </summary>
        public event EventHandler<ReportRenderingEventArgs> BeforeRendering;

        #endregion

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
        /// Parent band.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReportBand Parent { get; internal set; }

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

        private void _Render
            (
                [NotNull] ReportContext context
            )
        {
            ReportDriver driver = context.Driver;
            driver.BeginRow(context, this);
            foreach (ReportCell cell in Cells)
            {
                cell.Render(context);
            }
            driver.EndRow(context, this);
        }

        /// <summary>
        /// Called after <see cref="Render"/>.
        /// </summary>
        protected void OnAfterRendering
            (
                ReportContext context
            )
        {
            ReportRenderingEventArgs eventArgs
                = new ReportRenderingEventArgs(context);
            AfterRendering.Raise(this, eventArgs);
        }

        /// <summary>
        /// Called before <see cref="Render"/>.
        /// </summary>
        protected void OnBeforeRendering
            (
                ReportContext context
            )
        {
            ReportRenderingEventArgs eventArgs
                = new ReportRenderingEventArgs(context);
            BeforeRendering.Raise(this, eventArgs);
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
        public virtual void Render
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            RenderOnce(context, null);
        }

        /// <summary>
        /// Render the band once (ignore records).
        /// </summary>
        public void RenderOnce
            (
                [NotNull] ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            context.SetVariables(formatter);

            _Render(context);

            OnAfterRendering(context);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RenderWithRecords
            (
                [NotNull] ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            context.SetVariables(formatter);

            int index = 0;
            foreach (MarcRecord record in context.Records)
            {
                context.CurrentRecord = record;
                context.Index = index;

                _Render(context);

                index++;
            }

            context.Index = -1;
            context.CurrentRecord = null;

            OnAfterRendering(context);
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
