/* Require210.cs
 */

#region Using directives

using AM;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Выходные данные
    /// </summary>
    public sealed class Require210
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainText(field);

            SubField city = field.GetFirstSubField('a');
            SubField publisher = field.GetFirstSubField('c');
            SubField year = field.GetFirstSubField('d');

            if (city != null)
            {
                if (StringUtility.SafeContains(city.Value, ",", ";"))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Несколько городов в одном подполе 210^a"
                        );
                }

                if (StringUtility.SafeContains(city.Value, "б."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город Б. М. в подполе 210^a"
                        );
                }
                else if (StringUtility.SafeContains(city.Value, "."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город с сокращением в подполе 210^a"
                        );
                }
            }

            if (publisher != null)
            {
                if (StringUtility.SafeContains(publisher.Value, ",", ";"))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Несколько издательств в одном подполе 210^c"
                        );
                }

                if (StringUtility.SafeContains(publisher.Value, "б."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство Б. И. в подполе 210^c"
                        );
                }
                else if (StringUtility.SafeContains(publisher.Value, "."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство с сокращением в подполе 210^c"
                        );
                }
            }

            if (year != null)
            {
                if (StringUtility.SafeContains(year.Value, "б."))
                {
                    AddDefect
                        (
                            field,
                            year,
                            1,
                            "Год издания Б. Г. в подполе 210^d"
                        );
                }
            }

            if (field.HaveNotSubField('a') && field.HaveSubField('4'))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Город введен в подполе 200^4: Город на издании"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "210"; }
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
                        "210",
                        10,
                        "Отсутствует поле 210: Выходные данные"
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
