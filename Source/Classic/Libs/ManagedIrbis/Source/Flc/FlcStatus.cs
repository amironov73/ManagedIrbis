// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FlcStatus.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// первый символ результата форматирования определяет
    /// результат ФЛК
    /// </summary>
    public enum FlcStatus
    {
        /// <summary>
        /// означает положительный результат контроля.
        /// </summary>
        OK = 0,

        /// <summary>
        /// означает отрицательный результат и обнаруженные ошибки
        /// считаются непреодолимыми, т.е. подлежат
        /// обязательному устранению.
        /// </summary>
        Error = 1,

        /// <summary>
        /// означает отрицательный результат, но при этом ошибки
        /// считаются преодолимыми, т.е. их можно не исправлять.
        /// </summary>
        Warning = 2
    }
}
