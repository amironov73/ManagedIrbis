/* FieldAttribute.cs -- отображение поля на свойство.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// Задаёт отображение поля на свойство.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,
        AllowMultiple = false, Inherited = true)]
    public class FieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Тег.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Повторение.
        /// </summary>
        public int Occurrence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldAttribute 
            (
                string tag 
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldAttribute 
            ( 
                string tag, 
                int occurrence 
            )
        {
            Tag = tag;
            Occurrence = occurrence;
        }

        #endregion
    }
}
