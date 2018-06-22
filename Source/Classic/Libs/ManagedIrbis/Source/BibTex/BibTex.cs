// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BibTex.cs --
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

#endregion

namespace ManagedIrbis.BibTex
{
    //
    // https://github.com/MaikelH/BibtexLibrary
    //

    //
    // BibTeX — программное обеспечение для создания
    // форматированных списков библиографии. BibTeX используется
    // совместно с LaTeX'ом и входит во все известные
    // дистрибутивы TeX и LaTeX.
    //
    // BibTeX был создан Ореном Паташником и Лесли Лэмпортом
    // в 1985 году. BibTeX позволяет легко работать со списками
    // источников, отделяя библиографическую информацию от
    // её представления. Принцип отделения содержимого
    // от его представления использован как в самом LaTeX’е,
    // так и в XHTML, CSS и др.
    //
    // При подготовке документа в LaTeX система BibTeX предоставляет
    // по сравнению со стандартным LaTeX-окружением thebibliography
    // следующие преимущества:
    //
    // список литературы генерируется автоматически по всем
    // ссылкам \cite, упомянутым в тексте;
    // можно использовать единую библиографическую базу (bib-файл)
    // во всех своих текстах, во всех работах отдела, и т. д.;
    // легко обмениваться библиографическими базами с коллегами;
    // нет необходимости помнить правила оформления библиографии,
    // так как BibTeX делает эту работу автоматически с помощью
    // стилевых bst-файлов.
    //
    // Для вызова BibTeX’а достаточно заменить окружение
    // thebibliography командами
    // \bibliographystyle{stylefile} % bst-файл, задающий стиль оформления библиографии
    // \bibliography{bibfile} % имя bib-файла, содержащего библиографическую базу
    // например,
    // \bibliographystyle{gost780s} % ГОСТ 7.80
    // \bibliography{MachLearn} % MachLearn.bib
    //
    // BibTeX использует bib-файлы специального текстового формата
    // для хранения списков библиографических записей.
    // Каждая запись описывает ровно одну публикацию — статью, книгу,
    // диссертацию, и т. д.
    // Bib-файлы можно использовать для хранения библиографических
    // баз данных. Многие программы, работающие с библиографиями,
    // (такие, как JabRef) и онлайн-сервисы цитирования
    // (ADS, CiteULike) могут экспортировать ссылки в bib-формат.
    // Каждая запись выглядит следующим образом:
    // @ARTICLE{tag,
    // author = {Список авторов},
    // title = {Название статьи},
    // year = {год},
    // journal = {Название журнала}
    // }
    // Здесь ARTICLE — тип записи («статья»),
    // tag — метка-идентификатор записи (которая позволяет
    // ссылаться в тексте с помощью \cite{tag}),
    // дальше список полей со значениями.
    //
    // Типы записей
    // Каждая запись должна быть определённого типа,
    // описывающего тип публикации. Следующие типы являются
    // стандартными и обрабатываются почти всеми стилями BibTeX
    // (названия расположены по алфавиту и содержат списки полей,
    // возможные поля см. ниже):
    //
    // article Статья из журнала.
    // Необходимые поля: author, title, journal, year
    // Дополнительные поля: volume, number, pages, month, note, key
    //
    // book Определённое издание книги.
    // Необходимые поля: author/editor, title, publisher, year
    // Дополнительные поля: volume, series, address, edition, month, note, key, pages
    //
    // booklet Печатная работа, которая не содержит имя издателя или организатора (например, самиздат).
    // Необходимые поля: title
    // Дополнительные поля: author, howpublished, address, month, year, note, key
    //
    // conference Синоним inproceedings, оставлено для совместимости с Scribe.
    // Необходимые поля: author, title, booktitle, year
    // Дополнительные поля: editor, pages, organization, publisher, address, month, note, key
    //
    // inbook Часть книги, возможно без названия. Может быть главой (частью, параграфом), либо диапазоном страниц.
    // Необходимые поля: author/editor, title, chapter/pages, publisher, year
    // Дополнительные поля: volume, series, address, edition, month, note, key
    //
    // incollection Часть книги, имеющая собственное название (например, статья в сборнике).
    // Необходимые поля: author, title, booktitle, year
    // Дополнительные поля: editor, pages, organization, publisher, address, month, note, key
    //
    // inproceedings Тезис (труд) конференции.
    // Необходимые поля: author, title, booktitle, year
    // Дополнительные поля: editor, series, pages, organization, publisher, address, month, note, key
    //
    // manual Техническая документация.
    // Необходимые поля: title
    // Дополнительные поля: author, organization, address, edition, month, year, note, key
    //
    // mastersthesis Магистерская диссертация.
    // Необходимые поля: author, title, school, year
    // Дополнительные поля: address, month, note, key
    //
    // misc Использовать, если другие типы не подходят.
    // Необходимые поля: none
    // Дополнительные поля: author, title, howpublished, month, year, note, key
    //
    // phdthesis Кандидатская диссертация.
    // Необходимые поля: author, title, school, year
    // Дополнительные поля: address, month, note, key
    //
    // proceedings Сборник трудов (тезисов) конференции.
    // Необходимые поля: title, year
    // Дополнительные поля: editor, publisher, organization, address, month, note, key
    //
    // techreport Отчёт, опубликованный организацией, обычно пронумерованный внутри серии.
    // Необходимые поля: author, title, institution, year
    // Дополнительные поля: type, number, address, month, note, key
    //
    // unpublished Документ, имеющий автора и название, но формально не опубликованный (рукопись).
    // Необходимые поля: author, title, note
    // Дополнительные поля: month, year, key
    //
    // Поля записей
    // Каждая запись содержит некоторый список стандартных полей
    // (можно вводить любые другие поля, которые просто игнорируются
    // стандартными программами):
    //
    // address: Адрес издателя (обычно просто город, но может быть полным адресом для малоизвестных издателей)
    // annote (в JabRef — abstract): Аннотация для библиографической записи.
    // author: Имена авторов (если больше одного, то разделяются and)
    // booktitle: Наименование книги, содержащей данную работу.
    // chapter: Номер главы
    // crossref: Ключ кросс-ссылки (позволяет использовать другую библио-запись в качестве названия, например, сборника трудов)
    // edition: Издание (полная строка, например, «1-е, стереотипное»)
    // editor: Имена редакторов (оформление аналогично авторам)
    // eprint: A specification of an electronic publication, often a preprint or a technical report
    // howpublished: Способ публикации, если нестандартный
    // institution: Институт, вовлечённый в публикацию, необязательно издатель
    // journal: Название журнала, содержащего статью
    // key: Скрытое ключевое поле, задающее порядок сортировки (если «author» и «editor» не заданы).
    // month: Месяц публикации (может содержать дату). Если не опубликовано — создания.
    // note: Любые заметки
    // number: Номер журнала
    // organization: Организатор конференции
    // pages: Номера страниц, разделённые запятыми или двойным дефисом. Для книги — общее количество страниц.
    // publisher: Издатель
    // school: Институт, в котором защищалась диссертация.
    // series: Серия, в которой вышла книга.
    // title: Название работы
    // type: Тип отчёта, например «Заметки исследователя»
    // url: WWW-адрес
    // volume: Том журнала или книги.
    // year: Год публикации (если не опубликовано — создания)
    //
    // Дополнительно, каждая запись содержит ключевое поле, которое
    // служит для цитирования или кросс-ссылок на эту запись.
    // Это поле должно быть уникальным (в рамках использующей работы)
    // и непустым. Это поле не имеет названия, не является частью
    // других полей и идёт первым по-порядку.
    //
    // Пример
    // .bib-файл может содержать следующую запись,
    // которая описывают математическую книгу:
    // @Book{Korn,
    // author    = {Корн, Г. А. and Корн, Т. М.},
    // title     = {Справочник по математике для научных работников и инженеров},
    // publisher = {«Наука»},
    // year      = 1974,
    // address   = Москва,
    // language = russian
    // }
    //
    // Формат списка авторов
    //
    // Префиксы фамилий, такие как von, van и der обрабатываются
    // автоматически, если они начинаются со строчной буквы,
    // чтобы отличать их от фамилий. Фамилии из нескольких слов
    // отделяются от имён и отчеств (или средних имён) тем,
    // что они идут сначала, а потом, через запятую,
    // пишутся имена и отчества. Именные суффиксы, как Ср. или Мл.
    // или III обычно обрабатываются с помощью второй
    // запятой-разделителя, как в примере:
    // @Book{hicks2001,
    // author    = "von Hicks, III, Michael",
    // title     = "Design of a Carbon Fiber Composite Grid Structure for the GLAST
    // Spacecraft Using a Novel Manufacturing Technique",
    // publisher = "Stanford Press",
    // year      =  2001,
    // address   = "Palo Alto",
    // edition   = "1st,",
    // isbn      = "0-69-697269-4"
    // }
    //
    // Вместо использования запятой, чтобы отделить именной суффикс
    // от фамилии, можно выделить всё имя фигурными скобками: {Hicks III}.
    //
    // Авторы должны отделяться словом and, а не запятыми или «и»:
    // @Book{Torre2008,
    // author    = "Joe Torre and Tom Verducci",
    // publisher = "Doubleday",
    // title     = "The Yankee Years",
    // year      =  2008,
    // isbn      = "0385527403"
    // }
    //
    // Перекрёстные ссылки
    // BibTeX позволяет ссылаться на другие публикации с помощью
    // поля crossref. В следующем примере тезис ссылается
    // на сборник тезисов.
    // @INPROCEEDINGS {author:06,
    // title    = {Название доклада},
    // author   = {Первый Автор and Второй Автор},
    // crossref = {conference:06},
    // pages    = {330—331},
    // }
    // @PROCEEDINGS {conference:06,
    // editor    = {Первый Редактор and Второй Редактор},
    // title     = {Proceedings of the Xth Conference on XYZ},
    // booktitle = {Proceedings of the Xth Conference on XYZ},
    // year      = {2006},
    // month     = {October},
    // }
    // При этом следует добавлять booktitle к записи сборника,
    // чтобы избежать предупреждения BibTeX’а «empty booktitle».
    // Вывод LaTeX этого примера может выглядеть примерно так:
    //
    // Автор, Первый and Автор, Второй (October 2006),
    // Название доклада, in: Proceedings of the Xth Conference on XYZ, pp 330—331.
    //
    // См. https://ru.wikipedia.org/wiki/BibTeX
    // https://en.wikipedia.org/wiki/BibTeX
    // http://www.bibtex.org/
    // https://ru.wikibooks.org/wiki/LaTeX/%D0%A3%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5_%D0%B1%D0%B8%D0%B1%D0%BB%D0%B8%D0%BE%D0%B3%D1%80%D0%B0%D1%84%D0%B8%D0%B5%D0%B9
    //

    //
    // https://github.com/JabRef/jabref
    //

    class BibTex
    {
    }
}
