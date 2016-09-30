/* CheckEmptySubfields.cs -- обнаружение пустых подполей
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Обнаружение пустых подполей
    /// </summary>
    public sealed class CheckEmptySubfields
        : QualityRule
    {
        #region Private members

        private void _CheckField
            (
                RecordField field
            )
        {
            foreach (SubField subField in field.SubFields)
            {
                if (string.IsNullOrEmpty(subField.Value))
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Пустое подполе {0}^{1}",
                            field.Tag,
                            subField.Code
                        );
                }
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "!100,330,905,907,919,920,3005"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            foreach (RecordField field in fields)
            {
                _CheckField
                    (
                        field
                    );
            }

            return EndCheck();
        }

        #endregion
    }
}
