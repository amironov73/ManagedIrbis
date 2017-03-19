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

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforPlus7
    {
        #region Private members

        #endregion

        #region Public methods

        public static void AppendGlobal
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
#if PocketPC || WINMOBILE || SILVERLIGHT

                string[] parts = expression.Split(new[] { '#' });

#else

                string[] parts = expression.Split(new[] { '#' }, 2);

#endif

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

        public static void ClearGlobals
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            context.Globals.Clear();
        }

        private static bool _Contains
            (
                IEnumerable<RecordField> fields,
                RecordField oneField
            )
        {
            string text = oneField.ToString();
            foreach (RecordField field in fields)
            {
                if (field.ToString() == text)
                {
                    return true;
                }
            }

            return false;
        }

        public static void DistinctGlobal
            (
                PftContext context,
                PftNode node,
                string expression
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

        public static void MultiplyGlobals
            (
                PftContext context,
                PftNode node,
                string expression
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

        public static void ReadGlobal
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
#if PocketPC || WINMOBILE || SILVERLIGHT

                string[] parts = expression.Split(new[] { '#' });

#else

                string[] parts = expression.Split(new []{'#'}, 2);

#endif

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

        public static void SortGlobal
            (
                PftContext context,
                PftNode node,
                string expression
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

        public static void SubstractGlobals
            (
                PftContext context,
                PftNode node,
                string expression
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

        public static void UnionGlobals
            (
                PftContext context,
                PftNode node,
                string expression
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

        public static void WriteGlobal
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
#if PocketPC || WINMOBILE || SILVERLIGHT

                string[] parts = expression.Split(new[] { '#' });

#else

                string[] parts = expression.Split(new[] {'#'}, 2);

#endif

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

        #endregion
    }
}
