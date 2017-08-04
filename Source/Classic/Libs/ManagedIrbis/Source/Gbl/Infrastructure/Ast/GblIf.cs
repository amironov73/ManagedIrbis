// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblIf.cs -- 
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

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Official documentation:
    //
    // Определяет условие выполнения операторов,
    // следующих за ним до оператора FI.
    // Состоит из двух строк:
    // первая строка – имя оператора IF;
    // вторая строка – формат, результатом которого может быть
    // строка ‘1’, что означает разрешение на выполнение
    // последующих операторов, или любое другое значение,
    // что означает запрет на выполнение последующих операторов.
    //

    /// <summary>
    /// Условный оператор.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblIf
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "IF";

        #endregion

        #region Properties

        /// <summary>
        /// Children nodes (THEN branch).
        /// </summary>
        [NotNull]
        public GblNodeCollection Children { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblIf()
        {
            Children = new GblNodeCollection(this);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region GblNode members

        /// <summary>
        /// Execute the node.
        /// </summary>
        public override void Execute
            (
                GblContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Mnemonic;
        }

        #endregion
    }
}
