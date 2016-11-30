// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require215.cs -- количественные характеристики
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Количественные характеристики.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require215
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            SubField volume = field.GetFirstSubField('a');
            SubField units = field.GetFirstSubField('1');

            if (volume == null)
            {
                AddDefect
                    (
                        field,
                        5,
                        "Не заполнено подполе 215^a: Объем (цифры)"
                    );
            }

            if (units != null)
            {
                if (StringUtility.SafeCompare(units.Value, "С.", "С"))
                {
                    AddDefect
                        (
                            field,
                            units,
                            1,
                            "Указана единица измерения 'С' в подполе 215^1"
                        );
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "215"; }
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
                        "215",
                        5,
                        "Отсутствует поле 215: Количественные характеристики"
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
