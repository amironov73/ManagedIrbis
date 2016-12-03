// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Reflection.cs -- temporary solution for .NET Core compability
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE

using System;

namespace System.Reflection
{
    public sealed class Assembly
    {
    }
}

#endif
