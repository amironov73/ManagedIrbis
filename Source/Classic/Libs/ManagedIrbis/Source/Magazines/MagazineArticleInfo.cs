// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineArticleInfo.cs -- информация о статье из журнала
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Информация о статье из журнала/сборника.
    /// Рабочий лист ASP.
    /// </summary>
    [PublicAPI]
    [XmlRoot("article")]
    [MoonSharpUserData]
    public sealed class MagazineArticleInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Авторы, поля 70x и 710x.
        /// </summary>
        [CanBeNull]
        [XmlElement("author")]
        [Description("Авторы")]
        [DisplayName("Авторы")]
        [JsonProperty("authors", NullValueHandling = NullValueHandling.Ignore)]
        public AuthorInfo[] Authors { get; set; }

        /// <summary>
        /// Заглавие, поле 200.
        /// </summary>
        [CanBeNull]
        [XmlElement("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public TitleInfo Title { get; set; }

        /// <summary>
        /// Издание, в котором опубликована статья, поле 463.
        /// </summary>
        [CanBeNull]
        [XmlElement("source")]
        [Description("Издание, в котором опубликована статья")]
        [DisplayName("Издание, в котором опубликована статья")]
        [JsonProperty("sources", NullValueHandling = NullValueHandling.Ignore)]
        public SourceInfo[] Sources { get; set; }

        #endregion

        #region Private members

        private static int[] _authorTags = {700, 701, 702};

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ASP-записи.
        /// </summary>
        [NotNull]
        public static MagazineArticleInfo ParseAsp
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MagazineArticleInfo result = new MagazineArticleInfo();
            result.Authors = AuthorInfo.ParseRecord(record, _authorTags);
            RecordField field200 = record.Fields.GetFirstField(200);
            if (!ReferenceEquals(field200, null))
            {
                result.Title = TitleInfo.ParseField200(field200);
            }
            result.Sources = SourceInfo.ParseRecord(record);

            return result;
        }

        /// <summary>
        /// Разбор NJ-записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static MagazineArticleInfo[] ParseIssue
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<MagazineArticleInfo> result = new List<MagazineArticleInfo>();
            foreach (RecordField field in record.Fields.GetField(922))
            {
                MagazineArticleInfo article = ParseField330(field);
                result.Add(article);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор PAZK/SPEC-записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static MagazineArticleInfo[] ParseBook
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<MagazineArticleInfo> result = new List<MagazineArticleInfo>();
            foreach (RecordField field in record.Fields.GetField(330))
            {
                MagazineArticleInfo article = ParseField330(field);
                result.Add(article);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор поля (330 или 922).
        /// </summary>
        public static MagazineArticleInfo ParseField330
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MagazineArticleInfo result = new MagazineArticleInfo
            {
                Authors = AuthorInfo.ParseField330(field),
                Title = TitleInfo.ParseField330(field)
            };
            

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Authors = reader.ReadNullableArray<AuthorInfo>();
            Title = reader.RestoreNullable<TitleInfo>();
            Sources = reader.ReadNullableArray<SourceInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullableArray(Authors)
                .WriteNullable(Title)
                .WriteNullableArray(Sources);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<MagazineArticleInfo> verifier
                = new Verifier<MagazineArticleInfo>(this, throwOnError);

            verifier
                .NotNull(Title, "Title");

            return verifier.Result;
        }

        #endregion
    }
}
