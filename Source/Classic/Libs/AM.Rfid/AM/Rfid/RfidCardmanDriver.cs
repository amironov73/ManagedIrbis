/* RfidCardmanDriver.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using PCSC;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID Cardman driver.
    /// </summary>
    [PublicAPI]
    public sealed class RfidCardmanDriver
        : RfidDriver
    {
        #region Properties

        /// <inheritdoc/>
        public override RfidCapabilities Capabilities
        {
            get { return RfidCapabilities.None; }
        }

        /// <inheritdoc/>
        public override bool Connected
        {
            get { return _connected; }
        }

        /// <inheritdoc/>
        public override string Name
        {
            get { return "Cardman"; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidCardmanDriver()
        {
            _context = new SCardContext();
            _context.Establish(SCardScope.System);
            _reader = new SCardReader(_context);
        }

        #endregion

        #region Private members

        private readonly SCardContext _context;
        private readonly SCardReader _reader;

        private bool _connected;

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public override void Connect
            (
                object connectionData
            )
        {
            string connectionString = connectionData as string;
            if (ReferenceEquals(connectionString, null)
                || string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionData");
            }

            SCardError errorCode = _reader.Connect
                (
                    connectionString,
                    SCardShareMode.Shared,
                    SCardProtocol.Any
                );
            if (errorCode != 0)
            {
                throw new RfidException
                    (
                        SCardHelper.StringifyError(errorCode)
                    );
            }

            _connected = true;
        }

        /// <inheritdoc/>
        public override void Disconnect()
        {
            _reader.Disconnect(SCardReaderDisposition.Reset);
            _context.Dispose();
            _connected = false;
        }

        /// <summary>
        /// List installed readers.
        /// </summary>
        public string[] GetReaders()
        {
            string[] result = _context.GetReaders();
            return result;
        }

        /// <inheritdoc/>
        public override RfidSystemInfo GetSystemInfo
            (
                string uid
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string[] Inventory()
        {
            List<string> result = new List<string>();

            byte[] sendBuffer =
            {
                0xFF,
                0xCA,
                0x00,
                0x00,
                0x00
            };
            byte[] receiveBuffer = new byte[10];

            SCardError errorCode = _reader.BeginTransaction();
            if (errorCode != SCardError.Success)
            {
                throw new RfidException("Can't start RFID transaction");
            }
            SCardPCI ioreq = new SCardPCI(); // создаём пустой объект
            IntPtr sendPci = SCardPCI.GetPci(_reader.ActiveProtocol);
            errorCode = _reader.Transmit
                (
                    sendPci,
                    sendBuffer,
                    ioreq,
                    ref receiveBuffer
                );
            if (errorCode == SCardError.Success)
            {
                StringBuilder builder = new StringBuilder(8);
                for (int i = receiveBuffer.Length - 3; i >= 0; i--)
                {
                    builder.AppendFormat("{0:X2}", receiveBuffer[i]);
                }
                result.Add(builder.ToString());
            }
            else
            {
                throw new RfidException
                    (
                        SCardHelper.StringifyError(errorCode)
                    );
            }

            _reader.EndTransaction(SCardReaderDisposition.Leave);

            return result.ToArray();
        }

        /// <inheritdoc/>
        public override void Select
            (
                string uid
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetAFI
            (
                string uid, 
                int afi
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetEAS
            (
                string uid, 
                bool flag
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
