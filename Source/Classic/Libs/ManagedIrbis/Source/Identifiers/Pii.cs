// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Pii.cs -- Publisher Item Identifier.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // См. https://ru.wikipedia.org/wiki/Publisher_Item_Identifier
    //
    // Publisher Item Identifier (PII) — уникальный идентификатор,
    // применяемый некоторыми научными журналами для идентификации
    // научных работ. Он основан на более ранних идентификаторах
    // ISSN и ISBN, к которым добавлены символ для уточнения типа
    // публикации, номер сущности и контрольная цифра.
    //
    // Системой PII пользуются с 1996 года издатели American Chemical
    // Society, American Institute of Physics, American Physical
    // Society, Elsevier и IEEE.
    //
    // Формат
    //
    // Идентификатор PII представляет собой строку из 17 символов, состоящую из:
    //
    // Символ типа публикации: «S» означает периодическое издание
    // и код ISSN, «B» — книги и код ISBN.
    // ISSN (8 цифр) или ISBN (10 цифр)
    // для периодики добавлено 2 цифры, чтобы выровнять длину кода.
    // Часто используется 2 последние цифры года, в который произошло
    // присвоение номера PII.
    // пятизначный код, присвоенный данной работе издателем.
    // Должен быть уникален в рамках данного журнала или книги
    // контрольная цифра (0-9 или X)
    // При печати код PII может быть дополнен знаками пунктуации для
    // упрощения чтения, например, Sxxxx-xxxx(yy)iiiii-d
    // или Bx-xxx-xxxxx-x/iiiii-d.
    //

    /// <summary>
    /// Publisher Item Identifier.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Pii
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
