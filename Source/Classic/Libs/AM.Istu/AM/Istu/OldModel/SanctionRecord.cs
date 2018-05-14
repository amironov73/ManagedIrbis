// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SanctionRecord.cs --
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

using MoonSharp.Interpreter;

#endregion

namespace AM.Istu.OldModel
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SanctionRecord
    {
        #region Properties

        /// <summary>
        /// Whether is new sanction?
        /// </summary>
        [MapIgnore]
        [HiddenColumn]
        [Browsable(false)]
        public bool IsNew
        {
            [DebuggerStepThrough]
            get
            {
                return ID == 0;
            }
        }

        /// <summary>
        /// ID.
        /// </summary>
        [Browsable(false)]
        [MapField("id")]
        public int ID { get; set; }

        /// <summary>
        /// Ticket.
        /// </summary>
        [Browsable(false)]
        [MapField("ticket")]
        public string Ticket { get; set; }

        ///<summary>
        ///
        ///</summary>
        [SortIndex(0)]
        [ColumnIndex(0)]
        [ColumnWidth(100)]
        [ColumnHeader("Дата")]
        [MapField("moment")]
        //[DisplayTitle("Дата назначения")]
        public DateTime Moment { get; set; }

        ///<summary>
        ///
        ///</summary>
        [SortIndex(1)]
        [ColumnIndex(1)]
        [ColumnWidth(50)]
        [MapField("active")]
        [ColumnHeader("Действ.")]
        //[DisplayTitle("Действует")]
        public bool Active { get; set; }

        ///<summary>
        ///
        ///</summary>
        [SortIndex(2)]
        [ColumnIndex(2)]
        [ColumnWidth(150)]
        [MapField("description")]
        [ColumnHeader("Описание")]
        //[DisplayTitle("Описание")]
        public string Description { get; set; }

        ///<summary>
        ///
        ///</summary>
        [SortIndex(3)]
        [ColumnIndex(3)]
        [ColumnWidth(50)]
        [MapField("operator")]
        [ColumnHeader("Оператор")]
        //[DisplayTitle("Оператор")]
        public int Operator { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        ///
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static SanctionRecord CreateNew()
        {
            SanctionRecord result = new SanctionRecord
            {
                Active = true,
                Moment = DateTime.Now
            };

            return result;
        }


        #endregion
    }
}
