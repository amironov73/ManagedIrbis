// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisData.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace Irbis2istu2
{
    [TableName("irbisdata")]
    public class IrbisData
    {
        #region Properties

        [Identity]
        [PrimaryKey]
        [MapField("id")]
        public int Id { get; set; }

        [MapField("irbisid")]
        public string Index { get; set; }

        [MapField("bib_disc")]
        public string Description { get; set; }

        [MapField("rubrica")]
        public string Heading { get; set; }

        [MapField("title")]
        public string Title { get; set; }

        [MapField("avtors")]
        public string Author { get; set; }

        [MapField("cnt")]
        public int Count { get; set; }

        [MapField("year_izd")]
        public int Year { get; set; }

        [MapField("http_link")]
        public string Link { get; set; }

        [MapField("izd_type")]
        public string Type { get; set; }

        [MapField("place")]
        public string Place { get; set; }

        #endregion
    }
}
