// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LibraryService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using BLToolkit.DataAccess;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public class LibraryService
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get loan by the number.
        /// </summary>
        [CanBeNull]
        public Loan GetLoanByNumber
            (
                [JetBrains.Annotations.NotNull] string number
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get loan by the barcode.
        /// </summary>
        [CanBeNull]
        public Loan GetLoanByBarcode
            (
                [JetBrains.Annotations.NotNull] string barcode
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get loan by the RFID.
        /// </summary>
        [CanBeNull]
        public Loan GetLoanByRfid
            (
                [JetBrains.Annotations.NotNull] string rfid
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get loans for the ticket.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        [ItemNotNull]
        public Loan[] GetLoansForTicket
            (
                [JetBrains.Annotations.NotNull] string ticket
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get reader by the ticket.
        /// </summary>
        [CanBeNull]
        public ReaderRecord GetReaderForTicket
            (
                [JetBrains.Annotations.NotNull] string ticket
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get reader for the barcode.
        /// </summary>
        [CanBeNull]
        public ReaderRecord GetReaderForBarcode
            (
                [JetBrains.Annotations.NotNull] string barcode
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get reader for the RFID.
        /// </summary>
        [CanBeNull]
        public ReaderRecord GetReaderForRfid
            (
                [JetBrains.Annotations.NotNull] string rfid
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the document.
        /// </summary>
        public void GiveDocument
            (
                [JetBrains.Annotations.NotNull] Loan loan,
                [JetBrains.Annotations.NotNull] ReaderRecord reader
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the document.
        /// </summary>
        public void ReturnDocument
            (
                [JetBrains.Annotations.NotNull] Loan loan
            )
        {

        }

        #endregion

        #region Object members

        #endregion
    }
}
