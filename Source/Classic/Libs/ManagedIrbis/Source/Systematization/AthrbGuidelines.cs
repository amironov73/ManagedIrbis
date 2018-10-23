// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthrbGuidelines.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Методические рекомендации / описания.
    /// Поле 300.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AthrbGuidelines
    {
        #region Properties

        /// <summary>
        /// Методические рекомендации / описания.
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        public string Guidelines { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static AthrbGuidelines Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            AthrbGuidelines result = new AthrbGuidelines
            {
                Guidelines = field.GetFirstSubFieldValue('a')
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Guidelines.ToVisibleString();
        }

        #endregion
    }
}