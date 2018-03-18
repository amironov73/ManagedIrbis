// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class SearchException
        : IrbisException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException
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
