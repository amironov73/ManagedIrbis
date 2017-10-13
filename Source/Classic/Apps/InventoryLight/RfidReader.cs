/* RfidReader.cs
 */

#region Using directives

using System;
using System.Text;

using ManagedClient;

using PCSC;

#endregion

namespace InventoryLight
{
    public sealed class RfidReader
    {
        #region Properties

        public string ReaderName { get { return _readerName; } }

        #endregion

        #region Construction

        public RfidReader
            (
                string readerName
            )
        {
            _readerName = readerName;
        }

        #endregion

        #region Private members

        private readonly string _readerName;

        private SCardContext _context;
        private SCardReader _reader;

        private bool _InitRfid()
        {
            _context = new SCardContext();
            _context.Establish(SCardScope.System);

            string[] readerNames = _context.GetReaders();
            if ((readerNames == null) || (readerNames.Length == 0))
            {
                throw new ApplicationException("Нет считывателей!");
            }

            int selected = 0;
            if (!string.IsNullOrEmpty(ReaderName))
            {
                selected = Array.IndexOf(readerNames, ReaderName);
                if (selected < 0)
                {
                    throw new ApplicationException
                        (
                            "Нет считывателя с именем " 
                            + ReaderName
                        );
                }
            }

            _reader = new SCardReader(_context);
            SCardError errorCode = _reader.Connect
                (
                    readerNames[selected],
                    SCardShareMode.Shared,
                    SCardProtocol.Any
                );
            if (errorCode == ((SCardError)(-2146434967)))
            {
                _reader = null;
                return false;
            }
            if (errorCode != 0)
            {
                _reader.Disconnect(SCardReaderDisposition.Reset);
                _reader = null;
                throw new ApplicationException
                    ( string.Format (
                        "НЕ УДАЛОСЬ ПОДКЛЮЧИТЬСЯ: {0}",
                        SCardHelper.StringifyError(errorCode)
                    ));
            }
            return true;
        }

        private void _CloseRfid()
        {
            if (_reader != null)
            {
                _reader.Disconnect(SCardReaderDisposition.Reset);
            }
            _reader = null;
            _context.Dispose();
            _context = null;
        }

        #endregion

        #region Public methods

        public string ReadRfid()
        {
            string result = null;

            if (_InitRfid())
            {
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
                    throw new ApplicationException("НЕ УДАЛОСЬ НАЧАТЬ ТРАНЗАКЦИЮ");
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
                    result = builder.ToString();
                }
                else
                {
                    throw new ApplicationException
                        (string.Format(
                            "ОШИБКА: {0}",
                            SCardHelper.StringifyError(errorCode)
                            ));
                }

                _reader.EndTransaction(SCardReaderDisposition.Leave);
            }

            _CloseRfid();

            return result;
        }

        public void Close()
        {
            // Something to do?
        }

        #endregion
    }
}
