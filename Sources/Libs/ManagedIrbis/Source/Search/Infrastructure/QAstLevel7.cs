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

        /// <summary>
        /// Is complex expression?
        /// </summary>
        public bool IsComplex
        {
            get
            {
                return Items[0].IsComplex;
            }
        }

        /// <summary>
        /// Items.
        /// </summary>
        public QAstLevel6[] Items { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            QAstLevel6 level6 = Items[0];
            string result = level6.ToString();

            if (level6.IsComplex)
            {
                result = " ( " + result + " ) ";
            }

            return result;
        }

        #endregion

    }
}
