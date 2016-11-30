// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Check908.cs -- авторский знак
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Авторский знак.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Check908
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainSubfields(field);
            string text = field.Value;
            if (string.IsNullOrEmpty(text))
            {
                AddDefect
                    (
                        field,
                        5,
                        "Неверный формат поля 908: Авторский знак"
                    );
            }
            else
            {
                char firstLetter = text[0];
                bool isGood = ((firstLetter >= 'A') && (firstLetter <= 'Z'))
                              || ((firstLetter >= 'А') && (firstLetter <= 'Я'));
                if (!isGood)
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Неверный формат поля 908: Авторский знак"
                        );
                }
                else
                {
                    string regex = @"[А-Я]\s\d{2}";

                    if ((firstLetter >= 'A') && (firstLetter <= 'Z'))
                    {
                        regex = @"[A-Z]\d{2}";
                    }
                    if ((firstLetter == 'З') || (firstLetter == 'О')
                        || (firstLetter == 'Ч'))
                    {
                        regex = @"[ЗОЧ]-\d{2}";
                    }

                    if (!Regex.IsMatch(text, regex))
                    {
                        AddDefect
                            (
                                field,
                                1,
                                "Неверный формат поля 908: Авторский знак"
                            );
                    }
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "908"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            if (fields.Length > 1)
            {

                AddDefect
                    (
                        "908",
                        5,
                        "Повторяется поле 908: Авторский знак"
                    );
            }
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
