// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeIrbisFormatter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NativeIrbisFormatter
        : IPftFormatter
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public NativeIrbisProvider Provider { get; private set; }

        /// <inheritdoc cref="IPftFormatter.SupportsExtendedSyntax" />
        public bool SupportsExtendedSyntax { get { return false; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeIrbisFormatter
            (
                [NotNull] NativeIrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        #endregion

        #region IPftFormatter members

        /// <inheritdoc cref="IPftFormatter.FormatRecord(MarcRecord)" />
        public string FormatRecord
            (
                MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                return string.Empty;
            }

            Irbis64Dll irbis = Provider.Irbis64;
            NativeRecord native = NativeRecord.FromMarcRecord(record);
            irbis.SetRecord(native);
            string result = irbis.FormatRecord();

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.FormatRecord(Int32)" />
        public string FormatRecord(int mfn)
        {
            Irbis64Dll irbis = Provider.Irbis64;
            string result = irbis.FormatRecord(mfn);

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.FormatRecords" />
        public string[] FormatRecords
            (
                int[] mfns
            )
        {
            Code.NotNull(mfns, "mfns");

            string[] result = new string[mfns.Length];
            Irbis64Dll irbis = Provider.Irbis64;
            for (int i = 0; i < mfns.Length; i++)
            {
                result[i] = irbis.FormatRecord(mfns[i]);
            }

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.ParseProgram" />
        public void ParseProgram
            (
                string source
            )
        {
            Provider.Irbis64.SetFormat(source);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
