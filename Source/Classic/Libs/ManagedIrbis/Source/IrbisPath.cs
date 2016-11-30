// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisPath.cs -- path codes for IRBIS64
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Задает путь к файлам Ирбис
    /// </summary>
    public enum IrbisPath
    {
        /// <summary>
        /// Общесистемный путь
        /// </summary>
        System = 0,

        /// <summary>
        /// путь размещения сведений о базах данных сервера ИРБИС64
        /// </summary>
        Data = 1,

        /// <summary>
        /// путь на мастер-файл базы данных
        /// </summary>
        MasterFile = 2,

        /// <summary>
        /// путь на словарь базы данных
        /// </summary>
        InvertedFile = 3,
        
        /// <summary>
        /// путь на параметрию базы данных
        /// </summary>
        ParameterFile = 10,

        /// <summary>
        /// Полный текст
        /// </summary>
        FullText = 11,
        
        /// <summary>
        /// Внутренний ресурс
        /// </summary>
        InternalResource = 12
    }
}
