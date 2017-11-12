// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MessageEventArgs.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class MessageEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [NotNull]
        public string Message { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageEventArgs
            (
                [NotNull] string message
            )
        {
            Code.NotNullNorEmpty(message, "message");
            Message = message;
        }

        #endregion
    }
}