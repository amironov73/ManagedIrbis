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
        /// Заглавие. 461^c.
        /// </summary>
        [CanBeNull]
        [Field("461", 'c')]
        [XmlElement("title")]
        [JsonProperty("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        /// <summary>
        /// Роль (Нехарактерное заглавие, доб.карточка?). 461^u.
        /// </summary>
        [CanBeNull]
        [Field("461", 'u')]
        [XmlElement("specific")]
        [JsonProperty("specific")]
        [Description("Роль (Нехарактерное заглавие, доб.карточка?)")]
        [DisplayName("Роль (Нехарактерное заглавие, доб.карточка?)")]
        public string Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. 461^2.
        /// </summary>
        [CanBeNull]
        [Field("461", '2')]
        [XmlElement("general")]
        [JsonProperty("general")]
        [Description("Общее обозначение материала")]
        [DisplayName("Общее обозначение материала")]
        public string General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. 461^e.
        /// </summary>
        [CanBeNull]
        [Field("461", 'e')]
        [XmlElement("subtitle")]
        [JsonProperty("subtitle")]
        [Description("Сведения, относящиеся к заглавию")]
        [DisplayName("Сведения, относящиеся к заглавию")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Сведения об ответственности. 461^f.
        /// </summary>
        [CanBeNull]
        [Field("461", 'f')]
        [XmlElement("responsibility")]
        [JsonProperty("responsibility")]
        [Description("Сведения об ответственности")]
        [DisplayName("Сведения об ответственности")]
        public string Responsibility { get; set; }

        /// <summary>
        /// Издательство. 461^g.
        /// </summary>
        [CanBeNull]
        [Field("461", 'g')]
        [XmlElement("publisher")]
        [JsonProperty("publisher")]
        [Description("Издательство")]
        [DisplayName("Издательство")]
        public string Publisher { get; set; }

        /// <summary>
        /// Город. 461^d.
        /// </summary>
        [CanBeNull]
        [Field("461", 'd')]
        [XmlElement("city")]
        [JsonProperty("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string City { get; set; }

        /// <summary>
        /// Год начала издания. 461^h.
        /// </summary>
        [CanBeNull]
        [Field("461", 'h')]
        [XmlElement("beginningYear")]
        [JsonProperty("beginningYear")]
        [Description("Год начала издания")]
        [DisplayName("Год начала издания")]
        public string BeginningYear { get; set; }

        /// <summary>
        /// Год окончания издания. 461^z.
        /// </summary>
        [CanBeNull]
        [Field("461", 'z')]
        [XmlElement("endingYear")]
        [JsonProperty("endingYear")]
        [Description("Год окончания издания")]
        [DisplayName("Год окончания издания")]
        public string EndingYear { get; set; }

        /// <summary>
        /// ISBN. 461^i.
        /// </summary>
        [CanBeNull]
        [Field("461", 'i')]
        [XmlElement("isbn")]
        [JsonProperty("isbn")]
        [Description("ISBN")]
        [DisplayName("ISBN")]
        public string Isbn { get; set; }

        /// <summary>
        /// ISSN. 461^j.
        /// </summary>
        [CanBeNull]
        [Field("461", 'j')]
        [XmlElement("issn")]
        [JsonProperty("issn")]
        [Description("ISSN")]
        [DisplayName("ISSN")]
        public string Issn { get; set; }

        /// <summary>
        /// Сведения об издании. 461^p.
        /// </summary>
        [CanBeNull]
        [Field("461", 'p')]
        [XmlElement("reprint")]
        [JsonProperty("reprint")]
        [Description("Сведения об издании")]
        [DisplayName("Сведения об издании")]
        public string Reprint { get; set; }

        /// <summary>
        /// Перевод заглавия. 461^a.
        /// </summary>
        [CanBeNull]
        [Field("461", 'a')]
        [XmlElement("translation")]
        [JsonProperty("translation")]
        [Description("Перевод заглавия")]
        [DisplayName("Перевод заглавия")]
        public string Translation { get; set; }

        /// <summary>
        /// 1-й автор - Заголовок описания. 461^x.
        /// </summary>
        [CanBeNull]
        [Field("461", 'x')]
        [XmlElement("firstAuthor")]
        [JsonProperty("firstAuthor")]
        [Description("1-й автор - Заголовок описания")]
        [DisplayName("1-й автор - Заголовок описания")]
        public string FirstAuthor { get; set; }

        /// <summary>
        /// Коллектив или меропритие - Заголовок описания. 461^b.
        /// </summary>
        [CanBeNull]
        [Field("461", 'b')]
        [XmlElement("collective")]
        [JsonProperty("collective")]
        [Description("Коллектив или меропритие - Заголовок описания")]
        [DisplayName("Коллектив или меропритие - Заголовок описания")]
        public string Collective { get; set; }

        /// <summary>
        /// Вариант заглавия. 46^r.
        /// </summary>
        [CanBeNull]
        [Field("46", 'r')]
        [XmlElement("titleVariant")]
        [JsonProperty("titleVariant")]
        [Description("Вариант заглавия")]
        [DisplayName("Вариант заглавия")]
        public string TitleVariant { get; set; }

        /// <summary>
        /// Обозначение и № 2-й единицы деления (серия). 46^h.
        /// </summary>
        [CanBeNull]
        [Field("46", 'h')]
        [XmlElement("secondLevelNumber")]
        [JsonProperty("secondLevelNumber")]
        [Description("Обозначение и № 2-й единицы деления (серия)")]
        [DisplayName("Обозначение и № 2-й единицы деления (серия)")]
        public string SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-й единицы деления (серия). 46^i.
        /// </summary>
        [CanBeNull]
        [Field("46", 'i')]
        [XmlElement("secondLevelTitle")]
        [JsonProperty("secondLevelTitle")]
        [Description("Заглавие 2-й единицы деления (серия)")]
        [DisplayName("Заглавие 2-й единицы деления (серия)")]
        public string SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-й единицы деления (подсерия). 46^k.
        /// </summary>
        [CanBeNull]
        [Field("46", 'k')]
        [XmlElement("thirdLevelNumber")]
        [JsonProperty("thirdLevelNumber")]
        [Description("Обозначение и № 3-й единицы деления (подсерия)")]
        [DisplayName("Обозначение и № 3-й единицы деления (подсерия)")]
        public string ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-й единицы деления (подсерия). 46^m.
        /// </summary>
        [CanBeNull]
        [Field("46", 'm')]
        [XmlElement("thirdLevelTitle")]
        [JsonProperty("thirdLevelTitle")]
        [Description("Заглавие 3-й единицы деления (подсерия)")]
        [DisplayName("Заглавие 3-й единицы деления (подсерия)")]
        public string ThirdLevelTitle { get; set; }

        /// <summary>
        /// Параллельное заглавие. 46^l.
        /// </summary>
        [CanBeNull]
        [Field("46", 'l')]
        [XmlElement("parallelTitle")]
        [JsonProperty("parallelTitle")]
        [Description("Параллельное заглавие")]
        [DisplayName("")]
        public string ParallelTitle { get; set; }

        /// <summary>
        /// Заглавие серии. 46^a.
        /// </summary>
        [CanBeNull]
        [Field("46", 'a')]
        [XmlElement("seriesTitle")]
        [JsonProperty("seriesTitle")]
        [Description("Заглавие серии")]
        [DisplayName("Заглавие серии")]
        public string SeriesTitle { get; set; }

        /// <summary>
        /// Предыдущее заглавие издания. 46^c.
        /// </summary>
        [CanBeNull]
        [Field("46", 'c')]
        [XmlElement("previousTitle")]
        [JsonProperty("previousTitle")]
        [Description("Предыдущее заглавие издания")]
        [DisplayName("Предыдущее заглавие издания")]
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
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field46 { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static CommonInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<CommonInfo> result = new List<CommonInfo>();
            for (int i = 0;; i++)
            {
                RecordField field461 = record.Fields.GetField("461", i);
                RecordField field46 = record.Fields.GetField("46", i);
                if (ReferenceEquals(field461, field46))
                {
                    // This can happen only if both fields = null
                    break;
                }
                CommonInfo info = new CommonInfo();
                result.Add(info);

                if (!ReferenceEquals(field461, null))
                {
                    info.Title = field461.GetFirstSubFieldValue('c');
                    info.Specific = field461.GetFirstSubFieldValue('u');
                    info.General = field461.GetFirstSubFieldValue('2');
                    info.Subtitle = field461.GetFirstSubFieldValue('e');
                    info.Responsibility = field461.GetFirstSubFieldValue('f');
                    info.Publisher = field461.GetFirstSubFieldValue('g');
                    info.City = field461.GetFirstSubFieldValue('d');
                    info.BeginningYear = field461.GetFirstSubFieldValue('h');
                    info.EndingYear = field461.GetFirstSubFieldValue('z');
                    info.Isbn = field461.GetFirstSubFieldValue('i');
                    info.Issn = field461.GetFirstSubFieldValue('j');
                    info.Reprint = field461.GetFirstSubFieldValue('p');
                    info.Translation = field461.GetFirstSubFieldValue('a');
                    info.FirstAuthor = field461.GetFirstSubFieldValue('x');
                    info.Collective = field461.GetFirstSubFieldValue('b');
                    info.Field461 = field461;
                }

                if (!ReferenceEquals(field46, null))
                {
                    info.TitleVariant = field46.GetFirstSubFieldValue('r');
                    info.SecondLevelNumber = field46.GetFirstSubFieldValue('h');
                    info.SecondLevelTitle = field46.GetFirstSubFieldValue('i');
                    info.ThirdLevelNumber = field46.GetFirstSubFieldValue('k');
                    info.ThirdLevelTitle = field46.GetFirstSubFieldValue('m');
                    info.ParallelTitle = field46.GetFirstSubFieldValue('l');
                    info.SeriesTitle = field46.GetFirstSubFieldValue('a');
                    info.PreviousTitle = field46.GetFirstSubFieldValue('c');
                    info.Field46 = field46;
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
