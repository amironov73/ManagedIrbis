// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Iswc.cs -- ISWC
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
    // https://en.wikipedia.org/wiki/International_Standard_Musical_Work_Code
    //
    // International Standard Musical Work Code (ISWC) is a unique identifier
    // for musical works, similar to ISBN for books. It is adopted
    // as international standard ISO 15707. The ISO subcommittee with
    // responsibility for the standard is TC 46/SC 9.
    //
    // Format
    //
    // Each code is composed of three parts:
    //
    // * prefix element (1 character)
    // * work identifier (9 digits)
    // * check digit (1 digit)
    //
    // Currently, the only prefix defined is "T", indicating Musical works.
    // However, additional prefixes may be defined in the future to expand
    // the available range of identifiers and/or expand the system
    // to additional types of works.
    //
    // The check digit is calculated using the Luhn algorithm.
    //
    // ISWC identifiers are commonly written the form T-123.456.789-Z.
    // The grouping is for ease of reading only; the numbers do not
    // incorporate any information about the work's region, author,
    // publisher, etc. Rather, they are simply issued in sequence.
    // These separators are not required, and no other separators are allowed.
    //
    // The first ISWC was assigned in 1995, for the song "Dancing Queen"
    // by ABBA; the code is T-000.000.001-0.
    //

    /// <summary>
    /// International Standard Musical Work Code.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Iswc
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
