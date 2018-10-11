// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AccessRight.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Право доступа к ресурсу. Поле 3.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class AccessRight
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefg";

        #endregion

        #region Properties

        /// <summary>
        /// Элемент доступа. Подполе a.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "02".
        /// </remarks>
        [CanBeNull]
        [SubField('a')]
        public string ElementKind { get; set; }

        /// <summary>
        /// Значение элемента доступа. Подполе b.
        /// </summary>
        /// <remarks>
        /// Типичное значние: "В01".
        /// </remarks>
        [CanBeNull]
        [SubField('b')]
        public string ElementValue { get; set; }

        /// <summary>
        /// Значение права доступа. Подполе c.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "2".
        /// </remarks>
        [CanBeNull]
        [SubField('c')]
        public string AccessKind { get; set; }

        /// <summary>
        /// Количественное ограничение. Подполе f.
        /// </summary>
        [SubField('f')]
        public int LimitValue { get; set; }

        /// <summary>
        /// Единицы ограничения. Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        public string LimitKind { get; set; }

        /// <summary>
        /// Начальная дата периода доступа. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("from")]
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate FromDate { get; set; }

        /// <summary>
        /// Конечная дата периода доступа. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("till")]
        [JsonProperty("till", NullValueHandling = NullValueHandling.Ignore)]
        public IrbisDate TillDate { get; set; }

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
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static AccessRight Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            AccessRight result = new AccessRight
            {
                ElementKind = field.GetFirstSubFieldValue('a'),
                ElementValue = field.GetFirstSubFieldValue('b'),
                AccessKind = field.GetFirstSubFieldValue('c'),
                LimitValue = field.GetFirstSubFieldValue('f').SafeToInt32(),
                LimitKind = field.GetFirstSubFieldValue('g'),
                FromDate = IrbisDate.SafeParse(field.GetFirstSubFieldValue('d')),
                TillDate = IrbisDate.SafeParse(field.GetFirstSubFieldValue('e')),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static AccessRight[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(3)
                .Select(field => Parse(field))
                .ToArray();
        }

        #endregion
    }
}