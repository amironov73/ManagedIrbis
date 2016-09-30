/* Require461.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    public sealed class Require461
        : QualityRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "461"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            if (IsPazk())
            {
                if (fields.Length != 0)
                {
                    AddDefect
                        (
                            "461",
                            10,
                            "Присутствует поле 461 при рабочем листе PAZK"
                        );
                }
                goto DONE;
            }

            if (IsSpec())
            {
                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            "461",
                            10,
                            "Не заполнено поле 461: Основные сведения"
                        );
                }
                else
                {
                    RecordField field = fields[0];
                    if (field.HaveNotSubField('c'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 461^C: Заглавие"
                            );
                    }
                }
            }

        DONE: return EndCheck();
        }

        #endregion
    }
}
