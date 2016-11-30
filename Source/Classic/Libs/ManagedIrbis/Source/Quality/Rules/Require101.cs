// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require101.cs -- язык основного текста.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using ManagedIrbis.Menus;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Язык основного текста.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec { get { return "101"; } }

        /// <inheritdoc />
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
