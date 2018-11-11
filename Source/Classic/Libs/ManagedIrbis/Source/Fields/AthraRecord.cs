// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthraRecord.cs --
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

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Запись в базе данных ATHRA.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AthraRecord
    {
        #region Properties

        /// <summary>
        /// Заголовок - Основное (принятое) имя лица.
        /// Поле 210.
        /// </summary>
        [CanBeNull]
        [Field(210)]
        [XmlElement("mainTitle")]
        [JsonProperty("mainTitle", NullValueHandling = NullValueHandling.Ignore)]
        public AthraTitle MainTitle { get; set; }

        /// <summary>
        /// Место работы автора.
        /// Поле 910.
        /// </summary>
        [CanBeNull]
        [Field(910)]
        [XmlElement("workPlace")]
        [JsonProperty("workPlaces", NullValueHandling = NullValueHandling.Ignore)]
        public AthraWorkPlace[] WorkPlaces { get; set; }

        /// <summary>
        /// Ссылки типа СМ. (вариантные (другие) принятые
        /// формы имени лица).
        /// Поле 410.
        /// </summary>
        [CanBeNull]
        [Field(410)]
        [XmlElement("see")]
        [JsonProperty("see", NullValueHandling = NullValueHandling.Ignore)]
        public AthraSee[] See { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ. (связанные принятые формы
        /// имени лица).
        /// Поле 510.
        /// </summary>
        [CanBeNull]
        [Field(510)]
        [XmlElement("seeAlso")]
        [JsonProperty("seeAlso", NullValueHandling = NullValueHandling.Ignore)]
        public AthraSee[] SeeAlso { get; set; }

        /// <summary>
        /// Связанные принятые формы имени лица на других языках.
        /// Поле 710.
        /// </summary>
        [CanBeNull]
        [Field(710)]
        [XmlElement("linkedTitle")]
        [JsonProperty("linkedTitles", NullValueHandling = NullValueHandling.Ignore)]
        public object[] LinkedTitles { get; set; }

        /// <summary>
        /// Информационное примечание.
        /// Поле 300.
        /// </summary>
        [CanBeNull]
        [Field(300)]
        [XmlElement("note")]
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Notes { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см. также".
        /// Поле 305.
        /// </summary>
        [CanBeNull]
        [Field(305)]
        [XmlElement("seeAlsoNote")]
        [JsonProperty("seeAlsoNotes", NullValueHandling = NullValueHandling.Ignore)]
        public object[] SeeAlsoNote { get; set; }

        /// <summary>
        /// Примечания об области применения.
        /// Поле 330.
        /// </summary>
        [CanBeNull]
        [Field(330)]
        [XmlElement("usageNote")]
        [JsonProperty("usageNotes", NullValueHandling = NullValueHandling.Ignore)]
        public object[] UsageNotes { get; set; }

        /// <summary>
        /// Источник составления записи.
        /// Поле 801.
        /// </summary>
        [CanBeNull]
        [Field(801)]
        [XmlElement("informationSource")]
        [JsonProperty("informaionSources", NullValueHandling = NullValueHandling.Ignore)]
        public object[] InformationSources { get; set; }

        /// <summary>
        /// Источник, в котором выявлена информ. о заголовке.
        /// Поле 810.
        /// </summary>
        [CanBeNull]
        [Field(810)]
        [XmlElement("identificationSource")]
        [JsonProperty("identificationSources", NullValueHandling = NullValueHandling.Ignore)]
        public object[] IdentificationSources { get; set; }

        /// <summary>
        /// Источник, в котором не выявлена информ. о заголовке.
        /// Поле 815.
        /// </summary>
        [CanBeNull]
        [Field(815)]
        [XmlElement("nonIdentificationSource")]
        [JsonProperty("nonIdentificationSources", NullValueHandling = NullValueHandling.Ignore)]
        public object[] NonIdentificationSources { get; set; }

        /// <summary>
        /// Информации об использовании использовании заголовка в поле 200.
        /// Поле 820.
        /// </summary>
        [CanBeNull]
        [Field(820)]
        [XmlElement("usageInformation")]
        [JsonProperty("usageInformation", NullValueHandling = NullValueHandling.Ignore)]
        public object[] UsageInformation { get; set; }

        /// <summary>
        /// Пример,приведенный в примечании.
        /// Поле 825.
        /// </summary>
        [CanBeNull]
        [Field(825)]
        [XmlElement("example")]
        [JsonProperty("examples", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Examples { get; set; }

        /// <summary>
        /// Общее примечание каталогизатора.
        /// Поле 830.
        /// </summary>
        [CanBeNull]
        [Field(830)]
        [XmlElement("cataloguerNote")]
        [JsonProperty("cataloguerNotes", NullValueHandling = NullValueHandling.Ignore)]
        public object[] CataloguerNotes { get; set; }

        /// <summary>
        /// Информация об исключении принятого имени лица.
        /// Поле 835.
        /// </summary>
        [CanBeNull]
        [Field(835)]
        [XmlElement("exclusionInformation")]
        [JsonProperty("exclusionInformation", NullValueHandling = NullValueHandling.Ignore)]
        public object[] ExclusionInformation { get; set; }

        /// <summary>
        /// Правила каталогизации и предметизации.
        /// Поле 152.
        /// </summary>
        [CanBeNull]
        [Field(152)]
        [XmlElement("cataloguingRules")]
        [JsonProperty("cataloguingRules", NullValueHandling = NullValueHandling.Ignore)]
        public object CataloguingRules { get; set; }

        /// <summary>
        /// Каталогизатор, дата.
        /// Поле 907.
        /// </summary>
        [CanBeNull]
        [Field(907)]
        [XmlElement("technology")]
        [JsonProperty("technology", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Technology { get; set; }

        /// <summary>
        /// Имя рабочего листа.
        /// Поле 920.
        /// </summary>
        [CanBeNull]
        [Field(920)]
        [XmlElement("worksheet")]
        [JsonProperty("worksheet", NullValueHandling = NullValueHandling.Ignore)]
        public string Worksheet { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord" />.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MainTitle.ToVisibleString();
        }

        #endregion
    }
}
