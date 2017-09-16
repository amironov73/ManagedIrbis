// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Check10.cs -- ISBN и цена.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// ISBN и цена.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Check10
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainText(field);

            SubField isbn = field.GetFirstSubField('a');
            if (isbn != null)
            {
                if (isbn.Value.SafeContains("(", " ", ".", ";", "--"))
                {
                    AddDefect
                        (
                            field,
                            isbn,
                            1,
                            "Неверно введен ISBN в поле 10"
                        );
                }
            }

            SubField price = field.GetFirstSubField('d');
            if (price != null)
            {
                if (!Regex.IsMatch
                    (
                        price.Value.ThrowIfNull("price.Value"),
                        @"\d+\.\d{2}"
                    ))
                {
                    AddDefect
                        (
                            field,
                            price,
                            5,
                            "Неверный формат цены в поле 10"
                        );
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec" />
        public override string FieldSpec
        {
            get { return "10"; }
        }

        /// <inheritdoc cref="QualityRule.CheckRecord" />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
