// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioContext.cs -- 
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
using AM.Text;
using AM.Text.Output;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioContext
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Processor.
        /// </summary>
        [CanBeNull]
        public BiblioProcessor Processor { get; set; }

        /// <summary>
        /// Document.
        /// </summary>
        [NotNull]
        public BiblioDocument Document { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Log.
        /// </summary>
        [NotNull]
        public AbstractOutput Log { get; private set; }

        /// <summary>
        /// Count of <see cref="BiblioItem"/>s.
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// All the gathered records.
        /// </summary>
        [NotNull]
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// Bad records.
        /// </summary>
        [NotNull]
        public RecordCollection BadRecords { get; private set; }

        /// <summary>
        /// Context for report.
        /// </summary>
        [NotNull]
        public ReportContext ReportContext { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioContext
            (
                [NotNull] BiblioDocument document,
                [NotNull] IrbisProvider provider,
                [NotNull] AbstractOutput log
            )
        {
            Code.NotNull(document, "document");
            Code.NotNull(provider, "provider");
            Code.NotNull(log, "log");

            Document = document;
            Provider = provider;
            Log = log;
            ItemCount = 0;
            ReportContext = new ReportContext(provider);
            Records = new RecordCollection();
            BadRecords = new RecordCollection();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find the record with specified MFN.
        /// </summary>
        [CanBeNull]
        public MarcRecord FindRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord result = Records
                .FirstOrDefault(record => record.Mfn == mfn);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BiblioContext> verifier
                = new Verifier<BiblioContext>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
