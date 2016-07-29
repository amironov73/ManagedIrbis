/* System.cs -- temporary solution for .NET Core compability
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE

using System;

namespace System
{
    public sealed class NonSerializedAttribute
        : Attribute
    {
    }
}

#endif
