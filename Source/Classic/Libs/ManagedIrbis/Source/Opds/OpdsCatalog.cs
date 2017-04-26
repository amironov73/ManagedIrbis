// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OpdsCatalog.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Opds
{
    //
    // https://ru.wikipedia.org/wiki/OPDS
    //
    // OPDS (англ. Open Publication Distribution System)
    // — электронный каталог формата синдикации,
    // основанный на Atom и HTTP.
    // OPDS-каталоги позволяют читать, сортировать 
    // и распространять электронные издания.
    // OPDS-каталоги используют существующие
    // или формирующиеся открытые стандарты и конвенции,
    // направленные на упрощение взаимодействия.
    //
    // http://opds-spec.org/
    //
    // The Open Publication Distribution System (OPDS)
    // Catalog specification is a syndication format
    // for electronic publications based on Atom RFC4287
    // and HTTP RFC2616.
    //
    // All of the formal versions of the OPDS Catalog
    // specifications are available on the specifications
    // page http://opds-spec.org/specs/.
    //

    //
    // Feedbooks (Thousands of free books & books
    // from major publishers in the US, France & Germany,
    // http://www.feedbooks.com/catalog.atom)
    //
    // Internet Archive(1.8 million free books,
    // http://bookserver.archive.org/catalog/)
    //
    // O'Reilly Media (hundreds of technical ebooks,
    // http://opds.oreilly.com/opds/)
    //
    // The Pragmatic Programmers
    // (http://pragprog.com/catalog.opds)
    //
    // Revues.org (http://bookserver.revues.org)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class OpdsCatalog
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
