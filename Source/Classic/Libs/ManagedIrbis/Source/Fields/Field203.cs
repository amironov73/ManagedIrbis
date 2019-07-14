// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Field203.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// В связи с ГОСТ 7.0.100-2018 введено 203 поле.
    /// Это поле содержит подполя: вид содержания,
    /// средства доступа, характеристика содержания.
    /// </summary>
    [PublicAPI]
    public sealed class Field203
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 203;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "12345678abcdefgikloptuvwxyz";

        #endregion

        #region Properties

        /// <summary>
        /// Вид содержания. Подполя A, B, D, E, F, G, I, K, L.
        /// </summary>
        [CanBeNull]
        [ItemCanBeNull]
        public string[] ContentType { get; set; }

        /// <summary>
        /// Средства доступа. Подполя C, 1, 2, 3, 4, 5, 6, 7, 8
        /// </summary>
        [CanBeNull]
        [ItemCanBeNull]
        public string[] Access { get; set; }

        /// <summary>
        /// Характеристика содержания.
        /// Подполя O, P, U, Y, T, R, W, Q, X, V, Z
        /// </summary>
        [CanBeNull]
        [ItemCanBeNull]
        public string[] ContentDescription { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static Field203 Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            Field203 result = new Field203
            {
                ContentType = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('a'),
                        field.GetFirstSubFieldValue('b'),
                        field.GetFirstSubFieldValue('d'),
                        field.GetFirstSubFieldValue('e'),
                        field.GetFirstSubFieldValue('f'),
                        field.GetFirstSubFieldValue('g'),
                        field.GetFirstSubFieldValue('i'),
                        field.GetFirstSubFieldValue('k'),
                        field.GetFirstSubFieldValue('l')
                    )
                    .NonEmptyLines()
                    .ToArray(),

                Access = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('c'),
                        field.GetFirstSubFieldValue('1'),
                        field.GetFirstSubFieldValue('2'),
                        field.GetFirstSubFieldValue('3'),
                        field.GetFirstSubFieldValue('4'),
                        field.GetFirstSubFieldValue('5'),
                        field.GetFirstSubFieldValue('6'),
                        field.GetFirstSubFieldValue('7'),
                        field.GetFirstSubFieldValue('8')
                    )
                    .NonEmptyLines()
                    .ToArray(),

                ContentDescription = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue('o'),
                        field.GetFirstSubFieldValue('p'),
                        field.GetFirstSubFieldValue('u'),
                        field.GetFirstSubFieldValue('y'),
                        field.GetFirstSubFieldValue('t'),
                        field.GetFirstSubFieldValue('r'),
                        field.GetFirstSubFieldValue('w'),
                        field.GetFirstSubFieldValue('q'),
                        field.GetFirstSubFieldValue('x'),
                        field.GetFirstSubFieldValue('v'),
                        field.GetFirstSubFieldValue('z')
                    )
                    .NonEmptyLines()
                    .ToArray()
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            // TODO check whether this code actually works
            ContentType = reader.ReadNullableStringArray();
            Access = reader.ReadNullableStringArray();
            ContentDescription = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullableArray(ContentType)
                .WriteNullableArray(Access)
                .WriteNullableArray(ContentDescription);
        }

        #endregion
    }
}
