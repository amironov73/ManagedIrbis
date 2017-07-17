// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftSerializationException.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftSerializationException
        : IrbisException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException
            (
                string message, 
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion
    }
}
