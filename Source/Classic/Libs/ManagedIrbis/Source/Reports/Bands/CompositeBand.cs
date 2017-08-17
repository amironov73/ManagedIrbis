// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CompositeBand.cs -- 
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CompositeBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        /// Body bands.
        /// </summary>
        [NotNull]
        [XmlElement("body")]
        [JsonProperty("body")]
        public BandCollection<ReportBand> Body { get; internal set; }

        /// <summary>
        /// Footer band.
        /// </summary>
        [CanBeNull]
        [XmlElement("footer")]
        [JsonProperty("footer")]
        public ReportBand Footer
        {
            get { return _footer; }
            set
            {
                if (!ReferenceEquals(_footer, null))
                {
                    _footer.Report = null;
                    _footer.Parent = null;
                }
                _footer = value;
                if (!ReferenceEquals(_footer, null))
                {
                    _footer.Report = Report;
                    _footer.Parent = this;
                }
            }
        }

        /// <summary>
        /// Header band.
        /// </summary>
        [CanBeNull]
        [XmlElement("header")]
        [JsonProperty("header")]
        public ReportBand Header
        {
            get { return _header; }
            set
            {
                if (!ReferenceEquals(_header, null))
                {
                    _header.Report = null;
                    _header.Parent = null;
                }
                _header = value;
                if (!ReferenceEquals(_header, null))
                {
                    _header.Report = Report;
                    _header.Parent = this;
                }
            }
        }

        /// <inheritdoc cref="ReportBand.Report"/>
        public override IrbisReport Report
        {
            get { return base.Report; }
            internal set
            {
                base.Report = value;
                if (!ReferenceEquals(Header, null))
                {
                    Header.Report = value;
                }
                if (!ReferenceEquals(Footer, null))
                {
                    Footer.Report = value;
                }
                Body.SetReport(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompositeBand()
        {
            Body = new BandCollection<ReportBand>(null, this);
        }

        #endregion

        #region Private members

        private ReportBand _footer, _header;

        /// <inheritdoc cref="ReportBand.RenderOnce(ReportContext,PftFormatter)"/>
        public override void RenderOnce
            (
                [NotNull] ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            context.Index = -1;
            context.CurrentRecord = null;

            context.SetVariables(formatter);

            if (!ReferenceEquals(Header, null))
            {
                Header.Render(context);
            }

            Body.Render(context);

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Render(context);
            }
        }

        /// <inheritdoc 
        /// cref="ReportBand.RenderAllRecords(ReportContext,PftFormatter)"/>
        public override void RenderAllRecords
            (
                ReportContext context,
                PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            context.SetVariables(formatter);

            if (!ReferenceEquals(Header, null))
            {
                Header.Render(context);
            }

            int count = context.Records.Count;
            for (int index = 0; index < count; index++)
            {
                context.Index = index;
                context.CurrentRecord = context.Records[index];

                Body.Render(context);
            }

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Render(context);
            }

            context.Index = -1;
            context.CurrentRecord = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize <see cref="ReportBand.Cells"/>?
        /// </summary>
        public bool ShouldSerializeCells()
        {
            return false;
        }

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            RenderOnce(context);

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

            if (!ReferenceEquals(Header, null))
            {
                verifier
                    .VerifySubObject(Header, "header")
                    .ReferenceEquals
                        (
                            Header.Parent,
                            this,
                            "Header.Parent != this"
                        )
                    .ReferenceEquals
                        (
                            Header.Report,
                            Report,
                            "Header.Report != this.Report"
                        );
            }

            if (!ReferenceEquals(Footer, null))
            {
                verifier
                    .VerifySubObject(Footer, "footer")
                    .ReferenceEquals
                        (
                            Footer.Parent,
                            this,
                            "Footer.Parent != this"
                        )
                    .ReferenceEquals
                        (
                            Footer.Report,
                            Report,
                            "Footer.Report != this.Report"
                        );
            }

            verifier
                .Assert(Cells.Count == 0, "Cells.Count != 0")
                .VerifySubObject(Body, "body");

            foreach (ReportBand band in Body)
            {
                verifier
                    .ReferenceEquals
                        (
                            band.Parent,
                            this,
                            "band.Parent != this"
                        )
                    .ReferenceEquals
                        (
                            band.Report,
                            Report,
                            "band.Report != this.Report"
                        );
            }

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            if (!ReferenceEquals(Header, null))
            {
                Header.Dispose();
            }
            if (!ReferenceEquals(Footer, null))
            {
                Footer.Dispose();
            }
            Body.Dispose();

            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
