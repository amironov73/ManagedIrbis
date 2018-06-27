// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FictionBook.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Logging;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

namespace ManagedIrbis.Epub
{
    //
    // https://ru.wikipedia.org/wiki/Electronic_Publication
    //
    // Electronic Publication (ePub, произн. «ипаб») - открытый формат
    // электронных версий книг с расширением .epub, разработанный
    // Международным форумом по цифровым публикациям в 2007 году.
    // Формат позволяет издателям производить и распространять цифровую
    // публикацию в одном файле, обеспечивая совместимость между
    // программным и аппаратным обеспечением, необходимым для воспроизведения
    // цифровых книг и других публикаций с плавающей вёрсткой.
    //
    // Первоначальный вариант — Open eBook Publication Structure или
    // «OEB» был задуман в 1999 году, релиз ePub — 2007.
    //
    // Книга в формате epub представляет собой ZIP-архив, в котором
    // содержится следующее:
    //
    // папка META-INF.
    // файл container.xml:
    //
    // <container
    //   xmlns="urn:oasis:names:tc:opendocument:xmlns:container"
    //   version="1.0">
    //   <rootfiles>
    //     <rootfile
    //       full-path="OEBPS/content.opf"
    //       media-type="application/oebps-package+xml"/>
    //   </rootfiles>
    // </container>.
    //
    // папка OEBPS.
    // папка Text — текст публикации в виде XHTML- или HTML-страниц или файлов PDF.
    // папка Styles — CSS — стили к тексту.
    // папки Images, Video, Audio — медиа-контент публикации: изображения, видео, аудио.
    // папка Fonts — предпочтительные шрифты для публикации.
    // файл content.opf — содержание книги, метаданные.
    // файл toc.ncx — оглавление книги.
    // файл mimetype — Mime-тип файла: application/epub+zip
    //
    // Серьёзная фрагментация
    //
    // Стандарт ePub ссылается на HTML 5, формат крайне сложный.
    // Работают как минимум две платформы, поддерживающие HTML 5
    // и не полностью совместимые друг с другом (iBooks и Android),
    // и немалое количество старых устройств, поддерживающих небольшое
    // подмножество HTML и со своими ограничениями и ошибками. Наиболее
    // известное из ограничений -- книгу приходится дробить на мелкие
    // HTML-файлы, многие из устройств с крупными файлами работать
    // не способны.
    //
    // Запоздалое появление тегов логического форматирования, важных для книг
    //
    // Например, сноски появились в ePub 3 (предварительные версии 2010,
    // окончательная 2011).
    //
    // Трудности автоматической конвертации в другие форматы
    //
    // Конвертеры практически неспособны сохранить заданное фиксированной
    // версткой расположение объектов и наличие мультимедийных компонентов.
    //

    //
    // https://en.wikipedia.org/wiki/EPUB
    //
    // EPUB is an e-book file format with the extension .epub that can
    // be downloaded and read on devices like smartphones, tablets,
    // computers, or e-readers. It is a technical standard published
    // by the International Digital Publishing Forum (IDPF). The term
    // is short for electronic publication and is sometimes styled ePub.
    // EPUB became an official standard of the IDPF in September 2007,
    // superseding the older Open eBook standard. The Book Industry Study
    // Group endorses EPUB 3 as the format of choice for packaging content
    // and has stated that the global book publishing industry should
    // rally around a single standard. EPUB is the most widely supported
    // vendor-independent XML-based (as opposed to PDF) e-book format;
    // that is, it is supported by the largest number of hardware readers.
    //
    // A successor to the Open eBook Publication Structure, EPUB 2.0 was
    // approved in October 2007, with a maintenance update (2.0.1) approved
    // in September 2010.
    //
    // The EPUB 3.0 specification became effective in October 2011, superseded
    // by a minor maintenance update (3.0.1) in June 2014. New major features
    // include support for precise layout or specialized formatting
    // (Fixed Layout Documents), such as for comic books,[7] and MathML
    // support. The current version of EPUB is 3.1, effective January 5, 2017.
    // The (text of) format specification underwent reorganization
    // and clean-up; format supports remotely-hosted resources and new
    // font formats (WOFF 2.0 and SFNT)[10] and uses more pure HTML and CSS.
    //
    // In May 2016 IDPF Members approved World Wide Web Consortium (W3C)
    // merger, "to fully align the publishing industry and core Web technology".
    //
    // The format and many readers support the following:
    //
    // Reflowable document: optimize text for a particular display
    // Fixed-layout content: pre-paginated content can be useful for
    // certain kinds of highly designed content, such as illustrated
    // books intended only for larger screens, such as tablets.
    // Like an HTML web site, the format supports inline raster and
    // vector images, metadata, and CSS styling.
    // Page bookmarking
    // Passage highlighting and notes
    // A library that stores books and can be searched
    // Re-sizable fonts, and changeable text and background colors
    // Support for a subset of MathML
    // Digital rights management—can contain digital rights management
    // (DRM) as an optional layer
    // The EPUB specification does not enforce or suggest a particular
    // DRM scheme. This could affect the level of support for various
    // DRM systems on devices and the portability of purchased e-books.
    // Consequently, such DRM incompatibility may segment the EPUB format
    // along the lines of DRM systems, undermining the advantages
    // of a single standard format and confusing the consumer.
    //

    class Epub
    {
    }
}
