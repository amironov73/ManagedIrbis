// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Collections;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.BibTex
{
    /// <summary>
    /// Коллекция записей.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordCollection
        : NonNullCollection<BibTexRecord>
    {
    }
}
