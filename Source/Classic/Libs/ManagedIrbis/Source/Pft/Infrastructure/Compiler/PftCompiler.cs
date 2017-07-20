// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCompiler.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
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
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCompiler
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Call method for the node.
        /// </summary>
        public void CallNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");
        }

        /// <summary>
        /// Compile nodes.
        /// </summary>
        public void CompileNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                node.Compile(this);
            }
        }

        /// <summary>
        /// End method for the node.
        /// </summary>
        public void EndMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");
        }

        /// <summary>
        /// Start method for the node.
        /// </summary>
        public void StartMethod
            (
                [NotNull] PftNode node
            )
        {
            
        }

        #endregion

        #region Object members

        #endregion
    }
}
