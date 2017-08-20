// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IPftFormatter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPftFormatter
        : IDisposable
    {
        /// <summary>
        /// Whether the formatter supports the extended syntax.
        /// </summary>
        bool SupportsExtendedSyntax { get; }

        /// <summary>
        /// Format the record.
        /// </summary>
        [NotNull]
        string FormatRecord
            (
                [CanBeNull] MarcRecord record
            );

        /// <summary>
        /// Format the record.
        /// </summary>
        [NotNull]
        string FormatRecord
            (
                int mfn
            );

        /// <summary>
        /// Parse the program.
        /// </summary>
        void ParseProgram
            (
                [NotNull] string source
            );
    }
}
