// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftNodeKind.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// <see cref="PftNode"/> kind.
    /// </summary>
    [Flags]
    public enum PftNodeKind
    {
        /// <summary>
        /// None (default).
        /// </summary>
        None = 0,

        /// <summary>
        /// Constant expression.
        /// </summary>
        Constant = 1,

        /// <summary>
        /// Requires server connection.
        /// </summary>
        Connection = 2,

        /// <summary>
        /// Extended syntax.
        /// </summary>
        ExtendedSyntax = 4,

        /// <summary>
        /// Writes to database.
        /// </summary>
        WriteAccess = 8,

        /// <summary>
        /// Administrative privileges.
        /// </summary>
        Admin = 16,

        /// <summary>
        /// Dangerous.
        /// </summary>
        Dangerous = 32,

        /// <summary>
        /// Complex expression.
        /// </summary>
        Complex = 64
    }
}
