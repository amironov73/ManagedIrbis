// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InfoBlock.cs --
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
    /// Инфоблок.
    /// </summary>
    [PublicAPI]
    [TableName("b_iblock")]
    public class InfoBlock
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
        /// Идентификатор типа блока.
        /// </summary>
        [NotNullField]
        [MapField("iblock_type_id")]
        public string Type { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        [MapField("code")]
        public string Code { get; set; }

        /// <summary>
        /// Имя блока.
        /// </summary>
        [NotNullField]
        [MapField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        [MapField("active")]
        public char Active { get; set; }

        /// <summary>
        /// Для сортировки.
        /// </summary>
        [MapField("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// URL страницы со списком (шаблон).
        /// </summary>
        [MapField("list_page_url")]
        public string ListPageUrl { get; set; }

        /// <summary>
        /// URL страницы с подробным описанием (шаблон).
        /// </summary>
        [MapField("detail_page_url")]
        public string DetailPageUrl { get; set; }

        /// <summary>
        /// URL страницы с секцией (шаблон).
        /// </summary>
        [MapField("section_page_url")]
        public string SectionPageUrl { get; set; }

        /// <summary>
        /// Идентификатор картинки.
        /// </summary>
        [MapField("picture")]
        public int Picture { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [MapField("description")]
        public string Description { get; set; }

        /// <summary>
        /// Тип описания: text, html.
        /// </summary>
        [MapField("description_type")]
        public string DescriptionType { get; set; }

        #endregion
    }
}
