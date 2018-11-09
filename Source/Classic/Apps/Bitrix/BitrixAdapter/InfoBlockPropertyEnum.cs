// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InfoBlockPropertyEnum.cs --
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
    /// Значение-перечисление свойства инфоблока.
    /// </summary>
    [PublicAPI]
    [TableName("b_iblock_property_enum")]
    public class InfoBlockPropertyEnum
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
        [MapField("property_id")]
        public int PropertyId { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        [NotNullField]
        [MapField("value")]
        public string Value { get; set; }

        /// <summary>
        /// Для сортировки.
        /// </summary>
        [MapField("sort")]
        public int Sort { get; set; }

        #endregion
    }
}
