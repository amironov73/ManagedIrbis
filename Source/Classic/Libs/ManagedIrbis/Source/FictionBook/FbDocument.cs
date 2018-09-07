// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbDocument.cs --
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
using System.Xml.Serialization;

using AM;
using AM.Logging;
using AM.Text;
using AM.Xml;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

namespace ManagedIrbis.FictionBook
{
    //
    // https://ru.wikipedia.org/wiki/FictionBook
    //
    // FictionBook (также FeedBook) - формат представления электронных
    // версий книг в виде XML-документов, где каждый элемент книги
    // описывается своими тегами. Стандарт призван обеспечить
    // совместимость с любыми устройствами и форматами. XML позволяет
    // легко создавать документы, готовые к непосредственному использованию
    // и программной обработке (преобразованию, хранению, управлению)
    // в любой среде. Документы, обычно имеющие расширение .fb2,
    // могут содержать структурную разметку основных элементов текста,
    // некоторое количество информации о книге, а также вложения
    // с двоичными файлами, в которых могут храниться иллюстрации,
    // например обложка.
    //
    // Стандарт был разработан группой разработчиков во главе
    // с Дмитрием Грибовым и Михаилом Мацневым.
    //
    // Каждая электронная книга в формате FictionBook представлена
    // в виде одного файла формата XML. Иллюстрации (PNG и JPEG)
    // встраиваются прямо в XML, будучи представленными в кодировке
    // Base64. FictionBook часто сжимают в ZIP (получается файл .fb2.zip
    // или .fbz), многие программы чтения поддерживают и FB2 со сжатием.
    //
    // FictionBook похож идеологией на первые версии HTML: все теги
    // относятся к логическому форматированию, а не к визуальному.
    // Нет привязки ни к какому аппаратному обеспечению и ни к какому
    // формату бумаги, нигде в FB2 не указана какая бы то ни была
    // единица измерения — пиксель, пункт или кегль. Как будет выглядеть
    // текст, полученный из формата .fb2, зависит либо от настроек
    // программы-просмотрщика этого формата, либо от параметров,
    // заданных при конвертации файла в другой формат. К примеру,
    // тег заголовка в программе просмотра можно вывести крупным шрифтом,
    // другим цветом или как-то иначе. А при конвертации в формат HTML
    // каждому заголовку может быть сопоставлен определённый HTML-тег,
    // например, <H4> или <B>. Поэтому FB2 претендует на роль
    // универсального формата хранения книг, который можно автоматически
    // переводить в HTML, PDF и другие форматы.
    //
    // Многие из возможностей FB2 специфичны для электронных книг.
    // В метаданных хранится название книги, ISBN, информация об авторе
    // и жанре книги. Поддерживаются сноски, оглавление, стихи, цитаты.
    // Для переводных книг предусмотрена информация об исходной книге.
    //
    // Проработанные метаданные дают широкое поле для автоматической
    // обработки книг. К примеру, в электронную библиотеку поступила
    // книга в формате .fb2. Эта книга может быть автоматически помещена
    // в раздел автора книги, а название книги и аннотация могут
    // автоматически отобразиться в колонке новостей библиотеки.
    // Таким образом, намного упрощается процесс помещения в библиотеку
    // новых книг. У документа и авторов есть идентификаторы свободной
    // формы: писателей «Kipling, Rudyard» и «Киплинг, Редьярд» может
    // объединить по совпадению идентификаторов; старая версия книги
    // будет автоматически замещена исправленной. Впрочем, метаданные
    // оказались несколько «СССР-центричными»: были отдельные жанры
    // «русская литература» и «советская литература».
    //
    // Защита от копирования не предусмотрена. Однако, начиная
    // с версии 2.1, добавлены специальные инструкции для библиотеки,
    // позволяющие выдавать тексты за деньги.
    //
    // Формат недостаточно выразителен для учебников, справочников
    // и научных публикаций (о чём говорит даже название — «художественная
    // книга»). В формате нет сложной вёрстки текста, нет поддержки
    // нумерованных и маркированных списков, разрядки текста, средств
    // формирования «замечаний на полях», авторского форматирования стихов
    // и векторной графики. Чтобы отобразить минимальную информацию
    // о книге - название, автора и обложку - программе требуется прочитать
    // и разобрать почти весь XML.
    //
    // Ссылки через XPath, заявленные в стандарте, на поверку реализует
    // мало кто, ограничиваясь обычно формой #id.
    //
    // История
    //
    // В середине 1990-х годов энтузиасты начали оцифровывать советские книги
    // (за этим в те времена никто не следил). Форматы были самые разные.
    // Текстовый файл можно читать любой программой, однако он не особо
    // удобен в чтении (текст обычно форматируется моноширинным шрифтом).
    // Библиотека Максима Мошкова использовала форматированный TXT,
    // что отлично работает в текстовом режиме, но совершенно неудобно
    // в оконных интерфейсах и на мобильных устройствах, да и обработку
    // затрудняет. Microsoft Word и TeX крайне сложны в обработке.
    // PDF приспособлен только для бумажных копий, чтение PDF с экрана
    // затруднено. DocBook рассчитан на технические книги: вся мощь
    // формата избыточна для художественных книг, а стихи не поддерживаются.
    // Немногочисленные карманные устройства, появившиеся в начале 2000-х,
    // имели свои книжные форматы и зачастую некачественное ПО для их чтения.
    // Врéменным решением было использовать подмножество HTML, так как
    // полный HTML крайне сложен в реализации. По такому пути пошёл,
    // например, Open eBook (ныне декларирующий полное соответствие
    // HTML/CSS ePub).
    //
    // В условиях такого «вакуума» Грибов и предложил свой формат, задумывавшийся не как формат для чтения электронных книг, а как формат для их хранения[9] с целью конечной конвертации для пользователя. Однако, наглядность и простота изменения книги «даже руками» и возможность конвертировать при необходимости книгу в другие форматы придало FB2 популярность в Рунете, он стал стандартом де-факто в русских, украинских и белорусских сетевых библиотеках. В последние годы возросла популярность формата в нерусскоязычных странах: Болгарии[10], Латвии[11][12]. Некоторые электронные библиотеки перешли на формат FictionBook полностью, и не принимают книги, подготовленные в других форматах. Однако на страницах этих библиотек можно скачать одну и ту же книгу в виде файлов других распространённых форматов (текстовый файл, RTF, HTML, rb, .doc, PRC[en], ePub, PDF), полученных из .fb2 путём автоматической конвертации.
    //
    // FictionBook 3 должен был быть ZIP-контейнером, в котором хранятся
    // XML и дополнительные файлы (метаданные, рисунки). Жанры предлагалось
    // классифицировать по шести разным «осям» (государственная принадлежность
    // автора, возраст аудитории, описанная историческая эпоха, литературная
    // форма, сюжет и стиль повествования). Разработка fb3 «заглохла» ещё
    // в 2008 году, однако продолжена автором fb2 в 2013.
    //

