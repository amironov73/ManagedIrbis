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
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Menus;
using ManagedIrbis.Readers;

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
        /// Get reader name.
        /// </summary>
        [CanBeNull]
        public string GetReaderName
            (
                [NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            string previousDatabase = Provider.Database;
            Provider.Database = "RDR";
            try
            {
                string expression = string.Format
                (
                    "\"RI={0}\"",
                    ticket
                );
                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return null;
                }
                if (found.Length != 1)
                {
                    return null;
                }
                int mfn = found[0];
                MarcRecord record = Provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    return null;
                }
                ReaderInfo reader = ReaderInfo.Parse(record);
                string result = reader.FullName;

                return result;
            }
            finally
            {
                Provider.Database = previousDatabase;
            }
        }

        /// <summary>
        /// Get record for the resource.
        /// </summary>
        [CanBeNull]
        public MarcRecord GetResourceRecord
                (
                    [NotNull] string room,
                    [NotNull] string number
                )
        {
            Code.NotNullNorEmpty(room, "room");
            Code.NotNullNorEmpty(number, "number");

            string expression = string.Format
                (
                    "\"{0}{1}\" * \"{2}{3}\"",
                    ReservationUtility.RoomPrefix,
                    room,
                    ReservationUtility.NumberPrefix,
                    number
                );
            int[] found = Provider.Search(expression);
            if (found.Length != 1)
            {
                return null;
            }
            MarcRecord result = Provider.ReadRecord(found[0]);

            return result;
        }

        /// <summary>
        /// Give the resource.
        /// </summary>
        public ReservationInfo GiveResource
            (
                [NotNull] ReservationInfo resource,
                [NotNull] string ticket
            )
        {
            Code.NotNull(resource, "resource");
            Code.NotNullNorEmpty(ticket, "ticket");

            MarcRecord record = GetResourceRecord
                (
                    resource.Room.ThrowIfNull("resource.Room"),
                    resource.Number.ThrowIfNull("resource.Number")
                );
            if (ReferenceEquals(record, null))
            {
                throw new IrbisException();
            }
            resource = ReservationInfo.ParseRecord(record);
            if (resource.Status != ReservationStatus.Free)
            {
                // TODO some reaction?

                return resource;
            }
            resource.Status = ReservationStatus.Busy;

            string readerName = GetReaderName(ticket);
            HistoryInfo entry = new HistoryInfo
            {
                BeginDate = DateTime.Now,
                Ticket = ticket,
                Name = readerName
            };
            resource.History.Add(entry);

            //int mfn = record.Mfn;
            //int version = record.Version;
            resource.ApplyToRecord(record);
            //record = resource.ToRecord();
            //record.Database = Provider.Database;
            //record.Mfn = mfn;
            //record.Version = version;
            Provider.WriteRecord(record);
            resource.Record = record;

            return resource;
        }

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
