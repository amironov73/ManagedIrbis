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
        : DetailsBand
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
                    _footer.Parent = null;
                }
                _footer = value;
                if (!ReferenceEquals(_footer, null))
                {
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
                    _header.Parent = null;
                }
                _header = value;
                if (!ReferenceEquals(_header, null))
                {
                    _header.Parent = this;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompositeBand()
        {
            Body = new BandCollection<ReportBand>();
        }

        #endregion

        #region Private members

        private ReportBand _footer, _header;

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

        /// <inheritdoc />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeRendering(context);

            ReportBand header = Header;
            if (!ReferenceEquals(header, null))
            {
                context.Index = -1;
                context.CurrentRecord = null;
                header.Render(context);
            }

            int count = context.Records.Count;
            for (int index = 0; index < count; index++)
            {
                context.Index = index;
                context.CurrentRecord = context.Records[index];

                foreach (ReportBand band in Body)
                {
                    band.Render(context);
                }
            }

            // base.Render(context);

            ReportBand footer = Footer;
            if (!ReferenceEquals(footer, null))
            {
                context.Index = -1;
                context.CurrentRecord = null;
                footer.Render(context);
            }

            OnAfterRendering(context);
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