    //
    // https://en.wikipedia.org/wiki/FictionBook
    //
    // FictionBook is an open XML-based e-book format which originated
    // and gained popularity in Russia. FictionBook files have the .fb2
    // filename extension. Some readers also support ZIP-compressed
    // FictionBook files (.fb2.zip or .fbz)
    //
    // The FictionBook format does not specify the appearance of a document;
    // instead, it describes its structure. For example, there are special
    // tags for epigraphs, verses and quotations. All ebook metadata,
    // such as author name, title, and publisher, is also present in the
    // ebook file. This makes the format convenient for automatic processing,
    // indexing, and ebook collection management, and allows automatic
    // conversion into other formats.
    //
    // Features of FictionBook
    //
    // * Free and open format with multiple hardware and software implementations
    // * DRM-free
    // * Supports reflow by design
    // * Simple semantic markup
    // * Optimized for narrative literature
    // * Embeds metadata, proposes its own scheme for genre description
    // * Supports Unicode
    // * Documents may contain:
    //   * Structured text organized in nested sections (optionally titled)
    //   * Subtitles (which do not appear in the table of contents)
    //   * Epigraphs
    //   * Poetry
    //   * Quotations
    //   * References and footnotes
    //   * Tables (but not all readers support them)
    //   * Raster images (PNG or JPEG)
    // * Inline formatting:
    //   * Strong (usually bold)
    //   * Emphasized (usually slanted or italic)
    //   * Strikethrough
    //   * Superscript
    //   * Subscript
    //   * Source code (usually in monospace font)
    //
    // Differences from other ebook formats
    //
    // In contrast to other eBook formats (e.g. ePub), a FictionBook document
    // consists of a single XML file. Images are converted to Base64 and reside
    // inside the <binary> tag, so the size of the embedded images is increased
    // by approximately 37%. FictionBook files are often distributed inside Zip
    // archives, and most of hardware and software readers can work with
    // compressed FictionBook files (*.fb2.zip) directly. The metadata
    // and the plain text data are always placed in the beginning of the
    // FictionBook file, while more heavyweight binary images are placed
    // in the end. This allows software to start rendering or processing
    // FictionBook before the file is available entirely.
    //
    // FictionBook is the format of choice of some community-driven
    // online electronic libraries. It does not allow for digital rights
    // management of any kind.
    //
    // Software and hardware support
    //
    // The format is supported by e-book readers such as FBReader, AlReader,
    // Haali Reader, STDU Viewer, CoolReader, Fly Reader, Okular, Ectaco
    // jetBooks, Documents for iOS, and some others. Firefox can read
    // FictionBook by installing an extension: FB2 Reader. Many hardware
    // vendors support FictionBook in their firmware: BeBook One, BeBook
    // Mini and BeBook Club in Europe (and other Hanlin V3 and V5 based
    // devices), all PocketBook Readers, COOL-ER devices, Cybook Opus
    // and Cybook Gen3, and ASUS Eee Reader DR900. Devices based on the
    // Hanvon N516 design can read FictionBook if custom OpenInkpot firmware
    // is installed; it is factory default for Azbooka 516. Amazon's Kindle,
    // Barnes & Noble's Nook, and Sony devices do not support FictionBook directly.
    //
    // Conversion to and from FictionBook2 files (.fb2 and .fbz) is possible
    // via the cross-platform ebook management software Calibre. Conversion to,
    // but not from, FictionBook2 format is also available via Pandoc.
    //

