// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DublinRecord.cs --
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

namespace ManagedIrbis.Metadata.DublinCore
{
    //
    // https://en.wikipedia.org/wiki/Dublin_Core
    //
    // The Dublin Core Schema is a small set of vocabulary terms that
    // can be used to describe digital resources (video, images,
    // web pages, etc.), as well as physical resources such as books
    // or CDs, and objects like artworks. The full set of Dublin Core
    // metadata terms can be found on the Dublin Core Metadata Initiative
    // (DCMI) website. The original set of 15 classic metadata terms,
    // known as the Dublin Core Metadata Element Set (DCMES), is endorsed
    // in the following standards documents:
    //
    // * IETF RFC 5013
    // * ISO Standard 15836-2009
    // * NISO Standard Z39.85
    //
    // Dublin Core metadata may be used for multiple purposes, from
    // simple resource description to combining metadata vocabularies
    // of different metadata standards, to providing interoperability
    // for metadata vocabularies in the linked data cloud and Semantic
    // Web implementations.
    //
    // "Dublin" refers to Dublin, Ohio, USA where the schema originated during
    // the 1995 invitational OCLC/NCSA Metadata Workshop, hosted
    // by the OCLC (Online Computer Library Center), a library consortium
    // based in Dublin, and the National Center for Supercomputing
    // Applications (NCSA). "Core" refers to the metadata terms as
    // "broad and generic being usable for describing a wide range
    // of resources". The semantics of Dublin Core were established
    // and are maintained by an international, cross-disciplinary group
    // of professionals from librarianship, computer science, text encoding,
    // museums, and other related fields of scholarship and practice.
    //
    // Starting in 2000, the Dublin Core community focused on "application
    // profiles" - the idea that metadata records would use Dublin Core
    // together with other specialized vocabularies to meet particular
    // implementation requirements. During that time, the World Wide Web
    // Consortium's work on a generic data model for metadata, the
    // Resource Description Framework (RDF), was maturing. As part
    // of an extended set of DCMI metadata terms, Dublin Core became
    // one of the most popular vocabularies for use with RDF, more
    // recently in the context of the linked data movement.
    //
    // The Dublin Core Metadata Initiative (DCMI) provides an open
    // forum for the development of interoperable online metadata standards
    // for a broad range of purposes and of business models. DCMI's
    // activities include consensus-driven working groups, global
    // conferences and workshops, standards liaison, and educational
    // efforts to promote widespread acceptance of metadata standards
    // and practices. In 2008, DCMI separated from OCLC and incorporated
    // as an independent entity.
    //
    // Currently, any and all changes that are made to the Dublin Core
    // standard, are reviewed by a DCMI Usage Board within the context
    // of a DCMI Namespace Policy (DCMI-NAMESPACE). This policy describes
    // how terms are assigned and also sets limits on the amount
    // of editorial changes allowed to the labels, definitions,
    // and usage comments.
    //
    // The Dublin Core standard originally included two levels:
    // Simple and Qualified. Simple Dublin Core comprised 15 elements;
    // Qualified Dublin Core included three additional elements
    // (Audience, Provenance and RightsHolder), as well as a group
    // of element refinements (also called qualifiers) that could refine
    // the semantics of the elements in ways that may be useful
    // in resource discovery.
    //
    // Since 2012, the two have been incorporated into the DCMI Metadata
    // Terms as a single set of terms using the RDF data model.
    // The full set of elements is found under the namespace
    // http://purl.org/dc/terms/. Because the definition of the terms
    // often contains domains and ranges, which may not be compatible
    // with the pre-RDF definitions used for the original 15 Dublin Core
    // elements, there is a separate namespace for the original
    // 15 elements as previously defined: http://purl.org/dc/elements/1.1/.
    //
    // Dublin Core Metadata Element Set
    // The original DCMES Version 1.1 consists of 15 metadata elements:
    //
    // * Title
    // * Creator
    // * Subject
    // * Description
    // * Publisher
    // * Contributor
    // * Date
    // * Type
    // * Format
    // * Identifier
    // * Source
    // * Language
    // * Relation
    // * Coverage
    // * Rights
    //
    // Each Dublin Core element is optional and may be repeated.
    // The DCMI has established standard ways to refine elements
    // and encourage the use of encoding and vocabulary schemes.
    // There is no prescribed order in Dublin Core for presenting
    // or using the elements. The Dublin Core became ISO 15836 standard
    // in 2006 and is used as a base-level data element set for the
    // description of learning resources in the ISO/IEC 19788-2 Metadata
    // for learning resources (MLR) – Part 2: Dublin Core elements,
    // prepared by the ISO/IEC JTC1 SC36.
    //
    // Full information on element definitions and term relationships
    // can be found in the Dublin Core Metadata Registry.
    //
    // Encoding examples
    //
    // <meta name="DC.Format" content="video/mpeg; 10 minutes" />
    // <meta name="DC.Language" content="en" />
    // <meta name="DC.Publisher" content="publisher-name" />
    // <meta name="DC.Title" content="HYP" />
    //

