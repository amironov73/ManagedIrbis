// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthrbRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Запись в базе данных ATHRB.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AthrbRecord
    {
        #region Properties

        /// <summary>
        /// Основной заголовок рубрики.
        /// Поле 210.
        /// </summary>
        [CanBeNull]
        [Field(210)]
        public AthrbHeading MainHeading { get; set; }

        /// <summary>
        /// Связанные заголовки рубрики.
        /// Поле 510.
        /// </summary>
        [CanBeNull]
        [Field(510)]
        public AthrbHeading[] LinkedHeadings { get; set; }

        /// <summary>
        /// Методические указания / описания.
        /// Поле 300.
        /// </summary>
        [CanBeNull]
        [Field(300)]
        public AthrbGuidelines[] Guidelines { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static AthrbRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            AthrbRecord result = new AthrbRecord
            {
                MainHeading = AthrbHeading.Parse(record.Fields.GetFirstField(210)),
                LinkedHeadings = record.Fields
                    .GetField(510)
                    .Select(field => AthrbHeading.Parse(field))
                    .ToArray(),
                Guidelines = record.Fields
                    .GetField(300)
                    .Select(field => AthrbGuidelines.Parse(field))
                    .ToArray()
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MainHeading.ToVisibleString();
        }

        #endregion
    }
}
