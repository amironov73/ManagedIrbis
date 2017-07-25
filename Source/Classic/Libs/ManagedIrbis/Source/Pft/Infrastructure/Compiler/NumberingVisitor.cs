// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberingVisitor.cs -- 
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

using ManagedIrbis.Pft.Infrastructure.Walking;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class NumberingVisitor
        : PftVisitor
    {
        #region Properties

        [NotNull]
        public NodeDictionary Dictionary { get; private set; }

        public int LastId { get; private set; }

        #endregion

        #region Construction

        public NumberingVisitor
            (
                [NotNull] NodeDictionary dictionary
            )
        {
            Dictionary = dictionary;
            LastId = 0;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftVisitor members

        /// <inheritdoc cref="PftVisitor.VisitNode" />
        public override bool VisitNode
            (
                PftNode node
            )
        {
            int id = ++LastId;
            NodeInfo info = new NodeInfo(id, node);
            Dictionary.Add(info);

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
