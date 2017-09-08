// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforM.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforM
    {
        #region Private members

        class FieldToSort
        {
            public RecordField Field { get; set; }

            public string Text { get; set; }
        }

        internal static void SortField
            (
                [NotNull] MarcRecord record,
                int tag,
                char code,
                bool descending
            )
        {
            Code.NotNull(record, "record");

            RecordField[] found = record.Fields.GetField(tag);
            FieldToSort[] fields = new FieldToSort[found.Length];

            for (int i = 0; i < found.Length; i++)
            {
                fields[i] = new FieldToSort
                {
                    Field = found[i],
                    Text = code == '\0'
                        ? found[i].ToText()
                        : found[i].GetFirstSubFieldValue(code)
                          ?? string.Empty
                };
                record.Fields.Remove(found[i]);
            }

            fields =
                (
                    descending
                    ? fields.OrderByDescending(field => field.Text)
                    : fields.OrderBy(field => field.Text)
                )
                .ToArray();

            record.Fields.AddRange
                (
                    fields.Select
                    (
                        field => field.Field
                    )
                    .ToArray()
                );
        }

        #endregion

        #region Public methods

        public static void Sort
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(context.Record, null))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char direction = char.ToLower(navigator.ReadChar());
                if (direction != 'i' && direction != 'd')
                {
                    return;
                }
                string tagText = navigator.ReadUntil('^');
                if (string.IsNullOrEmpty(tagText))
                {
                    return;
                }
                int tag = NumericUtility.ParseInt32(tagText);
                char code = '\0';
                if (!navigator.IsEOF)
                {
                    navigator.ReadChar();
                    code = navigator.ReadChar();
                }

                SortField
                    (
                        context.Record,
                        tag,
                        code,
                        direction != 'i'
                    );
            }
        }

        #endregion
    }
}
