// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ModsRecord.cs -- Metadata Object Description Schema
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
    // https://en.wikipedia.org/wiki/Metadata_Object_Description_Schema
    //
    // The Metadata Object Description Schema (MODS) is an XML-based
    // bibliographic description schema developed by the United States
    // Library of Congress' Network Development and Standards Office.
    // MODS was designed as a compromise between the complexity
    // of the MARC format used by libraries and the extreme simplicity
    // of Dublin Core metadata.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ModsRecord
    {
    }
}
