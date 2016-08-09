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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Executive
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
        }

        #endregion
    }
}
