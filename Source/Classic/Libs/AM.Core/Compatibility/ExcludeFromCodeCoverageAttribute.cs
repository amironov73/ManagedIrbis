// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExcludeFromCodeCoverageAttribute.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW35 || WINMOBILE || PocketPC || UAP

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// For compatibility only.
    /// </summary>
    public sealed class ExcludeFromCodeCoverageAttribute
        : Attribute
    {
    }
}

#endif
