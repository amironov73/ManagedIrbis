// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisData.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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

        #endregion
    }
}
