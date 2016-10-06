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
