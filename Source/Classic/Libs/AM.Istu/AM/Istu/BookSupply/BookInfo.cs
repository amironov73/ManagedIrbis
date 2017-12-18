// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookInfo.cs -- 
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

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Istu.BookSupply
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BookInfo
    {
        #region Properties

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public int Amount { get; private set; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// Gets the card number.
        /// </summary>
        public string CardNumber { get; private set; }

        /// <summary>
        /// Gets the bibliographical description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is electronic.
        /// </summary>
        public bool IsElectronic { get; private set; }

        /// <summary>
        /// Gets or sets the ordinal.
        /// </summary>
        public int Ordinal { get; internal set; }

        /// <summary>
        /// Gets the record.
        /// </summary>
        public MarcRecord Record { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="BookInfo"/> is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets the stamp.
        /// </summary>
        public string Stamp { get; }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public string Year { get; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        public string Series { get; }

        /// <summary>
        /// Gets the publisher.
        /// </summary>
        public string Publisher { get; }

        /// <summary>
        /// Gets the city.
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Gets the year.
        /// </summary>
        public string Volume { get; }

        /// <summary>
        /// Gets the author sign.
        /// </summary>
        public string AuthorSign { get; }

        /// <summary>
        /// Gets the shelf code.
        /// </summary>
        public string ShelfCode { get; }

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
