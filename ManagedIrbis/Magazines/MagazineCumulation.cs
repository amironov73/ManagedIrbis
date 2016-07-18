/* MagazineCumulation.cs -- данные о кумуляции номеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
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
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [PublicAPI]
    [XmlRoot("cumulation")]
    [MoonSharpUserData]
    public sealed class MagazineCumulation
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "909";

        #endregion

        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("place")]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("numbers")]
        [JsonProperty("numbers")]
        public string Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("set")]
        [JsonProperty("set")]
        public string Set { get; set; }

        #endregion

        #region Construciton

        #endregion

        #region Private members

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
        public static MagazineCumulation[] Parse
            (
                MarcRecord record,
                string tag
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("tag");
            }

            return record.Fields
                .GetField(tag)
                .Select(Parse)
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
            return Parse
                (
                    record,
                    Tag
                );
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
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

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
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

        #region Object members

        #endregion
    }
}
