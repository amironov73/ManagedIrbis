// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Csl.cs -- Citation Style Language
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

namespace ManagedIrbis.ImportExport
{
    //
    // https://en.wikipedia.org/wiki/Citation_Style_Language
    //
    // The Citation Style Language (CSL) is an open XML-based language
    // to describe the formatting of citations and bibliographies.
    // Reference management programs using CSL include Zotero, Mendeley
    // and Papers.
    //
    // CSL was created by Bruce D'Arcus for use with OpenOffice.org,
    // and an XSLT-based "CiteProc" CSL processor. CSL was further
    // developed in collaboration with Zotero developer Simon Kornblith.
    // Since 2008, the core development team consists of D'Arcus, Frank
    // Bennett and Rintze Zelle.
    //
    // The releases of CSL are 0.8 (March 21, 2009), 0.8.1 (February 1, 2010),
    // 1.0 (March 22, 2010), and 1.0.1 (September 3, 2012). CSL 1.0 was
    // a backward-incompatible release, but styles in the 0.8.1 format
    // can be automatically updated to the CSL 1.0 format.
    //
    // On its release in 2006, Zotero became the first application to
    // adopt CSL. In 2008 Mendeley was released with CSL support,
    // and in 2011, Papers and Qiqqa gained support for CSL-based
    // citation formatting.
    //
    // See: http://www.citationstyles.org/
    //

    class Csl
    {
    }
}
