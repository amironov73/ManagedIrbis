// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthraWorkplace.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Место работы в базе данных ATHRA.
    /// Поле 910.
    /// </summary>
    [PublicAPI]
    [XmlRoot("workplace")]
    [MoonSharpUserData]
    public sealed class AthraWorkPlace
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "py";

        #endregion

        #region Properties

        /// <summary>
        /// Работает в данной организации.
        /// Подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlElement("here")]
        [JsonProperty("here", NullValueHandling = NullValueHandling.Ignore)]
        public string WorksHere { get; set; }

        /// <summary>
        /// Место работы автора.
        /// Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlElement("place")]
        [JsonProperty("place", NullValueHandling = NullValueHandling.Ignore)]
        public string WorkPlace { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

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
        /// Apply to the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ApplyTo
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field.ApplySubField('y', WorksHere)
                .ApplySubField('p', WorkPlace);

            return field;
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static AthraWorkPlace Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            AthraWorkPlace result = new AthraWorkPlace
            {
                WorksHere = field.GetFirstSubFieldValue('y'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(910)
                .AddNonEmptySubField('p', WorkPlace)
                .AddNonEmptySubField('y', WorksHere);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return WorkPlace.ToVisibleString();
        }

        #endregion
    }
}
