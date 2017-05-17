// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HudRecord.cs --
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
    public class HudRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        [MapField("invnum")]
        public string Inventory { get; set; }

        /// <summary>
        /// Gets or sets the barcode.
        /// </summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the moment of the barcode binding.
        /// </summary>
        [Browsable(false)]
        [MapField("whn")]
        public DateTime When { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        [Browsable(false)]
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        [MapIgnore]
        //[DisplayTitle("Издательство")]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        [MapIgnore]
        //[DisplayTitle("Автор")]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the numbers.
        /// </summary>
        /// <value>The numbers.</value>
        [MapIgnore]
        //[DisplayTitle("Имеющиеся номера")]
        public string[] Numbers { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [MapIgnore]
        //[DisplayTitle("Заглавие")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        [MapIgnore]
        //[DisplayTitle("Год издания")]
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [MapIgnore]
        //[DisplayTitle("Код в каталоге")]
        public string Index { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
