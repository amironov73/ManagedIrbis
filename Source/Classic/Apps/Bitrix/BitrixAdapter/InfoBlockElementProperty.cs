// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InfoBlockElementProperty.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using NotNullField=BLToolkit.Mapping.NotNullAttribute;

#endregion

// ReSharper disable StringLiteralTypo

namespace BitrixAdapter
{
    /// <summary>
    /// Значение свойства элемента инфоблока.
    /// </summary>
    [PublicAPI]
    [TableName("b_iblock_element_property")]
    public class InfoBlockElementProperty
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Identity]
        [PrimaryKey]
        [MapField("id")]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор свойства.
        /// </summary>
        [MapField("iblock_property_id")]
        public int PropertyId { get; set; }

        /// <summary>
        /// Идентификатор элемента.
        /// </summary>
        [MapField("iblock_element_id")]
        public int ElementId { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        [MapField("value")]
        public string Value { get; set; }

        /// <summary>
        /// Тип значения.
        /// </summary>
        [NotNullField]
        [MapField("value_type")]
        public string ValueType { get; set; }

        /// <summary>
        /// Значение-перечисление.
        /// </summary>
        [MapField("value_enum")]
        public int ValueEnum { get; set; }

        /// <summary>
        /// Значение с фиксированной точкой.
        /// </summary>
        [MapField("value_num")]
        public decimal ValueNum { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [MapField("description")]
        public string Description { get; set; }

        #endregion
    }
}
