// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PodsobRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;
using AM.Data;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PodsobRecord
    {
        #region Properties

        ///<summary>
        /// 
        ///</summary>
        [MapField("invent")]
        public int Inventory { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("chb")]
        public string Ticket { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("ident")]
        public string AdditionalInfo { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("operator")]
        public int Operator { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        ///<summary>
        /// 
        ///</summary>
        [MapField("prodlen")]
        public int Prolong { get; set; }

        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Примечания об экземпляре книги.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// Место хранения ЦОР, ЦНИ и т. д.
        /// </summary>
        [MapField("sigla")]
        public string Sigla { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
