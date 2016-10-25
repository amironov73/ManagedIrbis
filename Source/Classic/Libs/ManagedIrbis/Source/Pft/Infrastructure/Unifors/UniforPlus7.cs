/* UniforPlus7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
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
                string[] parts = expression.Split(new[] { '#' }, 2);
                if (parts.Length == 2)
                {
                    int index;
                    if (int.TryParse(parts[0], out index))
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

        public static void ReadGlobal
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split(new []{'#'}, 2);
                string indexText = parts[0];
                int repeat = context.Index;
                if (parts.Length == 2)
                {
                    string repeatText = parts[1];
                    if (!int.TryParse(repeatText, out repeat))
                    {
                        return;
                    }
                    repeat--;
                }
                int index;
                if (int.TryParse(indexText, out index))
                {
                    RecordField[] fields = context.Globals.Get(index);
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
                string[] parts = expression.Split(new[] {'#'}, 2);
                if (parts.Length == 2)
                {
                    int index;
                    if (int.TryParse(parts[0], out index))
                    {
                        context.Globals.Add(index, parts[1]);
                    }
                }
            }
        }

        #endregion
    }
}
