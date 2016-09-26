/* QAstLevel1.cs --
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
    /// Level 1.
    /// </summary>
    public sealed class QAstLevel1
    {
        #region Properties

        /// <summary>
        /// Is complex expression?
        /// </summary>
        public bool IsComplex
        {
            get
            {
                return Right != null && Right.Length != 0;
            }
        }

        /// <summary>
        /// Left part.
        /// </summary>
        public QAstLeaf Left { get; set; }

        /// <summary>
        /// Right part.
        /// </summary>
        public QAstLeaf[] Right { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Left);
            if (!ReferenceEquals(Right, null))
            {
                foreach (QAstLeaf right in Right)
                {
                    result.Append(" . ");
                    result.Append(right);
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
