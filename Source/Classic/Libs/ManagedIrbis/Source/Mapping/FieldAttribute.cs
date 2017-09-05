// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldAttribute.cs -- отображение поля на свойство.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Задаёт отображение поля на свойство.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Тег.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Повторение.
        /// </summary>
        public int Occurrence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute
            (
                string tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute
            (
                string tag,
                char code
            )
        {
            Tag = tag;
            Code = code;
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
