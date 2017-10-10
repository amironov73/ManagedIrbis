// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommandKind.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using ManagedIrbis.Infrastructure.Commands;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Kind of <see cref="AbstractCommand"/>
    /// </summary>
    [Flags]
    public enum CommandKind
    {
        /// <summary>
        /// None (default).
        /// </summary>
        None = 0,

        /// <summary>
        /// Extended command.
        /// </summary>
        Extended = 1,

        /// <summary>
        /// Writes to database.
        /// </summary>
        WriteAccess = 2,

        /// <summary>
        /// Administrative privileges.
        /// </summary>
        Admin = 4,

        /// <summary>
        /// Dangerous.
        /// </summary>
        Dangerous = 8
    }
}
