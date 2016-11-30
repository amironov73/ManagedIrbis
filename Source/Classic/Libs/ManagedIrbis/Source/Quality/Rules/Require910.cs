// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require910.cs -- сведения об экземплярах
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using ManagedIrbis.Fields;
using ManagedIrbis.Menus;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Сведения об экземплярах.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require910
        : QualityRule
    {
        #region Private members

        private MenuFile _statusMenu;
        private MenuFile _placeMenu;

        private void CheckField
            (
                RecordField field
            )
        {
            ExemplarInfo exemplar = ExemplarInfo.Parse(field);

            if (!CheckForMenu(_statusMenu, exemplar.Status))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Статус экземпляра не из словаря"
                    );
            }
            if (!CheckForMenu(_placeMenu, exemplar.Place))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Место хранения не из словаря"
                    );
            }
            if (string.IsNullOrEmpty(exemplar.Number))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Не задан номер экземпляра"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "910"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (!IsBook())
            {
                goto DONE;
            }

            RecordField[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        "910",
                        10,
                        "Нет сведений об экземплярах: поле 910"
                    );
            }

            _statusMenu = CacheMenu("ste.mnu", _statusMenu);
            _placeMenu = CacheMenu("mhr.mnu", _placeMenu);

            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            MustBeUniqueSubfield
                (
                    fields,
                    'b'
                );

            DONE: return EndCheck();
        }

        #endregion
    }
}
