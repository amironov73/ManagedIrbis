// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Hcard.cs --
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
    // https://ru.wikipedia.org/wiki/HCard
    //
    // hCard (сокращение для HTML vCard) - микроформат для публикации
    // контактной информации людей, компаний, организаций и мест в (X)HTML,
    // Atom, RSS или произвольном XML. hCard является представлением
    // один к одному параметров и значений формата vCard (RFC 2426).
    //
    // Пример HTML:
    //
    // <div>
    // <div>Вася Пупкин</div>
    // <div>ООО «Рога и Копыта»</div>
    // <div>604-555-1234</div>
    // <a href="http://example.com/">http://example.com/</a>
    // </div>
    //
    // С добавлением микроформатов выглядит так:
    //
    // <div class="vcard">
    // <div class="fn">Вася Пупкин</div>
    // <div class="org">ООО «Рога и Копыта»</div>
    // <div class="tel">604-555-1234</div>
    // <a class="url" href="http://example.com/">http://example.com/</a>
    // </div>
    //
    // Используемые здесь полное имя (fn), организация (org), телефонный номер
    // (tel) и веб-адрес (url) определены с использованием определённых
    // имён классов, а для всего блока задан class="vcard", который показывает,
    // что другие классы принадлежат hCard.
    //

    class Hcard
    {
    }
}
