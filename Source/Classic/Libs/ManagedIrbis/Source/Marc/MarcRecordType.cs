/* MarcRecordType.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Тип записи.
    /// </summary>
    [PublicAPI]
    public enum MarcRecordType
    {
        /// <summary>
        /// Текстовый материал.
        /// </summary>
        Text = (int) 'a',

        /// <summary>
        /// Архивный материал, рукописи.
        /// </summary>
        ArchiveMaterial = (int)'b',

        /// <summary>
        /// Печатные ноты.
        /// </summary>
        PrintedMusic = (int)'c',

        /// <summary>
        /// Рукописные ноты.
        /// </summary>
        HandwrittenMusic = (int)'d',

        /// <summary>
        /// Печатные карты.
        /// </summary>
        PrintedMap = (int)'e',

        /// <summary>
        /// Рукописные карты.
        /// </summary>
        HandwrittenMap = (int)'f',

        /// <summary>
        /// Фильмокопии, видеофильмы и проч.
        /// </summary>
        Film = (int)'g',

        /// <summary>
        /// Немузыкальные записи.
        /// </summary>
        NonMusicRecord = (int)'i',

        /// <summary>
        /// Музыкальные записи.
        /// </summary>
        MusicRecord = (int)'j',

        /// <summary>
        /// Картины, фотографии и т.д. (двумерная графика).
        /// </summary>
        Picture = (int)'k',

        /// <summary>
        /// Компьютерные файлы.
        /// </summary>
        ComputerFiles = (int)'m',

        /// <summary>
        /// Смешанные материалы.
        /// </summary>
        MixedMaterial = (int)'o',

        /// <summary>
        /// Скульптуры и другие трехмерные объекты.
        /// </summary>
        Sculpture = (int)'r'
    }
}
