// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RightRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Запись с правами доступа к ресурсам.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RightRecord
    {
        #region Properties

        /// <summary>
        /// Идентификатор записи. Поле 1.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "0001".
        /// </remarks>
        [CanBeNull]
        [Field(1)]
        [XmlAttribute("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Общий период действия права доступа. Поле 2.
        /// </summary>
        [CanBeNull]
        [Field(2)]
        [XmlElement("period")]
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public ValidityPeriod Period { get; set; }

        /// <summary>
        /// Права доступа. Поле 3 (повторяющееся).
        /// </summary>
        [CanBeNull]
        [Field(3)]
        [XmlElement("right")]
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public AccessRight[] Rights { get; set; }

        /// <summary>
        /// Описание/название. Поле 4.
        /// </summary>
        [CanBeNull]
        [Field(4)]
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static RightRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            RightRecord result = new RightRecord
            {
                Id = record.FM(1),
                Period = ValidityPeriod.Parse(record.Fields.GetFirstField(2)),
                Rights = AccessRight.Parse(record),
                Description = record.FM(4),
                Record = record
            };

            return result;
        }

        #endregion
    }
}
