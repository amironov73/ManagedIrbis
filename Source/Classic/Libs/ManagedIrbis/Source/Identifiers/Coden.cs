// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Coden.cs -- CODEN
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
    // https://en.wikipedia.org/wiki/CODEN
    //
    // CODEN - according to ASTM standard E250 - is a six character,
    // alphanumeric bibliographic code, that provides concise,
    // unique and unambiguous identification of the titles
    // of periodicals and non-serial publications from
    // all subject areas.
    //
    // CODEN became particularly common in the scientific community
    // as a citation system for periodicals cited in technical
    // and chemistry-related publications and as a search tool
    // in many bibliographic catalogues.
    //
    // The CODEN, designed by Charles Bishop (Chronic Disease
    // Research Institute at the University at Buffalo, State
    // University of New York, retired), was initially thought
    // as a memory aid for the publications in his reference
    // collection. Bishop took initial letters of words from
    // periodical titles thereby using a code, which helped
    // him arranging the collected publications. In 1953 he
    // published his documentation system, originally designed
    // as a four letter CODEN system; volume and page numbers
    // have been added, in order to cite and locate exactly
    // an article in a magazine. Later, a variation
    // was published 1957.
    //
    // After Bishop had assigned about 4,000 CODEN, the four letter
    // CODEN system was further developed since 1961 by Dr. Kuentzel
    // at the American Society for Testing of Material (ASTM).
    // He also introduced the fifth character to CODEN. In the
    // beginning of the computer age the CODEN was thought as
    // a machine-readable identification system for periodicals.
    // In several updates since 1963, CODEN were registered and
    // published in the CODEN for Periodical Titles by ASTM,
    // counting to about 128,000 at the end of 1974.
    //
    // Although it was soon recognized in 1966 that a five character
    // CODEN would not be sufficient to provide all future periodical
    // titles with CODEN, it was still defined as a five character
    // code as given in ASTM standard E250 until 1972. In 1976
    // the ASTM standard E250-76 defined a six-character CODEN.
    //
    // Beginning in the year 1975, the CODEN system was within the
    // responsibility of the American Chemical Society.
    //
    // Today, the first four characters of the six-character CODEN
    // for a periodical are taken from the initial letters
    // of the words from its title, followed by a fifth letter—one
    // of the first six letters (A–F) of the alphabet. The sixth
    // and last character of the CODEN is an alphanumeric check
    // character calculated from the preceding letters. CODEN
    // always uses capital letters.
    //
    // In contrast to a periodical CODEN, the first two characters
    // of a CODEN assigned to a non-serial publication (e.g.
    // conference proceedings) are digits. The third and fourth
    // characters are letters. The fifth and sixth character
    // corresponds to the serial CODEN, but differs in that
    // the fifth character is taken from all letters of the alphabet.
    //
    // In 1975 the International CODEN Service located at Chemical
    // Abstracts Service (CAS) became responsible for further
    // development of the CODEN. The CODEN is automatically
    // assigned to all publications referred on CAS. On request
    // of publishers the International CODEN Service also assigns
    // CODEN for non chemistry-related publications. For this reason
    // CODEN may also be found in other data bases (e.g. RTECS,
    // or BIOSIS), and are assigned also to serials or magazines,
    // which are not referred in CAS.
    //
    // CODEN assigned until 1966 can be looked up at the two-volume
    // CODEN for Periodical Titles issued by L.E. Kuentzel.
    // CODEN assigned until 1974 were published by J.G. Blumenthal.
    // CODEN assigned until 1998 and their disintegration can
    // be found at the International CODEN Directory (ISSN 0364-3670),
    // which has been published since 1980 as a microfiches issue.
    //
    // Finding a current CODEN is now best done with the online
    // database of CASSI (Chemical Abstracts Service Source Index),
    // covering all registered titles, CODEN, ISSN, ISBN,
    // abbreviations for publications indexed by CAS since 1907,
    // including serial and non-serial scientific and technical
    // publications.
    //
    // CASSI online is the replacement for CASSI as a printed
    // serial issue (ISSN 0738-6222, CODEN CASSE2), or as the
    // Collective Index (0001-0634, CODEN CASSI6). CASSI will
    // no longer be published in print. Only the CD-ROM issue
    // of CASSI (ISSN 1081-1990, CODEN CACDFE) will be published
    // furthermore.
    //
    // Examples
    //
    // * To the journal Nature the CODEN "NATUAS" is assigned.
    // * To Technology Review the CODEN "TEREAU" is assigned.
    // * The Proceedings of the International Conference
    // on Food Factors, Chemistry and Cancer Prevention
    // (ISBN 4-431-70196-6) uses the CODEN "66HYAL".
    // * To Recent Advances in Natural Products Research,
    // 3rd International Symposium on Recent Advances
    // in Natural Products Research the CODEN "69ACLK" is assigned.
    // * US patent applications use CODEN "USXXDP".
    // * German patent applications use CODEN "GWXXBX".
    //

    /// <summary>
    /// CODEN.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Coden
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
