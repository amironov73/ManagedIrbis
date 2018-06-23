// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Prism.cs -- Publishing Requirements for Industry Standard Metadata
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

namespace ManagedIrbis.Metadata
{
    //
    // https://en.wikipedia.org/wiki/Publishing_Requirements_for_Industry_Standard_Metadata
    //
    // The Publishing Requirements for Industry Standard Metadata (PRISM)
    // specification defines a set of XML metadata vocabularies for syndicating,
    // aggregating, post-processing and multi-purposing content. PRISM provides
    // a framework for the interchange and preservation of content and metadata,
    // a collection of elements to describe that content, and a set
    // of controlled vocabularies listing the values for those elements.
    // PRISM can be XML, RDF/XML, or XMP and incorporates Dublin Core elements.
    // PRISM can be thought of as a set of XML tags used to contain
    // the metadata of articles and even tag article content.
    //
    // PRISM conforms to the World Wide Web standard for Namespaces.
    // PRISM namespaces are PRISM (prism:), PRISM Usage Rights (pur:),
    // Dublin Core (dc: and dcterms:), PRISM Inline Metadata (pim:),
    // PRISM Rights Language (prl:), PRISM Aggregator Message (pam:),
    // and PRISM Controlled Vocabulary (pcv:). PRISM incorporated existing
    // industry standards such as Dublin Core and XHTML in order to leverage
    // work that had already been done in the publishing industry.
    // New elements were created only when required, and were assigned
    // to PRISM specific namespaces.
    //
    // PRISM consists of three specifications. The PRISM Specification,
    // itself, provides definition for the overall PRISM framework.
    // A second specification, the PRISM Aggregator Message (PAM) Schema/DTD,
    // is a standard format for publishers to use for delivery of content
    // to websites, aggregators, and syndicators. PAM is available
    // as an XML DTD and an XML schema (XSD). Both PAM formats provides
    // a simple, flexible model for transmitting content and PRISM metadata.
    // The third, and newest, specification provides an XML schema (XSD)
    // for capture of content usage rights metadata. This Guide to PRISM Usage
    // Rights utilizes the elements found in PRISM’s Usage Rights Namespace
    // to allow users to comprehensively capture and relay rights metadata
    // for text and media content.
    //
    // In 1999, IDEAlliance contracted Linda Burman to found the PRISM
    // Working Group to address emerging publisher requirements for
    // a metadata standard to facilitate “agile” content for search,
    // digital asset management, content aggregation. Since that time,
    // individuals from more than 50 IDEAlliance member companies have
    // participated in the development of the specifications.
    //
    // PRISM is an IDEAlliance specification but is available free of charge.
    // IDEAlliance (International Digital Enterprise Alliance)
    // is a not-for-profit membership organization. Its mission is
    // to advance user-driven, cross-industry solutions for all publishing
    // and content-related processes by developing standards, fostering
    // business alliances, and identifying best practices.
    //
    // Many organizations use PRISM because it provides a common metadata
    // standard across platforms, media types and business units.
    // Organizations who are involved in any type of content creation,
    // categorization, management, aggregation and distribution, both
    // commercially and within intranet and extranet frameworks can use
    // the PRISM standards.
    //
    // The PRISM Working Group is open to all IDEAlliance members and
    // includes: Adobe Systems, Hachette Filipacchi Media, Hearst,
    // L.A. Burman Associates, LexisNexis, The McGraw-Hill Companies,
    // Reader’s Digest, Source Interlink Media Companies, Time Inc.,
    // The Nature Publishing Group, and U.S. News & World Report.
    //
    // PRISM can be incorporated into other standards and at this time,
    // the PRISM Working Group is only aware of PRISM incorporation
    // with RSS 1.0. See RSS 1.0[2] and the RSS 1.0 PRISM Module
    // for more information.
    //
    // The PRISM specification defines a set of metadata vocabularies.
    // PRISM metadata may be expressed in a different syntax depending
    // on the specific use-case scenario. Currently PRISM metadata
    // can be encoded XML, XML/RDF, or as XMP. Each of these expressions
    // of PRISM metadata is called a profile.
    //
    // * Profile 1 is for the expression of PRISM metadata in XML.
    // An example is the XML PRISM Aggregator Message (PAM).
    // * Profile 2 is for the expression of PRISM metadata in XML/RDF
    // such as for expressing PRISM metadata in RSS feeds.
    // * Profile 3 is for embedding PRISM metadata in media objects
    // such as digital images or PDFs using XMP technology.
    //
    // PRISM describes many components of print, online, mobile,
    // and multimedia content including the following:
    //
    // * Who created, contributed to, and owns the rights to the content?
    // * What locations, organizations, topics, people, and/or events it covers, the media it contains, and under what conditions it may be reproduced?
    // * When it was published? (cover date, post date, volume, number), withdrawn?
    // * Where it can be republished, and the original platform on which it appeared?
    // * How it can be reused?
    //
    // Common PRISM Usage
    //
    // * Syndication to partners
    // * Content aggregation
    // * Content repurposing
    // * Resource discovery and search optimization
    // * Multiple platform and channel distribution
    // * Content archiving
    // * Capture rights usage information
    // * Creation of feeds, such as RSS
    // * Standalone services
    // * Embedded descriptions, such as XMP
    // * Web publishing
    //
    // See: https://www.idealliance.org/prism-metadata/
    //

    class Prism
    {
    }
}
