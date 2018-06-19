// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldCollection.cs --
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
    /// Коллекция полей записи.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldCollection
        : NonNullCollection<BibTexField>
    {
    }
}
