// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Bici.cs -- Book Item and Component Identifier.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // https://en.wikipedia.org/wiki/Book_Item_and_Component_Identifier
    //
    // The Book Item and Component Identifier, or BICI, is a draft
    // standard of the United States National Information Standards
    // Organization (NISO) that would provide a unique identifier
    // for items or components within a book or publication to which
    // an International Standard Book Number (ISBN) has been assigned.
    // It is related to the Serial Item and Contribution Identifier (SICI).
    //
    // http://www.niso.org/standards/resources/bici.html
    // http://www.niso.org/pdfs/BICI-DS.pdf
    //

    /// <summary>
    /// Book Item and Component Identifier.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Bici
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
