// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WorkPhase.cs -- этап работы, подполе 907^c
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Этап работы, подполе 907^c.
    /// Согласно справочнику etr.mnu.
    /// </summary>
    public static class WorkPhase
    {
        /// <summary>
        /// Создание записи, первичная каталогизация.
        /// </summary>
        public const string InitialCataloguing = "ПК";

        /// <summary>
        /// Размещение заказа.
        /// </summary>
        public const string PlaceAnOrder = "РЗ";

        /// <summary>
        /// Исполнение заказа.
        /// </summary>
        public const string OrderExecution = "ИЗ";

        /// <summary>
        /// Каталогизация.
        /// </summary>
        public const string Cataloguing = "КТ";

        /// <summary>
        /// Систематизация.
        /// </summary>
        public const string Systematization = "С";

        /// <summary>
        /// Обработка записи завершена.
        /// </summary>
        public const string Completed = "obrzv";

        /// <summary>
        /// Проферка фонда.
        /// </summary>
        public const string Inventarization = "ПРФ";

        /// <summary>
        /// Регистрация периодики.
        /// </summary>
        public const string RegistrationOfPeriodicals = "РЖ";

        /// <summary>
        /// Докомплектование.
        /// </summary>
        public const string Retrofitting = "ДК";

        /// <summary>
        /// Корректура.
        /// </summary>
        public const string Correction = "КР";

        /// <summary>
        /// Выбытие.
        /// </summary>
        public const string WriteOff = "ВБ";

        /// <summary>
        /// Передача в другое подразделение.
        /// </summary>
        public const string Transfer = "ПФ";

        /// <summary>
        /// Запись получена по обмену в формате UNIMARC.
        /// </summary>
        public const string ImportUnimarc = "ZU";

        /// <summary>
        /// Запись получена по обмену в формате USMARC.
        /// </summary>
        public const string ImportUsmarc = "ZS";

        /// <summary>
        /// Обработка не завершена (издание читателю не выдается).
        /// </summary>
        public const string NotCompleted = "ОБРНЗ";

    } // class WorkPhase

} // namespace ManagedIrbis.Fields
