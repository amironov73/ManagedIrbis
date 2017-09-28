// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftException.cs -- base class for PFT-related exceptions.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Base class for PFT script parsing and exection related exceptions.
    /// </summary>
    [PublicAPI]
    public class PftException
        : IrbisException
    {
        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftException
            (
                string message,
                Exception innerException
            )
            : base
            (
                message,
                innerException
            )
        {
        }

        #endregion
    }
}
