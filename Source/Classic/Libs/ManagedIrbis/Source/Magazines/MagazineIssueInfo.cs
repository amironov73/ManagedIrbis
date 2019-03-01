// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineIssueInfo.cs -- сведения о номере журнала
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

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
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи.
        /// </summary>
        [XmlAttribute("index")]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Index { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("document-code")]
        [JsonProperty("document-code", NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentCode { get; set; }

        /// <summary>
        /// Шифр журнала. Поле 933.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("magazine-code")]
        [JsonProperty("magazine-code", NullValueHandling = NullValueHandling.Ignore)]
        public string MagazineCode { get; set; }

        /// <summary>
        /// Год. Поле 934.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }

        /// <summary>
        /// Том. Поле 935.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("volume")]
        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public string Volume { get; set; }

        /// <summary>
        /// Номер, часть. Поле 936.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("number")]
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// Номер для нужд сортировки. Поля нет.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string NumberForSorting
        {
            get
            {
                string result = Number;

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Trim();
                }

                return result;
            }
        }

        /// <summary>
        /// Дополнение к номеру. Поле 931^c.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("supplement")]
        [JsonProperty("supplement", NullValueHandling = NullValueHandling.Ignore)]
        public string Supplement { get; set; }

        /// <summary>
        /// Рабочий лист. Поле 920.
        /// (чтобы отличать подшивки от выпусков журналов)
        /// </summary>
        [CanBeNull]
        [XmlAttribute("worksheet")]
        [JsonProperty("worksheet", NullValueHandling = NullValueHandling.Ignore)]
        public string Worksheet { get; set; }

        /// <summary>
        /// Расписанное оглавление. Поле 922.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("article")]
        [JsonProperty("articles", NullValueHandling = NullValueHandling.Ignore)]
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
        /// Loan count.
        /// </summary>
        [XmlElement("loanCount")]
        [JsonProperty("loanCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LoanCount { get; set; }

        /// <summary>
        /// Это подшивка?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsBinding
        {
            get { return Worksheet.SameString("NJK"); }
        }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record{get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static MagazineIssueInfo Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MagazineIssueInfo result = new MagazineIssueInfo
            {
                Mfn = record.Mfn,
                Index = record.FM(903),
                DocumentCode = record.FM(903),
                MagazineCode = record.FM(933),
                Year = record.FM(934),
                Volume = record.FM(935),
                Number = record.FM(936),
                Supplement = record.FM(931, 'c'),
                Worksheet = record.FM(920),

                Articles = record.Fields
                    .GetField(922)
                    .Select(field => MagazineArticleInfo.ParseField330(field))
                    .ToArray(),

                Exemplars = record.Fields
                    .GetField(910)
                    .Select(field => ExemplarInfo.Parse(field))
                    .ToArray(),

                LoanCount = record.FM(999).SafeToInt32(),

                Record = record
            };

            return result;
        }

        /// <summary>
        /// Превращение в запись.
        /// </summary>
        [NotNull]
        public MarcRecord ToRecord()
        {
            MarcRecord result = new MarcRecord();
            result
                .AddNonEmptyField(903, Index)
                .AddNonEmptyField(933, MagazineCode)
                .AddNonEmptyField(934, Year)
                .AddNonEmptyField(935, Volume)
                .AddNonEmptyField(936, Number)
                .AddNonEmptyField(920, Worksheet)
                .AddNonEmptyField(999, LoanCount);

            MagazineArticleInfo[] articles = Articles;
            if (!ReferenceEquals(articles, null))
            {
                foreach (MagazineArticleInfo article in articles)
                {
                    result.Fields.Add(article.ToField());
                }
            }

            ExemplarInfo[] exemplars = Exemplars;
            if (!ReferenceEquals(exemplars, null))
            {
                foreach (ExemplarInfo exemplar in exemplars)
                {
                    result.Fields.Add(exemplar.ToField());
                }
            }

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Articles"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeArticles()
        {
            return !Articles.IsNullOrEmpty();
        }

        /// <summary>
        /// Should serialize the <see cref="LoanCount"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLoanCount()
        {
            return LoanCount != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
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
            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            DocumentCode = reader.ReadNullableString();
            MagazineCode = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Supplement = reader.ReadNullableString();
            Worksheet = reader.ReadNullableString();
            Articles = reader.ReadNullableArray<MagazineArticleInfo>();
            Exemplars = reader.ReadNullableArray<ExemplarInfo>();
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
                .WriteNullable(Index)
                .WriteNullable(Description)
                .WriteNullable(DocumentCode)
                .WriteNullable(MagazineCode)
                .WriteNullable(Year)
                .WriteNullable(Volume)
                .WriteNullable(Number)
                .WriteNullable(Supplement)
                .WriteNullable(Worksheet);
            writer.WriteNullableArray(Articles);
            writer.WriteNullableArray(Exemplars);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<MagazineIssueInfo> verifier
                = new Verifier<MagazineIssueInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(DocumentCode, "DocumentCode")
                .NotNullNorEmpty(MagazineCode, "MagazineCode")
                .NotNullNorEmpty(Number, "Number")
                .NotNullNorEmpty(Year, "Year");

            return verifier.Result;
        }

        #endregion

        #region Object info

        /// <inheritdoc cref="object.ToString" />
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
