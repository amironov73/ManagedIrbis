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
    public sealed class DebuggerDisplayAttribute
        : Attribute
    {
        public DebuggerDisplayAttribute()
        {
        }

        public DebuggerDisplayAttribute(string text)
        {
        }
    }
}

#endif
