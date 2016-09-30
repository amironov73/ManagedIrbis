/* Require101.cs -- язык основного текста.
 */

#region Using directives

using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Язык основного текста.
    /// </summary>
    public sealed class Require101
        : QualityRule
    {
        #region Private members

        private MenuFile _menu;

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainSubfields
                (
                    field
                );
            if (!CheckForMenu(_menu, field.Value))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Поле 101 (язык основного текста) не из словаря"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        /// <summary>
        /// Затрагиваемые поля.
        /// </summary>
        /// <value>The field spec.</value>
        public override string FieldSpec { get { return "101"; } }

        /// <summary>
        /// Проверка записи.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>RuleReport.</returns>
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
                        "101",
                        10,
                        "Не заполнено поле 101: Язык основного текста"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            _menu = CacheMenu("jz.mnu", _menu);
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
