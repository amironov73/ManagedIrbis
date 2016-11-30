// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require903.cs -- шифр документа в базе
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Шифр документа в базе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require903
        : QualityRule
    {
        #region Private members

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "903"; }
        }

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