    //
    // http://www.fictionbook.org/index.php/FictionBook
    //
    // Формат FictionBook - это xml формат хранения книг, где каждый
    // элемент книги описывается своими тегами.
    //
    // Главная цель сторонников хранения книг в формате FictionBook
    // - четкое хранение структуры книги с возможностью без труда
    // сконвертировать (в том числе и автоматизированно) файл формата
    // FictionBook в другие популярные форматы: txt, doc, rtf, html и пр.
    // Кроме того многие программы чтения книг позволяют читать книги
    // в формате FictionBook без конвертации.
    //
    // Все это служит удобству чтения.
    //
    // Решаемые задачи:
    //
    // * Описание формата.
    // * Разработка технического задания для программного обеспечения:
    //   редакторов, ридеров, конверторов.
    // * Разработка программного обеспечения.
    // * Изготовление качественных по структуре и содержанию книг.
    //
    // Попутно одним махом достигается и еще одна цель: на основе данных,
    // хранящихся внутри файла FictionBook, с необычайной легкостью можно
    // построить хранилище книг любого масштаба как у себя дома, так
    // и публичное (сетевая библиотека). Это хранилище в обработке
    // требует значительно меньше времени и усилий, чем хранение книг
    // в любом другом виде. Более всего формат FictionBook подходит
    // для художественной литературы. Специальную литературу: научную,
    // техническую, - описать в терминах формата пока затруднительно.
    //
    // Основная информация о формате расположена в секциях навигатора
    // Статьи и Документы. В секции Программы описаны различные программы,
    // используемые для чтения и подготовки книг в формате FictionBook,
    // библиотекари и различные утилиты. На Тестовой площадке происходит
    // тестирование программ, сообщения об ошибках.
    //
    // Если вы желаете написать комментарий или статью, но не знаете
    // как - вам поможет раздел Справка. Если вы желаете помочь перевести
    // статью на английский язык прочтите в Справке статью Переводчикам
    // и смело переходите в раздел English.
    //

    /// <summary>
    /// Корневой элемент.
    /// </summary>
    [XmlRoot("FictionBook", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
    public sealed class FbDocument
    {
        #region Properties

        /// <summary>
        /// Стили.
        /// </summary>
        [XmlElement("stylesheet")]
        public string Stylesheet { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [XmlElement("description")]
        public FbDescription Description { get; set; }

        /// <summary>
        /// Тело.
        /// </summary>
        [XmlElement("body")]
        public FbBody[] Body { get; set; }

        /// <summary>
        /// Двоичные данные (рисунки).
        /// </summary>
        [XmlElement("binary")]
        public FbBinary[] Binary { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Загрузка книги из указанного sфайла.
        /// </summary>
        [NotNull]
        public static FbDocument LoadBook
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FbDocument result = XmlUtility.Deserialize<FbDocument>(fileName);

            return result;
        }

        #endregion
    }
}
