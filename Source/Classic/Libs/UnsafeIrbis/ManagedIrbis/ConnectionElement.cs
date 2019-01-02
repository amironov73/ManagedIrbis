// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectionElement.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ConnectionElement
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Host name or address.
        /// </summary>
        Host = 1,

        /// <summary>
        /// Port number.
        /// </summary>
        Port = 2,

        /// <summary>
        /// User name.
        /// </summary>
        Username = 4,

        /// <summary>
        /// Password.
        /// </summary>
        Password = 8,

        /// <summary>
        /// Workstation.
        /// </summary>
        Workstation = 16,

        /// <summary>
        /// All of above.
        /// </summary>
        All = 31
    }
}
