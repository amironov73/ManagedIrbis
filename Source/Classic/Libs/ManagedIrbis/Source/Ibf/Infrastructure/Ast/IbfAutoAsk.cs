// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfAutoAsk.cs -- 
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

namespace ManagedIrbis.Ibf.Infrastructure.Ast
{
    //
    // Установка интервала опроса состояния БД (в сек.).
    //
    // Операнды:
    // NN
    // Интервал в секундах.
    // Если задается 0 – опрос состояния БД не выполняется.
    //
    // Примечания:
    // параметр в INI-файл (irbisa.ini) AUTOASKINTERVAL
    //

    /// <summary>
    /// Установка интервала опроса состояния БД (в сек.).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfAutoAsk
        : IbfNode
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IbfNode members

        /// <inheritdoc cref="IbfNode.Execute" />
        public override void Execute
            (
                IbfContext context
            )
        {
            OnBeforeExecution(context);

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
