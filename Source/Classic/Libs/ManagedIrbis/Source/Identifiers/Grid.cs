// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Grid.cs -- GRid
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
    // https://en.wikipedia.org/wiki/Global_Release_Identifier
    //
    // The Global Release Identifier (GRid) is a system to identify
    // releases of digital sound recordings (and other digital data)
    // for electronic distribution. It is designed to be integrated
    // with identification systems deployed by key stakeholders from
    // across the music industry.
    //
    // (GRid should not be confused with the Global Repertoire Database
    // (GRD), a system to track ownership and control of musical works,
    // which was planned from 2008-2014 but ultimately failed.)
    //
    // Basic construction
    //
    // A GRid consists of 18 alphanumerical characters (numerical digits
    // and capital letters as defined in ISO/IEC 646:1991-IRV, which
    // is identical to ASCII) that are grouped into four elements as follows:
    //
    // * Identifier Scheme element (2 characters)
    // "A1" denotes a GRid.
    //
    // * Issuer Code element (5 characters)
    // A unique identifier of the organisation responsible for allocating
    // the GRid, issued by the International GRid Authority (i.e., the IFPI).
    //
    // * Release Number element (10 characters)
    // Uniquely identifies the specific bundle of digital resources compiled
    // by the issuer, where “a digital resource is a digital fixation
    // of an expression of an abstract work, such as a sound recording,
    // an audio-visual recording, a photograph, software, a graphic image
    // or a passage of text.”
    //
    // * Check Character element (1 character)
    // The check character is computed according to ISO 7064:1983 Mod 37, 36.
    //
    // When a GRid is written, printed or otherwise visually presented,
    // the four elements of the GRid should be separated from each other
    // by a hyphen. For clarity, it can also be prefixed with "GRid:".
    // Neither the hyphens nor the "GRid:"-prefix form part of the GRid.
    // It is recommended that when a GRid is visually presented, the font
    // used should clearly distinguish between the digits `1` and `0`
    // on the one hand, and the letters `I` and `O` on the other hand.
    //
    // The following character strings all denote the same GRid:
    //
    // A12425GABC1234002M
    // A1-2425G-ABC1234002-M
    // GRid:A1-2425G-ABC1234002-M
    //
    // Where:
    //
    // A1 - Identifier Scheme element
    // 2425G - Issuer Code element
    // ABC1234002 - Release Number element
    // M - Check Character element
    //

    /// <summary>
    /// Global Release Identifier.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Grid
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
