// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Hcalendar.cs --
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
    // https://ru.wikipedia.org/wiki/HCalendar
    //
    // hCalendar (сокращённо от HTML iCalendar) - микроформат для
    // представления семантической информации о событиях в формате
    // календаря iCalendar на (X)HTML-страницах.
    //
    // Он позволяет инструментам для парсинга (например, другим сайтам
    // или расширениям Operator и Tails для Firefox) извлекать
    // информацию о событии и отображать её на сайтах, индексировать,
    // искать её или загрузить её в программу календаря или дневника
    // и прочее.
    //
    // Вся информация о событии заключается в блок, которому указывается
    // класс vevent. В него включаются поля, классы которых соответствуют
    // приведённым ниже и являются параметрами, а содержание
    // (или параметр title, если отображаемый текст не соответствует тому,
    // который должен извлекаться при парсинге) является их значением.
    //
    // Обязательные:
    //
    // * dtstart (дата в формате ISO 8601) — дата/время начала
    // * summary — краткое описание
    //
    // Необязательные:
    //
    // * location — местоположение (в свободной форме)
    // * url — ссылка на страницу, связанную с событием
    // * dtend (дата в формате ISO 8601) — дата окончания
    // * duration (длительность в формате ISO 8601) — продолжительность
    // * rdate
    // * rrule
    // * category
    // * description — расширенное описание
    // * uid — уникальный идентификатор
    // * geo (latitude, longitude) — координаты
    //
    // Этот список параметров является основным, но не полным.
    // Полный список должен соответствовать RFC 2445.

    class HCalendar
    {
    }
}
