// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require60.cs -- раздел знаний
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
    /// Раздел знаний.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require60
        : QualityRule
    {
        #region Private members

        private MenuFile _menu;

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "60"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (IsBook())
            {
                RecordField[] fields = GetFields();

                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            "60",
                            3,
                            "Отстутсвует поле 60: Раздел знаний"
                        );
                }

                _menu = CacheMenu("rzn.mnu", _menu);


            }

            return EndCheck();
        }

        #endregion
    }
}
