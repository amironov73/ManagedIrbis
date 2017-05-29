// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Diagnostics.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE || PocketPC

using System;

namespace System.Diagnostics
{
    /// <summary>
    /// For compatibility only.
    /// </summary>
    public sealed class DebuggerDisplayAttribute
        : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DebuggerDisplayAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DebuggerDisplayAttribute(string text)
        {
        }
    }
}

#endif
