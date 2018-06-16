// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isadn.cs -- ISADN
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
    // https://en.wikipedia.org/wiki/International_Standard_Authority_Data_Number
    //
    // The International Standard Authority Data Number (ISADN) was
    // a registry proposed by the International Federation of Library
    // Associations and Institutions (IFLA) to provide and maintain
    // unique identifiers for entities described in authority data.
    // Having such a unique number would have the benefits of being
    // language-independent and system-independent.
    //
    // Francoise Bourdon was a major proponent of such a standard,
    // proposing a structure for the ISADN and recommending that the
    // number uniquely identify authority records, rather than
    // their subjects.
    //
    // A 1989 article by Delsey described the work on the IFLA
    // Working Group on an International Authority System, spending
    // a good portion of time on conceptualizing an international
    // standard number "that will facilitate the linkage of variant
    // authorities for the same identity." Their discussion was
    // very complex in its discussion of which agencies would actually
    // assign such numbers. For example, a national library might
    // be tasked with assigning identifiers to authors within
    // its country, but this would lead to duplicate identifiers
    // for authority data that describe transnational people.
    //
    // The project was ultimately determined to be unfeasible.
    // Tillett suggested that the cluster identifiers used by the
    // Virtual International Authority File might meet the needs
    // expressed in the proposal.
    //
    // The concept of an ISADN continues to be relevant to the information
    // science community, as it could be a great help in the problem
    // of measuring an individual author's research output.
    //

    /// <summary>
    /// International Standard Authority Data Number.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isadn
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
