// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InfoBlockElement.cs --
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
    /// Элемент инфоблока.
    /// </summary>
    [PublicAPI]
    [TableName("b_iblock_element")]
    public class InfoBlockElement
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
        /// Идентификатор инфоблока.
        /// </summary>
        [MapField("iblock_id")]
        public int BlockId { get; set; }

        /// <summary>
        /// Идентификатор секции инфоблока.
        /// </summary>
        [MapField("iblock_section_id")]
        public int SectionId { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        [MapField("active")]
        public char Active { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [NotNullField]
        [MapField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор картинки для предварительного просмотра.
        /// </summary>
        [MapField("preview_picture")]
        public int PreviewPicture { get; set; }

        /// <summary>
        /// Текст для предварительного просмотра.
        /// </summary>
        [MapField("preview_text")]
        public string PreviewText { get; set; }

        /// <summary>
        /// Тип текста для предварительного просмотра: text, html.
        /// </summary>
        [MapField("preview_text_type")]
        public string PreviewTextType { get; set; }

        /// <summary>
        /// Идентификатор картинки для детального просмотра.
        /// </summary>
        [MapField("detail_picture")]
        public int DetailPicture { get; set; }

        /// <summary>
        /// Текст для детального просмотра.
        /// </summary>
        [MapField("detail_text")]
        public string DetailText { get; set; }

        /// <summary>
        /// Тип текста для детального просмотра: text, html.
        /// </summary>
        [MapField("detail_text_type")]
        public string DetailTextType { get; set; }

        #endregion
    }
}
