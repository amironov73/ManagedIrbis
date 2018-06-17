// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isci.cs -- ISCI
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
    // https://en.wikipedia.org/wiki/Industry_Standard_Coding_Identification
    //
    // Industry Standard Coding Identification (ISCI, also known
    // as Industry Standard Commercial Identification) was a standard
    // created to identify commercials that aired on TV in the United States,
    // for ad agencies and advertisers from 1970.
    //

    /// <summary>
    /// Industry Standard Coding Identification.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isci
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
