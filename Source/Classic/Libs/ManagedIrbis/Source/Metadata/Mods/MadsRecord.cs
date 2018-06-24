// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MadsRecord.cs -- Metadata Authority Description Schema
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Metadata.Mods
{
    //
    // https://en.wikipedia.org/wiki/Metadata_Authority_Description_Schema
    //
    // Metadata Authority Description Schema (MADS) is an XML schema
    // developed by the United States Library of Congress' Network
    // Development and Standards Office that provides an authority
    // element set to complement the Metadata Object Description
    // Schema (MODS).

    /// <summary>
    /// Metadata Authority Description Schema
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MadsRecord
    {
    }
}
