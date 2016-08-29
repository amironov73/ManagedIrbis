/* RfidDriver.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// Abstract RFID driver.
    /// </summary>
    [PublicAPI]
    public abstract class RfidDriver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Capabilities.
        /// </summary>
        public abstract RfidCapabilities Capabilities { get; }

        /// <summary>
        /// Connected.
        /// </summary>
        public abstract bool Connected { get; }

        /// <summary>
        /// Name.
        /// </summary>
        public abstract string Name { get; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Connect.
        /// </summary>
        public abstract void Connect 
            (
                object connectionData
            );

        /// <summary>
        /// Disconnect.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Get system info.
        /// </summary>
        public abstract RfidSystemInfo GetSystemInfo
            (
                string uid
            );

        /// <summary>
        /// Inventory.
        /// </summary>
        public abstract string[] Inventory();

        /// <summary>
        /// Select.
        /// </summary>
        public abstract void Select
            (
                string uid
            );

        /// <summary>
        /// Set AFI.
        /// </summary>
        public abstract void SetAFI
            (
                string uid,
                int afi
            );

        /// <summary>
        /// Set EAS.
        /// </summary>
        public abstract void SetEAS
            (
                string uid,
                bool flag
            );

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}
