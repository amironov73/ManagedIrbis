/* Require920.cs
 */

#region Using directives

using AM;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    public sealed class Require920
        : QualityRule
    {
        #region Private members

        private static readonly string[] _goodWorksheets =
        {
            "PAZK",
            "SPEC",
            "PVK",
            "NJ",
            "NJK",
            "NJP",
            "ASP"
        };

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "920"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            string worksheet = Worksheet;

            if (string.IsNullOrEmpty(worksheet))
            {
                AddDefect
                    (
                        "920",
                        20,
                        "Отсутствует поле 920: Рабочий лист"
                    );
            }

            RecordField[] fields = GetFields();
            if (fields.Length > 1)
            {
                AddDefect
                    (
                        "920",
                        20,
                        "Повторяется поле 920: Рабочий лист"
                    );
            }
            foreach (RecordField field in fields)
            {
                MustNotContainSubfields
                    (
                        field
                    );

                worksheet = field.Value;
                if (!worksheet.OneOf(_goodWorksheets))
                {
                    AddDefect
                        (
                            field,
                            20,
                            "Неожиданный рабочий лист"
                        );
                }
            }

            return EndCheck();
        }

        #endregion
    }
}
