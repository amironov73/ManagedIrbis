/* UniforJ.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.PlatformSpecific;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforJ
    {
        #region Private members

        #endregion

        #region Public methods

        public static void GetTermRecordCountDB
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);

            string database = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(database))
            {
                database = context.Environment.Database;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            string term = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(term))
            {
                return;
            }

            string saveDatabase = context.Environment.Database;

            try
            {
                context.Environment.Database = database;

                int[] found = context.Environment.Search(term);
                string output = found.Length.ToInvariantString();
                context.Write(node, output);
                context.OutputFlag = true;
            }
            finally
            {
                context.Environment.Database = saveDatabase;
            }
        }

        #endregion
    }
}
