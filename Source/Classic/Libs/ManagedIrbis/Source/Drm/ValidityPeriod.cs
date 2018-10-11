// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ValidityPeriod.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Период действия записи о правах доступа.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ValidityPeriod
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "de";

        #endregion

        #region Properties

        /// <summary>
        /// Начальная дата. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("from")]
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate From { get; set; }

        /// <summary>
        /// Конечная дата. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("till")]
        [JsonProperty("till", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate Till { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static ValidityPeriod Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            ValidityPeriod result = new ValidityPeriod
            {
                From = IrbisDate.SafeParse(field.GetFirstSubFieldValue('d')),
                Till = IrbisDate.SafeParse(field.GetFirstSubFieldValue('e')),
                Field = field
            };

            return result;
        }

        #endregion
    }
}