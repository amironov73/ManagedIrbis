// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckEmptySubfields.cs -- обнаружение пустых подполей
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
    /// Обнаружение пустых подполей.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CheckEmptySubfields
        : QualityRule
    {
        #region Private members

        private void _CheckField
            (
                RecordField field
            )
        {
            foreach (SubField subField in field.SubFields)
            {
                if (string.IsNullOrEmpty(subField.Value))
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Пустое подполе {0}^{1}",
                            field.Tag,
                            subField.Code
                        );
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "!100,330,905,907,919,920,3005"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            foreach (RecordField field in fields)
            {
                _CheckField
                    (
                        field
                    );
            }

            return EndCheck();
        }

        #endregion
    }
}
