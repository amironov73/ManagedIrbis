/* Check10.cs -- ISBN и цена.
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// ISBN и цена.
    /// </summary>
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
                if (StringUtility.SafeContains(isbn.Value, "(", " ", ".", ";", "--"))
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
                if (!Regex.IsMatch(price.Value, @"\d+\.\d{2}"))
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

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "10"; }
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
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
