// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeIrbisProvider.cs -- 
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

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;
using ManagedIrbis.Server;

using MoonSharp.Interpreter;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NativeIrbisProvider
        : IrbisProvider
    {
        #region Properties

        /// <summary>
        /// IRBIS64.DLL wrapper.
        /// </summary>
        [NotNull]
        public Irbis64Dll Irbis64 { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] Irbis64Dll irbis64
            )
        {
            Code.NotNull(irbis64, "irbis64");

            Irbis64 = irbis64;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] ServerConfiguration configuration
            )
        {
            Code.NotNull(configuration, "configuration");

            Irbis64 = new Irbis64Dll(configuration);
            _ownIrbis64 = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisProvider
            (
                [NotNull] string serverIniPath
            )
            : this (ServerConfiguration.FromIniFile(serverIniPath))
        {
        }

        #endregion

        #region Private members

        private bool _ownIrbis64;

        #endregion

        #region Public methods

        #endregion

        #region IrbisProvider members

        /// <inheritdoc cref="IrbisProvider.ExactSearchLinks"/>
        public override TermLink[] ExactSearchLinks
            (
                string term
            )
        {
            Code.NotNull(term, "term");

            return Irbis64.ExactSearchLinks(term);
        }

        /// <inheritdoc cref="IrbisProvider.ExactSearchTrimLinks"/>
        public override TermLink[] ExactSearchTrimLinks
            (
                string term,
                int limit
            )
        {
            Code.NotNull(term, "term");
            Code.Positive(limit, "limit");

            return Irbis64.ExactSearchTrimLinks(term, limit);
        }

        /// <inheritdoc cref="IrbisProvider.FileExist" />
        public override bool FileExist
            (
                FileSpecification specification
            )
        {
            return Irbis64.FileExist(specification);
        }

        /// <inheritdoc cref="IrbisProvider.FormatRecord" />
        public override string FormatRecord
            (
                MarcRecord record,
                string format
            )
        {
            Irbis64.SetFormat(format);
            NativeRecord native = NativeRecord.FromMarcRecord(record);
            Irbis64.SetRecord(native);
            string result = Irbis64.FormatRecord();

            return result;
        }

        /// <inheritdoc cref="IrbisProvider.FormatRecords" />
        public override string[] FormatRecords
            (
                int[] mfns, 
                string format
            )
        {
            List<string> result = new List<string>(mfns.Length);

            Irbis64.SetFormat(format);
            foreach (int mfn in mfns)
            {
                string text = Irbis64.FormatRecord(mfn);
                result.Add(text);
            }

            return result.ToArray();
        }

        /// <inheritdoc cref="IrbisProvider.GetMaxMfn" />
        public override int GetMaxMfn()
        {
            return Irbis64.GetMaxMfn();
        }

        /// <inheritdoc cref="IrbisProvider.ReadRecord" />
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            Irbis64.ReadRecord(mfn);
            NativeRecord native = Irbis64.GetRecord();
            MarcRecord result = native.ToMarcRecord();

            return result;
        }

        /// <inheritdoc cref="IrbisProvider.ReadTerms" />
        public override TermInfo[] ReadTerms
            (
                TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            string startTerm = parameters.StartTerm
                .ThrowIfNull("parameters.StartTerm");
            int number = parameters.NumberOfTerms;
            TermInfo[] result = parameters.ReverseOrder
                ? Irbis64.ListTermsReverse(startTerm, number)
                : Irbis64.ListTerms(startTerm, number);

            return result;
        }

        /// <inheritdoc cref="IrbisProvider.Search" />
        public override int[] Search
            (
                string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return new int[0];
            }

            return Irbis64.Search(expression);
        }

        /// <inheritdoc cref="IrbisProvider.WriteRecord" />
        public override void WriteRecord
            (
                MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            // TODO update record

            NativeRecord native = NativeRecord.FromMarcRecord(record);
            Irbis64.SetRecord(native);
            Irbis64.WriteRecord(true, false);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            if (_ownIrbis64)
            {
                Irbis64.Dispose();
            }
            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
