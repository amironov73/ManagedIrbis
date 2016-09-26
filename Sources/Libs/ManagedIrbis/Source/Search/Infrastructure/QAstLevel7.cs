/* QAstLevel7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Level 7.
    /// </summary>
    public sealed class QAstLevel7
    {
        #region Properties

        public QAstLevel6[] Items { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Items.Length == 1)
            {
                return Items[0].ToString();
            }

            StringBuilder result = new StringBuilder();

            result.Append(" ( ");
            foreach (QAstLevel6 item in Items)
            {
                result.Append(item);
            }
            result.Append(" ) ");

            return result.ToString();
        }

        #endregion

    }
}
