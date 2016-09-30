/* Require215.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Количественные характеристики.
    /// </summary>
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

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "215"; }
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
