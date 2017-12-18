// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookSupplyException.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BookSupplyException
        : Exception
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BookSupplyException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BookSupplyException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BookSupplyException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BookSupplyException
            (
                [NotNull] SerializationInfo info,
                StreamingContext context
            )
            : base(info, context)
        {
        }

        #endregion
    }
}
