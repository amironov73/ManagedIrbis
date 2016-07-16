/* MagazineIssueInfo.cs -- сведения о номере журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Fields;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Сведения о номере журнала
    /// </summary>
    [PublicAPI]
    [XmlRoot("issue")]
    [MoonSharpUserData]
    [DebuggerDisplay("{Year} {Number} {Supplement}")]
    public sealed class MagazineIssueInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("document-code")]
        [JsonProperty("document-code")]
        public string DocumentCode { get; set; }

        /// <summary>
        /// Шифр журнала. Поле 933.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("magazine-code")]
        [JsonProperty("magazine-code")]
        public string MagazineCode { get; set; }

        /// <summary>
        /// Год. Поле 934.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Том. Поле 935.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Номер, часть. Поле 936.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дополнение к номеру. Поле 931^c.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("supplement")]
        [JsonProperty("supplement")]
        public string Supplement { get; set; }

        /// <summary>
        /// Рабочий лист. Поле 920.
        /// (чтобы отличать подшивки от выпусков журналов)
        /// </summary>
        [CanBeNull]
        [XmlAttribute("worksheet")]
        [JsonProperty("worksheet")]
        public string Worksheet { get; set; }

        /// <summary>
        /// Расписанное оглавление. Поле 922.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("article")]
        [JsonProperty("articles")]
        public MagazineArticleInfo[] Articles { get; set; }

        /// <summary>
        /// Экземпляры. Поле 910.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("exemplar")]
        [JsonProperty("exemplars")]
        public ExemplarInfo[] Exemplars { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        //[NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static MagazineIssueInfo Parse
            (
                [NotNull] IrbisRecord record
            )
        {
            Code.NotNull(record, "record");

            MagazineIssueInfo result = new MagazineIssueInfo
            {
                Mfn = record.Mfn,
                DocumentCode = record.FM("903"),
                MagazineCode = record.FM("933"),
                Year = record.FM("934"),
                Volume = record.FM("935"),
                Number = record.FM("936"),
                Supplement = record.FM("931", 'c'),
                Worksheet = record.FM("920"),
                Articles = record.Fields
                    .GetField("922")
                    .Select(MagazineArticleInfo.Parse)
                    .ToArray(),
                Exemplars = record.Fields
                    .GetField("910")
                    .Select(ExemplarInfo.Parse)
                    .ToArray()
            };

            return result;
        }

        /// <summary>
        /// Сравнение двух выпусков
        /// (с целью сортировки по возрастанию номеров).
        /// </summary>
        public static int CompareNumbers
            (
                [NotNull] MagazineIssueInfo first,
                [NotNull] MagazineIssueInfo second
            )
        {
            Code.NotNull(first, "first");
            Code.NotNull(second, "second");

            return NumberText.Compare
                (
                    first.Number, 
                    second.Number
                );
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Description = reader.ReadNullableString();
            DocumentCode = reader.ReadNullableString();
            MagazineCode = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Supplement = reader.ReadNullableString();
            Worksheet = reader.ReadNullableString();
            Articles = reader.ReadArray<MagazineArticleInfo>();
            Exemplars = reader.ReadArray<ExemplarInfo>();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(Description)
                .WriteNullable(DocumentCode)
                .WriteNullable(MagazineCode)
                .WriteNullable(Year)
                .WriteNullable(Volume)
                .WriteNullable(Number)
                .WriteNullable(Supplement)
                .WriteNullable(Worksheet);
            Articles.SaveToStream(writer);
            Exemplars.SaveToStream(writer);
        }

        #endregion

        #region Object info

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Supplement))
            {
                return string
                    .Format
                        (
                            "{0} ({1})", 
                            Number.ToVisibleString(), 
                            Supplement
                        )
                    .Trim();
            }
            return Number.ToVisibleString().Trim();
        }

        #endregion
    }
}
