/* RfidFeigDriver.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using OBID;
using OBID.TagHandler;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID FEIG driver.
    /// </summary>
    [PublicAPI]
    public sealed class RfidFeigDriver
        : RfidDriver
    {
        #region Properties

        /// <inheritdoc/>
        public override RfidCapabilities Capabilities
        {
            get { return RfidCapabilities.AFI | RfidCapabilities.EAS | RfidCapabilities.SystemInfo; }
        }

        /// <inheritdoc/>
        public override bool Connected
        {
            get { return _connected; }
        }

        /// <inheritdoc/>
        public override string Name
        {
            get { return "FEIG"; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RfidFeigDriver()
        {
            _reader = new FedmIscReader();
            _reader.SetBusAddress(0);
            _reader.SetTableSize(FedmIscReaderConst.ISO_TABLE, 100);
        }

        #endregion

        #region Private members

        private bool _connected;

        private readonly FedmIscReader _reader;

        #endregion

        #region RfidDriver methods

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
                throw new RfidException();
            }
            if (connectionString.ToUpperInvariant() == "USB")
            {
                ConnectUSB();
            }
            else
            {
                int portNumber = int.Parse(connectionString.Substring(3));
                ConnectCOM(portNumber);
            }
        }

        /// <summary>
        /// Connect USB.
        /// </summary>
        public void ConnectUSB()
        {
            _reader.ConnectUSB(0);
            _connected = true;
        }

        /// <summary>
        /// Connect COM port.
        /// </summary>
        /// <param name="portNumber"></param>
        public void ConnectCOM
            (
                int portNumber
            )
        {
            _reader.ConnectCOMM(portNumber, false);
            _reader.SetPortPara("timeout", "5000");
        }

        /// <summary>
        /// Destroy.
        /// </summary>
        public void Destroy
            (
                string epc,
                string password
            )
        {
            byte mode = 0; // Mode (always 0)
            //byte epcLen = (byte) epc.Length; // Number of bytes in EPC
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_EPC_DESTROY_MODE, 0);
            //_reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_EPC_DESTROY_LEN, epcLen);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_EPC_DESTROY_PASSWORD, password);
            //_reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_EPC_DESTROY_EPC, epc);
            _reader.SendProtocol(0x18);
        }

        /// <inheritdoc/>
        public override void Disconnect()
        {
            _reader.DisConnect();
            _connected = false;
        }

        /// <inheritdoc/>
        public override RfidSystemInfo GetSystemInfo
            (
                string uid
            )
        {
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
                return null;
            }
            RfidSystemInfo result = new RfidSystemInfo();
            byte afi;
            _reader.GetTableData
                (
                    index, 
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_AFI, 
                    out afi
                );
            result.UID = uid;
            result.AFI = afi;
            byte dsfid;
            _reader.GetTableData
                (
                    index, 
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_DSFID, 
                    out dsfid
                );
            result.DSFID = dsfid;
            //byte[] memSize = { 0, 0 };
            //reader.GetTableData(index, FedmIscReaderConst.ISO_TABLE,
            //    (int)FedmIscReaderConst.DATA_MEM_SIZE, out memSize);
            byte icref;
            _reader.GetTableData(index, FedmIscReaderConst.ISO_TABLE,
                FedmIscReaderConst.DATA_IC_REF, out icref);
            result.ICReference = icref;

            return result;
        }

        /// <inheritdoc/>
        public override string[] Inventory()
        {
            List<string> result = new List<string>();

            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x01);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.ResetTable(FedmIscReaderConst.ISO_TABLE);
            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }

            // Просим заполнить таблицу данными о метках в поле зрения считывателя
            for (
                    int index = 0;
                    index < _reader.GetTableLength(FedmIscReaderConst.ISO_TABLE);
                    index++
                )
            {
                //byte transponderType; // Тип транспондера
                //_reader.GetTableData
                //    (
                //        index,
                //        FedmIscReaderConst.ISO_TABLE,
                //        FedmIscReaderConst.DATA_TRTYPE,
                //        out transponderType
                //    );

                string uid; // Идентификатор метки
                _reader.GetTableData
                    (
                        index,
                        FedmIscReaderConst.ISO_TABLE,
                        FedmIscReaderConst.DATA_SNR,
                        out uid
                    );
                result.Add(uid);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Inventory.
        /// </summary>
        public string[] Inventory2()
        {
            List<string> result = new List<string>();

            //RF Reset
            _reader.SendProtocol(0x69);

            Dictionary<string, FedmIscTagHandler> tags = _reader.TagInventory(true, 0x00, 0x01);
            var handlers = tags.Values;
            foreach (FedmIscTagHandler handler in handlers)
            {
                result.Add(handler.GetUid());
                var isoHandler = handler as FedmIscTagHandler_ISO15693_NXP_ICODE_SLI;
                if (isoHandler != null)
                {
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Reset to Ready State.
        /// </summary>
        public void ResetToReady
            (
                string uid
            )
        {
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_REQ_UID, uid);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x26);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE_ADR, 0x01);

            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }
        }

        /// <inheritdoc/>
        public override void Select
            (
                string uid
            )
        {
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_REQ_UID, uid);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x25);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE_ADR, 0x01);

            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }
        }

        /// <summary>
        /// Stay quiet.
        /// </summary>
        public void StayQuiet
            (
                string uid
            )
        {
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_REQ_UID, uid);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x02);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE_ADR, 0x01);

            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }
            
        }

        /// <inheritdoc/>
        public override void SetAFI
            (
                string uid, 
                int afi
            )
        {
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_REQ_UID, uid);
            int index = _reader.FindTableIndex
                (
                    0, 
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_SNR, 
                    uid
                );
            if (index < 0)
            {
                return;
            }
            _reader.SetTableData
                (
                    index, 
                    FedmIscReaderConst.ISO_TABLE,
                    FedmIscReaderConst.DATA_AFI, 
                    checked ((byte)afi)
                );
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_CMD, 0x27);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE, 0x00);
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B0_MODE_ADR, 0x01);

            int status = _reader.SendProtocol(0xB0);
            if (status != 0x00)
            {
                throw new RfidException();
            }
        }

        /// <inheritdoc/>
        public override void SetEAS
            (
                string uid, 
                bool flag
            )
        {
            byte subcommand = flag ? (byte)0xA2 : (byte)0xA3;
            _reader.SetData(FedmIscReaderID.FEDM_ISC_TMP_B1_CMD, subcommand);
            _reader.SetData
                (
                    FedmIscReaderID.FEDM_ISC_TMP_B1_MFR,
                    FedmIscReaderConst.ISO_MFR_PHILIPS
                );
            _reader.SetData
                (
                    FedmIscReaderID.FEDM_ISC_TMP_B1_MODE,
                    FedmIscReaderConst.ISO_MODE_ADR
                );
            _reader.SetData
                (
                    FedmIscReaderID.FEDM_ISC_TMP_B1_REQ_UID, 
                    uid
                );
            int status = _reader.SendProtocol(0xB1);
            if (status == 0x95)
            {
                byte isoError;
                _reader.GetData
                    (
                        FedmIscReaderID.FEDM_ISC_TMP_B1_ISO_ERROR, 
                        out isoError
                    );
                // Do something with isoError
            }
            else if (status != 0)
            {
                throw new RfidException();
            }
        }

        #endregion
    }
}
