// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectedFormatter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectedFormatter
        : IPftFormatter
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Format source.
        /// </summary>
        [CanBeNull]
        public string Source { get; set; }

        /// <inheritdoc cref="IPftFormatter.SupportsExtendedSyntax" />
        public bool SupportsExtendedSyntax { get { return false; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectedFormatter
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region IPftFormatter members

        /// <inheritdoc cref="IPftFormatter.FormatRecord(MarcRecord)" />
        public string FormatRecord
            (
                MarcRecord record
            )
        {
            if (ReferenceEquals(record, null)
                || string.IsNullOrEmpty(Source))
            {
                return string.Empty;
            }

            string result = Connection.FormatRecord(Source, record)
                ?? string.Empty;

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.FormatRecord(Int32)" />
        public string FormatRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            if (string.IsNullOrEmpty(Source))
            {
                return string.Empty;
            }

            string result = Connection.FormatRecord(Source, mfn)
                ?? string.Empty;

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.ParseProgram" />
        public void ParseProgram
            (
                string source
            )
        {
            Source = source;
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
