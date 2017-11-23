// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineCumulation.cs -- данные о кумуляции номеров
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [PublicAPI]
    [XmlRoot("cumulation")]
    [MoonSharpUserData]
    public sealed class MagazineCumulation
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 909;

        #endregion

        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("volume")]
        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public string Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("place")]
        [JsonProperty("place", NullValueHandling = NullValueHandling.Ignore)]
        public string Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("numbers")]
        [JsonProperty("numbers", NullValueHandling = NullValueHandling.Ignore)]
        public string Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("set")]
        [JsonProperty("set", NullValueHandling = NullValueHandling.Ignore)]
        public string Set { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static MagazineCumulation Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MagazineCumulation result = new MagazineCumulation
            {
                Year = field.GetFirstSubFieldValue('q'),
                Volume = field.GetFirstSubFieldValue('f'),
                Place = field.GetFirstSubFieldValue('d'),
                Numbers = field.GetFirstSubFieldValue('h'),
                Set = field.GetFirstSubFieldValue('k')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static MagazineCumulation[] Parse
            (
                [NotNull] MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineCumulation[] Parse
            (
                MarcRecord record
            )
        {
            return Parse(record, Tag);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            Numbers = reader.ReadNullableString();
            Set = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Year)
                .WriteNullable(Volume)
                .WriteNullable(Place)
                .WriteNullable(Numbers)
                .WriteNullable(Set);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<MagazineCumulation> verifier
                = new Verifier<MagazineCumulation>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Year, "Year")
                .NotNullNorEmpty(Numbers, "Number");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Year + ":" + Numbers;
        }

        #endregion
    }
}
