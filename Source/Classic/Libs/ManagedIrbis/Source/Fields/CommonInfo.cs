// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommonInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Mapping;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Общая информация о многотомнике. Поля 461 и 46.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CommonInfo
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string MainTag = "461";

        /// <summary>
        /// 
        /// </summary>
        public const string AdditionalTag = "46";

        #endregion

        #region Properties

        /// <summary>
        /// 461^c.
        /// </summary>
        [CanBeNull]
        [Field("461", 'c')]
        [XmlElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 461^u.
        /// </summary>
        [CanBeNull]
        [Field("461", 'u')]
        [XmlElement("specific")]
        [JsonProperty("specific")]
        public string Specific { get; set; }

        /// <summary>
        /// 461^2.
        /// </summary>
        [CanBeNull]
        [Field("461", '2')]
        [XmlElement("general")]
        [JsonProperty("general")]
        public string General { get; set; }

        /// <summary>
        /// 461^e.
        /// </summary>
        [CanBeNull]
        [Field("461", 'e')]
        [XmlElement("subtitle")]
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// 461^f.
        /// </summary>
        [CanBeNull]
        [Field("461", 'f')]
        [XmlElement("responsibility")]
        [JsonProperty("responsibility")]
        public string Responsibility { get; set; }

        /// <summary>
        /// 461^g.
        /// </summary>
        [CanBeNull]
        [Field("461", 'g')]
        [XmlElement("publisher")]
        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        /// <summary>
        /// 461^d.
        /// </summary>
        [CanBeNull]
        [Field("461", 'd')]
        [XmlElement("city")]
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 461^h.
        /// </summary>
        [CanBeNull]
        [Field("461", 'h')]
        [XmlElement("beginningYear")]
        [JsonProperty("beginningYear")]
        public string BeginningYear { get; set; }

        /// <summary>
        /// 461^z.
        /// </summary>
        [CanBeNull]
        [Field("461", 'z')]
        [XmlElement("endingYear")]
        [JsonProperty("endingYear")]
        public string EndingYear { get; set; }

        /// <summary>
        /// 461^i.
        /// </summary>
        [CanBeNull]
        [Field("461", 'i')]
        [XmlElement("isbn")]
        [JsonProperty("isbn")]
        public string Isbn { get; set; }

        /// <summary>
        /// 461^j.
        /// </summary>
        [CanBeNull]
        [Field("461", 'j')]
        [XmlElement("issn")]
        [JsonProperty("issn")]
        public string Issn { get; set; }

        /// <summary>
        /// 461^p.
        /// </summary>
        [CanBeNull]
        [Field("461", 'p')]
        [XmlElement("reprint")]
        [JsonProperty("reprint")]
        public string Reprint { get; set; }

        /// <summary>
        /// 461^a.
        /// </summary>
        [CanBeNull]
        [Field("461", 'a')]
        [XmlElement("translation")]
        [JsonProperty("translation")]
        public string Translation { get; set; }

        /// <summary>
        /// 461^x.
        /// </summary>
        [CanBeNull]
        [Field("461", 'x')]
        [XmlElement("firstAuthor")]
        [JsonProperty("firstAuthor")]
        public string FirstAuthor { get; set; }

        /// <summary>
        /// 461^b.
        /// </summary>
        [CanBeNull]
        [Field("461", 'b')]
        [XmlElement("collective")]
        [JsonProperty("collective")]
        public string Collective { get; set; }

        /// <summary>
        /// 46^r.
        /// </summary>
        [CanBeNull]
        [Field("46", 'r')]
        [XmlElement("titleVariant")]
        [JsonProperty("titleVariant")]
        public string TitleVariant { get; set; }

        /// <summary>
        /// 46^h.
        /// </summary>
        [CanBeNull]
        [Field("46", 'h')]
        [XmlElement("secondLevelNumber")]
        [JsonProperty("secondLevelNumber")]
        public string SecondLevelNumber { get; set; }

        /// <summary>
        /// 46^i.
        /// </summary>
        [CanBeNull]
        [Field("46", 'i')]
        [XmlElement("secondLevelTitle")]
        [JsonProperty("secondLevelTitle")]
        public string SecondLevelTitle { get; set; }

        /// <summary>
        /// 46^k.
        /// </summary>
        [CanBeNull]
        [Field("46", 'k')]
        [XmlElement("thirdLevelNumber")]
        [JsonProperty("thirdLevelNumber")]
        public string ThirdLevelNumber { get; set; }

        /// <summary>
        /// 46^m.
        /// </summary>
        [CanBeNull]
        [Field("46", 'm')]
        [XmlElement("thirdLevelTitle")]
        [JsonProperty("thirdLevelTitle")]
        public string ThirdLevelTitle { get; set; }

        /// <summary>
        /// 46^p.
        /// </summary>
        [CanBeNull]
        [Field("46", 'p')]
        [XmlElement("parallelTitle")]
        [JsonProperty("parallelTitle")]
        public string ParallelTitle { get; set; }

        /// <summary>
        /// 46^a.
        /// </summary>
        [CanBeNull]
        [Field("46", 'a')]
        [XmlElement("seriesTitle")]
        [JsonProperty("seriesTitle")]
        public string SeriesTitle { get; set; }

        /// <summary>
        /// 46^c.
        /// </summary>
        [CanBeNull]
        [Field("46", 'c')]
        [XmlElement("previousTitle")]
        [JsonProperty("previousTitle")]
        public string PreviousTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field461 { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public RecordField Field46 { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
