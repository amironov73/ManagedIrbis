/* Require200.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    public sealed class Require200
        : QualityRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "200"; }
        }

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
