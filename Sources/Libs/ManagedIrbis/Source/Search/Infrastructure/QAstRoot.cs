/* QAstRoot.cs --
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
    /// Root of the syntax tree.
    /// </summary>
    public sealed class QAstRoot
    {
        #region Properties

        public QAstLevel9 Level9 { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (ReferenceEquals(Level9, null))
            {
                return string.Empty;
            }

            string result = Level9.ToString()
                .Trim();

            return result;
        }

        #endregion
    }
}
