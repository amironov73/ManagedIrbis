// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isli.cs -- ISLI
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
    // https://en.wikipedia.org/wiki/International_Standard_Link_Identifier
    //
    // The International Standard Link Identifier (ISLI), is an identifier
    // standard. ISLI is a universal identifier for links between entities
    // in the field of information and documentation. It was developed
    // by the International Organization for Standardization (ISO)
    // and published on May 15, 2015. ISO/TC 46/SC 9 is responsible
    // for the development of the ISLI standard.
    //
    // ISLI is used for identifying links between entities in the field
    // of information and documentation. A linked entity can be physical,
    // e.g. a print book or an electronic resource (text, audio, and video);
    // or something abstract, e.g. a physical position within a frame
    // of reference or the time of day.
    //
    // In the context of modern information technology, the application
    // of resources in the field of information and documentation
    // is increasingly getting diversified. Isolated content products
    // can no longer satisfy the ever-increasing user demand.
    //
    // Using a link identifier to build links between resources
    // in the field of information and documentation provides a basis
    // for a combined application of resources in the field, and supports
    // collaborative creation of content and data interoperability
    // between systems.
    //
    // The openness of the ISLI system will boost the emergence of new
    // applications in both multimedia and other fields, which increases
    // the value of the linked-resources.
    //
    // The link model of ISLI includes three elements: a source, a target,
    // and the link between them.
    //

    /// <summary>
    /// International Standard Link Identifier.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Isli
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
