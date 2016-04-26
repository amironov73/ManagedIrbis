/* SubFieldAttribute.cs -- отображение подполя на свойство
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// Задаёт отображение подполя на свойство.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public sealed class SubFieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Код.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Повторение.
        /// </summary>
        public int Occurrence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public SubFieldAttribute
            (
                char code
            )
        {
            Code = code;
            Occurrence = -1;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SubFieldAttribute
            (
                char code,
                int occurrence
            )
        {
            Code = code;
            Occurrence = occurrence;
        }

        #endregion
    }
}
