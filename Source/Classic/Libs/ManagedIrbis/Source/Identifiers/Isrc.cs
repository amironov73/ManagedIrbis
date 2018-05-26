// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Isrc.cs -- Международный стандартный номер аудио/видео записи
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
    // См. https://ru.wikipedia.org/wiki/ISRC
    //
    // Международный стандартный номер аудио/видео записи
    // ((ISRC),
    // определённый ISO 3901 — международный стандартный код
    // для точного определения уникальной аудио- или видеозаписи.
    // Он выделяется IFPI, уполномоченной на эти действия ISO.
    // За этот стандарт отвечает технический комитет 46,
    // подкомитет 9 ISO. Обратите внимание что ISO определяет
    // именно конкретную запись, а не песню в целом. Таким образом,
    // различные записи, редакции и ремиксы одной и той же песни будут
    // иметь различные коды ISRC.
    // Песни определяются аналогичным кодом ISWC.
    //
    // Коды ISRC выделяются национальными агентствами ISRC как
    // для частных, так и для юридических лиц. Обычно это бесплатно,
    // но национальные агентства могут взимать разумную плату
    // для покрытия издержек на эту операцию.
    //
    // Код ISRC всегда состоит из 12 символов и записывается
    // в формате "CC-XXX-YY-NNNNN" (Дефисы не являются частью кода ISRC,
    // но этот код часто пишут таким образом чтобы облегчить его чтение.)
    // Упомянутые выше четыре части означают:
    //
    // "CC" означает код страны согласно ISO 3166-1 alpha-2
    // "XXX" — трёхзначный алфавитно-цифровой регистрационный код,
    // уникальным образом определяющий организацию, которая регистрирует код.
    // Например, в Великобритании это Phonographic Performance Limited (PPL).
    // "YY" — последние две цифры года регистрации (не обязательно
    // соответствуют году, когда произведена запись)
    // "NNNNN" — уникальная последовательность из пяти цифр,
    // определяющая определённую аудиозапись.
    //
    // Например, GBEMI0300013.
    //
    // Например, запись песни "Enquanto Houver Sol",
    // выполненная бразильской группой Titãs получила код ISRC BR-BMG-03-00729:
    //
    // BR для Бразилии
    // BMG для BMG
    // 03 для 2003 года
    // 00729 уникальный идентификатор записи
    //
    // Другой пример: USPR37300012 — запись песни "Love's Theme"
    // группы Love Unlimited Orchestra.
    //
    // US-PR3-73/00012
    // US для США
    // PR3 для организации
    // 73 для 1973 года
    // 00012 уникальный идентификатор записи
    //
    // Красная книга (стандарт аудио CD) определяет кодирование
    // кодов ISRC на компакт-дисках.
    //

    /// <summary>
    /// International Standard Recording Code.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Isrc
    {
        #region Properties

        /// <summary>
        /// Country code.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Registrant code.
        /// </summary>
        public string Registrant { get; set; }

        /// <summary>
        /// Last two digits of the reference year.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Number.
        /// </summary>
        public string Number { get; set; }

        #endregion
    }
}