    //
    // https://ru.wikipedia.org/wiki/%D0%94%D1%83%D0%B1%D0%BB%D0%B8%D0%BD%D1%81%D0%BA%D0%BE%D0%B5_%D1%8F%D0%B4%D1%80%D0%BE
    //
    // Дублинское ядро (англ. Dublin Core) - словарь (семантическая сеть)
    // основных понятий английского языка, предназначенный для унификации
    // метаданных для описания широчайшего диапазона ресурсов.
    // С 2005 года словарь представлен и в формате RDF и является
    // популярной основой для описания ресурсов в Семантической паутине.
    //
    // Словарь разделён на два уровня:
    //
    // * простой (неквалифицированный, simple), состоящий из 15 элементов;
    // * компетентный (квалифицированный, qualified), состоящий
    // из 18 элементов и группы т. н. тонкостей (или квалификаторов),
    // которые уточняют семантику элементов для повышения полезности
    // поиска ресурсов.
    // Семантика Дублинского ядра была создана международной
    // междисциплинарной группой профессионалов библиотечного дела,
    // компьютерных наук, кодирования текстов, музейного дела
    // и других смежных групп.
    //
    // В России с 1 июля 2011 года действует ГОСТ Р 7.0.10-2010
    // (ИСО 15836:2003) "Национальный стандарт Российской Федерации.
    // Система стандартов по информации, библиотечному и издательскому делу.
    // Набор элементов метаданных "Дублинское ядро"".
    //
    // Простой набор элементов метаданных Дублинского ядра (Dublin Core
    // Metadata Element Set; DCMES) состоит из 15 элементов метаданных:
    //
    // * Title — название;
    // * Creator — создатель;
    // * Subject — тема;
    // * Description — описание;
    // * Publisher — издатель;
    // * Contributor — внёсший вклад;
    // * Date — дата;
    // * Type — тип;
    // * Format — формат документа;
    // * Identifier — идентификатор;
    // * Source — источник;
    // * Language — язык;
    // * Relation — отношения;
    // * Coverage — покрытие;
    // * Rights — авторские права.
    //
    // Квалифицированный (компетентный) набор элементов метаданных
    // Дублинского ядра, помимо 15 вышеперечисленных, может включать:
    //
    // * Audience — аудитория (зрители);
    // * Provenance — происхождение;
    // * RightsHolder — правообладатель.
    //
    // Каждый элемент опционален и может повторяться. Инициатива метаданных
    // Дублинского ядра (Dublin Core Metadata Initiative; DCMI) описала
    // стандартные пути определения элементов и поощряет использование
    // схем кодирования и словарей. Не существует заранее заданного
    // порядка перечисления этих элементов. DCMI также поддерживает
    // небольшой общий словарь, который рекомендуется использовать
    // с элементом Type (Тип) и который состоит из 12 слов.
    //
    // Полная информация по определениям элементов и отношениям между
    // ними описана в Реестре метаданных Дублинского ядра (Dublin Core
    // Metadata Registry).
    //

    /// <summary>
    /// DC record.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DublinRecord
    {
        #region Properties

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
        public FieldCollection Fields { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DublinRecord()
        {
            Fields = new FieldCollection();
        }

        #endregion
    }
}
