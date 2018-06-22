// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Doap.cs --
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

namespace ManagedIrbis.Rdf
{
    //
    // https://ru.wikipedia.org/wiki/DOAP
    //
    // DOAP (англ. Description of a Project - описание проекта) - это
    // RDF схема и XML словарь свойств, а также набор инструментов
    // для описания проектов разработки программного обеспечения,
    // в частности свободного ПО. Эта схема предназначена для обмена
    // данными между каталогами программного обеспечения и для
    // децентрализированного выражения участия в проектах.
    //
    // Инструмент был создан и разработан Эддом Дамбилломruen
    // для передачи семантической мета-информации, связанной
    // с проектами с открытым исходным кодом.
    //
    // В настоящее время генераторы, валидаторы, просмоторщики
    // и конвертаторы позволяют многим проектам включаться
    // в семантическую паутину. На Freshmeat уже 43 тысячи проектов
    // опубликованы с использованием DOAP. В настоящее время
    // он используется в Mozilla Foundation на странице проекта
    // и в ряде других репозиториев программного обеспечения,
    // в частности в Python Package Index.
    //
    // Основные свойства: doap:homepage, doap:developer,
    // doap:programming-language, doap:os
    //
    // Ниже приведен пример в RDF/XML:
    //
    // <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    // xmlns:doap="http://usefulinc.com/ns/doap#">
    //  <doap:Project>
    //   <doap:name xml:lang="en">Example project</doap:name>
    //   <doap:name xml:lang="ru">Пример проекта</doap:name>
    //   <doap:homepage rdf:resource="http://example.com" />
    //   <doap:programming-language>javascript</doap:programming-language>
    //   <doap:license rdf:resource="http://example.com/doap/licenses/gpl"/>
    //  </doap:Project>
    // </rdf:RDF>
    //
    // Остальные свойства Implements specification, anonymous root,
    // platform, browse, mailing list, category, description, helper,
    // tester, short description, audience, screenshots, translator,
    // module, documenter, wiki, repository, name, repository location,
    // language, service endpoint, created, download mirror, vendor,
    // old homepage, revision, download page, license, bug database,
    // maintainer, blog, file-release и release.
    //

    class Doap
    {
    }
}
