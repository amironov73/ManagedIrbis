// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require700.cs -- индивидуальные авторы
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Fields;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Индивидуальные авторы.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require700
        : QualityRule
    {
        #region Private members

        private void _CheckField
            (
                RuleContext context,
                RecordField field
            )
        {
            AuthorInfo author = AuthorInfo.ParseField700(field);
            if (!ReferenceEquals(author, null))
            {
                if (!string.IsNullOrEmpty(author.Initials)
                    && !string.IsNullOrEmpty(author.FullName))
                {
                    if (author.Initials == author.FullName)
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Совпадают инициалы и расширение"
                            );
                    }
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec" />
        public override string FieldSpec
        {
            get { return "70[01]"; }
        }

        /// <inheritdoc cref="QualityRule.CheckRecord" />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            MarcRecord record = Record.ThrowIfNull("Record");

            foreach (RecordField field
                in record.Fields.GetFieldBySpec(FieldSpec))
            {
                _CheckField(context, field);
            }

            if (IsPazk())
            {
                RecordField[] fields = record.Fields
                    .GetFieldBySpec("7[01][012]");
                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            700,
                            10,
                            "Отсутствуют сведения об авторах"
                        );
                }
                else
                {
                    if (Record.HaveField(700) && Record.HaveField(710))
                    {
                        AddDefect
                            (
                                700,
                                10,
                                "Одновременно присутствуют поля 700 и 710"
                            );
                    }
                }
            }

            return EndCheck();
        }

        #endregion
    }
}
