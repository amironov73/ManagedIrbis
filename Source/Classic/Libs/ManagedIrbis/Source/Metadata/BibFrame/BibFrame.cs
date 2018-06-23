// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BibFrame.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Metadata.BibFrame
{
    //
    // https://en.wikipedia.org/wiki/BIBFRAME
    //
    // BIBFRAME (Bibliographic Framework) is a data model for bibliographic
    // description. BIBFRAME was designed to replace the MARC standards,
    // and to use linked data principles to make bibliographic data more
    // useful both within and outside the library community.
    //
    // History
    //
    // The MARC Standards, which BIBFRAME seeks to replace, were developed
    // by Henriette Avram at the US Library of Congress during the 1960s.
    // By 1971, MARC formats had become the national standard for dissemination
    // of bibliographic data in the United States, and the international
    // standard by 1973.
    //
    // In a provocatively titled 2002 article, library technologist Roy Tennant
    // argued that "MARC Must Die", noting that the standard was old;
    // used only within the library community; and designed to be a display,
    // rather than a storage or retrieval format. A 2008 report from
    // the Library of Congress wrote that MARC is "based on forty-year
    // old techniques for data management and is out of step with programming
    // styles of today."
    //
    // In 2012, the Library of Congress announced that it had contracted
    // with Zepheira, a data management company, to develop a linked data
    // alternative to MARC. Later that year, the library announced a new model
    // called MARC Resources (MARCR). That November, the library released
    // a more complete draft of the model, renamed BIBFRAME.
    //
    // The Library of Congress released version 2.0 of BIBFRAME in 2016.
    //
    // Design
    //
    // BIBFRAME is expressed in RDF and based on three categories of abstraction
    // (work, instance, item), with three additional classes (agent, subject,
    // event) that relate to the core categories. While the work entity
    // in BIBFRAME may be "considered as the union of the disjoint work
    // and expression entities" in IFLA's Functional Requirements
    // for Bibliographic Records (FRBR) entity relationship model,
    // BIBFRAME's instance entity is analogous to the FRBR manifestation
    // entity. This represents an apparent break with FRBR and the FRBR-based
    // Resource Description and Access (RDA) cataloging code. However,
    // the original BIBFRAME model argues that the new model "can reflect
    // the FRBR relationships in terms of a graph rather than as hierarchical
    // relationships, after applying a reductionist technique." Since
    // both FRBR and BIBFRAME have been expressed in RDF, interoperability
    // between the two models is technically possible.
    //
    // Specific formats
    //
    // While the BIBFRAME model currently includes a serial entity, there
    // are still a number of issues to be addressed before the model can
    // be used for serials cataloging. BIBFRAME lacks several serials-related
    // data fields available in MARC.
    //
    // A 2014 report was very positive on BIBFRAME's suitability for
    // describing audio and video resources. However, the report also
    // expressed some concern about the high-level Work entity, which
    // is unsuitable for modeling certain audio resources.
    //
    // Implementations
    //
    // Colorado College's Tutt Library has created several experimental
    // apps using BIBFRAME.
    // 14 other research libraries are testing the model.
    // ExLibris published a roadmap to implement BIBFRAME in its library
    // systems, which includes a MARC-to-BIBFRAME transformation.
    //
    // Related initiatives and standards
    //
    // RDA, FRBR, FRBRoo, FRAD, and FRSAD are available in RDF in the
    // Open Metadata Registry, a metadata registry.
    // Schema Bib Extend project, a W3C-sponsored community group has
    // worked to extend Schema.org to make it suitable for bibliographic
    // description.
    //
    // See: https://www.loc.gov/bibframe/
    // See: http://id.loc.gov/ontologies/bibframe
    //
    // See also: https://github.com/zepheira/bibframe
    //

    class BibFrame
    {
    }
}
