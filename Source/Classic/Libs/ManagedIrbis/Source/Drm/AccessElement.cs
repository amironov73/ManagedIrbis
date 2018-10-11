// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AccessElement.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Элемент доступа.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AccessElement
    {
        #region Constants

        /// <summary>
        /// Идентификатор читателя.
        /// </summary>
        public const string Identifier = "01";

        /// <summary>
        /// Категория читателя.
        /// </summary>
        public const string Cathegory = "02";

        /// <summary>
        /// IP-адрес клиента.
        /// </summary>
        public const string Address = "03";

        /// <summary>
        /// Доменное имя клиента.
        /// </summary>
        public const string DomainQualifiedName = "04";

        /// <summary>
        /// Факультет.
        /// </summary>
        public const string Department = "05";

        /// <summary>
        /// Семестр.
        /// </summary>
        public const string Semester = "06";

        /// <summary>
        /// Специальность.
        /// </summary>
        public const string Speciality = "07";

        #endregion
    }
}