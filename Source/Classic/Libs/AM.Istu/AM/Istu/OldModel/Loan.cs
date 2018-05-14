// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Loan.cs --
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
    public class Loan
    {
        #region Properties

        /// <summary>
        /// Kind.
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        /// Inventory number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Barcode.
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// RFID.
        /// </summary>
        public string Rfid { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ticket.
        /// </summary>
        public string Ticket1 { get; set; }

        /// <summary>
        /// Ticket.
        /// </summary>
        public string Ticket2 { get; set; }

        /// <summary>
        /// Issue date.
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Due date.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Operator ID.
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// Machine name.
        /// </summary>
        public string MachineName { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
