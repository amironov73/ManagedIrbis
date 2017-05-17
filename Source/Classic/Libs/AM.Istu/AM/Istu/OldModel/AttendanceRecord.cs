// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AttendanceRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class AttendanceRecord
    {
        #region Properties

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private Loan _loan;

        ///// <summary>
        ///// 
        ///// </summary>
        //[MapIgnore]
        //public Loan Loan
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return _loan;
        //    }
        //    [DebuggerStepThrough]
        //    set
        //    {
        //        _loan = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the reader.
        /// </summary>
        [MapIgnore]
        public ReaderRecord Reader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MapIgnore]
        public object Context { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("id")]
        public int ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("machine")]
        public string Machine { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("abonement")]
        public string Abonement { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("ticket")]
        public string Ticket { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("moment")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("operator")]
        public int Operator { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("typ")]
        public char Type { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("number")]
        public string Number { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
