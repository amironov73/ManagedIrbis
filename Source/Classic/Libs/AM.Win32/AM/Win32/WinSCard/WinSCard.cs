// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WinSCard.cs -- Windows smart card support
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;

#endregion

// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Windows smart cards support.
    /// </summary>
    [PublicAPI]
    public static class WinSCard
    {
        #region Constants

        /// <summary>
        /// DLL name.
        /// </summary>
        public const string DllName = "winscard.dll";

        #endregion

        #region Public methods

        /// <summary>
        /// The GetOpenCardName function displays the smart card
        /// "select card" dialog box. Call the function SCardUIDlgSelectCard
        /// instead of GetOpenCardName. The GetOpenCardName function
        /// is maintained for backward compatibility with version 1.0
        /// of the Microsoft Smart Card Base Components, but calls
        /// to GetOpenCardName are mapped to SCardUIDlgSelectCard.
        /// </summary>
        /// <param name="arg">A pointer to the OPENCARDNAME structure
        /// for the "select card" dialog box.</param>
        /// <returns>SCARD_S_SUCCESS on success.</returns>
        [DllImport(DllName)]
        public static extern int GetOpenCardName (ref OPENCARDNAME arg);

        /// <summary>
        /// The SCardBeginTransaction function starts a transaction.
        /// The function waits for the completion of all other
        /// transactions before it begins. After the transaction
        /// starts, all other applications are blocked from accessing
        /// the smart card while the transaction is in progress.
        /// </summary>
        /// <param name="hCard"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardBeginTransaction(IntPtr hCard);

        /// <summary>
        /// The SCardCancel function terminates all outstanding
        /// actions within a specific resource manager context.
        /// The only requests that you can cancel are those that
        /// require waiting for external action by the smart card
        /// or user. Any such outstanding action requests will
        /// terminate with a status indication that the action
        /// was canceled. This is especially useful to force
        /// outstanding SCardGetStatusChange calls to terminate.
        /// </summary>
        /// <param name="hContext"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardCancel (IntPtr hContext);

        /// <summary>
        /// The SCardConnect function establishes a connection
        /// (using a specific resource manager context) between
        /// the calling application and a smart card contained
        /// by a specific reader. If no card exists in the specified
        /// reader, an error is returned.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="szReader"></param>
        /// <param name="dwShareMode"></param>
        /// <param name="dwPreferredProtocols"></param>
        /// <param name="phCard"></param>
        /// <param name="pdwActiveProtocol"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardConnect
            (
                IntPtr hContext,
                string szReader,
                int dwShareMode,
                int dwPreferredProtocols,
                out IntPtr phCard,
                ref int pdwActiveProtocol
            );

        /// <summary>
        /// The SCardControl function gives you direct control
        /// of the reader. You can call it any time after
        /// a successful call to SCardConnect and before
        /// a successful call to SCardDisconnect. The effect
        /// on the state of the reader depends on the control code.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="dwControlCode"></param>
        /// <param name="lpInBuffer"></param>
        /// <param name="cbInBufferSize"></param>
        /// <param name="lpOutBuffer"></param>
        /// <param name="cbOutBufferSize"></param>
        /// <param name="lpBytesReturned"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardControl
            (
                IntPtr hCard,
                int dwControlCode,
                byte[] lpInBuffer,
                int cbInBufferSize,
                byte[] lpOutBuffer,
                int cbOutBufferSize,
                out int lpBytesReturned
            );

        /// <summary>
        /// The SCardDisconnect function terminates a connection
        /// previously opened between the calling application
        /// and a smart card in the target reader.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="dwDisposition"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardDisconnect
            (
                IntPtr hCard,
                int dwDisposition
            );

        /// <summary>
        /// The SCardEndTransaction function completes a previously
        /// declared transaction, allowing other applications
        /// to resume interactions with the card.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="dwDisposition"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardEndTransaction
            (
                IntPtr hCard,
                int dwDisposition
            );

        /// <summary>
        /// The SCardEstablishContext function establishes the resource
        /// manager context (the scope) within which database operations are performed.
        /// </summary>
        /// <param name="dwScope"></param>
        /// <param name="pvReserved1"></param>
        /// <param name="pvReserved2"></param>
        /// <param name="phContext"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardEstablishContext
            (
                int dwScope,
                IntPtr pvReserved1,
                IntPtr pvReserved2,
                out IntPtr phContext
            );

        /// <summary>
        /// The SCardFreeMemory function releases memory that
        /// has been returned from the resource manager using
        /// the SCARD_AUTOALLOCATE length designator.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="pvMem"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardFreeMemory
            (
                IntPtr hContext,
                IntPtr pvMem
            );

        /// <summary>
        /// The SCardGetAttrib function retrieves the current
        /// reader attributes for the given handle. It does
        /// not affect the state of the reader, driver, or card.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="dwAttrId"></param>
        /// <param name="pbAttr"></param>
        /// <param name="pcbAttrLen"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardGetAttrib
            (
                IntPtr hCard,
                int dwAttrId,
                byte[] pbAttr,
                ref int pcbAttrLen
            );

        /// <summary>
        /// The SCardGetDeviceTypeId function gets the device type
        /// identifier of the card reader for the given reader name.
        /// This function does not affect the state of the reader.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="szReaderName"></param>
        /// <param name="pdwDeviceTypeId"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardGetDeviceTypeIdA
            (
                IntPtr hContext,
                string szReaderName,
                out int pdwDeviceTypeId
            );

        /// <summary>
        /// The SCardGetReaderIcon function gets an icon of the smart
        /// card reader for a given reader's name. This function does
        /// not affect the state of the card reader.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="szReaderName"></param>
        /// <param name="pbIcon"></param>
        /// <param name="pcbIcon"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardGetReaderIcon
            (
                IntPtr hContext,
                string szReaderName,
                byte[] pbIcon,
                ref int pcbIcon
            );

        /// <summary>
        /// The SCardGetStatusChange function blocks execution
        /// until the current availability of the cards in
        /// a specific set of readers changes.
        /// The caller supplies a list of readers to be monitored
        /// by an SCARD_READERSTATE array and the maximum amount
        /// of time (in milliseconds) that it is willing to wait
        /// for an action to occur on one of the listed readers.
        /// Note that SCardGetStatusChange uses the user-supplied
        /// value in the dwCurrentState members of the
        /// rgReaderStates SCARD_READERSTATE array as the definition
        /// of the current state of the readers. The function returns
        /// when there is a change in availability, having filled
        /// in the dwEventState members of rgReaderStates appropriately.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="dwTimeout"></param>
        /// <param name="rgReaderStates"></param>
        /// <param name="cReaders"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardGetStatusChange
            (
                IntPtr hContext,
                int dwTimeout,
                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
                SCARD_READERSTATE[] rgReaderStates,
                int cReaders
            );

        /// <summary>
        /// The SCardListInterfaces function provides a list
        /// of interfaces supplied by a given card.
        /// The caller supplies the name of a smart card previously
        /// introduced to the subsystem, and receives the list
        /// of interfaces supported by the card.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="szCard"></param>
        /// <param name="pguidInterfaces"></param>
        /// <param name="pcguidInterfaces"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardListInterfaces
            (
                IntPtr hContext,
                string szCard,
                byte[] pguidInterfaces,
                ref int pcguidInterfaces
            );

        /// <summary>
        /// The SCardListReaders function provides the list of readers
        /// within a set of named reader groups, eliminating duplicates.
        /// The caller supplies a list of reader groups, and receives
        /// the list of readers within the named groups. Unrecognized
        /// group names are ignored. This function only returns readers
        /// within the named groups that are currently attached
        /// to the system and available for use.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="mszGroups"></param>
        /// <param name="mszReaders"></param>
        /// <param name="pcchReaders"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardListReaders
            (
                IntPtr hContext,
                string mszGroups,
                StringBuilder mszReaders,
                ref int pcchReaders
        );

        /// <summary>
        /// The SCardLocateCards function searches the readers
        /// listed in the rgReaderStates parameter for a card with
        /// an ATR string that matches one of the card names
        /// specified in mszCards, returning immediately with the result.
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="mszCards"></param>
        /// <param name="rgReaderStates"></param>
        /// <param name="cReaders"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardLocateCards
            (
                IntPtr hContext,
                string mszCards,
                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
                SCARD_READERSTATE[] rgReaderStates,
                int cReaders
            );

        /// <summary>
        /// The SCardReconnect function reestablishes an existing
        /// connection between the calling application and a smart card.
        /// This function moves a card handle from direct access
        /// to general access, or acknowledges and clears an error
        /// condition that is preventing further access to the card.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="dwShareMode"></param>
        /// <param name="dwPreferredProtocols"></param>
        /// <param name="dwInitialization"></param>
        /// <param name="pdwActiveProtocol"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardReconnect
            (
                IntPtr hCard,
                int dwShareMode,
                int dwPreferredProtocols,
                int dwInitialization,
                ref int pdwActiveProtocol
            );

        /// <summary>
        /// The SCardReleaseContext function closes an established
        /// resource manager context, freeing any resources allocated
        /// under that context, including SCARDHANDLE objects and memory
        /// allocated using the SCARD_AUTOALLOCATE length designator.
        /// </summary>
        /// <param name="hContext"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardReleaseContext (IntPtr hContext);

        /// <summary>
        /// The SCardStatus function provides the current status of
        /// a smart card in a reader. You can call it any time after
        /// a successful call to SCardConnect and before a successful
        /// call to SCardDisconnect. It does not affect the state
        /// of the reader or reader driver.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="mszReaderNames"></param>
        /// <param name="pcchReaderLen"></param>
        /// <param name="pdwState"></param>
        /// <param name="pdwProtocol"></param>
        /// <param name="pbAtr"></param>
        /// <param name="pcbAtrLen"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardStatus
            (
                IntPtr hCard,
                string mszReaderNames,
                ref int pcchReaderLen,
                ref int pdwState,
                ref int pdwProtocol,
                byte[] pbAtr,
                ref int pcbAtrLen
            );

        /// <summary>
        /// The SCardTransmit function sends a service request to
        /// the smart card and expects to receive data back from the card.
        /// </summary>
        /// <param name="hCard"></param>
        /// <param name="pioSendPci"></param>
        /// <param name="pbSendBuffer"></param>
        /// <param name="cbSendLength"></param>
        /// <param name="pioRecvPci"></param>
        /// <param name="pbRecvBuffer"></param>
        /// <param name="pcbRecvLength"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int SCardTransmit
            (
                IntPtr hCard,
                IntPtr pioSendPci,
                byte[] pbSendBuffer,
                int cbSendLength,
                IntPtr pioRecvPci,
                byte[] pbRecvBuffer,
                ref int pcbRecvLength
        );

        #endregion
    }
}
