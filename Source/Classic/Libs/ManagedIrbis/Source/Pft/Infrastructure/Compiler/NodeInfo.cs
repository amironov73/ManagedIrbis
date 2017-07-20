// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class NodeInfo
    {
        #region Properties

        /// <summary>
        /// Node.
        /// </summary>
        [NotNull]
        public PftNode Node { get; set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ready?
        /// </summary>
        public bool Ready { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeInfo
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            Node = node;
        }

        #endregion
    }
}
