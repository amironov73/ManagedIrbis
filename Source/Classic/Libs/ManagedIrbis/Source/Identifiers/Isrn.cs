// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isrn.cs -- ISRN
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
    // https://en.wikipedia.org/wiki/Technical_report#ISRN
    //
    // Technical reports are now commonly published electronically,
    // whether on the Internet or on the originating organization's
    // intranet.
    //
    // Many organizations collect their technical reports into
    // a formal series. Reports are then assigned an identifier
    // (report number, volume number) and share a common cover-page
    // layout. The entire series might be uniquely identified by an ISSN.
    //
    // A registration scheme for a globally unique International Standard
    // Technical Report Number (ISRN) was standardized in 1994 (ISO 10444),
    // but was never implemented in practice. ISO finally withdrew this
    // standard in December 2007. It aimed to be an international extension
    // of a report identifier scheme used by U.S. government agencies
    // (ANSI/NISO Z39.23).
    //

    /// <summary>
    /// International Standard Technical Report Number.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isrn
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
