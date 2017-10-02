// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservationManager.cs -- 
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

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

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
    public sealed class ReservationManager
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservationManager
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// List rooms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public MenuEntry[] ListRooms()
        {
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    Provider.Database,
                    ReservationUtility.RoomMenu
                );
            MenuFile menuFile = Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menuFile, null))
            {
                return new MenuEntry[0];
            }

            return menuFile.Entries.ToArray();
        }

        /// <summary>
        /// List resources.
        /// </summary>
        [NotNull]
        public ReservationInfo[] ListResources
            (
                [CanBeNull] string roomCode
            )
        {
            string expression = string.Format
                (
                    "\"{0}{1}\"",
                    ReservationUtility.RoomPrefix,
                    roomCode
                );
            int[] found = Provider.Search(expression);
            List<ReservationInfo> result
                = new List<ReservationInfo>(found.Length);
            foreach (int mfn in found)
            {
                MarcRecord record = Provider.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    ReservationInfo item = ReservationInfo.ParseRecord(record);
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        #endregion
    }
}
