// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthrbHeading.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Заголовок рубрики в базе ATHRB.
    /// Поля 210 и 510.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AthrbHeading
    {
        #region Properties

        /// <summary>
        /// Основной заголовок рубрики.
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        public string Heading { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        public string Code1 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        public string Code2 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        public string Code3 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        public string Code4 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        public string Code5 { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static AthrbHeading Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            AthrbHeading result = new AthrbHeading
            {
                Heading = field.GetFirstSubFieldValue('a'),
                Code1 = field.GetFirstSubFieldValue('b'),
                Code2 = field.GetFirstSubFieldValue('c'),
                Code3 = field.GetFirstSubFieldValue('d'),
                Code4 = field.GetFirstSubFieldValue('e'),
                Code5 = field.GetFirstSubFieldValue('f'),
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Heading.ToVisibleString();
        }

        #endregion
    }
}