// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require200.cs -- основное заглавие
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Основное заглавие.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require200
        : QualityRule
    {
        #region Private members

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "200"; }
        }

        /// <inheritdoc />
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
                        "200",
                        10,
                        "Не заполнено поле 200: Заглавие"
                    );
            }
            else if (fields.Length != 1)
            {
                AddDefect
                    (
                        "200",
                        10,
                        "Повторяется поле 200: Заглавие"
                    );
            }
            else
            {
                RecordField field = fields[0];
                if (IsSpec())
                {
                    if (field.HaveNotSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 200^v: Обозначение и номер тома"
                            );
                    }
                }
                else
                {
                    if (field.HaveSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Присутствует подполе 200^v: Обозначение и номер тома"                                
                            );
                    }
                    if (field.HaveNotSubField('a'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 200^a: Заглавие"
                            );
                    }
                }
            }

            return EndCheck();
        }

        #endregion
    }
}
