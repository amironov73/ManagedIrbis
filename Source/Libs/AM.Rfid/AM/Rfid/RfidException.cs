/* RfidException.cs -- RFID-specific exception
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID-specific exception.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class RfidException
        : ArsMagnaException
    {
        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidException
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

        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //public RfidException
        //    (
        //        SerializationInfo info,
        //        StreamingContext context
        //    )
        //    : base
        //    (
        //        info,
        //        context
        //    )
        //{
        //}

        #endregion
    }
}
