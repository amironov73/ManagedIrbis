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
        : byte
    {
        /// <summary>
        /// Текстовый материал.
        /// </summary>
        Text = (byte) 'a',

        /// <summary>
        /// Архивный материал, рукописи.
        /// </summary>
        ArchiveMaterial = (byte)'b',

        /// <summary>
        /// Печатные ноты.
        /// </summary>
        PrintedMusic = (byte)'c',

        /// <summary>
        /// Рукописные ноты.
        /// </summary>
        HandwrittenMusic = (byte)'d',

        /// <summary>
        /// Печатные карты.
        /// </summary>
        PrintedMap = (byte)'e',

        /// <summary>
        /// Рукописные карты.
        /// </summary>
        HandwrittenMap = (byte)'f',

        /// <summary>
        /// Фильмокопии, видеофильмы и проч.
        /// </summary>
        Film = (byte)'g',

        /// <summary>
        /// Немузыкальные записи.
        /// </summary>
        NonMusicRecord = (byte)'i',

        /// <summary>
        /// Музыкальные записи.
        /// </summary>
        MusicRecord = (byte)'j',

        /// <summary>
        /// Картины, фотографии и т.д. (двумерная графика).
        /// </summary>
        Picture = (byte)'k',

        /// <summary>
        /// Компьютерные файлы.
        /// </summary>
        ComputerFiles = (byte)'m',

        /// <summary>
        /// Смешанные материалы.
        /// </summary>
        MixedMaterial = (byte)'o',

        /// <summary>
        /// Скульптуры и другие трехмерные объекты.
        /// </summary>
        Sculpture = (byte)'r'
    }
}
