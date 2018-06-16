// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isan.cs -- ISAN
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
    // https://en.wikipedia.org/wiki/International_Standard_Audiovisual_Number
    //
    // International Standard Audiovisual Number (ISAN) is a unique identifier
    // for audiovisual works and related versions, similar to ISBN for books.
    // It was developed within an ISO (International Organisation
    // for Standardisation) TC46/SC9 working group. ISAN is managed
    // and run by ISAN-IA.
    //
    // The ISAN standard (ISO standard 15706:2002 & ISO 15706-2)
    // is recommended or required as the audiovisual identifier
    // of choice for producers, studios, broadcasters, Internet media
    // providers and video games publishers who need to encode, track,
    // and distribute video in a variety of formats. It provides a unique,
    // internationally recognized and permanent reference number
    // for each audiovisual work and related versions registered
    // in the ISAN system.
    //
    // ISAN identifies works throughout their entire life cycle from
    // conception, to production, to distribution and consumption.
    //
    // ISANs can be incorporated in both digital and physical media,
    // such as theatrical release prints, DVDs, publications, advertising,
    // marketing materials and packaging, as well as licensing contracts
    // to uniquely identify works.
    //
    // The ISAN identifier is incorporated in many draft and final standards
    // such as AACS, DCI, MPEG, DVB, and ATSC. The identifier can be
    // provided under descriptor 13 (0x0D) for Copyright identification
    // system and reference within an ITU-T Rec. H.222
    // or ISO/IEC 13818 program.
    //
    // The ISAN is a 12 byte block comprising three segments:
    // a 6 byte root, a 2 byte episode or part, and a 4 byte version.

    /// <summary>
    /// International Standard Audiovisual Number.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isan
    {
        #region Properties

        /// <summary>
        /// 6-byte root.
        /// </summary>
        public long Root { get; set; }

        /// <summary>
        /// 2-byte episode or part.
        /// </summary>
        public short Episode { get; set; }

        /// <summary>
        /// 4-byte version.
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}
