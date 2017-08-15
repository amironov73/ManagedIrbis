// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IbfExitException.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Ibf.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IbfExitException
        : IrbisException
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                int returnCode
            )
            : base(returnCode)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
