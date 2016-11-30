// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require920.cs -- рабочий лист
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Рабочий лист
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "920"; }
        }

        /// <inheritdoc />
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
