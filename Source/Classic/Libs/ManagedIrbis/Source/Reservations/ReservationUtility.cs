// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationUtility.cs -- 
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

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ReservationUtility
    {
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabaseName = "RESERV";

        /// <summary>
        /// Default prefix for number search index.
        /// </summary>
        public const string DefaultNumberPrefix = "N=";

        /// <summary>
        /// Default prefix for room search index.
        /// </summary>
        public const string DefaultRoomPrefix = "ROOM=";

        /// <summary>
        /// Default room menu.
        /// </summary>
        public const string DefaultRoomMenu = "10.mnu";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public static NonNullValue<string> DatabaseName { get; set; }

        /// <summary>
        /// Number search prefix.
        /// </summary>
        public static NonNullValue<string> NumberPrefix { get; set; }

        /// <summary>
        /// Room search prefix.
        /// </summary>
        public static NonNullValue<string> RoomPrefix { get; set; }

        /// <summary>
        /// Room menu name.
        /// </summary>
        public static NonNullValue<string> RoomMenu { get; set; }

        #endregion

        #region Construction

        static ReservationUtility()
        {
            DatabaseName = DefaultDatabaseName;
            NumberPrefix = DefaultNumberPrefix;
            RoomPrefix = DefaultRoomPrefix;
            RoomMenu = DefaultRoomMenu;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
