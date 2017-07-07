// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus7
    {
        #region Private members

        private static readonly char[] _numberSign = {'#'};

        private static bool _Contains
            (
                [NotNull] IEnumerable<RecordField> fields,
                [NotNull] RecordField oneField
            )
        {
            string text = oneField.ToText();
            foreach (RecordField field in fields)
            {
                if (field.ToText() == text)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public methods

        // ================================================================

        public static void AppendGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        _numberSign,
                        2
                    );

                if (parts.Length == 2)
                {
                    int index;
                    if (NumericUtility.TryParseInt32(parts[0], out index))
                    {
                        context.Globals.Append(index, parts[1]);
                    }
                }
            }
        }

        // ================================================================

        public static void ClearGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.Globals.Clear();
        }

        // ================================================================

        public static void DistinctGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7G1')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                int index;
                if (NumericUtility.TryParseInt32(expression, out index))
                {
                    RecordField[] fields = context.Globals.Get(index);
                    List<RecordField> result = new List<RecordField>();
                    foreach (RecordField field in fields)
                    {
                        if (!_Contains(result, field))
                        {
                            result.Add(field);
                        }
                    }
                    context.Globals.Set(index, result);
                }
            }
        }

        // ================================================================

        public static void MultiplyGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7M1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }
                int firstIndex, secondIndex;
                if (!NumericUtility.TryParseInt32(parts[0], out firstIndex)
                    || !NumericUtility.TryParseInt32(parts[1], out secondIndex))
                {
                    return;
                }
                RecordField[] first = context.Globals.Get(firstIndex);
                RecordField[] second = context.Globals.Get(secondIndex);
                RecordField[] result = first.Where
                    (
                        one => _Contains(second, one)
                    )
                    .ToArray();
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        public static void ReadGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        _numberSign,
                        2
                    );

                string indexText = parts[0];
                bool haveRepeat = !ReferenceEquals(context.CurrentGroup, null);
                int repeat = context.Index;
                if (parts.Length == 2)
                {
                    string repeatText = parts[1];
                    if (!NumericUtility.TryParseInt32(repeatText, out repeat))
                    {
                        return;
                    }
                    haveRepeat = true;
                    repeat--;
                }
                int index;
                if (NumericUtility.TryParseInt32(indexText, out index))
                {
                    RecordField[] fields = context.Globals.Get(index);

                    if (haveRepeat)
                    {
                        RecordField field = fields.GetOccurrence(repeat);
                        if (!ReferenceEquals(field, null))
                        {
                            string output = field.ToText();
                            if (!string.IsNullOrEmpty(output))
                            {
                                context.Write(node, output);
                                context.OutputFlag = true;
                            }
                        }
                    }
                    else
                    {
                        StringBuilder output = new StringBuilder();
                        bool first = true;
                        foreach (RecordField field in fields)
                        {
                            if (!first)
                            {
                                output.AppendLine();
                            }
                            first = false;
                            output.Append(field.ToText());
                        }
                        if (output.Length != 0)
                        {
                            context.Write(node, output.ToString());
                            context.OutputFlag = true;
                        }
                    }
                }
            }
        }

        // ================================================================

        public static void SortGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7T1')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                int index;
                if (NumericUtility.TryParseInt32(expression, out index))
                {
                    RecordField[] fields = context.Globals.Get(index);
                    Array.Sort
                        (
                            fields, 
                            (left, right) => string.Compare
                                (
                                    left.ToString(),
                                    right.ToString(),
                                    StringComparison.CurrentCulture
                                )
                        );
                    context.Globals.Set(index, fields);
                }
            }
        }

        // ================================================================

        public static void SubstractGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7S1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }
                int firstIndex, secondIndex;
                if (!NumericUtility.TryParseInt32(parts[0], out firstIndex)
                    || !NumericUtility.TryParseInt32(parts[1], out secondIndex))
                {
                    return;
                }
                RecordField[] first = context.Globals.Get(firstIndex);
                RecordField[] second = context.Globals.Get(secondIndex);
                RecordField[] result = first.Where
                    (
                        one => !_Contains(second, one)
                    )
                    .ToArray();
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        public static void UnionGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7S1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }
                int firstIndex, secondIndex;
                if (!NumericUtility.TryParseInt32(parts[0], out firstIndex)
                    || !NumericUtility.TryParseInt32(parts[1], out secondIndex))
                {
                    return;
                }
                RecordField[] first = context.Globals.Get(firstIndex);
                RecordField[] second = context.Globals.Get(secondIndex);
                List<RecordField> result = new List<RecordField>(second);
                foreach (RecordField field in first)
                {
                    if (!_Contains(result, field))
                    {
                        result.Add(field);
                    }
                }
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        public static void WriteGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {

                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        _numberSign,
                        2
                    );

                if (parts.Length == 2)
                {
                    int index;
                    if (NumericUtility.TryParseInt32(parts[0], out index))
                    {
                        context.Globals.Add(index, parts[1]);
                    }
                }
            }
        }

        // ================================================================

        #endregion
    }
}
