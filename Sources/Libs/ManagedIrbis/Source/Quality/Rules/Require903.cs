/* Require903.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    public sealed class Require903
        : QualityRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "903"; }
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
                        "903",
                        20,
                        "Отсутствует поле 903: Шифр документа"
                    );
            }
            else if (fields.Length > 1)
            {
                AddDefect
                    (
                        "903",
                        20,
                        "Повторяется поле 903: Шифр документа"
                    );
            }
            foreach (RecordField field in fields)
            {
                MustNotContainSubfields
                    (
                        field
                    );
            }

            return EndCheck();
        }

        #endregion
    }
}
