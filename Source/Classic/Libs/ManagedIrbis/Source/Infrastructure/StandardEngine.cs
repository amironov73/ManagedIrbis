// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StandardEngine.cs -- standard execution engine
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Standard execution engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class StandardEngine
        : AbstractEngine
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StandardEngine
            (
                [NotNull] IrbisConnection connection,
                [CanBeNull] AbstractEngine nestedEngine
            )
            : base(connection, nestedEngine)
        {
            Log.Trace("StandardEngine::Constructor");
        }

        #endregion
    }
}
