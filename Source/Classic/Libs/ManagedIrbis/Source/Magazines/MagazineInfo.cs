// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineInfo.cs -- информация о журнале в целом
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Информация о журнале в целом.
    /// </summary>
    [PublicAPI]
    [XmlRoot("magazine")]
    [MoonSharpUserData]
    public sealed class MagazineInfo
        : IHandmadeSerializable
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Код документа в базе. Поле 903.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Заглавие. Поле 200^a.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Подзаголовочные сведения.
        /// Поле 200^e.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("sub-title")]
        [JsonProperty("sub-title")]
        public string SubTitle { get; set; }

        /// <summary>
        /// Обозначение и выпуск серии.
        /// Поле 923^1.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("series-number")]
        [JsonProperty("series-number")]
        public string SeriesNumber { get; set; }

        /// <summary>
        /// Заголовок серии.
        /// Поле 923^i.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("series-title")]
        [JsonProperty("series-title")]
        public string SeriesTitle { get; set; }

        /// <summary>
        /// Расширенное заглавие. 
        /// Включает заголовок выпуск и заголовок серии.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public string ExtendedTitle
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(Title);
                if (!string.IsNullOrEmpty(SeriesNumber))
                {
                    result.AppendFormat(". {0}", SeriesNumber);
                }
                if (!string.IsNullOrEmpty(SeriesTitle))
                {
                    result.AppendFormat(". {0}", SeriesTitle);
                }
                if (!string.IsNullOrEmpty(SubTitle))
                {
                    result.AppendFormat(": {0}", SubTitle);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Тип издания. Поле 110^t
        /// </summary>
        [CanBeNull]
        [XmlAttribute("magazine-type")]
        [JsonProperty("magazine-type")]
        public string MagazineType { get; set; }

        /// <summary>
        /// Вид издания. Поле 110^b
        /// </summary>
        [CanBeNull]
        [XmlAttribute("magazine-kind")]
        [JsonProperty("magazine-kind")]
        public string MagazineKind { get; set; }

        /// <summary>
        /// Периодичность (число). Поле 110^x
        /// </summary>
        [CanBeNull]
        [XmlAttribute("periodicity")]
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }

        /// <summary>
        /// Кумуляция. Поле 909
        /// </summary>
        [CanBeNull]
        [XmlElement("cumulation")]
        [JsonProperty("cumulation")]
        public MagazineCumulation[] Cumulation { get; set; }

        /// <summary>
        /// MFN записи журнала.
        /// </summary>
        [XmlElement("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [CanBeNull]
        public static MagazineInfo Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MagazineInfo result = new MagazineInfo
            {
                Index = record.FM("903"),
                Title = record.FM("200", 'a'),
                SubTitle = record.FM("200", 'e'),
                Cumulation = MagazineCumulation.Parse(record),
                SeriesNumber = record.FM("923",'h'),
                SeriesTitle = record.FM("923", 'i'),
                Mfn = record.Mfn
            };

            if (string.IsNullOrEmpty(result.Title)
                || string.IsNullOrEmpty(result.Index)
                //|| string.IsNullOrEmpty(result.MagazineKind)
                //|| string.IsNullOrEmpty(result.MagazineType)
                )
            {
                return null;
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            SubTitle = reader.ReadNullableString();
            SeriesNumber = reader.ReadNullableString();
            SeriesTitle = reader.ReadNullableString();
            MagazineType = reader.ReadNullableString();
            MagazineKind = reader.ReadNullableString();
            Periodicity = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();

            // TODO Handle Cumulation array
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Index)
                .WriteNullable(Description)
                .WriteNullable(Title)
                .WriteNullable(SubTitle)
                .WriteNullable(SeriesNumber)
                .WriteNullable(SeriesTitle)
                .WriteNullable(MagazineType)
                .WriteNullable(MagazineKind)
                .WriteNullable(Periodicity)
                .WritePackedInt32(Mfn);

            // TODO Handle Cumulation array
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return ExtendedTitle;
        }

        #endregion
    }
}
