// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Istc.cs -- ISTC
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
    // https://en.wikipedia.org/wiki/International_Standard_Text_Code
    //
    // The International Standard Text Code (ISTC) is a unique identifier
    // for text-based works. The ISO standard was developed by TC 46/SC 9
    // and published in March 2009 as ISO 21047:2009. The authority responsible
    // for implementing the standard is The International ISTC Agency.
    //
    // By including one or more ISTC numbers as an attribute of
    // a bibliographic record (e.g., an ISBN record), the aggregation,
    // collocation, filtering, etc. of publication records can be achieved
    // automatically based on the content of the relevant publications.
    // This solves the problem of identifying the relevant content when
    // it is published under different titles, or where different content
    // is published under the same title. The ISTC also enables many
    // improvements in efficiency, such as enabling retail websites
    // to accurately re-use reviews and subject classifications applied
    // to one publication on every other publication of the same work.
    //
    // Another application of ISTCs involves using them to identify distinct
    // but related works. E.g., the bibliographic records for a number
    // of derivations, such as translations of the same work, can include
    // the ISTC for that original work and thus be automatically grouped
    // together, even though the records are for publications of distinct
    // works with their own individual titles.
    //
    // A single database is used to hold all ISTC records, regardless
    // of which country they were registered in. Anybody wishing to register
    // a textual work, e.g., an author, agent or publisher, must submit
    // a request to an ISTC registration agency with the necessary metadata
    // needed to distinguish that work from all others. This enables each
    // request for the registration of a textual work to be checked
    // for global uniqueness. If a work has not already been registered
    // (i.e., if the metadata supplied on the registration request
    // is found to be unique), then a new ISTC number is returned
    // by the system; if a work has already been registered (i.e.,
    // if the metadata supplied on the registration request matches
    // that of an existing ISTC record), then the existing ISTC number
    // is returned. There is no concept of ownership of an ISTC number;
    // the same number should be used by anyone wherever the same work
    // appears and needs to be identified. There is no restriction
    // concerning which registration agency each registration request
    // must be submitted through. Anybody wishing to check whether
    // or not a particular work has already been registered will
    // be able to do so by accessing a free-to-use search facility
    // available on the International ISTC Agency website.
    //
    // Format
    //
    // ISTC numbers are hexadecimal, so may be formed from numbers
    // 0-9 and letters A-F. They are made up of four parts:
    //
    // * registry agency element - denotes which agency the work was
    //   registered through;
    // * registration year element - denotes what year the work was
    //   registered in;
    // * work element - 8 digit hexadecimal number, unique within
    //   year/agency;
    // * check digit - calculated using a MOD 16-3 algorithm defined
    //   by ISO 7064.
    //
    // Example: ISTC A02-2009-000004BE-A
    //

    /// <summary>
    /// International Standard Text Code.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Istc
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
