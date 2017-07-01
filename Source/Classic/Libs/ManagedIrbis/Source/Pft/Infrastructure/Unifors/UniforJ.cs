// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
                database = context.Provider.Database;
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

            string saveDatabase = context.Provider.Database;

            try
            {
                context.Provider.Database = database;

                int[] found = context.Provider.Search(term);
                string output = found.Length.ToInvariantString();
                context.Write(node, output);
                context.OutputFlag = true;
            }
            finally
            {
                context.Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
