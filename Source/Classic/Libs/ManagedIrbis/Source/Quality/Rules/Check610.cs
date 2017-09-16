// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Check610.cs -- ключевые слова
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Ключевые слова.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Check610
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                [NotNull] RecordField field
            )
        {
            MustNotContainSubfields(field);

            string text = field.Value;
            if (text.SafeContains("."))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Ключевые слова с сокращениями"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec" />
        public override string FieldSpec
        {
            get { return "610"; }
        }

        /// <inheritdoc cref="QualityRule.CheckRecord" />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        610,
                        5,
                        "Отсутствуют ключевые слова: поле 610"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            foreach (RecordField field in fields)
            {
                CheckField(field);
            }


            return EndCheck();
        }

        #endregion
    }
}
