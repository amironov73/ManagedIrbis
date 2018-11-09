// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InfoBlockProperty.cs --
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
    //
    // Интересующие нас свойства:
    //
    // 3 = дата начала
    // 4 = дата окончания
    // 66 = категория
    //

    /// <summary>
    /// Описание свойства инфоблока.
    /// </summary>
    [PublicAPI]
    [TableName("b_iblock_property")]
    public class InfoBlockProperty
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
        /// Имя свойства.
        /// </summary>
        [NotNullField]
        [MapField("name")]
        public string Name { get; set; }

        #endregion
    }
}
