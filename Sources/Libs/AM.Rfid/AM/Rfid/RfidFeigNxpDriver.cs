/* RfidFeigNxpDriver.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using OBID;
using OBID.TagHandler;

using HandlerType=OBID.TagHandler.FedmIscTagHandler_ISO15693_NXP_ICODE_SLI;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID tag list.
    /// </summary>
    [PublicAPI]
    public sealed class RfidTagList
    {
        #region Properties

        /// <summary>
        /// Handlers.
        /// </summary>
        public HandlerType[] Handlers
        {
            get
            {
                return _dictionary
                    .Values
                    .ToArray();
            }
        }

        /// <summary>
        /// Identifiers.
        /// </summary>
        public string[] Identifiers
        {
            get
            {
                return _dictionary
                    .Keys
                    .ToArray();
            }
        }

        /// <summary>
        /// Index.
        /// </summary>
        public HandlerType this[string uid]
        {
            get
            {
                if (string.IsNullOrEmpty(uid))
                {
                    throw new ArgumentNullException("uid");
                }
                if (_dictionary.ContainsKey("uid"))
                {
                    throw new RfidException("No such UID: " + uid);
                }
                return _dictionary[uid];
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidTagList
            (
                Dictionary<string, FedmIscTagHandler> source
            )
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source");
            }
            
            _dictionary = new Dictionary<string, HandlerType>();
            
            foreach (var pair in source)
            {
                HandlerType handler = pair.Value as HandlerType;
                if (!ReferenceEquals(handler, null))
                {
                    _dictionary.Add
                        (
                            pair.Key,
                            handler
                        );
                }
            }
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, HandlerType> _dictionary;

        #endregion

        #region Public methods

        #endregion
    }

    /// <summary>
    /// System information.
    /// </summary>
    [Serializable]
    public sealed class RfidSystemInformation
    {
        #region Properties

        /// <summary>
        /// Manufacturer name.
        /// </summary>
        public string ManufacturerName { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// UID.
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// DSFID.
        /// </summary>
        public byte DSFID { get; set; }

        /// <summary>
        /// AFI.
        /// </summary>
        public byte AFI { get; set; }

        /// <summary>
        /// Memory size.
        /// </summary>
        public int MemorySize { get; set; }

        /// <summary>
        /// IC reference.
        /// </summary>
        public byte ICReference { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "ManufacturerName: {0}{7}"
                +   "TagName: {1}{7}"
                +   "UID: {2}{7}"
                +   "DSFID: {3}{7}"
                +   "AFI: {4}{7}"
                +   "MemorySize: {5}{7}"
                +   "ICReference: {6}", 
                    ManufacturerName, 
                    TagName, 
                    UID, 
                    DSFID, 
                    AFI, 
                    MemorySize, 
                    ICReference,
                    Environment.NewLine
                );
        }

        #endregion
    }

    /// <summary>
    /// RFID NXP driver.
    /// </summary>
    public class RfidFeigNxpDriver
        : IDisposable
    {
        #region Properties

        /// <inheritdoc/>
        public bool Connected { get { return _connected; } }

        /// <summary>
        /// ISO table capacity.
        /// </summary>
        public static int IsoTableCapacity = 100;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidFeigNxpDriver()
        {
            _reader = new FedmIscReader();

            string message;
            if (_reader.EvalLibDependencies(out message) < 0)
            {
                throw new RfidException(message);
            }

            _reader.SetBusAddress(0);
            _reader.SetTableSize
                (
                    FedmIscReaderConst.ISO_TABLE, 
                    IsoTableCapacity
                );
        }

        #endregion

        #region Private members

        private bool _connected;

        private readonly FedmIscReader _reader;

        // ReSharper disable UnusedParameter.Local
        private void _CheckReturnCode
            (
                int code
            )
        {
            if (code < 0)
            {
                throw new RfidException
                    (
                        _reader.GetErrorText(code)
                    );
            }
        }
        // ReSharper restore UnusedParameter.Local

        private void _CheckConnection()
        {
            if (!_connected)
            {
                throw new RfidException();
            }
        }

        private RfidSystemInformation _GetSystemInfo
            (
                HandlerType tag
            )
        {
            string uid;
            byte afi;
            uint memSize;
            byte icRef;
            _CheckReturnCode
                (
                    tag.GetSystemInformation
                        (
                            out uid, 
                            out afi, 
                            out memSize, 
                            out icRef
                        )
                );
            RfidSystemInformation result = new RfidSystemInformation
                {
                    TagName = tag.GetTagName(),
                    ManufacturerName = tag.GetManufacturerName(),
                    UID = uid,
                    AFI = afi,
                    DSFID = GetDSFID(uid),
                    MemorySize = (int) memSize,
                    ICReference = icRef
                };
            return result;
        }

        private void _SetEAS
            (
                HandlerType tag,
                bool flag
            )
        {
            int result = flag
                ? tag.SetEAS()
                : tag.ResetEAS();
            
            _CheckReturnCode(result);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Connect.
        /// </summary>
        public RfidFeigNxpDriver Connect
            (
                object connectionData
            )
        {
            if (!_connected)
            {
                string connectionString = connectionData as string;
                if (ReferenceEquals(connectionString, null)
                    || string.IsNullOrEmpty(connectionString))
                {
                    throw new RfidException();
                }
                if (connectionString.Length < 3)
                {
                    throw new RfidException();
                }

                string method = connectionString
                    .Substring(0, 3)
                    .ToUpperInvariant();
                int portNumber;

                switch (method)
                {
                    case "USB":
                        ConnectUSB();
                        break;

                    case "TCP":
                        string hostName = connectionString.Substring(4);
                        int index = connectionString.IndexOf(':');
                        if (index < 0)
                        {
                            throw new RfidException();
                        }
                        portNumber = int.Parse(connectionString.Substring(index + 1));
                        hostName = hostName.Substring(0, index);
                        ConnectTCP
                            (
                                hostName,
                                portNumber
                            );
                        break;

                    case "COM":
                        portNumber = int.Parse(connectionString.Substring(3));
                        ConnectCOM(portNumber);
                        break;

                    default:
                        throw new RfidException();
                }
            }
            return this;
        }

        /// <summary>
        /// Connect via USB.
        /// </summary>
        public RfidFeigNxpDriver ConnectUSB()
        {
            if (!_connected)
            {
                _reader.ConnectUSB(0);
                _connected = true;
            }
            return this;
        }

        /// <summary>
        /// Connect via TCP.
        /// </summary>
        public RfidFeigNxpDriver ConnectTCP
            (
                string host,
                int portNumber
            )
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            if (!_connected)
            {
                _reader.ConnectTCP
                    (
                        host,
                        portNumber
                    );
                _connected = true;
            }
            return this;
        }

        /// <summary>
        /// Connect via COM port.
        /// </summary>
        public RfidFeigNxpDriver ConnectCOM
            (
                int portNumber
            )
        {
            if (!_connected)
            {
                _reader.ConnectCOMM(portNumber, false);
                _reader.SetPortPara("timeout", "5000");
                _connected = true;
            }
            return this;
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public RfidFeigNxpDriver Disconnect()
        {
            if (_connected)
            {
                _reader.DisConnect();
                _connected = false;
            }
            return this;
        }

        /// <summary>
        /// Get DSFID.
        /// </summary>
        public byte GetDSFID
            (
                string uid
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("uid");
            }

            _CheckConnection();

            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_REQ_UID, uid);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x2B);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE_ADR, 0x01);

            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }
            int index = _reader.FindTableIndex
                (
                    0,
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_SNR,
                    uid
                );
            if (index < 0)
            {
                throw new RfidException();
            }
            byte dsfID;
            _reader.GetTableData
                (
                    index,
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_DSFID,
                    out dsfID
                );
            return dsfID;
        }

        /// <summary>
        /// Get reader info.
        /// </summary>
        public string GetReaderInfo()
        {
            _CheckConnection();
            return _reader.GetReaderInfo().GetReport();
        }

        /// <summary>
        /// Get reader name.
        /// </summary>
        public string GetReaderName()
        {
            _CheckConnection();
            return _reader.GetReaderName();
        }

        /// <summary>
        /// Get system information.
        /// </summary>
        /// <returns></returns>
        public RfidSystemInformation[] GetSystemInformation()
        {
            _CheckConnection();
            ResetRF();

            return ListTags()
                .Handlers
                // ReSharper disable once ConvertClosureToMethodGroup
                .Select(tag => _GetSystemInfo(tag))
                .ToArray();
        }

        /// <summary>
        /// Inventory.
        /// </summary>
        public string[] Inventory()
        {
            _CheckConnection();
            ResetRF();
            
            return new RfidTagList 
                (
                    _reader.TagInventory
                        (
                            true,
                            0x00,
                            0x01
                        )
                )
                .Identifiers;
        }

        /// <summary>
        /// List tags.
        /// </summary>
        public RfidTagList ListTags()
        {
            return new RfidTagList
                (
                    _reader.GetTagList()
                );
        }

        /// <summary>
        /// Lock AFI.
        /// </summary>
        public RfidFeigNxpDriver LockAFI
            (
                string uid
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.LockAFI());
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.LockAFI());
            }

            return this;
        }

        /// <summary>
        /// Lock DSFID.
        /// </summary>
        public RfidFeigNxpDriver LockDSFID
            (
                string uid
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.LockDSFID());
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.LockDSFID());
            }

            return this;
        }

        /// <summary>
        /// Lock EAS.
        /// </summary>
        public RfidFeigNxpDriver LockEAS
            (
                string uid
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.LockEAS());
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.LockEAS());
            }

            return this;
        }

        /// <summary>
        /// Read.
        /// </summary>
        public byte[] Read
            (
                string uid,
                uint firstBlock,
                uint blockCount
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new RfidException();
            }

            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            HandlerType tag = tags[uid];
            byte[] result;
            uint blockSize;
            int returnCode = tag.ReadMultipleBlocks
                (
                    firstBlock, 
                    blockCount, 
                    out blockSize, 
                    out result
                );
            _CheckReturnCode(returnCode);

            return result;
        }

        /// <summary>
        /// Reset RF.
        /// </summary>
        public RfidFeigNxpDriver ResetRF()
        {
            _CheckConnection();
            _CheckReturnCode(_reader.SendProtocol(0x69));
            
            return this;
        }

        /// <summary>
        /// Reset to Ready State.
        /// </summary>
        public RfidFeigNxpDriver ResetToReady
            (
                string uid
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.ResetToReady());
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.ResetToReady());
            }

            return this;
        }

        /// <summary>
        /// Select.
        /// </summary>
        public HandlerType Select
            (
                string uid
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("uid");
            }

            RfidTagList tags = ListTags();
            HandlerType tag = tags[uid];

            return (HandlerType)_reader.TagSelect
                (
                    tag, 
                    tag.GetTagDriverType()
                );
        }

        /// <summary>
        /// Set AFI.
        /// </summary>
        public RfidFeigNxpDriver SetAFI
            (
                string uid,
                byte value
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.WriteAFI(value));
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.WriteAFI(value));
            }

            return this;
        }

        /// <summary>
        /// Set DSFID.
        /// </summary>
        public RfidFeigNxpDriver SetDSFID
            (
                string uid,
                byte dsfID
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.WriteDSFID(dsfID));
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.WriteDSFID(dsfID));
            }

            return this;
        }

        /// <summary>
        /// Set EAS.
        /// </summary>
        public RfidFeigNxpDriver SetEAS
            (
                string uid,
                bool flag
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _SetEAS ( tag, flag );
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _SetEAS ( tag, flag );
            }

            return this;
        }

        /// <summary>
        /// Stay quiet.
        /// </summary>
        public RfidFeigNxpDriver StayQuiet
            (
                string uid
            )
        {
            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            if (string.IsNullOrEmpty(uid))
            {
                foreach (HandlerType tag in tags.Handlers)
                {
                    _CheckReturnCode(tag.StayQuiet());
                }
            }
            else
            {
                HandlerType tag = tags[uid];
                _CheckReturnCode(tag.StayQuiet());
            }

            return this;
        }

        /// <summary>
        /// Convert byte array to hex text.
        /// </summary>
        public static string ToHex
            (
                byte[] array
            )
        {
            if (ReferenceEquals(array, null))
            {
                throw new ArgumentNullException("array");
            }

            return FeHexConvert.ByteArrayToHexStringWithSpaces(array);
        }

        /// <summary>
        /// Write binary data.
        /// </summary>
        public RfidFeigNxpDriver Write
            (
                string uid,
                uint firstBlock,
                uint blockCount,
                uint blockSize,
                byte[] data
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("uid");
            }
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException("data");
            }

            _CheckConnection();
            ResetRF();

            RfidTagList tags = ListTags();
            HandlerType tag = tags[uid];
            int returnCode = tag.WriteMultipleBlocks
                (
                    firstBlock,
                    blockCount,
                    blockSize,
                    data
                );
            _CheckReturnCode(returnCode);

            return this;
        }

        /// <summary>
        /// Write ANSI text.
        /// </summary>
        public RfidFeigNxpDriver Write
            (
                string uid,
                uint firstBlock,
                string text
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("uid");
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            Encoding encoding = Encoding.Default;
            uint length = (uint) encoding.GetByteCount(text);
            length = ((length + 3)/4)*4;
            byte[] data = new byte[length];
            encoding.GetBytes(text, 0, text.Length, data, 0);
            uint blockCount = length/4;
            
            return Write
                (
                    uid,
                    firstBlock,
                    blockCount,
                    4,
                    data
                );
        }

        /// <summary>
        /// Write UTF8 text.
        /// </summary>
        public RfidFeigNxpDriver WriteUtf8
            (
                string uid,
                uint firstBlock,
                string text
            )
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("uid");
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            Encoding encoding = Encoding.UTF8;
            uint length = (uint)encoding.GetByteCount(text);
            length = ((length + 3) / 4) * 4;
            byte[] data = new byte[length];
            encoding.GetBytes(text, 0, text.Length, data, 0);
            uint blockCount = length / 4;

            return Write
                (
                    uid,
                    firstBlock,
                    blockCount,
                    4,
                    data
                );
        }

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
