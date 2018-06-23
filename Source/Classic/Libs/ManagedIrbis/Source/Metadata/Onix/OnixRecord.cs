// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OnixRecord.cs --
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

namespace ManagedIrbis.Metadata.Onix
{
    //
    // https://en.wikipedia.org/wiki/ONIX_for_Books
    //
    // ONIX for Books is an XML format for sharing bibliographic data
    // pertaining to both traditional books and eBooks. It is the oldest
    // of the three ONIX standards, and is widely implemented in the book
    // trade in North America, Europe and increasingly in the Asia-Pacific
    // region. It allows book and ebook publishers to create and manage
    // a corpus of rich metadata about their products, and to exchange
    // it with their customers (distributors and retailers) in a coherent,
    // unambiguous, and largely automated manner.
    //
    // The ONIX for Books standard provides a free-to-use format for passing
    // descriptive metadata about books between publishers, data aggregators,
    // book retailers and other interested parties in the publishing industry.
    // Metadata concerning one or more book titles can be stored in a suitably
    // formatted XML file known as an 'ONIX message' ready for dissemination.
    // Whereas other data standards exist for storing the contents of
    // a book - the text, layout and graphics - the ONIX for Books standard
    // holds information about the book, similar to, but more extensive than,
    // the information one would typically find on the cover or title page
    // of a printed book or in a library catalog. The ONIX for Books standard
    // provides a way to communicate information about a book's author,
    // publisher, price, publication date, physical dimensions, synopsis
    // and many other details besides. The standard is quite extensive
    // and most publishers currently provide only a few dozen of the many
    // hundreds of pieces of information that the standard is designed to carry.
    //
    // The organisations responsible for creating the ONIX for Books standard were:
    //
    // * Association of American Publishers
    // * EDItEUR
    //
    // aided by:
    //
    // * Book Industry Study Group (BISG) in the US
    // * Book Industry Communication (BIC) in the UK
    //
    // The standard is now maintained and developed for global use by EDItEUR,
    // with guidance from an international steering committee representing
    // the book trade in countries where ONIX is used.
    //
    // ONIX for Books Release 1.0 was published in 2000. Revisions were made
    // in releases 1.1, 1.2 and 1.2.1.
    //
    // Release 2.0 was issued in 2001. A backwards-compatible version,
    // Release 2.1, arrived in June 2003. Three minor revisions intended
    // for general use have been made since then, the most recent
    // in January 2006. A further revision intended solely for use
    // in Japan was issued in 2010.
    //
    // Release 3.0 was published in April 2009 with some corrections
    // in 2010, and the first minor revision (labelled 3.0.1) was issued
    // in January 2012. A second minor revision (3.0.2) was published
    // in January 2014 and a third in April 2016. The latest version
    // is 3.0.4, released in October 2017, and the standard continues
    // to evolve to meet new business requirements as they emerge.
    // This 3.0 release has not yet completely replaced 2.1, though
    // implementation of 3.0 is becoming more widespread (particularly
    // outside the English-language publishing markets). There is also
    // an Acknowledgement message format (published 2015) which recipients
    // of ONIX data files may send to confirm receipt of ONIX messages.
    //
    // The authors have stated that any new revisions will be based on,
    // and backwards-compatible with, Release 3.0.[1] The international
    // steering committee announced in January 2012 that support for
    // version 2.1 would be reduced at the end of December 2014.
    //
    // Releases 2.1 and 3.0 share a set of 'Codelists' or controlled
    // vocabularies, that are extended regularly to allow new types
    // of information to be carried without having to revise the main
    // specifications. From Issue 37 of the controlled vocabularies,
    // additions are applicable only to ONIX 3.0, and ONIX 2.1 is limited
    // to Issue 36 or earlier.
    //

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class OnixRecord
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public FieldCollection Fields { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public OnixRecord()
        {
            Fields = new FieldCollection();
        }

        #endregion
    }
}
